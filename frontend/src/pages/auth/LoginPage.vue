<template>
  <AuthCard 
  title="Workload Planner X"
  subtitle="Log in to access your workspace"
  class="login-card">
    <form @submit.prevent="handleLogin" class="login-form">
        <input
          v-model="form.email"
          type="email"
          placeholder="Email"
          required
          autocomplete="email"
          class="auth-input"
        />
        <input
          v-model="form.password"
          type="password"
          placeholder="Password"
          required
          autocomplete="current-password"
          class="auth-input"
        />
        <button type="submit" class="auth-button" :disabled="loading">{{ loading ? "Logging in..." : "Login" }}</button>
      </form>
      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>

      <template #footer>
        <div class="login-footer">
          <span>Don't have an account?</span>
          <router-link to="/register" class="link">Register</router-link>
        </div>
      </template>
  </AuthCard>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useAuth } from "@/composables/auth";
import AuthCard from "@/components/AuthCard.vue";
import type { LoginRequest } from "@/types/auth";

const { login } = useAuth();
const router = useRouter();
const loading = ref(false);

const form = reactive<LoginRequest>({
  email: "",
  password: ""
});
const localErrors = ref<string[]>([]);

const handleLogin = async () => {
  loading.value = true;

  try {
    const result = await login({ email: form.email, password: form.password });
  
    if (result.success) {
      localErrors.value = [];
      router.push("/dashboard");
    }
    else {
      localErrors.value = result.errors;
    }
  }
  finally {
    loading.value = false;
  }
};
</script>

<style src="@/assets/auth.css"></style>