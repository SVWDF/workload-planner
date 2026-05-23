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
        { path: "", redirect: "/dashboard" },
        { path: "dashboard",  component: DashboardPage, name: "Dashboard" },
        { path: "boards/create", component: CreateBoardPage, name: "CreateBoard" },
        { path: "boards/:slug", component: BoardPage, name: "Board" }
    ] }
  ],
});

router.beforeEach(async to => {
    const { loggedIn, initialized, fetchCurrentUser } = useAuth();

    if (!initialized.value) await fetchCurrentUser();
    if (to.meta.requiresAuth === true && !loggedIn.value) return "/login"; 
    if (to.meta.requiresAuth === false && loggedIn.value) return "/dashboard";
  }
);

export default router
