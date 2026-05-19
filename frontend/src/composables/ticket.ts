import http from "@/api/http";
import type { CreateTicketRequest, UpdateTicketRequest } from "@/types/ticket";


const getScrumboardTickets = async (scrumboardId: number) => {
    return await http.get(`/tickets/scrumboard/${scrumboardId}`);
};

const createTicket = async (data: CreateTicketRequest) => {
    try {
        const response = await http.post("/tickets", data);
        return { success: true, errors: [] as string[], data: response.data };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Failed to create ticket"];
        return { success: false, errors, data: null };
    }
};

const updateTicket = async (id: number, data: UpdateTicketRequest) => {
    try {
        const response = await http.put(`/tickets/${id}`, data);
        return { success: true, errors: [] as string[], data: response.data };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Failed to update ticket"];
        return { success: false, errors, data: null };
    }
};

const deleteTicket = async (id: number) => {
    try {
        await http.delete(`/tickets/${id}`);
        return { success: true, errors: [] as string[] };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Failed to delete the ticket"];
        return { success: false, errors, data: null };
    }
};

const assignSelfToTicket = async (id: number) => {
    try {
        const response = await http.patch(`/tickets/${id}/assign`);
        return { success: true, errors: [] as string[], data: response.data };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Failed to assign to the ticket"];
        return { success: false, errors, data: null };
    }
};

const updateStatus = async (id: number, status: number) => {
    try {
        const response = await http.patch(`/tickets/${id}/status`, { status });
        return { success: true, errors: [] as string[], data: response.data };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Failed to update the status of the ticket"];
        return { success: false, errors, data: null };
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