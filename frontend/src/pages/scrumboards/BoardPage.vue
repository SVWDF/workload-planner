<template>
  <div v-if="board" class="board-page">
    <div class="board-header">
      <div>
        <h1>{{ board.name }}</h1>
        <p>{{ board.members }} members • {{ board.tickets }} tickets</p>
      </div>
      <button v-if="board.isManager" @click="showCreateTicket = true" class="create-ticket-btn">+ New Ticket</button>
    </div>

    <div class="board-columns">

      <div class="board-column">
        <div class="column-header">
          <h3>To Do</h3>
          <span>{{ todoTickets.length }}</span>
        </div>
        <div v-for="ticket in todoTickets" :key="ticket.id" @click="openTicket(ticket)" class="ticket-card">
          <h4>{{ ticket.title }}</h4>
          <p>{{ ticket.description }}</p>
        </div>
      </div>

      <div class="board-column">
        <div class="column-header">
          <h3>In Progress</h3>
          <span>{{ inProgressTickets.length }}</span>
        </div>

        <div v-for="ticket in inProgressTickets" :key="ticket.id" @click="openTicket(ticket)" class="ticket-card">
          <h4>{{ ticket.title }}</h4>
          <p>{{ ticket.description }}</p>
        </div>
      </div>

      <div class="board-column">
        <div class="column-header">
          <h3>Done</h3>
          <span>{{ doneTickets.length }}</span>
        </div>

        <div v-for="ticket in doneTickets" :key="ticket.id" @click="openTicket(ticket)" class="ticket-card">
          <h4>{{ ticket.title }}</h4>
          <p>{{ ticket.description }}</p>
        </div>
      </div>
    </div>
  </div>

  <div v-else class="loading-state">
    Loading...
  </div>

  <div v-if="showCreateTicket" class="modal-overlay">
    <div class="ticket-modal">
      <h2>Create Ticket</h2>
      <input v-model="title" placeholder="Title"/>
      <textarea v-model="description" placeholder="Description"/>
      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>

      <div class="modal-actions">
        <button @click="showCreateTicket = false">Cancel</button>
        <button @click="handleCreateTicket">Create</button>
      </div>
    </div>
  </div>

  <div v-if="showTicketModal" class="modal-overlay">
    <div class="ticket-modal">
      <h2>Ticket Details</h2>
      <input v-model="editTitle" :disabled="!board?.isManager" />
      <textarea v-model="editDescription" :disabled="!board?.isManager"></textarea>
      <p>Assigned: {{ selectedTicket?.assignedUser ?? "Nobody" }}</p>
      <button @click="handleAssignSelfToTicket">Assign to me</button>
      <select v-model="selectedStatus" @change="handleStatusChange">
        <option :value="0">
          To Do
        </option>
        <option :value="1">
          In Progress
        </option>
        <option :value="2">
          Done
        </option>
      </select>
      <div class="modal-actions">
        <button @click="showTicketModal = false">Close</button>
        <template v-if="board?.isManager">
          <button @click="handleUpdateTicket">Save</button>
          <button @click="handleDeleteTicket">Delete</button>
        </template>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed, onMounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import { useScrumBoards } from '@/composables/scrumboard';
import type { ScrumBoard } from "@/types/scrumboard";
import axios from "axios";
import { useTickets } from '@/composables/ticket';
import type { Ticket } from "@/types/ticket";
import signalRConnection from "@/services/signalr";

const route = useRoute();
const router = useRouter();
const { getBoard } = useScrumBoards();
const { getScrumboardTickets, createTicket, updateTicket, deleteTicket, assignSelfToTicket, updateStatus } = useTickets();
const slug = route.params.slug as string;
const boardId = Number(slug.split("-").pop());
const board = ref<ScrumBoard | null>(null);
const tickets = ref<Ticket[]>([]);
const todoTickets = computed(() => tickets.value.filter(t => t.status === 0));
const inProgressTickets = computed(() => tickets.value.filter(t => t.status === 1));
const doneTickets = computed(() => tickets.value.filter(t => t.status === 2));
const showCreateTicket = ref(false);
const title = ref("");
const description = ref("");
const localErrors = ref<string[]>([]);
const selectedTicket = ref<Ticket | null>(null);
const showTicketModal = ref(false);
const editTitle = ref("");
const editDescription = ref("");
const selectedStatus = ref(0);

