// // ChequesService.js

// const API_URL = 'https://localhost:44379/api/Cheque';

// export default {
//   // Obtener todos los cheques
//   async getCheques() {
//     try {
//       const response = await fetch(API_URL);
//       if (!response.ok) {
//         throw new Error(`Error al obtener cheques: ${response.status}`);
//       }
//       return await response.json();
//     } catch (error) {
//       console.error('❌ Error en getCheques:', error);
//       throw error;
//     }
//   },

//   // Obtener cheques disponibles
//   async getChequesDisponibles() {
//     try {
//       const response = await fetch(`${API_URL}/disponibles`);
//       if (!response.ok) {
//         throw new Error(`Error al obtener cheques disponibles: ${response.status}`);
//       }
//       return await response.json();
//     } catch (error) {
//       console.error('❌ Error en getChequesDisponibles:', error);
//       throw error;
//     }
//   },

//   // Obtener un cheque por ID
//   async getChequeById(id) {
//     try {
//       const response = await fetch(`${API_URL}/${id}`);
//       if (!response.ok) {
//         throw new Error(`Error al obtener cheque: ${response.status}`);
//       }
//       return await response.json();
//     } catch (error) {
//       console.error('❌ Error en getChequeById:', error);
//       throw error;
//     }
//   },


//   async actualizarCheque(id, chequeData) {
//     const response = await fetch(`https://localhost:44379/api/Cheque/${id}`, {
//       method: 'PUT',
//       headers: {
//         'Content-Type': 'application/json'
//       },
//       body: JSON.stringify(chequeData)
//     })
  
//     if (!response.ok) {
//       throw new Error(`Error al actualizar cheque con ID ${id}`)
//     }
  
//     return await response.json()
//   }
// ,  







//   // Insertar cheques seleccionados en la tabla destino
//   async insertarSeleccionados(chequesAInsertar) {
//     try {
//       const response = await fetch(`${API_URL}/insertar`, {
//         method: 'POST',
//         headers: {
//           'Content-Type': 'application/json',
//         },
//         body: JSON.stringify(chequesAInsertar),
//       });

//       if (!response.ok) {
//         throw new Error('Error al insertar los cheques');
//       }

//       return await response.json();
//     } catch (error) {
//       console.error('❌ Error en insertarSeleccionados:', error);
//       throw error;
//     }
//   }
// };





// ChequesService.js

const API_URL = 'https://localhost:44379/api/Cheque';

// Clave para almacenamiento de sesión
const STORAGE_KEY = 'cheques_seleccionados';

export default {
  // Obtener todos los cheques
  async getCheques() {
    try {
      const response = await fetch(API_URL);
      if (!response.ok) {
        throw new Error(`Error al obtener cheques: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('❌ Error en getCheques:', error);
      throw error;
    }
  },

  // Obtener cheques disponibles
  async getChequesDisponibles() {
    try {
      const response = await fetch(`${API_URL}/disponibles`);
      if (!response.ok) {
        throw new Error(`Error al obtener cheques disponibles: ${response.status}`);
      }
      
      // Devolvemos la respuesta directamente sin manipulación
      return await response.json();
    } catch (error) {
      console.error('❌ Error en getChequesDisponibles:', error);
      throw error;
    }
  },

  // Obtener un cheque por ID
  async getChequeById(id) {
    try {
      const response = await fetch(`${API_URL}/${id}`);
      if (!response.ok) {
        throw new Error(`Error al obtener cheque: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('❌ Error en getChequeById:', error);
      throw error;
    }
  },

  // Actualizar un cheque existente
  async actualizarCheque(id, chequeData) {
    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify(chequeData)
      });
    
      if (!response.ok) {
        throw new Error(`Error al actualizar cheque con ID ${id}`);
      }
    
      return await response.json();
    } catch (error) {
      console.error('❌ Error en actualizarCheque:', error);
      throw error;
    }
  },




  // Insertar cheques seleccionados en la tabla destino
  async insertarSeleccionados(chequesAInsertar) {
    try {
      const response = await fetch(`${API_URL}/insertar`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(chequesAInsertar),
      });

      if (!response.ok) {
        throw new Error('Error al insertar los cheques');
      }
      
      // Limpiamos la lista de cheques seleccionados en sessionStorage
      this.limpiarSeleccionados();
      
      return await response.json();
    } catch (error) {
      console.error('❌ Error en insertarSeleccionados:', error);
      throw error;
    }
  },
  
  // Guardar IDs de cheques seleccionados en sessionStorage
  guardarSeleccionados(idsSeleccionados) {
    try {
      sessionStorage.setItem(STORAGE_KEY, JSON.stringify(idsSeleccionados));
    } catch (error) {
      console.error('❌ Error al guardar seleccionados:', error);
    }
  },
  
  // Recuperar IDs de cheques seleccionados de sessionStorage
  obtenerSeleccionados() {
    try {
      const seleccionados = sessionStorage.getItem(STORAGE_KEY);
      return seleccionados ? JSON.parse(seleccionados) : [];
    } catch (error) {
      console.error('❌ Error al obtener seleccionados:', error);
      return [];
    }
  },
  
  // Limpiar la lista de cheques seleccionados en sessionStorage
  limpiarSeleccionados() {
    try {
      sessionStorage.removeItem(STORAGE_KEY);
    } catch (error) {
      console.error('❌ Error al limpiar seleccionados:', error);
    }
  },
  
  // Validar si un ID de cheque ya existe en la lista
  existeIdEnLista(lista, idBuscar) {
    return lista.some(cheque => cheque.id === idBuscar);
  }
};