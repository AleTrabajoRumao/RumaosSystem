export default defineNuxtRouteMiddleware((to, from) => {
  if (process.client) {
    const usuario = localStorage.getItem('usuario')
    const rutasPublicas = ['/login']

    if (!usuario && !rutasPublicas.includes(to.path)) {
      return navigateTo('/login')
    }
  }
})
