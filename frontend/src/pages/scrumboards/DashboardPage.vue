<template>
  <div class="dashboard">
    <h2 class="dashboard-title">Your Workspaces</h2>
    <button class="create-board-button" @click="goToCreateBoard">
        Create Board
    </button>
    <div class="scrumboards-grid">
      <BoardCard
        v-for="board in boards"
        :key="board.id"
        :board="board"
        @click="openBoard(board)"
      />
      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">
          {{ e }}
        </p>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import { useRouter } from 'vue-router';
import { useScrumBoards } from '@/composables/scrumboard';
import { type ScrumBoard } from '@/types/scrumboard';
import { createSlug } from '@/helpers/slug';
import BoardCard from '@/components/BoardCard.vue';
import signalRConnection from '@/services/signalr';

const router = useRouter();
const { getBoards } = useScrumBoards();
const boards = ref<ScrumBoard[]>([]);
const localErrors = ref<string[]>([]);

const loadBoards = async () => {
  try {
    const result = await getBoards()
    boards.value = result.data
    localErrors.value = [];
  }
  catch (err: unknown) {
    localErrors.value = (err as { customErrors?: string[]}).customErrors ?? ["Failed to load boards"];
  }
} 

const goToCreateBoard = () => {
  router.push({ name: "CreateBoard" });
};

const openBoard = (board: ScrumBoard) => {
  const slug = createSlug(board.name);
  router.push({ name: "Board", params: { slug: `${slug}-${board.id}`}});
};

const handleScrumboardCreated = (newScrumboard: ScrumBoard) => {
  const exists = boards.value.some(sb => sb.id === newScrumboard.id);
  if (!exists) boards.value.push(newScrumboard);
};

onMounted(async () => {
  if (signalRConnection.state === "Disconnected") await signalRConnection.start();
  signalRConnection.on("ScrumboardCreated", handleScrumboardCreated);
  await loadBoards();
});

onUnmounted(() => {
  signalRConnection.off("ScrumboardCreated", handleScrumboardCreated);
});
</script>

<style scoped>
div.dashboard > h2 {
  font-size: 1.25rem;
  font-weight: 700;
  margin-right: 24px;
}

div.dashboard > button.create-board-button {
  border: none;
  background-color: #4f8cff;
  transition: background 0.25s ease, transform 0.1s ease;
}

div.dashboard > button.create-board-button:hover {
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

error-box {
  background: rgba(255, 77, 77, 0.1);
  color: #ff6b6b;
  border: 1px solid rgba(255, 107, 107, 0.3);
  border-radius: 8px;
  padding-inline: 0.5rem;
  margin-top: 1rem;
  text-align: left;
  font-size: 0.9rem;
  width: 100%;
  max-width: 200px;
  box-sizing: border-box;
}

.error-box > p {
  margin-top: 6px;
  margin-bottom: 6px;
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