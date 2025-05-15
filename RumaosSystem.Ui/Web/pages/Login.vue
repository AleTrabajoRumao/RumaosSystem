<template>
  <section class="w-full h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 overflow-hidden">
    <div class="w-full max-w-md bg-white rounded-lg shadow dark:border dark:bg-gray-800 dark:border-gray-700">
      <div class="p-6 space-y-6">
        <h1 class="text-xl font-bold text-gray-900 md:text-2xl dark:text-white">
          Inicia Sesión en tu cuenta
        </h1>
        <form @submit.prevent="handleLogin" class="space-y-4">
          <div>
            <label for="nombre" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Nombre</label>
            <input
              type="text"
              v-model="user"
              required
              class="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-blue-600 focus:border-blue-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
              placeholder="Usuario"
            />
          </div>
          <div>
            <label for="contraseña" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Contraseña</label>
            <input
              type="password"
              v-model="password"
              required
              placeholder="••••••••"
              class="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-blue-600 focus:border-blue-600 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
            />
          </div>
          <div v-if="error" class="text-red-500 text-sm">{{ error }}</div>
          <button
            type="submit"
            class="w-full text-white bg-blue-600 hover:bg-blue-700 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 text-center dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            Iniciar Sesión
          </button>
        </form>
      </div>
    </div>
  </section>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import UsuarioService from '@/services/UsuarioService'

const user = ref('')
const password = ref('')
const error = ref('')
const router = useRouter()

onMounted(() => {
  // Verifica si ya hay un usuario guardado en localStorage
  if (localStorage.getItem('usuario')) {
    router.push('/AdminLayout') // Redirige si ya hay un usuario logueado
  }
})

const handleLogin = async () => {
  try {
    const data = await UsuarioService.login(user.value, password.value)
    localStorage.setItem('usuario', JSON.stringify(data))
    router.push('/AdminLayout') // Redirige a AdminLayout después de un login exitoso
  } catch (err) {
    error.value = err.message || 'Error al iniciar sesión'
  }
}
</script>

<style>
html,
body {
  padding: 0;
  margin: 0;
  overflow: hidden;
  height: 100%;
}
</style>
