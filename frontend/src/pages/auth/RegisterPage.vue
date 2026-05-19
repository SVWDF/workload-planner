<template>
  <AuthCard
  title="Workload Planner X"
  subtitle="Register here to start working in your new workspace"
  class="register-card">
    <form @submit.prevent="handleRegister" class="register-form">
      <input
        v-model="form.firstName"
        type="text"
        placeholder="First name"
        required
        class="auth-input"
      />
      <input 
        v-model="form.lastName"
        type="text"
        placeholder="Last name"
        required
        class="auth-input"
      />
      <input 
        v-model="form.username"
        type="text"
        placeholder="Username"
        required
        class="auth-input"
      />
      <input
        v-model="form.email"
        type="email"
        placeholder="Email"
        required
        class="auth-input"
      />
      <input
        v-model="form.password"
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

    <template #footer>
      <div class="register-footer">
        <span>Already have an account?</span>
        <router-link to="/login" class="link">Login</router-link>
      </div>
    </template>
  </AuthCard>
</template>

<script setup lang="ts">
import { ref, reactive } from "vue";
import { useRouter } from "vue-router";
import { useAuth } from "@/composables/auth";
import AuthCard from "@/components/AuthCard.vue";

const { register } = useAuth();
const router = useRouter();

const form = reactive({
  firstName: "",
  lastName: "",
  username: "",
  email: "",
  password: ""
});
const localErrors = ref<string[]>([]);

const handleRegister = async () => {
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
};
</script>

<style src="@/assets/auth.css"></style>