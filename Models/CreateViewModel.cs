using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace SU_API.Models
{
    public class OrderItem
    {
        [Required]
        public string Dish { get; set; }

        [Required]
        public int Quantity { get; set; }
    }

    public class CreateViewModel
    {

        [Required]
        public string Order_name{ get; set; }

        [Required]
        public string Res_name{ get; set; }

        [Required]
        public string Place{ get; set; }

        [Required]
        public string Comment{ get; set; }

        [Required]
        public bool Isgrab { get; set; }


        public List<OrderItem> Items { get; set; }





    }
}
