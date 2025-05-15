<script setup>
import { ref, onMounted, computed } from 'vue';
import ChequesService from '@/services/ChequesService';
import Editar from '@/components/EditarCheque.vue';

const cheques = ref([]);
const selectedCheques = ref([]); // Guarda IDs
const search = ref('');
const chequeSeleccionado = ref(null);
const mostrarModal = ref(false);
const loading = ref(false);


onMounted(async () => {
  await actualizarTabla();
});

function abrirEditor(cheque) {
  chequeSeleccionado.value = cheque;
  mostrarModal.value = true;
}

function cerrarModal() {
  chequeSeleccionado.value = null;
  mostrarModal.value = false;
}

async function actualizarCheque(chequeActualizado) {
  try {
    await ChequesService.actualizarCheque(chequeActualizado.id, {
      banco: chequeActualizado.banco,
      nrocheque: chequeActualizado.nrocheque,
      fechavtosql: chequeActualizado.fechavtosql,
    });

    // ✅ Marcar como seleccionado (si no lo está)
    if (!selectedCheques.value.includes(chequeActualizado.id)) {
      selectedCheques.value.push(chequeActualizado.id);
    }

    // 🔝 Mover al principio del array
    const index = cheques.value.findIndex(c => c.id === chequeActualizado.id);
    if (index !== -1) {
      const cheque = cheques.value.splice(index, 1)[0];
      cheques.value.unshift(cheque);
    }

    cerrarModal();
  } catch (error) {
    console.error('Error actualizando cheque:', error);
  }
}

async function actualizarTabla() {
  loading.value = true;
  try {
    cheques.value = await ChequesService.getChequesDisponibles();
  } catch (error) {
    console.error("Error al cargar cheques disponibles:", error);
  } finally {
    loading.value = false;
  }
}

const filteredCheques = computed(() => {
  const term = search.value.trim();
  if (!term) return cheques.value;

  return cheques.value.filter(c =>
    String(c.nrocheque).toLowerCase().includes(term.toLowerCase())
  );
});

function toggleChequeSelection(id) {
  const idx = selectedCheques.value.indexOf(id);
  if (idx === -1) {
    selectedCheques.value.push(id);
  } else {
    selectedCheques.value.splice(idx, 1);
  }
}

async function insertarSeleccionados() {
  try {
    const chequesAInsertar = cheques.value.filter(c =>
      selectedCheques.value.includes(c.id)
    ).map(c => ({
      fechaIngreso: new Date(c.fechaIngreso).toISOString().split('T')[0], // Aquí se formatea
      uen: String(c.uen),
      banco: String(c.banco),
      nroCheque: parseInt(c.nrocheque),
      importe: parseFloat(c.importe),
      fechaVto: new Date(c.fechavtosql).toISOString().split('T')[0],
      ptovta: parseInt(c.ptovtarec),
      nroRecibo: parseInt(c.nrorecibo)
    }));

    await ChequesService.insertarSeleccionados(chequesAInsertar);

    alert("✅ Cheques insertados con éxito.");
    selectedCheques.value = [];
    await actualizarTabla();
  } catch (error) {
    console.error(error);
    alert("❌ Error al insertar cheques.");
  }

  function claseAlertaFila(fechaIngreso) {
  if (!fechaIngreso) return ''; // Evita errores si no hay fecha

  const hoy = new Date();
  const ingreso = new Date(fechaIngreso);
  if (isNaN(ingreso)) return ''; // Evita errores si la fecha no es válida

  const diffDias = Math.floor((hoy - ingreso) / (1000 * 60 * 60 * 24));

  if (diffDias >= 4) return 'bg-red-100 dark:bg-red-800';
  if (diffDias >= 2) return 'bg-yellow-100 dark:bg-yellow-800';
  return 'bg-green-100 dark:bg-green-800';
}







}
</script>

