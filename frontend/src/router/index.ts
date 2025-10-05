import { createRouter, createWebHistory } from 'vue-router'
import { useAuthStore } from "../store/auth"
import LoginPage from '../pages/LoginPage.vue'
import RegisterPage from '../pages/RegisterPage.vue'
import AppPage from '../pages/AppPage.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', redirect: "login" },
    { path: "/login", component: LoginPage },
    { path: "/register", component: RegisterPage },
    { path: "/app", component: AppPage, meta: { requireAuth: true } }
  ],
})

router.beforeEach((to, _, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    next("/login");
  } else {
    next();
  }
});

export default router
