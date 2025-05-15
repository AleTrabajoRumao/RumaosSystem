using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RumaosSystem.Application.Dtos
{
    public class ChequeDto
    {
        public int Id { get; set; }
        //public string BANCO { get; set; }
        //public int NROCHEQUE { get; set; }
        //public DateTime FECHAVTOSQL { get; set; }
        //public string UEN { get; set; }
        //public decimal NRORECIBO { get; set; }

        //public decimal PTOVTAREC { get; set; }
        public string Banco { get; set; }
        public int Nrocheque { get; set; }
        public DateTime Fechavtosql { get; set; }
        public string Uen { get; set; }
        public decimal Nrorecibo { get; set; }

        public decimal Ptovtarec { get; set; }




    }
}


