<template>
    <div v-if="open" class="modal-overlay">
    <div class="ticket-modal">
      <h2>Create Ticket</h2>
      <input v-model="title" placeholder="Title"/>
      <textarea v-model="description" placeholder="Description"/>
      <div v-if="errors.length" class="error-box">
        <p v-for="(e, i) in errors" :key="i">{{ e }}</p>
      </div>

      <div class="modal-actions">
        <button @click="reset">Cancel</button>
        <button @click="handleCreateTicket">Create</button>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";

const props = defineProps<{
    open: boolean;
    errors: string[];
}>();

const emit = defineEmits<{
    (e: "close"): void;
    (e: "create", data: {
        title: string;
        description: string;
    }): void;
}>();

const title = ref("");
const description = ref("");

const handleCreateTicket = () => {
    emit("create", { title: title.value, description: description.value });

};

const reset = () => {
    title.value = "";
    description.value = "";
    emit("close");
};
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