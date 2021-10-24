using Domain;
using Domain.DTO;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly ICosmosReadRepository<Order> _orderReadRepository;
        private readonly ICosmosWriteRepository<Order> _orderWriteRepository;

        public OrderService(ICosmosReadRepository<Order> orderReadRepository, ICosmosWriteRepository<Order> orderWriteRepository)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<Order> AddOrder(OrderDTO orderDTO)
        {
            Order order = new Order(orderDTO.ProductId,Guid.NewGuid(),orderDTO.Quantity,orderDTO.TotalPrice,OrderStatus.ordered);
            return await _orderWriteRepository.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orderReadRepository.GetAll().ToListAsync();
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            var orderGuid = Guid.Parse(orderId);
            var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(t => t.OrderId == orderGuid);
            return order;
        }

        public async Task<Order> UpdateOrderStatus(OrderStatusDTO order, string orderId)
        {
            var existingOrder = await GetOrderById(orderId);
            if (existingOrder != null)
            {
                var status=(OrderStatus)Enum.Parse(typeof(OrderStatus), order.OrderStatus);
                existingOrder.OrderStatus = status;
                return await _orderWriteRepository.Update(existingOrder);
            }
            else
            {
                throw new InvalidOperationException("The order ID provided does not exist");
            }
        }


        public async Task DeleteOrderAsync(string orderId)
        {
            var id = !string.IsNullOrEmpty(orderId) ? orderId : throw new ArgumentNullException($"{orderId} cannot be null or empty string.");
            Order order = await GetOrderById(id);

            if (order != null)
            {
                await _orderWriteRepository.Delete(order);
            }
            else
            {
                throw new InvalidOperationException($"The user ID {orderId} provided is invalid.");
            }
        }


    }
}
