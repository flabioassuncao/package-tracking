using System;
using System.Collections.Generic;
using System.Text;

namespace CTT_PORTUGAL.Domain.Entities
{
    public class StatusOrder
    {
        public string Data { get; set; }
        public int Descricao { get; set; }
        public string UltimoAttHora { get; set; }
        public string UltimoAttEstado { get; set; }
        public string UltimoAttMotivo { get; set; }
        public string UltimoAttLocal { get; set; }
        public string UltimoAttRecetor { get; set; }
    }
}
