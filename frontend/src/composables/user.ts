import http from "../api/http";

const searchUsers = async(query: string) => {
    return await http.get(`/users/search?query=${query}`);
};

export function useUsers() {
    return {
        searchUsers
    }
}