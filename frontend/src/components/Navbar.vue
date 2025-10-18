<template>
    <header>
        <span>Workload Planner X</span>
        <nav>
            <button @click="handleLogout">Logout</button>
        </nav>
    </header>
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

<style scoped>
header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 1.1rem 1.25rem;
    background: #2d2d2d;
    box-shadow: 0 0 25px rgba(0, 0, 0, 0.4);
}

span {
    font-size: 1.75rem;
    font-weight: 700;
}

button {
  background-color: #4f8cff;
  color: #fff;
  border: none;
  padding: 0.7rem 1.75rem;
  font-size: 1rem;
  border-radius: 8px;
  cursor: pointer;
  transition: background 0.25s ease, transform 0.1s ease;
}

button:hover {
  background-color: #3d79e6;
}

button:active {
  transform: scale(0.98);
}
</style>