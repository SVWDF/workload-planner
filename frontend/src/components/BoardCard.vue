<template>
    <div class="board-card">
        <div class="board-color" :style="{ backgroundColor: board.color }"></div>
        <div class="board-content">
            <h4>{{ board.name }}</h4>
            <div class="board-stats">
                <span>{{ board.members }} {{ board.members === 1 ? "member" : "members" }}</span>
                <span>{{ board.tickets }} {{ board.tickets === 1 ? "ticket" : "tickets" }}</span>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { type ScrumBoard } from '../types/scrumboard';

defineProps<{
    board: ScrumBoard;
}>();
</script>

<style scoped>
.board-card {
    border-radius: var(--radius-card);
    cursor: pointer;
    background-color: var(--bg-secondary);
    transition:
        transform 0.2s ease,
        box-shadow 0.2s ease,
        background-color 0.2s ease;
}

.board-card:hover {
    transform: translateY(-4px);
    box-shadow: var(--shadow-hover);
    background-color: var(--bg-primary);
}

.board-color {
    width: 100%;
    height: 60px;
    border-top-left-radius: var(--radius-card);
    border-top-right-radius: var(--radius-card);
}

.board-content {
    padding: 1rem;
    display: grid;
    grid-template-rows: auto;
}

.board-content > h4 {
    font-weight: 500;
    margin: 8px 0 24px 0;
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
}

div.board-stats > span {
    display: grid;
    grid-template-rows: auto;
    font-size: 0.9rem;
    font-weight: 300;
}
</style>