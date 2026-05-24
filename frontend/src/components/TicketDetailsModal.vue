<template>
    <div v-if="open && ticket" class="modal-overlay">
        <div class="ticket-modal">
            <h2>Ticket Details</h2>
            <input v-model="form.title" :disabled="!isManager" />
            <textarea v-model="form.description" :disabled="!isManager"></textarea>
            <select v-model="form.priority" :disabled="!isManager">
                <option :value="TicketPriority.Low">Low</option>
                <option :value="TicketPriority.Medium">Medium</option>
                <option :value="TicketPriority.High">High</option>
            </select>
            <p>Assigned: {{ ticket.assignedUser ?? "Nobody" }}</p>
            <button @click="emit('assign')" :disabled="!!ticket.assignedUser">Assign to me</button>
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
import { reactive, ref, watch } from "vue";
import { TicketPriority, TicketStatus, type Ticket, type UpdateTicketRequest } from "@/types/ticket";

const props = defineProps<{
    open: boolean;
    ticket: Ticket | null;
    isManager: boolean;
    errors: string[];
}>();

const emit = defineEmits<{
    (e: "close"): void;
    (e: "save", data: UpdateTicketRequest): void;
    (e: "delete"): void;
    (e: "assign"): void;
    (e: "status-change", status: TicketStatus): void;
}>();

const form = reactive<UpdateTicketRequest>({
    title: "",
    description: "",
    priority: TicketPriority.Medium
});
const localStatus = ref<TicketStatus>(TicketStatus.Todo);

watch(() => props.ticket,
    ticket => {
        if (!ticket) return;
        form.title = ticket.title;
        form.description = ticket.description;
        form.priority = ticket.priority;
        localStatus.value = ticket.status;
    },
    { immediate: true }
);

watch(() => props.open, isOpen => {
    if (!isOpen) {
      form.title = "";
      form.description = "";
      form.priority = TicketPriority.Medium;
      localStatus.value = TicketStatus.Todo;
    }
  }
);

const handleSave = () => {
    emit("save", { ...form });
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