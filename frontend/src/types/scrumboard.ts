export interface ScrumBoard {
    id: number;
    name: string;
    color: string;
    members: number;
    tickets: number;
}

export interface CreateScrumBoardRequest {
    name: string;
    color: string;
    memberIds: string[];
}

export interface ScrumBoardCreatedResponse {
    id: number;
    name: string;
}

export interface ScrumBoardDetail extends ScrumBoard {
    isManager: boolean;
}