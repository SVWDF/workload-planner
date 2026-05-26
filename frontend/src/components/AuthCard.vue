<template>
    <div class="auth-card-container">
        <div class="auth-card">
            <h2>{{ title }}</h2>
            <p>{{ subtitle }}</p>
            <slot />
            <footer>
                <slot name="footer"></slot>
            </footer>
        </div>
    </div>
</template>

<script setup lang="ts">
defineProps<{
    title: string;
    subtitle: string;
}>();
</script>

<style scoped>
.auth-card-container {
    min-height: 100dvh;
    margin: 0 auto;
    padding: 0 2rem;
    display: grid;
    align-items: center;
}

.auth-card {
    background: var(--bg-tertiary);
    padding: 2rem;
    border-radius: var(--radius-card);
    box-shadow: var(--shadow-card);
    text-align: center;
}

p {
    font-size: 0.95rem;
    color: var(--text-secondary);
    margin-bottom: 1.5rem;
}

footer {
    margin: 24px 0 8px 0;
    display: flex;
    justify-content: center;
    align-items: center;
    gap: 0.35rem;
    flex-wrap: wrap;
    text-align: center;
}
</style>