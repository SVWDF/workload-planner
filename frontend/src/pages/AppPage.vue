<template>
  <div class="app-container">
    <h2>Welcome to the App Page 🎉</h2>
    <button @click="handleLogout">Logout</button>
  </div>
</template>

<script setup lang="ts">
import { useAuth } from "../composables/auth";
import { useRouter } from "vue-router";

const { logout } = useAuth();
const router = useRouter();

const handleLogout = async () => {
  await logout();
  router.push("/login");
};
</script>
