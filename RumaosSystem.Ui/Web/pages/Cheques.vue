<template>
  <div class="container mx-auto px-4 py-6">
    <!-- 🔍 Controles y botones de acción -->
    <div class="mb-4 flex flex-wrap items-center justify-between gap-2">
      <div class="flex items-center">
        <input
          v-model="search"
          type="text"
          placeholder="Buscar por UEN..."
          class="px-3 py-2 border border-gray-300 dark:border-gray-600 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 dark:bg-gray-700 dark:text-white"
        />
      </div>

      <div class="flex space-x-2">
        <button
          @click="insertarSeleccionados"
          :disabled="selectedCheques.length === 0"
          class="px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500 disabled:opacity-50 disabled:cursor-not-allowed"
        >
          Insertar Seleccionados ({{ selectedCheques.length }})
        </button>
      </div>
      
    </div>

    <!-- 🔄 Cargando -->
    <div v-if="loading" class="flex justify-center items-center py-12">
      <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-500"></div>
    </div>

    <!-- 🧾 Tabla -->
    <div v-else class="relative overflow-x-auto max-h-[700px]">
      <table class="w-full text-sm text-left text-gray-900 dark:text-gray-100">
        <thead class="text-xs uppercase bg-gray-100 dark:bg-gray-800 sticky top-0 z-10">
          <tr>
            <th class="p-4"></th>
            <th class="px-6 py-3">
              <div class="flex items-center cursor-pointer" @click="ordenarPorFecha">
                Fecha Ingreso
                <span class="ml-1">
                  <template v-if="sortConfig.key === 'fechaIngreso'">
                    <span v-if="sortConfig.direction === 'asc'">▲</span>
                    <span v-else>▼</span>
                  </template>
                  <span v-else>↕</span>
                </span>
              </div>
            </th>
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
        <tbody class="overflow-y-auto max-h-[500px]">
          <tr
            v-for="cheque in sortedAndFilteredCheques"
            :key="cheque.id"
            :class="calcularClaseFila(cheque.fechaIngreso) + ' border-b dark:border-gray-700'"
          >
            <td class="p-4">
              <input
                type="checkbox"
                :checked="selectedCheques.includes(cheque.id)"
                @change="toggleChequeSelection(cheque.id)"
                class="w-4 h-4 text-blue-600 bg-gray-100 dark:bg-gray-800 border-gray-300 rounded-sm focus:ring-blue-500"
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
                class="font-medium text-blue-600 dark:text-black hover:underline"
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


<script setup>
import { ref, onMounted, computed, watch } from 'vue';
import ChequesService from '@/services/ChequesService';
import Editar from '@/components/EditarCheque.vue';

const cheques = ref([]);
const selectedCheques = ref([]); // Guarda IDs
const search = ref('');
const chequeSeleccionado = ref(null);
const mostrarModal = ref(false);
const loading = ref(false);
const chequesLoaded = ref(false); // Flag para controlar si ya cargamos los cheques
const banco = ref([]);

// Configuración para el ordenamiento
const sortConfig = ref({
  key: 'fechaIngreso',
  direction: null // Por defecto, más recientes primero
});

// Función para ordenar por fecha
function ordenarPorFecha() {
  if (sortConfig.value.key === 'fechaIngreso') {
    // Si ya estamos ordenando por fecha, invertimos la dirección
    sortConfig.value.direction = sortConfig.value.direction === 'asc' ? 'desc' : 'asc';
  } else {
    // Si no estamos ordenando por fecha, establecemos la fecha como clave y descendente por defecto
    sortConfig.value.key = 'fechaIngreso';
    sortConfig.value.direction = 'desc';
  }
}

// Función para calcular la clase de la fila según la fecha
function calcularClaseFila(fechaIngreso) {
  const fecha = new Date(fechaIngreso);
  const hoy = new Date();
  const diferenciaDias = Math.floor((hoy - fecha) / (1000 * 60 * 60 * 24));

  if (diferenciaDias >= 4) return 'bg-red-400 text-black'; 
  if (diferenciaDias >= 2) return 'bg-yellow-200 text-black'; 
  if (diferenciaDias >= 1) return 'bg-green-500 text-black'; 
  return 'bg-green-500 text-black'; 
}

// Guardar IDs de seleccionados cuando cambie la selección
watch(selectedCheques, (newValues) => {
  ChequesService.guardarSeleccionados(newValues);
}, { deep: true });

// Al montar el componente
onMounted(async () => {
  // Cargar cheques y restaurar seleccionados
  await actualizarTabla();
  
  // Recuperar los IDs seleccionados del almacenamiento
  const idsSeleccionados = ChequesService.obtenerSeleccionados();
  
  // Validar que los IDs existan en la lista actual de cheques
  // Esto evita que aparezcan cheques que ya no existen
  selectedCheques.value = idsSeleccionados.filter(id => 
    cheques.value.some(cheque => cheque.id === id)
  );
  
  // Marcar que ya se cargaron los cheques
  chequesLoaded.value = true;
});

