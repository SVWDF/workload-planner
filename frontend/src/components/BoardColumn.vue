<template>
    <div class="board-column">
        <div class="column-header">
            <h3>{{ title }}</h3>
            <span>{{ tickets.length }}</span>
        </div>
        <div v-for="ticket in tickets" :key="ticket.id" @click="$emit('open-ticket', ticket)" class="ticket-card"
        :class="{
            low: ticket.priority === TicketPriority.Low,
            medium: ticket.priority === TicketPriority.Medium,
            high: ticket.priority === TicketPriority.High
        }"
        >
            <h4>{{ ticket.title }}</h4>
            <p>{{ ticket.description }}</p>
        </div>
    </div>
</template>

<script setup lang="ts">
import { TicketPriority, type Ticket } from '@/types/ticket';

defineProps<{
    title: string;
    tickets: Ticket[];
}>();
</script>

<style scoped>
.board-column {
    min-width: 300px;
    min-height: 500px;
    flex: 1;
    border-radius: 18px;
    background: var(--bg-secondary);
    padding: 1rem;
}

.column-header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    margin-bottom: 1rem;
}

.column-header span {
    background: var(--bg-tertiary);
    padding: 0.25rem 0.65rem;
    border-radius: 999px;
    color: var(--text-secondary);
    font-size: 0.9rem;
}

.ticket-card {
    background: #2a2a2a;
    border: 1px solid rgba(255, 255, 255, 0.05);
    border-radius: var(--radius-card);
    padding: 1rem;
    margin-bottom: 1rem;
    cursor: pointer;
    transition:
        transform 0.2s ease,
        background-color 0.2s ease;
    min-height: 92px;
    position: relative;
    overflow: hidden;
}

.ticket-card::before {
    content: "";
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    width: 5px;
    border-radius: 999px;
}

.ticket-card.low::before {
  background: #6ea8fe;
}

.ticket-card.medium::before {
  background: #d6a82c;
}

.ticket-card.high::before {
  background: #ef4444;
}

.ticket-card:hover {
    transform: translateY(-3px);
    background: #343434;
}

.ticket-card h4 {
    margin-bottom: 0.5rem;
    overflow: hidden;
    text-overflow: ellipsis;
    white-space: nowrap;
}

.ticket-card p {
    color: #b0b0b0;
    font-size: 0.95rem;
    overflow: hidden;
    display: -webkit-box;
    line-clamp: 2;
    -webkit-line-clamp: 2;
    -webkit-box-orient: vertical;
}
</style>