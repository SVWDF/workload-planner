<template>
  <AuthCard 
  title="Workload Planner X"
  subtitle="Log in to access your workspace">
    <form @submit.prevent="handleLogin" class="login-form">
        <input
          v-model="email"
          type="email"
          placeholder="Email"
          required
          class="auth-input"
        />
        <input
          v-model="password"
          type="password"
          placeholder="Password"
          required
          class="auth-input"
        />
        <button type="submit" class="auth-button">Login</button>
      </form>
      <div v-if="localErrors.length" class="error-box">
        <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
      </div>

      <template #footer class="register-text">
        Don't have an account?
        <router-link to="/register" class="link">Register</router-link>
      </template>
  </AuthCard>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuth } from "../composables/auth";
import AuthCard from "../components/AuthCard.vue";

const { login } = useAuth();
const router = useRouter();

const email = ref("");
const password = ref("");
const localErrors = ref<string[]>([]);

const handleLogin = async () => {
  const result = await login({ email: email.value, password: password.value });

  if (result.success) {
    localErrors.value = result.errors;
    router.push("/app");
  }
  else {
    localErrors.value = result.errors;
  }
};
</script>

<style src="../assets/auth.css"></style>