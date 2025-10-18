<template>
  <AuthCard
  title="Workload Planner X"
  subtitle="Register here to start working in your new workspace"
  class="register-card">
    <form @submit.prevent="handleRegister" class="register-form">
      <input
        v-model="firstName"
        type="text"
        placeholder="First name"
        required
        class="auth-input"
      />
      <input 
        v-model="lastName"
        type="text"
        placeholder="Last name"
        required
        class="auth-input"
      />
      <input 
        v-model="username"
        type="text"
        placeholder="Username"
        required
        class="auth-input"
      />
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
      <button type="submit" class="auth-button">Register</button>
    </form>
    <div v-if="localErrors.length" class="error-box">
      <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
    </div>

    <template #footer class="login-text">
      Already have an account?
      <router-link to="/login" class="link">Login</router-link>
    </template>
  </AuthCard>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuth } from "../composables/auth";
import { useRouter } from "vue-router";
import AuthCard from "../components/AuthCard.vue";

const { register } = useAuth();
const router = useRouter();

const firstName = ref("");
const lastName = ref("");
const username = ref("");
const email = ref("");
const password = ref("");
const localErrors = ref<string[]>([]);

const handleRegister = async () => {
  const result = await register({
      firstName: firstName.value,
      lastName: lastName.value,
      username: username.value,
      email: email.value,
      password: password.value,
  });
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