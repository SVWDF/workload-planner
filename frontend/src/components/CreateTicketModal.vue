<template>
    <div v-if="open" class="modal-overlay">
    <div class="ticket-modal">
      <h2>Create Ticket</h2>
      <input v-model="form.title" placeholder="Title"/>
      <textarea v-model="form.description" placeholder="Description"/>
      <select v-model="form.priority">
        <option :value="TicketPriority.Low">Low</option>
        <option :value="TicketPriority.Medium">Medium</option>
        <option :value="TicketPriority.High">High</option>
      </select>
      <div v-if="errors.length" class="error-box">
        <p v-for="(e, i) in errors" :key="i">{{ e }}</p>
      </div>

      <div class="modal-actions">
        <button @click="emit('close')">Cancel</button>
        <button @click="handleCreateTicket">Create</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { TicketPriority, type CreateTicketRequest } from "@/types/ticket";
import { reactive, watch } from "vue";

const props = defineProps<{
    open: boolean;
    errors: string[];
}>();

const emit = defineEmits<{
    (e: "close"): void;
    (e: "create", data: Omit<CreateTicketRequest,"scrumboardId">): void;
}>();

const form = reactive<Omit<CreateTicketRequest,"scrumboardId">>({
    title: "",
    description: "",
    priority: TicketPriority.Medium
});

const handleCreateTicket = () => {
    emit("create", { ...form });
};

watch(() => props.open, isOpen => {
    if (!isOpen) {
        form.title = "";
        form.description = "";
        form.priority = TicketPriority.Medium;
    }
});
</script>

<style scoped>
.modal-overlay {
    position: fixed;
    inset: 0;
    display: flex;
    justify-content: center;
    align-items: center;
    background: rgba(0,0,0,0.5);
}

.ticket-modal {
    width: 500px;
    max-width: 90%;
    border-radius: 20px;
    background: #1e1e1e;
    padding: 2rem;
}

.ticket-modal input,
.ticket-modal textarea {
    width: 100%;
    margin-top: 1rem;
}

.modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
}
</style>