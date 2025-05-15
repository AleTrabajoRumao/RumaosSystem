<script setup>

</script>

<template>
    <div class="flex h-screen bg-gray-100">
      <!-- Sidebar -->
      <aside class="w-64 bg-white border-r shadow-md">
        <div class="p-4 text-2xl font-bold text-blue-700">
          Rumaos Admin
        </div>
        <nav class="mt-4">
          <ul class="space-y-2 px-4">
            <li>
              <NuxtLink to="/cheques" class="flex items-center p-2 rounded hover:bg-blue-100">
                <svg class="w-5 h-5 mr-2 text-blue-600" fill="none" stroke="currentColor" stroke-width="2"
                     viewBox="0 0 24 24">
                  <path stroke-linecap="round" stroke-linejoin="round"
                        d="M9 17v-2a4 4 0 014-4h5M5 15V5a2 2 0 012-2h5l5 5v2"/>
                </svg>
                Cheques
              </NuxtLink>
            </li>
            
            <!-- Podés seguir agregando más módulos acá -->
          </ul>
        </nav>
      </aside>
  
      <!-- Main Content -->
      <main class="flex-1 overflow-y-auto p-6">
        <slot />
      </main>
    </div>
  </template>
  
