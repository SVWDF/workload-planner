<template>
    <div v-if="open" class="modal-overlay">
    <div class="ticket-modal">
      <h2>Create Ticket</h2>
      <form @submit.prevent="handleCreateTicket">
          <div class="form-group">
            <label for="ticket-create-title">Title</label>
            <input id="ticket-create-title" v-model="form.title" placeholder="Title" required/>
          </div>
          <div class="form-group">
            <label for="ticket-create-description">Description</label>
            <textarea id="ticket-create-description" v-model="form.description" placeholder="Description"/>
          </div>
          <div class="form-group">
            <label>Priority</label>
            <select v-model="form.priority">
                <option :value="TicketPriority.Low">Low</option>
                <option :value="TicketPriority.Medium">Medium</option>
                <option :value="TicketPriority.High">High</option>
            </select>
          </div>
          <div v-if="errors.length" class="error-box">
            <p v-for="(e, i) in errors" :key="i">{{ e }}</p>
          </div>
          <div class="modal-actions">
            <button type="button" @click="emit('close')">Cancel</button>
            <button type="submit">Create</button>
          </div>
      </form>
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
    background: var(--bg-secondary);
    padding: 2rem;
}

h2 {
    margin-bottom: 24px;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.5rem;
}

.form-group:not(:last-child) {
    margin-bottom: 12px;
}

input,
textarea {
    width: 100%;
}

textarea {
    min-height: 100px;
    max-width: 100%;
}

select {
    width: fit-content;
    min-width: 160px;
}

.modal-actions {
    display: flex;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
}
</style>