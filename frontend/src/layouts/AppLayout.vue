<template>
    <Navbar />
    <main>
      <RouterView />
    </main>
</template>

<script setup lang="ts">
import Navbar from '@/components/Navbar.vue';
</script>

<style scoped>
main {
    max-width: 1200px;
    margin: 50px auto 0;
    padding: 0.5rem 2rem;
}
</style>