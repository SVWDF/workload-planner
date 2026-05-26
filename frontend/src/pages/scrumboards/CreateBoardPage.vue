<template>
  <div class="create-board-page">
    <h2>Create Scrum Board</h2>

    <form @submit.prevent="createBoard">
      <div class="form-group">
        <label for="board-name">Board Name</label>
        <input
          id="board-name"
          v-model="form.name"
          placeholder="Enter board name"
          required
        />
      </div>

      <div class="form-group">
        <label>Board Color</label>
        <div class="color-picker">
          <button
            type="button"
            v-for="color in colors"
            :key="color"
            class="color-option"
            :class="{ selected: form.color === color }"
            :style="{ backgroundColor: color }"
            @click="form.color = color"
          />
        </div>
      </div>

      <div class="member-search">
        <div class="form-group">
          <label for="search-users">Add Members</label>
          <input
            id="search-users"
            v-model="search"
            @input="handleSearchUsers"
            placeholder="Search users..."
          />
        </div>
        <div class="user-results">
          <div v-for="user in users" :key="user.id" class="user-result">
            <div class="user-search-data">
              <span>{{ user.username }}</span>
              <span>{{ user.email }}</span>
            </div>
            <button type="button" @click="addUser(user)">Add</button>
          </div>
        </div>
      </div>

      <div class="selected-users">
        <div v-for="user in selectedUsers" :key="user.id" class="selected-user">
          <span>{{ user.username }}</span>
          <button type="button" @click="removeUser(user.id)">X</button>
        </div>
      </div>

      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>
      <div class="form-actions">
        <button type="submit">Create Board</button>
        <button type="button" @click="cancelBoardCreation">Cancel</button>
      </div>
    </form>
  </div>
</template>

<script setup lang="ts">
import { onMounted, reactive, ref } from "vue";
import { useRouter } from "vue-router";
import { useScrumBoards } from '../../composables/scrumboard';
import { useUsers } from "../../composables/user";
import { type User } from "../../types/user";
import { createSlug } from "../../helpers/slug";
import { BOARD_COLORS } from "@/constants/boardColors";
import type { CreateScrumBoardRequest } from "@/types/scrumboard";

const form = reactive<CreateScrumBoardRequest>({
  name: "",
  color: "",
  memberIds: []
});
const colors = ref<string[]>([]);
const search = ref("");
const users = ref<User[]>([]);
const selectedUsers = ref<User[]>([]);
const localErrors = ref<string[]>([]);
const router = useRouter();
const { createScrumBoard } = useScrumBoards();
const { searchUsers } = useUsers();
const boardColors = BOARD_COLORS;
let timeout: ReturnType<typeof setTimeout>;

const loadColors = async () => {
    colors.value = boardColors;

    form.color =
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
      const response = await searchUsers(query);
      users.value = response.data
        .filter((user: User) => !selectedUsers.value
          .some((selected: User) => selected.id === user.id));
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
  handleSearchUsers();
};

const createBoard = async () => {
  localErrors.value = [];

  form.memberIds = selectedUsers.value.map((u: User) => u.id);
  const response = await createScrumBoard(form);

  if (!response.success || !response.data) {
    localErrors.value = response.errors;
    return;
  }

  form.name = "";
  form.memberIds = [];
  selectedUsers.value = [];
  search.value = "";
  users.value = [];
  
  const slug = createSlug(response.data.name);
  router.push({ name: "Board", params: { slug: `${slug}-${response.data.id}`}});
};

const cancelBoardCreation = () => {
  form.name = "";
  form.memberIds = [];
  selectedUsers.value = [];
  search.value = "";
  users.value = [];
  localErrors.value = [];
  router.push({ name: "Dashboard"});
};

onMounted(loadColors);
</script>

<style scoped>
.create-board-page {
  max-width: 700px;
  margin: 3rem auto;
  padding: 2rem;
  border-radius: 20px;
  background: var(--bg-secondary);
  box-shadow: 0 8px 30px rgba(0,0,0,0.15);
}

.create-board-page h2 {
  margin-bottom: 2rem;
}

.create-board-page form {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.create-board-page input {
  padding: 0.9rem 1rem;
  background: #262626;
}

.form-group {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  margin-bottom: 12px;
}

.form-group label {
  font-weight: 600;
  color: var(--text-primary);
  margin-left: 4px;
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

.member-search {
  position: relative;
}

.user-results {
  position: absolute;
  top: 95%;
  left: 0;
  z-index: 10;
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  width: 100%;
  max-width: 300px;
  max-height: 180px;
  overflow-y: auto;
  background-color: var(--bg-secondary);
  box-shadow: var(--shadow-card);
}

.user-search-data {
  display: flex;
  flex-direction: column;
}

.user-search-data > span:first-child {
  font-weight: 500;
}

.user-search-data > span:last-child {
  display: none;
  
}

.user-result {
  display: flex;
  justify-content: space-between;
  align-items: center;
  padding: 0.85rem 1rem;
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
  border-radius: 8px;
  background: #313131;
}

.selected-user button {
  border: none;
  background: transparent;
  color: #c1c1c1;
  padding: 0;
  cursor: pointer;
  font-size: 1rem;
  margin-left: 8px;
  color: red;
}

.selected-user button:hover {
  color: #ef4444;
}

.form-actions {
  display: flex;
  gap: 1rem;
}

.form-actions button {
  align-self: flex-start;
  padding: 0.9rem 1.5rem;
}

@media (min-width: 36em) {
  .user-search-data > span:last-child {
    display: block;
    font-size: 0.9rem;
    color: #c1c1c1;
  }
}
</style>