<template>
    <header>
        <RouterLink to="/dashboard" class="logo">Workload Planner X</RouterLink>
        <nav>
            <button @click="handleLogout">
              <span>Logout</span>
              <LogOut :size="26" class="logout-icon" />
            </button>
        </nav>
    </header>
</template>

<script setup lang="ts">
import { stopSignalR } from "@/services/signalr";
import { useAuth } from "../composables/auth";
import { useRouter } from "vue-router";
import { LogOut } from "lucide-vue-next";

const { logout } = useAuth();
const router = useRouter();

const handleLogout = async () => {
  try {
    await stopSignalR();
    await logout();
  }
  finally {
    router.push({ name: "Login" });
  }
};
</script>

<style scoped>
header {
    display: flex;
    align-items: center;
    justify-content: space-between;
    padding: 1.1rem 1.25rem;
    background: var(--bg-tertiary);
    box-shadow: var(--shadow-card);
}

.logo {
    font-size: clamp(1.5rem, 3.5vw, 1.75rem);
    font-weight: 700;
    color: var(--text-primary);
}

.logo:hover {
  text-decoration: none;
}

button {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.4rem;
  padding: 0.6rem 1.4rem;
}

.logout-icon {
  display: none;
}

@media (max-width: 36em) {
  button {
    padding: 0.5rem 0.75rem;
  }

  button > span {
    display: none;
  }

  .logout-icon {
    display: block;
  }
}
</style>