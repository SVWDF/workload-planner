<template>
  <div v-if="isLoading">
    <p>Loading Workspaces...</p>
  </div>
  <div v-else-if="boards.length === 0">
    <h3>No workspaces yet</h3>
    <p>Create your first one to get started.</p>
  </div>
  <div v-else class="dashboard">
    <div class="dashboard-header">
      <h3>Your Workspaces</h3>
      <button @click="goToCreateBoard">Create Board</button>
    </div>
    <div class="scrumboards-grid">
      <BoardCard
        v-for="board in boards"
        :key="board.id"
        :board="board"
        @click="openBoard(board)"
      />
      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>
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
const isLoading = ref(true);
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
  finally {
    isLoading.value = false;
  }
} 

const goToCreateBoard = () => {
  router.push({ name: "CreateBoard" });
};

const openBoard = (board: ScrumBoard) => {
  const slug = createSlug(board.name);
  router.push({ name: "Board", params: { slug: `${slug}-${board.id}`}});
};

onMounted(async () => {
  await loadBoards();
});
</script>

<style scoped>
div.dashboard-header h3 {
  margin: 0 0 12px 0;
}

div.scrumboards-grid {
  margin-top: 16px;
  border-radius: 12px;
  display: grid;
  grid-template-columns: 1fr;
  gap: 16px;

}

@media (min-width: 36em) {
  div.dashboard-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
  }

  div.dashboard-header h3 {
    margin: 0
  }

  div.scrumboards-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (min-width: 48em) {
  div.scrumboards-grid {
    grid-template-columns: repeat(3, 1fr);
  }
}

@media (min-width: 75em) {
  div.scrumboards-grid {
    grid-template-columns: repeat(4, 1fr);
  }
}

</style>