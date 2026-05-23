<template>
    <div class="board-column">
        <div class="column-header">
            <h3>{{ title }}</h3>
            <span>{{ tickets.length }}</span>
        </div>
        <div v-for="ticket in tickets" :key="ticket.id" @click="$emit('open-ticket', ticket)" class="ticket-card">
            <h4>{{ ticket.title }}</h4>
            <p>{{ ticket.description }}</p>
        </div>
    </div>
</template>

<script setup lang="ts">
import type { Ticket } from '@/types/ticket';

defineProps<{
    title: string;
    tickets: Ticket[];
}>();
</script>

<style scoped>
.board-column {
    min-width: 320px;
    flex: 1;
    border-radius: 18px;
    background: #1e1e1e;
    padding: 1rem;
}

.column-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
}

.column-header span {
    background: #2d2d2d;
    padding: 0.25rem 0.65rem;
    border-radius: 999px;
}

.ticket-card {
    background: #292929;
    border-radius: 14px;
    padding: 1rem;
    margin-bottom: 1rem;
    cursor: pointer;
    transition:
        transform 0.2s ease,
        background-color 0.2s ease;
}

.ticket-card:hover {
    transform: translateY(-3px);
    background: #343434;
}

.ticket-card h4 {
    margin-bottom: 0.5rem;
}

.ticket-card p {
    color: #b0b0b0;
    font-size: 0.95rem;
}
</style>