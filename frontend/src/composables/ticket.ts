import http from "@/api/http";
import type { CreateTicketRequest, TicketStatus, UpdateTicketRequest } from "@/types/ticket";

const ticketSuccess = (data: unknown = null) => ({
    success: true, errors: [], data
});

const ticketFail = (err: unknown, fallback: string) => ({
    success: false, errors: (err as { customErrors?: string[] }).customErrors ?? [fallback], data: null
});

const getScrumboardTickets = async (scrumboardId: number) => {
    return await http.get(`/tickets/scrumboard/${scrumboardId}`);
};

const createTicket = async (data: CreateTicketRequest) => {
    try {
        const response = await http.post("/tickets", data);
        return ticketSuccess(response.data);
    }
    catch (err: unknown) {
        return ticketFail(err, "Failed to create ticket");
    }
};

const updateTicket = async (id: number, data: UpdateTicketRequest) => {
    try {
        const response = await http.put(`/tickets/${id}`, data);
        return ticketSuccess(response.data);
    }
    catch (err: unknown) {
        return ticketFail(err, "Failed to update ticket");
    }
};

const deleteTicket = async (id: number) => {
    try {
        await http.delete(`/tickets/${id}`);
        return ticketSuccess();
    }
    catch (err: unknown) {
        return ticketFail(err, "Failed to delete ticket");
    }
};

const assignSelfToTicket = async (id: number) => {
    try {
        const response = await http.patch(`/tickets/${id}/assign`);
        return ticketSuccess(response.data);
    }
    catch (err: unknown) {
        return ticketFail(err, "Failed to assign user to ticket");
    }
};

const updateStatus = async (id: number, status: TicketStatus) => {
    try {
        const response = await http.patch(`/tickets/${id}/status`, { status });
        return ticketSuccess(response.data)
    }
    catch (err: unknown) {
        return ticketFail(err, "Failed to update status of ticket")
    }
};

export function useTickets() {
    return {
        getScrumboardTickets,
        createTicket,
        updateTicket,
        deleteTicket,
        assignSelfToTicket,
        updateStatus
    }
};