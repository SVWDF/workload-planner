<template>
    <Navbar />
    <main>
      <RouterView />
    </main>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue';
import Navbar from '@/components/Navbar.vue';
import signalRConnection from '@/services/signalr';
import { useScrumBoards } from '@/composables/scrumboard';
import type { ScrumBoard } from '@/types/scrumboard';

const { cachedBoards } = useScrumBoards();
const handleScrumboardCreated = (newScrumboard: ScrumBoard) => {
    const exists = cachedBoards.value.some(sb => sb.id === newScrumboard.id);
    if (!exists) cachedBoards.value.push(newScrumboard);
};

onMounted(async () => {
    if (signalRConnection.state === "Disconnected") await signalRConnection.start();
    signalRConnection.on("ScrumboardCreated", handleScrumboardCreated);
});

onUnmounted(() => {
    signalRConnection.off("ScrumboardCreated", handleScrumboardCreated);
});
</script>

<style scoped>
main {
    max-width: 1200px;
    margin: 50px auto 0;
    padding: 0.5rem 2rem;
}
</style>