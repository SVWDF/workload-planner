import { reactive, computed } from "vue";
import http from "../api/http";
import { type LoginRequest, type RegisterRequest } from "../types/auth";

const state = reactive({
  loggedIn: false,
  initialized: false
});

const authSuccess = () => ({
  success: true, errors: []
});

const authFail = (err: unknown, fallback: string) => ({
  success: false, errors: (err as { customErrors?: string[] }).customErrors ?? [fallback]
});

const login = async (data: LoginRequest) => {
  try {
    await http.post("/auth/login", data);
    state.loggedIn = true;
    return authSuccess();
  } 
  catch (err: unknown) {
    return authFail(err, "Login failed");
  }
}

const register = async (data: RegisterRequest) => {
  try {
    await http.post("/auth/register", data);
    state.loggedIn = true;
    return authSuccess();
  } 
  catch (err: unknown) {
    return authFail(err, "Registration failed");
  }
}

const logout = async () => {
  try {
    await http.post("/auth/logout");
  }
  finally {
    clearAuth();
  }
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
  state.initialized = false;
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