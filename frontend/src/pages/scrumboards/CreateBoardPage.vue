<template>
  <div class="create-board-page">
    <h1>Create Scrum Board</h1>

    <form @submit.prevent="createBoard">
      <input
        v-model="name"
        placeholder="Board name"
        required
      />

      <div class="color-picker">
        <button
          type="button"
          v-for="color in colors"
          :key="color"
          class="color-option"
          :class="{ selected: selectedColor === color }"
          :style="{ backgroundColor: color }"
          @click="selectedColor = color"
        />
      </div>

      <input
        v-model="search"
        @input="handleSearchUsers"
        placeholder="Search users..."
      />
      <div class="user-results">
        <div v-for="user in users" :key="user.id" class="user-result">
          <span>{{ user.username }}</span>
          <button @click="addUser(user)">Add</button>
        </div>
      </div>

      <div class="selected-users">
        <div v-for="user in selectedUsers" :key="user.id" class="selected-user">
          {{ user.username }}
          <button @click="removeUser(user.id)">
            X
          </button>
        </div>
      </div>

      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>
      <button type="submit" class="submit-button">
        Create Board
      </button>
    </form>
  </div>
</template>

<script setup lang="ts">
import { onMounted, ref } from "vue";
import { useRouter } from "vue-router";
import { useScrumBoards } from '../../composables/scrumboard';
import { useUsers } from "../../composables/user";
import { type User } from "../../types/user";
import { createSlug } from "../../helpers/slug";

const name = ref("");
const colors = ref<string[]>([]);
const selectedColor = ref("");
const search = ref("");
const users = ref<User[]>([]);
const selectedUsers = ref<User[]>([]);
const localErrors = ref<string[]>([]);
const router = useRouter();
const { createScrumBoard, getBoardColors } = useScrumBoards();
const { searchUsers } = useUsers();
let timeout: number;

const loadColors = async () => {
    const response = await getBoardColors();

    colors.value = response.data;

    selectedColor.value =
        colors.value[
            Math.floor(Math.random() * colors.value.length)
        ]!;
};

const handleSearchUsers = async () => {
  clearTimeout(timeout);
  timeout = setTimeout(async () => {
    const query = search.value.trim();
  
    if (query.length < 3) {
      users.value = [];
      return;
    }
  
    try {
      const response = await searchUsers(search.value);
      users.value = response.data;
    }
    catch {
      users.value = [];
    }
  }, 300);
};

const addUser = (user: User) => {
  const exists = selectedUsers.value.some(
    (u: User) => u.id === user.id
  );

  if (exists) return;

  selectedUsers.value.push(user);

  search.value = "";
  users.value = [];
};

const removeUser = (id: string) => {
  selectedUsers.value = 
    selectedUsers.value.filter(
      (u: User) => u.id !== id
    );
};

const createBoard = async () => {
  const response = await createScrumBoard({
    name: name.value,
    color: selectedColor.value,
    memberIds: selectedUsers.value.map((u: User) => u.id)
  });

  if (!response.success) {
    localErrors.value = response.errors;
    return;
  }

  localErrors.value = [];

  const slug = createSlug(response.data.name);
  router.push(`/boards/${slug}-${response.data.id}`);
};

onMounted(loadColors);
</script>

<style scoped>
.create-board-page {
    max-width: 700px;
    margin: 3rem auto;
    padding: 2rem;

    border-radius: 20px;
    background: #1e1e1e;

    box-shadow:
        0 8px 30px rgba(0,0,0,0.15);
}

.create-board-page h1 {
    margin-bottom: 2rem;

    font-size: 2rem;
    font-weight: 600;
}

.create-board-page form {
    display: flex;
    flex-direction: column;
    gap: 1.5rem;
}

.create-board-page input {
    padding: 0.9rem 1rem;

    border: 1px solid #333;
    border-radius: 12px;

    background: #262626;
    color: white;

    font-size: 1rem;
    transition: border-color 0.2s ease;
}

.create-board-page input:focus {
    outline: none;
    border-color: #4f46e5;
}

.color-picker {
    display: flex;
    flex-wrap: wrap;
    gap: 0.75rem;
}

.color-option {
    width: 42px;
    height: 42px;

    border-radius: 50%;
    border: 3px solid transparent;

    cursor: pointer;

    transition:
        transform 0.2s ease,
        border-color 0.2s ease;
}

.color-option:hover {
    transform: scale(1.08);
}

.color-option.selected {
    border-color: white;
    transform: scale(1.12);
}

.user-results {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.user-result {
    display: flex;
    justify-content: space-between;
    align-items: center;

    padding: 0.85rem 1rem;

    border-radius: 12px;
    background: #262626;

    transition: background-color 0.2s ease;
}

.user-result:hover {
    background: #313131;
}

/* Selected Users */
.selected-users {
    display: flex;
    flex-wrap: wrap;
    gap: 0.75rem;
}

.selected-user {
    display: flex;
    align-items: center;
    gap: 0.5rem;

    padding: 0.5rem 0.9rem;

    border-radius: 999px;
    background: #313131;
}

.selected-user button {
    border: none;
    background: transparent;
    color: #c1c1c1;

    cursor: pointer;
    font-size: 0.85rem;
}

.selected-user button:hover {
    color: #ef4444;
}

.error-box {
  background: rgba(255, 77, 77, 0.1);
  color: #ff6b6b;
  border: 1px solid rgba(255, 107, 107, 0.3);
  border-radius: 8px;
  padding-inline: 0.5rem;
  margin-top: 1rem;
  text-align: left;
  font-size: 0.9rem;
  width: fit-content;
}

.error-box > p {
  margin-top: 6px;
  margin-bottom: 6px;
}

.submit-button {
    align-self: flex-start;
    padding: 0.9rem 1.5rem;

    border: none;
    border-radius: 12px;

    background: #4f46e5;
    color: white;

    font-weight: 600;
    cursor: pointer;

    transition:
        transform 0.2s ease,
        background-color 0.2s ease;
}

.submit-button:hover {
    background: #4338ca;
    transform: translateY(-2px);
}
</style>