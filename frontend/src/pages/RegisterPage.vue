<template>
  <div class="auth-container">
    <h2>Register</h2>
    <form @submit.prevent="handleRegister">
      <input v-model="firstName" placeholder="First name" required />
      <input v-model="lastName" placeholder="Last name" required />
      <input v-model="username" placeholder="Username" required />
      <input v-model="email" type="email" placeholder="Email" required />
      <input v-model="password" type="password" placeholder="Password" required />
      <button type="submit">Register</button>
    </form>
    <div v-if="errors.length">
      <p v-for="(e, i) in errors" :key="i" style="color:red">{{ e }}</p>
    </div>
    <p>Already have an account? <router-link to="/login">Login</router-link></p>
  </div>
</template>

<script setup lang="ts">
import { ref } from "vue";
import { useAuth } from "../composables/auth";
import { useRouter } from "vue-router";

const {register, loggedIn, errors } = useAuth();
const router = useRouter();

const firstName = ref("");
const lastName = ref("");
const username = ref("");
const email = ref("");
const password = ref("");

const handleRegister = async () => {
    await register({
        firstName: firstName.value,
        lastName: lastName.value,
        username: username.value,
        email: email.value,
        password: password.value,
    });
    if (loggedIn) router.push("/app");
  
};
</script>
