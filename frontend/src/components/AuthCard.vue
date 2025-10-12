<template>
    <div class="auth-card">
        <h2 class="title">{{ title }}</h2>
        <p class="subtitle">{{ subtitle }}</p>
        <slot></slot>
        <p class="auth-footer">
            <slot name="footer"></slot>
        </p>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    title: string,
    subtitle: string
}>();
</script>

<style scoped>
.auth-card {
    background: #2d2d2d;
    padding: 2rem 4rem;
    border-radius: 16px;
    box-shadow: 0 0 25px rgba(0, 0, 0, 0.4);
    color: #f0f0f0;
    text-align: center;
}

@media (max-width: 768px) {
    .auth-card {
        padding: 2rem;
    }
}

.title {
    font-size: 1.8rem;
    font-weight: 600;
    margin-bottom: 0.5rem;
    color: #ffffff;
}

.subtitle {
    font-size: 0.95rem;
    color: #aaaaaa;
    margin-bottom: 1.5rem;
}
</style>