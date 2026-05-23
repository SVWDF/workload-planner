import http from "../api/http";
import { type CreateScrumBoardRequest, type ScrumBoardCreatedResponse } from '../types/scrumboard';

const scrumboardSuccess = (data: ScrumBoardCreatedResponse) => ({
    success: true, errors: [], data
});

const scrumboardFail = (err: unknown, fallback: string) => ({
    success: false, errors: (err as { customErrors?: string[]}).customErrors ?? [fallback], data: null
});

const getBoards = async () => {
    return await http.get("/scrumboards");
};

const getBoard = async(id: number) => {
    return await http.get(`/scrumboards/${id}`);
};

const createScrumBoard = async (data: CreateScrumBoardRequest) => {
    try {
        const response = await http.post<ScrumBoardCreatedResponse>("/scrumboards", data);
        return scrumboardSuccess(response.data);
    }
    catch (err: unknown) {
        return scrumboardFail(err, "Scrumboard creation failed");
    }
};

export function useScrumBoards() {
    return {
        getBoards,
        getBoard,
        createScrumBoard
    }
}