// Abrir modal de edición
function abrirEditor(cheque) {
  chequeSeleccionado.value = {...cheque}; // Copia para evitar referencia directa
  mostrarModal.value = true;
}

// Cerrar modal
function cerrarModal() {
  chequeSeleccionado.value = null;
  mostrarModal.value = false;
}

// Actualizar cheque después de editar
async function actualizarCheque(chequeActualizado) {
  try {
    loading.value = true;
    
    // Llamada a la API para actualizar el cheque
    await ChequesService.actualizarCheque(chequeActualizado.id, {
      banco: chequeActualizado.banco,
      nrocheque: chequeActualizado.nrocheque,
      fechavtosql: chequeActualizado.fechavtosql,
    });

    // Marcar como seleccionado (si no lo está)
    if (!selectedCheques.value.includes(chequeActualizado.id)) {
      selectedCheques.value.push(chequeActualizado.id);
    }

    // Actualizar en la lista local
    const index = cheques.value.findIndex(c => c.id === chequeActualizado.id);
    if (index !== -1) {
      // Crear un nuevo objeto para evitar problemas de referencia
      cheques.value[index] = {
        ...cheques.value[index],
        banco: chequeActualizado.banco,
        nrocheque: chequeActualizado.nrocheque,
        fechavtosql: chequeActualizado.fechavtosql,
      };
    }

    // Cerrar el modal
    cerrarModal();
    
    // Guardar el estado actual en sessionStorage
    ChequesService.guardarSeleccionados(selectedCheques.value);
    
  } catch (error) {
    console.error('Error actualizando cheque:', error);
    alert('Error al actualizar el cheque');
  } finally {
    loading.value = false;
  }
}

// Cargar datos de cheques desde la API
async function actualizarTabla() {
  loading.value = true;
  try {
    // Obtener los cheques disponibles
    const chequesData = await ChequesService.getChequesDisponibles();
    
    // Verificar que no haya duplicados por ID
    const idsUnicos = new Set();
    cheques.value = chequesData.filter(cheque => {
      if (!idsUnicos.has(cheque.id)) {
        idsUnicos.add(cheque.id);
        return true;
      }
      return false;
    });
    
  } catch (error) {
    console.error("Error al cargar cheques disponibles:", error);
    alert("Error al cargar la lista de cheques");
  } finally {
    loading.value = false;
  }
}

// Filtrar cheques según término de búsqueda
const filteredCheques = computed(() => {
  const term = search.value.trim().toLowerCase();
  if (!term) return cheques.value;

  return cheques.value.filter(c =>
    String(c.uen).toLowerCase().includes(term)
  );
});

// Cheques filtrados y ordenados
const sortedAndFilteredCheques = computed(() => {
  const filtered = filteredCheques.value;

  if (sortConfig.value.key && sortConfig.value.direction) {
    return [...filtered].sort((a, b) => {
      const dateA = new Date(a[sortConfig.value.key]);
      const dateB = new Date(b[sortConfig.value.key]);

      if (sortConfig.value.direction === 'asc') {
        return dateA - dateB;
      } else if (sortConfig.value.direction === 'desc') {
        return dateB - dateA;
      }
    });
  }

  // Si no hay orden, devolver los datos filtrados sin cambios
  return filtered;
});

// Alternar selección de cheque
function toggleChequeSelection(id) {
  const idx = selectedCheques.value.indexOf(id);
  if (idx === -1) {
    selectedCheques.value.push(id);
  } else {
    selectedCheques.value.splice(idx, 1);
  }
  
  // Guardar el estado actualizado
  ChequesService.guardarSeleccionados(selectedCheques.value);
}

// Insertar cheques seleccionados
async function insertarSeleccionados() {
  if (selectedCheques.value.length === 0) {
    alert("No hay cheques seleccionados para insertar");
    return;
  }
  
  try {
    loading.value = true;
    
    // Obtener los objetos de cheque completos para los IDs seleccionados
    const chequesAInsertar = cheques.value.filter(c =>
      selectedCheques.value.includes(c.id)
    );

    // Llamar al servicio para insertar
    await ChequesService.insertarSeleccionados(chequesAInsertar);

    // Eliminar los insertados de la lista local
    cheques.value = cheques.value.filter(c =>
      !selectedCheques.value.includes(c.id)
    );

    // Limpiar la selección
    selectedCheques.value = [];

    // Actualizar el estado guardado
    ChequesService.limpiarSeleccionados();
    
    alert("✅ Cheques insertados con éxito.");
  } catch (error) {
    console.error(error);
    alert("❌ Error al insertar cheques");
  } finally {
    loading.value = false;
  }
}
</script>