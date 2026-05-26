<template>
  <AuthCard
  title="Workload Planner X"
  subtitle="Register to start managing your workload"
  class="register-card">
    <form @submit.prevent="handleRegister" class="register-form">
      <div class="name-row">
        <input
          v-model="form.firstName"
          type="text"
          placeholder="First name"
          required
          autocomplete="given-name"
        />
        <input 
          v-model="form.lastName"
          type="text"
          placeholder="Last name"
          required
          autocomplete="family-name"
        />
      </div>
      <input 
        v-model="form.username"
        type="text"
        placeholder="Username"
        required
        autocomplete="username"
      />
      <input
        v-model="form.email"
        type="email"
        placeholder="Email"
        required
        autocomplete="email"
      />
      <input
        v-model="form.password"
        type="password"
        placeholder="Password"
        required
        autocomplete="new-password"
      />
      <button type="submit" :disabled="loading">Register</button>
    </form>
    <div v-if="localErrors.length" class="error-box">
      <p v-for="(e, i) in localErrors" :key="i">{{ e }}</p>
    </div>

    <template #footer>
      <span>Already have an account?</span>
      <router-link to="/login" class="link">Login</router-link>
    </template>
  </AuthCard>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useAuth } from "@/composables/auth";
import AuthCard from "@/components/AuthCard.vue";
import type { RegisterRequest } from "@/types/auth";

const { register } = useAuth();
const router = useRouter();

const form = reactive<RegisterRequest>({
  firstName: "",
  lastName: "",
  username: "",
  email: "",
  password: ""
});
const loading = ref(false);
const localErrors = ref<string[]>([]);

const handleRegister = async () => {
  loading.value = true;
  try {
    const result = await register({
        firstName: form.firstName,
        lastName: form.lastName,
        username: form.username,
        email: form.email,
        password: form.password,
    });
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

<style scoped>
.register-card {
    max-width: 640px;
}

.register-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

.name-row {
  display: flex;
  gap: 1rem;
}

@media (max-width: 36em) {
  .name-row {
    flex-direction: column;
  }
}
</style>