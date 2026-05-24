import { ref } from "vue";
import http from "../api/http";
import { type CreateScrumBoardRequest, type ScrumBoard, type ScrumBoardCreatedResponse } from '../types/scrumboard';

const cachedBoards = ref<ScrumBoard[]>([]);
let loaded = false;

const scrumboardSuccess = (data: ScrumBoardCreatedResponse) => ({
    success: true, errors: [], data
});

const scrumboardFail = (err: unknown, fallback: string) => ({
    success: false, errors: (err as { customErrors?: string[]}).customErrors ?? [fallback], data: null
});

const getBoards = async () => {
    if (loaded) return { data: cachedBoards.value };

    const response = await http.get("/scrumboards");
    cachedBoards.value = response.data;
    loaded = true;
    return response;
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
        createScrumBoard,
        cachedBoards
    }
}