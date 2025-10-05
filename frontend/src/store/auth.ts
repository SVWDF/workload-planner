import { defineStore } from "pinia";
import { loginUser, registerUser, type AuthResponse, type LoginRequest, type RegisterRequest } from "../api/auth";
import router from "../router";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    token: localStorage.getItem("token") || "",
  }),
  getters: {
    isAuthenticated: (state) => !!state.token,
  },
  actions: {
    async register(data: RegisterRequest) {
      const res: AuthResponse = await registerUser(data);
      this.token = res.token;
      localStorage.setItem("token", res.token);
      router.push("/app");
    },

    async login(data: LoginRequest) {
      const res: AuthResponse = await loginUser(data);
      this.token = res.token;
      localStorage.setItem("token", res.token);
      router.push("/app");
    },

    logout() {
      this.token = "";
      localStorage.removeItem("token");
      router.push("/login");
    },
  },
});
