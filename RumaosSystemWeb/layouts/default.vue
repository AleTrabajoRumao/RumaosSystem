<template>
  <div class="min-h-screen bg-gray-900 text-gray-900">
    <nav class="bg-[#2C3E50] text-white p-4 flex justify-between items-center">
      <h1 class="text-xl font-bold">Sistema Rumaos</h1>
      <div v-if="usuario">        
        <button
          class="bg-red-600 hover:bg-red-700 px-4 py-2 rounded"
          @click="cerrarSesion"
        >
          Cerrar sesión
        </button>
      </div>
    </nav>
    <main class="p-4">
      <slot />
    </main>
  </div>
</template>

<script setup>
import { useRouter, useRoute } from 'vue-router'
import { ref, watch, onMounted } from 'vue'

const router = useRouter()
const route = useRoute()
const usuario = ref(null)

const cargarUsuario = () => {
  if (typeof localStorage !== 'undefined') {
    const storedUser = localStorage.getItem('usuario')
    usuario.value = storedUser ? JSON.parse(storedUser) : null
  }
}

onMounted(() => {
  cargarUsuario()
})

// 👀 Reacciona cuando la ruta cambia
watch(route, () => {
  cargarUsuario()
})

const cerrarSesion = () => {
  localStorage.removeItem('usuario')
  cargarUsuario()
  router.push('/login')
}
</script>
