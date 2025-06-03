using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FastUserTransactionAPI.Models
{
    public class TransactionDTO
    {
        public int TransactionId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
