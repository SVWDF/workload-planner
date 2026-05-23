<template>
    <div class="board-card">
        <div class="board-color" :style="{ backgroundColor: board.color }"></div>
        <div class="board-content">
            <h3>{{ board.name }}</h3>
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
    border-radius: 12px;
    cursor: pointer;
    background-color: #1e1e1e;
    transition:
        transform 0.2s ease,
        box-shadow 0.2s ease,
        background-color 0.2s ease;
    min-height: 200px;
}

.board-card:hover {
    transform: translateY(-4px);
    box-shadow: 0 8px 24px rgba(0,0,0,0.2);
    background-color: #262626;
}

.board-color {
    width: 100%;
    height: 60px;
    border-top-left-radius: 12px;
    border-top-right-radius: 12px;
}

.board-content {
    padding: 1rem;
    display: grid;
    grid-template-rows: auto;
}

.board-content > h3 {
    font-size: 1.4rem;
    font-weight: 500;
    margin-bottom: 30px;
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