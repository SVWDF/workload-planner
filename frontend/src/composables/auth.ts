import { reactive, computed } from "vue";
import http from "../api/http";
import { type LoginRequest, type RegisterRequest } from "../types/auth";

const state = reactive({
  loggedIn: false,
});

const login = async (data: LoginRequest) => {
  try {
    await http.post("/auth/login", data);
    state.loggedIn = true;
    return { success: true, errors: [] as string[] };
  } 
  catch (err: any) {
    const errors = err.customErrors || ["Login failed"];
    return { success: false, errors };
  }
}

const register = async (data: RegisterRequest) => {
  try {
    await http.post("/auth/register", data);
    state.loggedIn = true;
    return { success: true, errors: [] as string[] };
  } 
  catch (err: any) {
    const errors = err.customErrors || ["Registration failed"];
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
  catch (err: any) {
    state.loggedIn = false;
  }
}

export function useAuth() {
  return {
    loggedIn: computed(() => state.loggedIn),
    login,
    register,
    logout,
    fetchCurrentUser,
  }
};