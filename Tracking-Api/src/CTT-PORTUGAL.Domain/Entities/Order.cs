using System.Collections.Generic;

namespace CTT_PORTUGAL.Domain.Entities
{
    public class Order
    {
        public Order()
        {
            StatusHistory = new List<StatusOrder>();
        }

        public string CodeTracking { get; set; }

        public string From { get; set; }
        public string FromLocal { get; set; }
        public string FromDate { get; set; }
        public string FromHour { get; set; }

        public string LastLocal { get; set; }
        public string LastDate { get; set; }
        public string LastHour { get; set; }

        public string To { get; set; }
        public string ToLocal { get; set; }
        public string ToDate { get; set; }
        public string ToHour { get; set; }

        public string LastCodeUpdate { get; set; }
        public string LastProductUpdate { get; set; }
        public string LastDateUpdate { get; set; }
        public string LastHourUpdate { get; set; }
        public string LastStatusUpdate { get; set; }

        public virtual ICollection<StatusOrder> StatusHistory { get; set; }

    }
}
