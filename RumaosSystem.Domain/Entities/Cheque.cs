using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumaosSystem.Domain.Entities
{
    public class Cheque
    {
        public int Id { get; set; }

       
        public string Uen { get; set; } = null!;

        public decimal? Ptovtarec { get; set; }

        public decimal Nrorecibo { get; set; }

        public decimal? Mediopago { get; set; }

        public string? Banco { get; set; }

        public DateTime Fechavtosql { get; set; }

        public decimal? Nrocheque { get; set; }        

        public decimal? Importe { get; set; }

        [NotMapped]
        public DateTime FechaIngreso { get; set; }



    }
}

