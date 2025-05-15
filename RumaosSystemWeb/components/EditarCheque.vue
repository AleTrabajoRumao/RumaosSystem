<template>
  <div class="fixed inset-0 z-50 flex items-center justify-center bg-black bg-opacity-50">
    <div class="relative p-4 w-full max-w-2xl">
      <div class="relative p-4 bg-white rounded-lg shadow dark:bg-gray-800 sm:p-5">
        <!-- Header -->
        <div class="flex justify-between items-center pb-4 mb-4 border-b dark:border-gray-600">
          <h3 class="text-lg font-semibold text-gray-900 dark:text-white">Editar Cheque</h3>
          <button
            @click="$emit('cerrar')"
            type="button"
            class="text-gray-400 bg-transparent hover:bg-gray-200 hover:text-gray-900 rounded-lg text-sm p-1.5"
          >
            <svg class="w-5 h-5" fill="currentColor" viewBox="0 0 20 20">
              <path
                fill-rule="evenodd"
                d="M4.293 4.293a1 1 0 011.414 0L10 8.586l4.293-4.293a1 1 0 111.414 1.414L11.414 10l4.293 4.293a1 1 0 01-1.414 1.414L10 11.414l-4.293 4.293a1 1 0 01-1.414-1.414L8.586 10 4.293 5.707a1 1 0 010-1.414z"
                clip-rule="evenodd"
              />
            </svg>
          </button>
        </div>

        <!-- Formulario -->
        <form @submit.prevent="guardarCambios">
          <div class="grid gap-4 mb-4 sm:grid-cols-2">
            <div>
              <label for="banco" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Banco</label>
              <input v-model="editado.banco" id="banco" type="text" class="input" />
            </div>
            <div>
              <label for="nrocheque" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">N° Cheque</label>
              <input v-model.number="editado.nrocheque" id="nrocheque" type="number" class="input" />
            </div>
            <div class="sm:col-span-2">
              <label for="fechavto" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Fecha Vto</label>
              <input v-model="editado.fechavto" id="fechavto" type="date" class="input" />
            </div>
          </div>

          <div class="flex justify-end space-x-2 mt-4">
            <button type="submit" class="bg-green-600 hover:bg-green-700 text-white font-medium rounded-lg text-sm px-4 py-2">
              Guardar
            </button>
            <button type="button" @click="$emit('cerrar')" class="bg-red-600 hover:bg-red-700 text-white font-medium rounded-lg text-sm px-4 py-2">
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch } from 'vue'

const props = defineProps({
  chequeEditar: Object
})
const emit = defineEmits(['guardar', 'cerrar'])

const editado = reactive({
  banco: '',
  nrocheque: null,
  fechavto: ''
})

watch(
  () => props.chequeEditar,
  (nuevo) => {
    if (nuevo) {
      editado.banco = nuevo.banco || ''
      editado.nrocheque = nuevo.nrocheque || null
      editado.fechavto = nuevo.fechavtosql ? nuevo.fechavtosql.slice(0, 10) : ''
    }
  },
  { immediate: true }
)

function guardarCambios() {
  if (!editado.banco || !editado.nrocheque || !editado.fechavto) {
    alert("Todos los campos son obligatorios.");
    return;
  }

  emit('guardar', {
    ...props.chequeEditar,
    banco: editado.banco,
    nrocheque: editado.nrocheque,
    fechavtosql: editado.fechavto // formato yyyy-mm-dd
  });
}

</script>

<style scoped>

</style>
