using System.Security.Claims;
using Ecommerce.Core.src.Common;
using Ecommerce.Core.src.Entity;
using Ecommerce.Core.src.ValueObject;
using Ecommerce.Service.src.DTO;
using Ecommerce.Service.src.ServiceAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controller.src.Controller
{
    [ApiController]
    [Route("api/v1/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost()]
        public async Task<Order> CreateOrderAsync(OrderCreateDto order)
        {
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);

            order.UserId = userId; // assign current userId to order create dto
            return await _orderService.CreateOrderAsync(order);
        }

        [Authorize(Roles = "Admin")] // only allow admins to update orders
        [HttpPut("{id}")]
        public async Task<bool> UpdateOrderAsync([FromRoute] Guid id, OrderUpdateDto orderUpdate)
        {
            return await _orderService.UpdateOrderByIdAsync(id, orderUpdate);
        }

        [Authorize()]
        [HttpGet("{id}")]
        public async Task<Order> RetrieveOrderAsync([FromRoute] Guid id)
        {
            var orderFound = await _orderService.GetOrderByIdAsync(id);
            
            var claims = HttpContext.User;
            var userId = Guid.Parse(claims.FindFirst(ClaimTypes.NameIdentifier).Value);
            var userRole = claims.FindFirst(ClaimTypes.Role).Value;
            
            if (userRole != UserRole.Admin.ToString() && userId != orderFound.UserId)
            {
                throw new UnauthorizedAccessException();
            }
            return orderFound;
        }

        [Authorize(Roles = "Admin")] // only allow admins to list all orders
        [HttpGet()]
        public async Task<IEnumerable<Order>> ListOrdersAsync([FromQuery] QueryOptions options)
        {
            return await _orderService.GetAllOrdersAsync(options);
        }

        [Authorize(Roles = "Admin")] // only allow admins to delete an order
        [HttpDelete("{id}")]
        public async Task<bool> DeleteOrderAsync([FromRoute] Guid id)
        {
            return await _orderService.DeleteOrderByIdAsync(id);
        }
    }
}
