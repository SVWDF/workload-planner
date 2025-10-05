<template>
  <div class="auth-container">
    <h2>Login</h2>
    <form @submit.prevent="handleLogin">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Password" required />
      <button type="submit">Login</button>
    </form>
    <div v-if="authStore.errors.length">
      <p v-for="(e, i) in authStore.errors" :key="i" style="color:red">{{ e }}</p>
    </div>
    <p>Don't have an account? <router-link to="/register">Register</router-link></p>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuthStore } from "../store/auth";
import { useRouter } from "vue-router";

const authStore = useAuthStore();
const router = useRouter();
const email = ref("");
const password = ref("");

const handleLogin = async () => {
    await authStore.login({ email: email.value, password: password.value });
    if (authStore.loggedIn) router.push("/app")
};
</script>

<style scoped>
.auth-container {
  max-width: 400px;
  margin: auto;
  padding: 2rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
}
</style>
