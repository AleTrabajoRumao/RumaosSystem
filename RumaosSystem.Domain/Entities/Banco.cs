using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumaosSystem.Domain.Entities
{
    public class Banco
    {
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Contacto { get; set; }

        public string Telefono { get; set; }

        public bool? Activo { get; set; }

        public string Direccion { get; set; }

        public string Observaciones { get; set; }



    }
}
