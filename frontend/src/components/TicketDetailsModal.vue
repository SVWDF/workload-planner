<template>
    <div v-if="open && ticket" class="modal-overlay">
        <div class="ticket-modal">
            <h2>Ticket Details</h2>
            <div class="form-group-details">
                <div class="form-group full-width">
                    <label for="ticket-detail-title">Title</label>
                    <input id="ticket-detail-title" v-model="form.title" :disabled="!isManager" />
                </div>
                <div class="form-group full-width">
                    <label for="ticket-detail-description">Description</label>
                    <textarea id="ticket-detail-description" v-model="form.description" :disabled="!isManager"></textarea>
                </div>
                <div class="form-group">
                    <label>Priority</label>
                    <select v-model="form.priority" :disabled="!isManager">
                        <option :value="TicketPriority.Low">Low</option>
                        <option :value="TicketPriority.Medium">Medium</option>
                        <option :value="TicketPriority.High">High</option>
                    </select>
                </div>
                <div class="form-group">
                    <label>Status</label>
                    <select v-model="localStatus" @change="handleStatusChange">
                        <option :value="TicketStatus.Todo">To Do</option>
                        <option :value="TicketStatus.InProgress">In Progress</option>
                        <option :value="TicketStatus.Done">Done</option>
                    </select>
                </div>
                <div class="form-group full-width assignment-card">
                    <label>Assigned To</label>
                    <div>
                        <p class="assigned-user">{{ ticket.assignedUser ?? "Nobody" }}</p>
                        <button @click="emit('assign')" :disabled="!!ticket.assignedUser">
                            <span class="assign-text">Assign to me</span>
                            <UserPlus :size="18" class="assign-icon "/>
                        </button>
                    </div>
                </div>
            </div>
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
import { UserPlus } from "lucide-vue-next";

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
    background: var(--bg-secondary);
    padding: 2rem;
}

h2 {
    margin-bottom: 1.5rem;
}

.form-group-details {
    display: grid;
    gap: 1.1rem;
}

.form-group {
    display: flex;
    flex-direction: column;
    gap: 0.4rem;
}

.assignment-card > div {
    display: flex;
    align-items: center;
    justify-content: space-between;
    gap: 1rem;
    margin-top: 0.5rem;
    padding: 0.7rem 0.9rem;
    border-radius: 14px;
    background-color: rgba(255, 255, 255, 0.04);
    border: 1px solid rgba(255, 255, 255, 0.05);
}

.assignment-card button {
    flex-shrink: 0;
    width: auto;
}

span.assign-text {
    display: none;
}

.assigned-user {
    color: var(--text-secondary);
    font-size: 0.95rem;
    font-weight: 500;
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
    width: auto;
    min-width: 160px;
}

.modal-actions {
    display: flex;
    flex-wrap: wrap;
    justify-content: flex-end;
    gap: 1rem;
    margin-top: 1.5rem;
}

.modal-actions button {
    flex: 1;
}

@media (min-width: 36em) {
    .form-group-details {
        grid-template-columns: repeat(2, 1fr);
    }

    .form-group.full-width {
        grid-column: span 2;
    }

    span.assign-text {
        display: block;
    }

    .assign-icon {
        display: none;
    }
}
</style>