// /services/UsuarioService.js

export default {
    async login(user, password) {
        const response = await fetch('https://localhost:44379/api/Usuario/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            user,
            password,
          }),
        });
      
        if (!response.ok) {
          throw new Error('Credenciales incorrectas o error en el servidor');
        }
      
        const data = await response.json();
        return data;
      }
}
  