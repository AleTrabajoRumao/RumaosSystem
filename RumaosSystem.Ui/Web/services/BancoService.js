const API_URL = 'https://localhost:44379/api/Banco';

export default {
  // Obtener todos los bancos
  async getAllBancos() {
    try {
      const response = await fetch(API_URL);
      if (!response.ok) {
        throw new Error(`Error al obtener bancos: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('❌ Error en getBancos:', error);
      throw error;
    }
  },

  // Obtener un banco por ID
  async getBancoById(id) {
    try {
      const response = await fetch(`${API_URL}/${id}`);
      if (!response.ok) {
        throw new Error(`Error al obtener banco: ${response.status}`);
      }
      return await response.json();
    } catch (error) {
      console.error('❌ Error en getBancoById:', error);
      throw error;
    }
  },

  // Actualizar un banco por ID
  async actualizarBanco(id, bancoData) {
    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(bancoData),
      });

      if (!response.ok) {
        throw new Error(`Error al actualizar banco con ID ${id}`);
      }

      return await response.json();
    } catch (error) {
      console.error('❌ Error en actualizarBanco:', error);
      throw error;
    }
  },

  // Insertar un nuevo banco
  async insertarBanco(bancoData) {
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(bancoData),
      });

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.error || 'Error desconocido al insertar banco');
      }

      return await response.json();
    } catch (error) {
      console.error('❌ Error en insertarBanco:', error);
      throw error;
    }
  },

  // Eliminar un banco por ID
  async eliminarBanco(id) {
    try {
      const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        throw new Error(`Error al eliminar banco con ID ${id}`);
      }

      return true; // Indica que la operación fue exitosa
    } catch (error) {
      console.error('❌ Error en eliminarBanco:', error);
      throw error;
    }
  },
};