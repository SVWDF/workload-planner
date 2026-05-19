import http from "../api/http";
import { type CreateScrumBoardRequest } from '../types/scrumboard';

const getBoards = async () => {
    return await http.get("/scrumboards");
};

const getBoard = async(id: number) => {
    return await http.get(`/scrumboards/${id}`);
};

const createScrumBoard = async (data: CreateScrumBoardRequest) => {
    try {
        const response = await http.post("/scrumboards", data);
        return { success: true, errors: [] as string[], data: response.data };
    }
    catch (err: unknown) {
        const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Scrumboard creation failed"];
        return { success: false, errors, data: null };
    }
};

const getBoardColors = async () => {
    return await http.get("/boardcolors");
};

export function useScrumBoards() {
    return {
        getBoards,
        getBoard,
        createScrumBoard,
        getBoardColors
    }
}