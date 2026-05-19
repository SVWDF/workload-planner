import { reactive, computed } from "vue";
import http from "../api/http";
import { type LoginRequest, type RegisterRequest } from "../types/auth";

const state = reactive({
  loggedIn: false,
  initialized: false
});

const login = async (data: LoginRequest) => {
  try {
    await http.post("/auth/login", data);
    state.loggedIn = true;
    return { success: true, errors: [] as string[] };
  } 
  catch (err: unknown) {
    const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Login failed"];
    return { success: false, errors };
  }
}

const register = async (data: RegisterRequest) => {
  try {
    await http.post("/auth/register", data);
    state.loggedIn = true;
    return { success: true, errors: [] as string[] };
  } 
  catch (err: unknown) {
    const errors = (err as { customErrors?: string[]; }).customErrors ?? ["Registration failed"];
    return { success: false, errors };
  }
}

const logout = async () => {
  await http.post("/auth/logout");
  state.loggedIn = false;
}

const fetchCurrentUser = async () => {
  try {
    const response = await http.get("/auth/user");
    state.loggedIn = response.data.authenticated;
  }
  catch {
    clearAuth();
  }
  finally {
    state.initialized = true;
  }
}

const clearAuth = () => {
  state.loggedIn = false;
};

export function useAuth() {
  return {
    loggedIn: computed(() => state.loggedIn),
    initialized: computed(() => state.initialized),
    login,
    register,
    logout,
    fetchCurrentUser,
    clearAuth
  }
};