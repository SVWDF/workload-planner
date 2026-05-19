<template>
  <div class="dashboard">
    <span>Your Workspaces</span>
    <button @click="goToCreateBoard">
        Create Board
    </button>
    <div class="scrumboards-grid">
      <BoardCard
        v-for="board in boards"
        :key="board.id"
        :board="board"
        @click="openBoard(board)"
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { useScrumBoards } from '@/composables/scrumboard';
import { type ScrumBoard } from '@/types/scrumboard';
import { createSlug } from '@/helpers/slug';
import BoardCard from '@/components/BoardCard.vue';

const router = useRouter();
const { getBoards } = useScrumBoards();
const boards = ref<ScrumBoard[]>([]);

const loadBoards = async () => {
  try {
    const result = await getBoards()
    boards.value = result.data
  }
  catch (error) {
    console.error("Failed to load boards", error);
  }
} 

const goToCreateBoard = () => {
  router.push("/boards/create");
};

const openBoard = (board: ScrumBoard) => {
  const slug = createSlug(board.name);
  router.push(`/boards/${slug}-${board.id}`);
};

onMounted(loadBoards)
</script>

<style scoped>
div.dashboard > span {
  font-size: 1.25rem;
  font-weight: 700;
  margin-right: 24px;
}

div.dashboard > button {
  border: none;
  background-color: #4f8cff;
  transition: background 0.25s ease, transform 0.1s ease;
}

div.dashboard > button:hover {
  background-color: #3d79e6;
}

div.scrumboards-grid {
  margin-top: 16px;
  border: 2px solid #404040;
  padding: 0.75rem;
  border-radius: 12px;
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 16px;

}
@media (min-width: 992px) {
  div.scrumboards-grid {
    grid-template-columns: repeat(3, 1fr);
  }
 }

@media (min-width: 1200px) {
  div.scrumboards-grid {
    grid-template-columns: repeat(4, 1fr);
  }
}

</style>