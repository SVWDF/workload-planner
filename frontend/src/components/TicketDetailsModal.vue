<template>
    <div v-if="open && ticket" class="modal-overlay">
        <div class="ticket-modal">
            <h2>Ticket Details</h2>
            <input v-model="localTitle" :disabled="!isManager" />
            <textarea v-model="localDescription" :disabled="!isManager"></textarea>
            <p>Assigned: {{ ticket.assignedUser ?? "Nobody" }}</p>
            <button @click="emit('assign')">Assign to me</button>
            <select v-model="localStatus" @change="handleStatusChange">
                <option :value="TicketStatus.Todo">To Do</option>
                <option :value="TicketStatus.InProgress">In Progress</option>
                <option :value="TicketStatus.Done">Done</option>
            </select>
            <div v-if="errors.length" class="error-box">
                <p v-for="(e, i) in errors" :key="i">{{ e }}</p>
            </div>
            <div class="modal-actions"> 
                <button @click=" emit('close')">Close</button>
                <template v-if="isManager">
                    <button @click="handleSave">Save</button>
                    <button @click="emit('delete')">Delete</button>
                </template>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref, watch } from "vue";
import { TicketStatus, type Ticket } from "@/types/ticket";

const props = defineProps<{
    open: boolean;
    ticket: Ticket | null;
    isManager: boolean;
    errors: string[];
}>();

const emit = defineEmits<{
    (e: "close"): void;
    (e: "save", data: {
        title: string;
        description: string;
    }): void;
    (e: "delete"): void;
    (e: "assign"): void;
    (e: "status-change", status: TicketStatus): void;
}>();

const localTitle = ref("");
const localDescription = ref("");
const localStatus = ref<TicketStatus>(TicketStatus.Todo);

watch(() => props.ticket,
    ticket => {
        if (!ticket) return;

        localTitle.value = ticket.title;
        localDescription.value = ticket.description;
        localStatus.value = ticket.status;
    },
    { immediate: true }
);

const handleSave = () => {
    emit("save", { title: localTitle.value, description: localDescription.value });
};

const handleStatusChange = () => {
    emit("status-change", localStatus.value);
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