using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SU_API.Models;
using System.Text.Json;
using MongoDB.Driver;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace API_SU.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<CreateViewModel> _orderCollection;

        public OrderController()
        {
            var client = new MongoClient("mongodb+srv://admin:admin@cluster0.v59caxa.mongodb.net/?retryWrites=true&w=majority");
            var database = client.GetDatabase("su");
            _orderCollection = database.GetCollection<CreateViewModel>("Ordersu");
        }

        [HttpGet]
        public ActionResult<List<string>> GetOrder()
        {
            var user = new List<string>
            {
                "User1",
                "User2",
                "User3"
            };
            return Ok(user);
        }


        [HttpGet]
        [Route("test")]
        public ActionResult TestConnection()
        {
            try
            {
                var document = new CreateViewModel
                {
                    Order_name = "Sample Order",
                    Res_name = "Sample Restaurant",
                    Place = "Sample Place",
                    Comment = "This is a test order",
                    Isgrab = true,
                    Items = new System.Collections.Generic.List<OrderItem>
                {
                    new OrderItem { Dish = "Item 1", Quantity = 2 },
                    new OrderItem { Dish = "Item 2", Quantity = 1 }
                }
                };
                _orderCollection.InsertOne(document);
                return Ok("Connection to MongoDB is successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Connection to MongoDB failed: {ex.Message}");
            }
        }

        [HttpPost]
        [Route("create")]
        public ActionResult CreateOrder([FromBody] CreateViewModel orderModel)
        {
            if (orderModel == null)
            {
                return BadRequest("Invalid order data. Request body is missing or not in the correct format.");
            }

            if (string.IsNullOrWhiteSpace(orderModel.Order_name) || string.IsNullOrWhiteSpace(orderModel.Res_name)
                || string.IsNullOrWhiteSpace(orderModel.Place) || string.IsNullOrWhiteSpace(orderModel.Comment)
                || orderModel.Items == null || orderModel.Items.Count == 0)
            {
                return BadRequest("Invalid order data. Make sure all required fields are provided.");
            }

            string orderName = orderModel.Order_name;
            string resName = orderModel.Res_name;
            string place = orderModel.Place;
            string comment = orderModel.Comment;
            bool isGrab = orderModel.Isgrab;
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
                    Order_name = orderName,
                    Res_name = resName,
                    Place = place,
                    Comment = comment,
                    Isgrab = false,
                    Items = items
                };
                _orderCollection.InsertOne(document);
                return Ok("Connection to MongoDB is successful.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Connection to MongoDB failed: {ex.Message}");
            }

        }
    }
}
