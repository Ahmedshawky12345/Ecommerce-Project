using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.Coupons
{
    public class applaycopounDTO
    {
        public int CouponId { get;set; }
        public List<int> ProductIds { get; set; }
    }
}
