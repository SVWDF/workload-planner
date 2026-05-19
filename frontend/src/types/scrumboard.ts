export interface ScrumBoard {
    id: number;
    name: string;
    color: string;
    members: number;
    tickets: number;
    isManager?: boolean;
}

export interface CreateScrumBoardRequest {
    name: string;
    color: string;
    memberIds: string[];
}