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
              <select v-model="chequeLocal.banco" id="banco" class="input w-70">
                <option v-for="banco in bancos" :key="banco.id" :value="banco.nombre" >
                  {{ banco.nombre }}
                </option>
              </select>
            </div>
            <div>
              <label for="nrocheque" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">N° Cheque</label>
              <input v-model.number="chequeLocal.nrocheque" id="nrocheque" type="number" class="input" />
            </div>
            <div class="sm:col-span-2">
              <label for="fechavto" class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Fecha Vto</label>
              <input v-model="fechaInput" id="fechavto" type="date" class="input" />
            </div>
          </div>

          <div class="flex justify-end space-x-2 mt-4">
            <button 
              type="submit" 
              class="bg-green-600 hover:bg-green-700 text-white font-medium rounded-lg text-sm px-4 py-2"
            >
              Guardar
            </button>
            <button 
              type="button" 
              @click="$emit('cerrar')" 
              class="bg-red-600 hover:bg-red-700 text-white font-medium rounded-lg text-sm px-4 py-2"
            >
              Cancelar
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue';
import bancoService from '@/services/BancoService'; // Asegúrate de importar correctamente el servicio

// Props para recibir el cheque a editar
const props = defineProps({
  chequeEditar: {
    type: Object,
    required: true
  }
});

// Emits para comunicarse con el componente padre
const emit = defineEmits(['guardar', 'cerrar']);

// Crear una copia local del cheque para no modificar directamente el prop
const chequeLocal = ref({
  id: 0,
  banco: '',
  nrocheque: 0,
  fechavtosql: ''
});

// Ref para manejar la fecha en formato yyyy-mm-dd (para el input date)
const fechaInput = ref('');

// Lista de bancos
const bancos = ref([]);

// Función para formatear la fecha en hora local (evita el desfase)
function formatDateLocal(date) {
  const year = date.getFullYear();
  const month = String(date.getMonth() + 1).padStart(2, '0'); // Mes empieza desde 0
  const day = String(date.getDate()).padStart(2, '0');
  return `${year}-${month}-${day}`;
}

// Inicializar los valores al montar el componente
onMounted(async () => {
  chequeLocal.value = {
    id: props.chequeEditar.id,
    banco: props.chequeEditar.banco || '',
    nrocheque: props.chequeEditar.nrocheque || 0,
    fechavtosql: props.chequeEditar.fechavtosql || ''
  };

  if (chequeLocal.value.fechavtosql) {
    fechaInput.value = chequeLocal.value.fechavtosql.slice(0, 10);
  }

  // Cargar los bancos desde el servicio
  bancos.value = await bancoService.getAllBancos();
});

// Función para guardar los cambios
function guardarCambios() {
  // Validación básica
  if (!chequeLocal.value.banco || chequeLocal.value.banco.trim() === '') {
    alert('El campo Banco es obligatorio.');
    return;
  }

  if (!chequeLocal.value.nrocheque || chequeLocal.value.nrocheque <= 0) {
    alert('El número de cheque debe ser un valor positivo.');
    return;
  }

  if (!fechaInput.value) {
    alert('La fecha de vencimiento es obligatoria.');
    return;
  }

  // Asignar la fecha del input como fechavtosql
  chequeLocal.value.fechavtosql = fechaInput.value;

  // Emitir evento 'guardar' con los datos actualizados
  emit('guardar', {
    id: chequeLocal.value.id,
    banco: chequeLocal.value.banco,
    nrocheque: chequeLocal.value.nrocheque,
    fechavtosql: chequeLocal.value.fechavtosql
  });
}
</script>

<style scoped>
</style>