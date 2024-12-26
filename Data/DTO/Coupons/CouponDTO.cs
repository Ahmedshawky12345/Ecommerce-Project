using Data.CustomDataAnotaion;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.DTO.Coupons
{
    public class CouponDTO
    {

        //[Required(ErrorMessage = "Coupon code is required.")]
        //[StringLength(20, ErrorMessage = "Coupon code can't exceed 20 characters.")]
        //public string Code { get; set; }

        [Range(1, 100, ErrorMessage = "Discount must be between 1% and 100%.")]
        public int DiscountPercentage { get; set; }
        [Required(ErrorMessage = "Expiry date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [CustomExpiryDate(ErrorMessage = "Expiry date must be in the future")]
        public DateTime ExpiryDate { get; set; }
    }
    public class showcoupondata
    {
        public int CouponId { get; set; }
        public string Code { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime ExpiryDate { get; set; }

    }
    public class updatecouponDTO
    {
        [Required(ErrorMessage = "please enter coponId")]
        public int CouponId { get; set; }
        [Range(1, 100, ErrorMessage = "Discount must be between 1% and 100%.")]
        public int DiscountPercentage { get; set; }
        [Required(ErrorMessage = "Expiry date is required")]
        [DataType(DataType.DateTime, ErrorMessage = "Invalid date format")]
        [CustomExpiryDate(ErrorMessage = "Expiry date must be in the future")]
        public DateTime ExpiryDate { get; set; }

    }
}