const openTicket = (ticket: Ticket) => {
  selectedTicket.value = ticket;
  editTitle.value = ticket.title;
  editDescription.value = ticket.description;
  selectedStatus.value = ticket.status;
  showTicketModal.value = true;
};

const handleCreateTicket = async () => {
  const result = await createTicket({title: title.value, description: description.value, scrumboardId: boardId, priority: 1});
  
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  tickets.value.push(result.data);
  title.value = "";
  description.value = "";
  localErrors.value = [];
  showCreateTicket.value = false;
};

const handleUpdateTicket = async () => {
  if (!selectedTicket.value) return;

  const result = await updateTicket(selectedTicket.value.id, { title: editTitle.value, description: editDescription.value, priority: selectedTicket.value.priority });
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  const index = tickets.value.findIndex(t => t.id === result.data.id);
  tickets.value[index] = result.data;

  showTicketModal.value = false;
};

const handleDeleteTicket = async () => {
  if (!selectedTicket.value) return;

  const result = await deleteTicket(selectedTicket.value.id);
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }
  
  tickets.value = tickets.value
    .filter(t=> t.id !== selectedTicket.value?.id);

  showTicketModal.value = false;
};

const handleAssignSelfToTicket = async () => {
  if (!selectedTicket.value) return;

  const result = await assignSelfToTicket(selectedTicket.value.id);
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  const index = tickets.value
    .findIndex(t => t.id === result.data.id);

  tickets.value[index] = result.data;
  selectedTicket.value = result.data;
};

const handleStatusChange = async () => {
  if (!selectedTicket.value) return;

  const result = await updateStatus(selectedTicket.value.id, selectedStatus.value);
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  const index = tickets.value
    .findIndex(t => t.id === result.data.id);

  tickets.value[index] = result.data;
  selectedTicket.value = result.data;
};

onMounted(async () => {
    if (isNaN(boardId)) {
        router.push("/dashboard");
        return;
    }

    try {
      const [boardResponse, ticketResponse ] = await Promise.all([getBoard(boardId), getScrumboardTickets(boardId)]);
      board.value = boardResponse.data;
      tickets.value = ticketResponse.data;

      await signalRConnection.start();
      await signalRConnection.invoke("JoinScrumboard", boardId);

      signalRConnection.on("TicketAssigned", (updatedTicket: Ticket) => {
        const index = tickets.value
          .findIndex(t => t.id === updatedTicket.id);

        if (index !== -1) tickets.value[index] = updatedTicket;
        if (selectedTicket.value?.id === updatedTicket.id) selectedTicket.value = updatedTicket;
      }) 
    }
    catch (err: unknown) {
      if (axios.isAxiosError(err)) {
        if (err.response?.status === 404) {
          router.push("/dashboard");
        } 
      }
    }
});
</script>

<style scoped>
.board-page {
    padding: 2rem;
    height: 100%;
}

.board-header {
    display: flex;
    justify-content: space-between;
    align-items: center;

    margin-bottom: 2rem;
}

.board-header h1 {
    font-size: 2rem;
    margin-bottom: 0.3rem;
}

.board-header p {
    color: #a1a1aa;
}

.create-ticket-btn {
    padding: 0.8rem 1.2rem;
    border: none;
    border-radius: 12px;

    cursor: pointer;
}

/* Columns */

.board-columns {
    display: flex;
    gap: 1.5rem;

    overflow-x: auto;
}

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

    padding:
        0.25rem 0.65rem;

    border-radius: 999px;
}

/* Tickets */

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
    transform:
        translateY(-3px);

    background: #343434;
}

.ticket-card h4 {
    margin-bottom: 0.5rem;
}

.ticket-card p {
    color: #b0b0b0;
    font-size: 0.95rem;
}

.loading-state {
    padding: 3rem;
}

/* Modal */
.modal-overlay {
    position: fixed;
    inset: 0;

    display: flex;
    justify-content: center;
    align-items: center;

    background:
        rgba(0,0,0,0.5);
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