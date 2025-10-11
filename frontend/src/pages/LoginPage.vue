<template>
  <div class="auth-container">
    <h2>Login</h2>
    <form @submit.prevent="handleLogin">
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Password" required />
      <button type="submit">Login</button>
    </form>
    <div v-if="errors.length">
      <p v-for="(e, i) in errors" :key="i" style="color:red">{{ e }}</p>
    </div>
    <p>Don't have an account? <router-link to="/register">Register</router-link></p>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useAuth } from "../composables/auth";

const { login, loggedIn, errors } = useAuth();
const router = useRouter();

const email = ref("");
const password = ref("");

const handleLogin = async () => {
  await login({ email: email.value, password: password.value });
  if (loggedIn.value) router.push("/app");
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
