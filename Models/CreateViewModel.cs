using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;

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
        public ObjectId _id { get; set; }

        [Required]
        public string OrderName { get; set; }

        [Required]
        public string res_name{ get; set; }

        [Required]
        public string place{ get; set; }

        [Required]
        public string Comment{ get; set; }

        [Required]
        public bool IsGrab{ get; set; }


        public List<OrderItem> Items{ get; set; }





    }
}
