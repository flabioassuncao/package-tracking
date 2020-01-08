using System;
using System.Collections.Generic;
using System.Text;

namespace CTT_PORTUGAL.Domain.Entities
{
    public class StatusOrder
    {
        public string Date { get; set; }
        public int Description { get; set; }
        public string Hour { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Local { get; set; }
        public string Receiver { get; set; }

        public Guid OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
