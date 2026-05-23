<template>
  <div v-if="board" class="board-page">
    <div class="board-header">
      <div>
        <h1>{{ board.name }}</h1>
        <p>{{ board.members }} members • {{ tickets.length }} tickets</p>
      </div>
      <button v-if="board.isManager" @click="showCreateTicket = true" class="create-ticket-btn">+ New Ticket</button>
    </div>

    <div class="board-columns">

      <BoardColumn
        title="To Do"
        :tickets="todoTickets"
        @open-ticket="openTicket"
      />

      <BoardColumn
        title="In Progress"
        :tickets="inProgressTickets"
        @open-ticket="openTicket"
      />

      <BoardColumn
        title="Done"
        :tickets="doneTickets"
        @open-ticket="openTicket"
      />
    </div>
  </div>

  <div v-else class="loading-state">
    Loading...
  </div>

  <CreateTicketModal 
    :open="showCreateTicket"
    :errors="localErrors"
    @close="closeCreateModal"
    @create="handleCreateTicket"
  />

  <TicketDetailsModal 
    :open="showTicketModal"
    :ticket="selectedTicket"
    :is-manager="board?.isManager ?? false"
    :errors="localErrors"
    @close="closeTicketModal"
    @save="handleUpdateTicket"
    @delete="handleDeleteTicket"
    @assign="handleAssignSelfToTicket"
    @status-change="handleStatusChange"
  />
</template>

<script setup lang="ts">
import { computed, onMounted, onUnmounted, ref } from "vue";
import { useRoute, useRouter } from "vue-router";
import axios from "axios";
import { useScrumBoards } from '@/composables/scrumboard';
import { useTickets } from '@/composables/ticket';
import signalRConnection from "@/services/signalr";
import type { ScrumBoard } from "@/types/scrumboard";
import { TicketPriority, TicketStatus, type Ticket } from "@/types/ticket";
import BoardColumn from "@/components/BoardColumn.vue";
import CreateTicketModal from "@/components/CreateTicketModal.vue";
import TicketDetailsModal from "@/components/TicketDetailsModal.vue";

const route = useRoute();
const router = useRouter();
const { getBoard } = useScrumBoards();
const { getScrumboardTickets, createTicket, updateTicket, deleteTicket, assignSelfToTicket, updateStatus } = useTickets();
const slug = route.params.slug as string;
const boardId = Number(slug.split("-").pop());
const board = ref<ScrumBoard | null>(null);
const tickets = ref<Ticket[]>([]);
const todoTickets = computed(() => tickets.value.filter(t => t.status === TicketStatus.Todo));
const inProgressTickets = computed(() => tickets.value.filter(t => t.status === TicketStatus.InProgress));
const doneTickets = computed(() => tickets.value.filter(t => t.status === TicketStatus.Done));
const showCreateTicket = ref(false);
const localErrors = ref<string[]>([]);
const selectedTicket = ref<Ticket | null>(null);
const showTicketModal = ref(false);

const openTicket = (ticket: Ticket) => {
  selectedTicket.value = ticket;
  showTicketModal.value = true;
};

const closeCreateModal = () => {
  localErrors.value = [];
  showCreateTicket.value = false;
};

const closeTicketModal = () => {
  localErrors.value = [];
  selectedTicket.value = null;
  showTicketModal.value = false;
};

const updateTicketInList = (ticket: Ticket) => {
  const index = tickets.value.findIndex(t => t.id === ticket.id);
  if(index !== -1) tickets.value[index] = ticket;
};

//Handle methods UI interactions / back-end interactions
const handleCreateTicket = async (data: { title: string; description: string; }) => {
  const result = await createTicket({title: data.title, description: data.description, scrumboardId: boardId, priority: TicketPriority.Medium});
  
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  localErrors.value = [];
  showCreateTicket.value = false;
};

const handleUpdateTicket = async (data: { title: string; description: string}) => {
  if (!selectedTicket.value) return;

  const result = await updateTicket(selectedTicket.value.id, { title: data.title, description: data.description, priority: selectedTicket.value.priority });
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  closeTicketModal();
};

const handleDeleteTicket = async () => {
  if (!selectedTicket.value) return;

  const result = await deleteTicket(selectedTicket.value.id);
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }

  closeTicketModal();
};

//Handle methods SignalR real-time updates
const handleAssignSelfToTicket = async () => {
  if (!selectedTicket.value) return;

  const result = await assignSelfToTicket(selectedTicket.value.id);
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }
};

const handleStatusChange = async (status: TicketStatus) => {
  if (!selectedTicket.value) return;

  const result = await updateStatus(selectedTicket.value.id, { status });
  if (!result.success) {
    localErrors.value = result.errors;
    return;
  }
};

const handleTicketCreated = (createdTicket: Ticket) => {
  const exists = tickets.value.some(t => t.id === createdTicket.id);
  if (!exists) tickets.value.push(createdTicket);
};

const handleTicketUpdated = (updatedTicket: Ticket) => {
  updateTicketInList(updatedTicket);
  if (selectedTicket.value?.id === updatedTicket.id) {
    selectedTicket.value = updatedTicket;
  };
};

const handleTicketDeleted = (deletedId: number) => {
  tickets.value = tickets.value.filter(t => t.id !== deletedId);
  if (selectedTicket.value?.id === deletedId) {
    selectedTicket.value = null;
    showTicketModal.value = false;
  }
};

const handleTicketAssigned = (updatedTicket: Ticket) => {
  updateTicketInList(updatedTicket);
  if (selectedTicket.value?.id === updatedTicket.id) selectedTicket.value = updatedTicket;
};

const handleTicketStatusChanged = (updatedTicket: Ticket) => {
  updateTicketInList(updatedTicket);
  if (selectedTicket.value?.id === updatedTicket.id) {
    selectedTicket.value = updatedTicket;
  }
};

const signalREvents = [
  ["TicketAssigned", handleTicketAssigned],
  ["TicketStatusChanged", handleTicketStatusChanged],
  ["TicketCreated", handleTicketCreated],
  ["TicketUpdated", handleTicketUpdated],
  ["TicketDeleted", handleTicketDeleted]
] as const;

const registerSignalREvents = () => {
  signalREvents.forEach(
    ([event, handler]) => {
      signalRConnection.on(
        event,
        handler
      );
    }
  );
};

const unregisterSignalREvents = () => {
  signalREvents.forEach(
    ([event, handler]) => {
      signalRConnection.off(
        event,
        handler
      );
    }
  );
};

const loadBoard = async () => {
  const [boardResponse, ticketResponse ] = await Promise.all([getBoard(boardId), getScrumboardTickets(boardId)]);
  board.value = boardResponse.data;
  tickets.value = ticketResponse.data;
};



onMounted(async () => {
    if (isNaN(boardId)) {
        router.push("/dashboard");
        return;
    }

    try {
      await loadBoard();

      if (signalRConnection.state === "Disconnected") await signalRConnection.start();
      registerSignalREvents();
      await signalRConnection.invoke("JoinScrumboard", boardId);
    }
    catch (err: unknown) {
      if (axios.isAxiosError(err)) {
        if (err.response?.status === 404) {
          router.push("/dashboard");
        } 
      }
    }
});

onUnmounted(async () => {
  try {
    await signalRConnection.invoke("LeaveScrumboard", boardId);
  }
  catch {}
  unregisterSignalREvents();
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

.loading-state {
    padding: 3rem;
}

/* Modal overlay */
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