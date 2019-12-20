using System;
using System.Collections.Generic;
using System.Text;

namespace CTT_PORTUGAL.Domain.Entities
{
    public class Order
    {
        public Order()
        {
            HistoricoStatus = new List<StatusOrder>();
        }

        public string CodigoRastreio { get; set; }

        public string De { get; set; }
        public string DeLocal { get; set; }
        public string DeData { get; set; }
        public string DeHora { get; set; }

        public string UltimoLocal { get; set; }
        public string UltimaData { get; set; }
        public string UltimaHora { get; set; }

        public string Para { get; set; }
        public string ParaLocal { get; set; }
        public string ParaData { get; set; }
        public string ParaHora { get; set; }

        public string UltimoAttCodigo { get; set; }
        public string UltimoAttProduto { get; set; }
        public string UltimoAttData { get; set; }
        public string UltimoAttHora { get; set; }
        public string UltimoAttEstado { get; set; }

        public virtual List<StatusOrder> HistoricoStatus { get; set; }

    }
}
