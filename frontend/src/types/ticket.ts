export const TicketStatus = {
    Todo: 0,
    InProgress: 1,
    Done: 2
} as const;

export type TicketStatus =
    typeof TicketStatus[keyof typeof TicketStatus];

export const TicketPriority = {
    Low: 0,
    Medium: 1,
    High: 2
} as const;

export type TicketPriority =
    typeof TicketPriority[keyof typeof TicketPriority];

export interface Ticket {
    id: number;
    title: string;
    description: string;
    status: TicketStatus;
    priority: TicketPriority;
    assignedUser?: string;
}

export interface CreateTicketRequest {
    title: string;
    description: string;
    scrumboardId: number;
    priority: TicketPriority;
}

export interface UpdateTicketRequest {
    title: string;
    description: string;
    priority: TicketPriority;
}