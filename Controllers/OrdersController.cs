using AutoMapper;
using ECPAPI.Data;
using ECPAPI.Models;
using ECPAPI.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECPAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersController> _logger;
        private APIResponse _apiResponse;

        public OrdersController(ECPDbContext dbContext, IOrderRepository orderRepository, IMapper mapper, ILogger<OrdersController> logger)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _apiResponse = new APIResponse();
        }

        [HttpGet("AllOrders", Name = "GetAllOrders")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> GetAllOrdersAsync()
        {
            try
            {
                _logger.LogInformation("Get All Orders Method Started");
                var orders = await _orderRepository.GetAllAsync();
                _apiResponse.Data = _mapper.Map<List<OrderDTO>>(orders);
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;

                return Ok(_apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Status = false;
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Errors.Add(ex.Message);

                return _apiResponse;
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) return NotFound();
            return Ok(order);
        }

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [AllowAnonymous]
        public async Task<ActionResult<APIResponse>> CreateOrderAsync([FromBody] OrderDTO orderDto)
        {
            try
            {
                if (orderDto == null)
                    return BadRequest();

                var order = _mapper.Map<Order>(orderDto);

                await _orderRepository.AddAsync(order);

                var response = _mapper.Map<OrderDTO>(order);

                //Order orderAfterCreation = await _orderRepository.AddAsync(orders);
                //order.Id = orderAfterCreation.Id;

                _apiResponse.Data = response;
                _apiResponse.Status = true;
                _apiResponse.StatusCode = HttpStatusCode.OK;
                return CreatedAtRoute("GetOrderById", new { Id = order.Id}, _apiResponse);
                //return CreatedAtRoute("GetOrderById", new { Id = order.Id }, _apiResponse);
            }
            catch (Exception ex)
            {
                _apiResponse.Errors.Add(ex.Message);
                _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
                _apiResponse.Status = false;
                return _apiResponse;
            }

            //await _orderRepository.AddAsync(order);
            //return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
        }

        //[HttpPost("add")]
        //[ProducesResponseType(StatusCodes.Status200OK)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //public IActionResult AddToCart([FromBody] OrderRequest request)
        //{
        //    Product.OrderProduct(request.ProductId, request.Quantity);
        //    return Ok();
        //}
        //public class OrderRequest
        //{
        //    public int ProductId { get; set; }
        //    public int Quantity { get; set; }
        //}

        [HttpPost("checkout")]
        [AllowAnonymous]
        public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
        {
            if (request.Items == null || !request.Items.Any())
            {
                return BadRequest(new { message = "Cart is empty" });
            }

            //
            var order = new Order
            {
                UserId = request.UserId,
                ShippingAddress = request.ShippingAddress,
                PaymentMethod = request.PaymentMethod,
                CreatedAt = DateTime.UtcNow,
                Items = new List<OrderItem>()
            };
            //

            decimal total = 0;
            foreach (var item in request.Items)
            {
                decimal price = GetProductPrice(item.ProductId);  //(ამოშლის რეკომენდაცია)
                total += price * item.Quantity;
                //
                order.Items.Add(new OrderItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = price
                });
                //
            }

            //await _orderRepository.CreateAsync(order);

            int receiptNumber = new Random().Next(1000, 999999);
            return Ok(new
            {
                success = true,
                data = new
                {
                    total,
                    receiptNumber,
                    date = DateTime.Now
                }
            });
        }

        private decimal GetProductPrice(int productId)
        {
           return 10;
        }
    }

    public class CheckoutRequest
    {
        public int UserId { get; set; }
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public List<CheckoutItem> Items { get; set; }
    }
    public class CheckoutItem
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }



    //[HttpPost("checkout")]
    //[AllowAnonymous]
    //public async Task<IActionResult> Checkout([FromBody] CheckoutRequest request)
    //{
    //    APIResponse response = new();
    //    try
    //    {
    //        decimal total = 0;
    //        List<OrderItem> orderItems = new();

    //        foreach (var item in request.Items)
    //        {
    //            var product = await _orderRepository.GetProductByIdAsync(item.ProductId); // ახალი მეთოდი IRepository-ში
    //            if (product == null)
    //                return NotFound($"Product with ID {item.ProductId} not found.");

    //            if (product.Stock < item.quantity)
    //                return BadRequest($"Not enough stock for {product.Name}");

    //            total += product.Price * item.quantity;

    //            orderItems.Add(new OrderItem
    //            {
    //                ProductId = product.Id,
    //                Quantity = item.quantity,
    //                Price = product.Price
    //            });

    //            product.Stock -= item.quantity;
    //        }

    //        var order = new Order
    //        {
    //            UserId = request.UserId,
    //            ShippingAddress = request.ShippingAddress,
    //            PaymentMethod = request.PaymentMethod,
    //            TotalAmount = total,
    //            CreatedAt = DateTime.UtcNow,
    //            Items = orderItems
    //        };

    //        await _orderRepository.AddAsync(order);
    //        await _orderRepository.SaveAsync(); // საჭირო იყოს SaveChangesAsync()

    //        var receipt = new
    //        {
    //            ReceiptNumber = $"N-{new Random().Next(1000, 999999)}",
    //            TotalAmount = total,
    //            Date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
    //            Message = "Thank you for your order."
    //        };

    //        response.Status = true;
    //        response.StatusCode = HttpStatusCode.OK;
    //        response.Data = receipt;
    //        return Ok(response);
    //    }
    //    catch (Exception ex)
    //    {
    //        response.Status = false;
    //        response.StatusCode = HttpStatusCode.InternalServerError;
    //        response.Errors.Add(ex.Message);
    //        return StatusCode(500, response);
    //    }
    //}
}


//public async Task<ActionResult<APIResponse>> GetAll()
//{
//    try
//    {   _logger.LogInformation("Get All Orders Method Started");
//        var orders = await _orderRepository.GetAllAsync();
//        _apiResponse.Data = _mapper.Map<List<OrderDTO>>(orders);
//        _apiResponse.Status = true;
//        _apiResponse.StatusCode = HttpStatusCode.OK;
//        return Ok(orders);
//    }
//    catch (Exception ex)
//    {   _apiResponse.Errors.Add(ex.Message);
//        _apiResponse.StatusCode = HttpStatusCode.InternalServerError;
//        _apiResponse.Status = false;
//        return _apiResponse;
//    }}