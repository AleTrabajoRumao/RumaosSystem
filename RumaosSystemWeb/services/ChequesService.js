// ChequesService.js

const API_URL = 'https://localhost:44379/api/Cheque';

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


  async actualizarCheque(id, chequeData) {
    const response = await fetch(`https://localhost:44379/api/Cheque/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify(chequeData)
    })
  
    if (!response.ok) {
      throw new Error(`Error al actualizar cheque con ID ${id}`)
    }
  
    return await response.json()
  }
,  







  // Insertar cheques seleccionados en la tabla destino
  async insertarSeleccionados(chequesAInsertar) {
    try {
      const response = await fetch(`${API_URL}/insertar-destino`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(chequesAInsertar),
      });

      if (!response.ok) {
        throw new Error('Error al insertar los cheques');
      }

      return await response.json();
    } catch (error) {
      console.error('❌ Error en insertarSeleccionados:', error);
      throw error;
    }
  }
};
