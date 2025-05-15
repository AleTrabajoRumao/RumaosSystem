using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumaosSystem.Domain.Entities
{
    public class RecVenta
    {
        public int Id { get; set; }

        public string? Observaciones { get; set; }

        //public string? RecvtaGuid { get; set; }

        //public string? RecvtaSite { get; set; }

        public string Uen { get; set; } = null!;

        public DateTime? Fechasql { get; set; }

        public decimal? Ptovta { get; set; }

        public decimal Nrorecibo { get; set; }

        public decimal? Nrocliente { get; set; }

        public decimal? Mediopago { get; set; }

        public decimal? Importe { get; set; }

        public decimal? Nroreca { get; set; }

        public string? Condventa { get; set; }

        public string? Usuario { get; set; }

        public int? Iddespacho { get; set; }
    }
}
