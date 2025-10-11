import { createRouter, createWebHistory } from 'vue-router'
import { useAuth } from "../composables/auth"
import LoginPage from '../pages/LoginPage.vue'
import RegisterPage from '../pages/RegisterPage.vue'
import AppPage from '../pages/AppPage.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: "login" },
    { path: "/login", component: LoginPage, name: "Login", meta: { requiresAuth: false } },
    { path: "/register", component: RegisterPage, name: "Register", meta: { requiresAuth: false } },
    { path: "/app", component: AppPage, name: "App", meta: { requiresAuth: true } }
  ],
});

router.beforeEach(async (to, _, next) => {
    const { loggedIn, fetchCurrentUser } = useAuth();

    await fetchCurrentUser();

    if (to.meta.requiresAuth && !loggedIn.value) {
        next("/login");
    } 
    else if (!to.meta.requiresAuth && loggedIn.value) {
        next("/app");
    }
    else {
        next();
    }
});

export default router
