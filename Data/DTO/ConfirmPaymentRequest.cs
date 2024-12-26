using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO
{
    public class ConfirmPaymentRequest
    {
        public int OrderId { get; set; }
        public string PaymentIntentId { get; set; }
    }

}