<template>
  <div class="pb-4 bg-white dark:bg-gray-900 px-4 sm:px-6 lg:px-8">
    <!-- 🔍 Search y botones -->
    <div class="flex flex-wrap gap-4 items-center mb-4">
      <div class="relative flex-1 min-w-[200px]">
        <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg class="w-4 h-4 text-gray-500 dark:text-gray-400" fill="none" viewBox="0 0 20 20">
            <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2"
              d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
          </svg>
        </div>
        <input
          v-model="search"
          type="text"
          placeholder="Buscar por número de cheque..."
          class="block w-full ps-10 pt-2 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:text-white"
        />
      </div>

      <button
        @click="insertarSeleccionados"
        class="text-white bg-green-600 hover:bg-green-700 font-medium rounded-lg text-sm px-4 py-2"
      >
        Insertar cheques
      </button>

      <button
        @click="actualizarTabla"
        class="text-white bg-blue-600 hover:bg-blue-700 font-medium rounded-lg text-sm px-4 py-2"
      >
        Actualizar
      </button>
    </div>

    <!-- ⏳ Loader -->
    <div v-if="loading" class="flex justify-center items-center py-10">
      <div class="animate-spin rounded-full h-10 w-10 border-t-4 border-blue-500 border-solid"></div>
    </div>

    <!-- 🧾 Tabla de cheques -->
    <div v-else class="overflow-x-auto flex flex-col max-h-[500px]">
      <table class="w-full text-sm text-left text-gray-500 dark:text-gray-400 min-w-[800px]">
        <thead class="text-xs text-gray-700 uppercase bg-gray-50 dark:bg-gray-700 dark:text-gray-400">
          <tr>
            <th class="p-4"></th>
            <th class="px-6 py-3">Fecha Ingreso</th>
            <th class="px-6 py-3">UEN</th>
            <th class="px-6 py-3">Banco</th>
            <th class="px-6 py-3">Nro de cheque</th>
            <th class="px-6 py-3">Importe</th>
            <th class="px-6 py-3">Fecha Vencimiento</th>
            <th class="px-6 py-3">Punto Venta</th>
            <th class="px-6 py-3">Número de recibo</th>
            <th class="px-6 py-3">Acción</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="cheque in filteredCheques"
            :key="cheque.id"
            
            class="bg-white border-b dark:bg-gray-800 dark:border-gray-700 hover:bg-gray-50 dark:hover:bg-gray-600"
          >
            <td class="p-4">
              <input
                type="checkbox"
                :checked="selectedCheques.includes(cheque.id)"
                @change="toggleChequeSelection(cheque.id)"
                class="w-4 h-4 text-blue-600 bg-gray-100 border-gray-300 rounded-sm focus:ring-blue-500 dark:focus:ring-blue-600"
              />
            </td>
            
            <td class="px-6 py-4">{{ new Date(cheque.fechaIngreso).toLocaleDateString() }}</td>
            <td class="px-6 py-4">{{ cheque.uen }}</td>
            <td class="px-6 py-4">{{ cheque.banco }}</td>
            <td class="px-6 py-4">{{ cheque.nrocheque }}</td>
            <td class="px-6 py-4">${{ cheque.importe.toFixed(2) }}</td>
            <td class="px-6 py-4">{{ new Date(cheque.fechavtosql).toLocaleDateString() }}</td>
            <td class="px-6 py-4">{{ cheque.ptovtarec }}</td>
            <td class="px-6 py-4">{{ cheque.nrorecibo }}</td>
            <td class="px-6 py-4">
              <a
                href="#"
                class="font-medium text-blue-600 dark:text-blue-500 hover:underline"
                @click.prevent="abrirEditor(cheque)"
              >
                Editar
              </a>
            </td>
          </tr>
        </tbody>
      </table>
    </div>

    <!-- 🛠 Modal de edición -->
    <Editar
      v-if="mostrarModal && chequeSeleccionado"
      :chequeEditar="chequeSeleccionado"
      @guardar="actualizarCheque"
      @cerrar="cerrarModal"
    />
  </div>
</template>
