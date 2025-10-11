import { reactive, computed } from "vue";
import http from "../api/http";
import { type LoginRequest, type RegisterRequest } from "../types/auth";

const state = reactive({
  loggedIn: false,
  errors: [] as string[]
});

async function login(data: LoginRequest) {
  try {
    await http.post("/auth/login", data);
    state.loggedIn = true;
    state.errors = [];
  } 
  catch (err: any) {
    state.errors = err.customErrors || ["Login failed"];
  }
}

async function register(data: RegisterRequest) {
  try {
    await http.post("/auth/register", data);
    state.loggedIn = true;
    state.errors = [];
  } 
  catch (err: any) {
    state.errors = err.customErrors || ["Registration failed"];
  }
}

async function logout() {
  await http.post("/auth/logout");
  state.loggedIn = false;
  state.errors = [];
}

async function fetchCurrentUser() {
  try {
    await http.get("/auth/user");
    state.loggedIn = true;
  }
  catch (err: any) {
    state.loggedIn = false;
  }
}

export function useAuth() {
  return {
    loggedIn: computed(() => state.loggedIn),
    errors: computed(() => state.errors),
    login,
    register,
    logout,
    fetchCurrentUser
  }
};