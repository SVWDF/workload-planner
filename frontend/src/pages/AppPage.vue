<template>
  <div class="app-container">
    <h2>Welcome to the App Page 🎉</h2>
    <button @click="logout">Logout</button>
  </div>
</template>

<script setup lang="ts">
import { useAuthStore } from "../store/auth";

const authStore = useAuthStore();

const logout = () => {
  authStore.logout();
};
</script>
