using Microsoft.AspNetCore.Mvc;
using SU_API.Models;
using MongoDB.Driver;


namespace API_SU.Controllers
{

    [ApiController]
    [Route("/api/")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<CreateViewModel> _orderCollection;

        public OrderController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0.v59caxa.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("su");
            _orderCollection = database.GetCollection<CreateViewModel>("Ordersu");
        }


        [HttpPost]
        [Route("create")]
        public ActionResult CreateOrder([FromBody] CreateViewModel orderModel)
        {
            if (orderModel == null)
            {
                return BadRequest("Invalid order data. Request body is missing or not in the correct format.");
            }

            if (string.IsNullOrWhiteSpace(orderModel.OrderName) || string.IsNullOrWhiteSpace(orderModel.res_name)
                || string.IsNullOrWhiteSpace(orderModel.place) || string.IsNullOrWhiteSpace(orderModel.Comment)
                || orderModel.Items == null || orderModel.Items.Count == 0)
            {
                return BadRequest("Invalid order data. Make sure all required fields are provided.");
            }

            string orderName = orderModel.OrderName;
            string resName = orderModel.res_name;
            string place = orderModel.place;
            string comment = orderModel.Comment;
            bool isGrab = orderModel.IsGrab;
            List<OrderItem> items = orderModel.Items;

            string message = $"Order '{orderName}' created successfully with the following items:\n";

            foreach (var item in items)
            {
                message += $"Item: {item.Dish}, Quantity: {item.Quantity}\n";
            }

            try
            {
                var document = new CreateViewModel
                {
                    OrderName = orderName,
                    res_name = resName,
                    place = place,
                    Comment = comment,
                    IsGrab = false,
                    Items = items
                };
                _orderCollection.InsertOne(document);
                return StatusCode(201,"Create successful");
            }
            catch (Exception ex)
            {
                return BadRequest($"Connection to MongoDB failed: {ex.Message}");
            }

        }


        [HttpGet("getOrders")]
        public ActionResult<IEnumerable<CreateViewModel>> GetOrders()
        {
            try
            {
                var orders = _orderCollection.Find(_ => true).ToList();
                var serializedOrders = orders.Select(order => new
                {
                    Id = order._id.ToString(),
                    OrderName = order.OrderName,
                    res_name = order.res_name,
                    place = order.place,
                    comment = order.Comment,
                    isgrab = order.IsGrab,
                    items = order.Items
                }).ToList();

                return Ok(serializedOrders);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to fetch orders from MongoDB: {ex.Message}");
            }
        }

        [HttpGet("getOrder/{orderId}")]
        public ActionResult<CreateViewModel> GetOrder(string orderId)
        {
            try
            {
                var order = _orderCollection.Find(o => o._id.ToString() == orderId).FirstOrDefault();

                if (order == null)
                {
                    return NotFound();
                }

                return Ok(order);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to fetch the order from MongoDB: {ex.Message}");

            }
        }

        [HttpPut("isGrabTrue/{orderId}")]
        public ActionResult UpdateTrueOrder(string orderId, [FromBody] CreateViewModel updatedOrder)
        {
            try
            {
                var existingOrder = _orderCollection.Find(o => o._id.ToString() == orderId).FirstOrDefault();

                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.IsGrab = true;


                _orderCollection.ReplaceOne(o => o._id.ToString() == orderId, existingOrder);

                return Ok(existingOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update the order in MongoDB: {ex.Message}");
            }
        }

        [HttpPut("isGrabFalse/{orderId}")]
        public ActionResult UpdateFalseOrder(string orderId, [FromBody] CreateViewModel updatedOrder)
        {
            try
            {
                var existingOrder = _orderCollection.Find(o => o._id.ToString() == orderId).FirstOrDefault();

                if (existingOrder == null)
                {
                    return NotFound();
                }

                existingOrder.IsGrab = true;


                _orderCollection.ReplaceOne(o => o._id.ToString() == orderId, existingOrder);

                return Ok(existingOrder);
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update the order in MongoDB: {ex.Message}");
            }
        }

        [HttpDelete("deleteOrder/{orderId}")]
        public ActionResult DeleteOrder(string orderId)
        {
            try
            {
                var existingOrder = _orderCollection.Find(o => o._id.ToString() == orderId).FirstOrDefault();

                if (existingOrder == null)
                {
                    return NotFound();
                }
                _orderCollection.DeleteOne(o => o._id.ToString() == orderId);

                return Ok("Order deleted successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete the order from MongoDB: {ex.Message}");
            }
        }
    }
}
