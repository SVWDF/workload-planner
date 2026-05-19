import { createRouter, createWebHistory } from 'vue-router'
import { useAuth } from "@/composables/auth"
import LoginPage from '@/pages/auth/LoginPage.vue'
import RegisterPage from '@/pages/auth/RegisterPage.vue'
import DashboardPage from '@/pages/scrumboards/DashboardPage.vue'
import CreateBoardPage from '@/pages/scrumboards/CreateBoardPage.vue'
import BoardPage from '@/pages/scrumboards/BoardPage.vue'
import AppLayout from '@/layouts/AppLayout.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: "/login", component: LoginPage, name: "Login", meta: { requiresAuth: false } },
    { path: "/register", component: RegisterPage, name: "Register", meta: { requiresAuth: false } },
    { path: "/", component: AppLayout, meta: { requiresAuth: true },
    children: [
        { path: "dashboard",  component: DashboardPage, name: "Dashboard" },
        { path: "boards/create", component: CreateBoardPage, name: "Create board" },
        { path: "boards/:slug", component: BoardPage, name: "Board" }
    ] }
  ],
});

router.beforeEach(async (to, _, next) => {
    const { loggedIn, initialized, fetchCurrentUser } = useAuth();

    if (!initialized.value) await fetchCurrentUser();

    if (to.meta.requiresAuth && !loggedIn.value) {
        next("/login");
    } 
    else if (!to.meta.requiresAuth && loggedIn.value) {
        next("/dashboard");
    }
    else {
        next();
    }
});

export default router
