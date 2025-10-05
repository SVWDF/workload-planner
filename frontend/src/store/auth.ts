import { defineStore } from "pinia";
import http from "../api/http";


export const useAuthStore = defineStore("auth", {
    state: () => ({
        loggedIn: false,
        user: { firstName: "", lastName: "", email: ""},
        errors: [] as string[]
    }),
    actions: {
        async login(data: { email: string; password: string }) {
            try {
                await http.post("/auth/login", data);
                this.loggedIn = true;
                this.errors = [];
            }
            catch (err: any) {
                this.errors = err.customErrors;
            }
            
        },
        async register(data: { firstName: string; lastName: string; username: string; email: string; password: string }) {
            try {
                await http.post("/auth/register", data);
                this.loggedIn = true;
                this.user = { firstName: data.firstName, lastName: data.lastName, email: data.email };
                this.errors = [];
            }
            catch (err: any) {
                this.errors = err.customErrors;
            }
            
        },
        async logout() {
            await http.post("/auth/logout");
            this.loggedIn = false;
            this.user = { firstName: "", lastName: "", email: ""};
        }
    }
});
