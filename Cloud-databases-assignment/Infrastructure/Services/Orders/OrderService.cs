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
            Order order = new Order(Guid.NewGuid(),Guid.NewGuid(),orderDTO.Quantity,orderDTO.TotalPrice,OrderStatus.ordered);
            return await _orderWriteRepository.AddAsync(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orderReadRepository.GetAll().ToListAsync();
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
            var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(t => t.OrderId == orderId);
            return order;
        }

        public async Task<Order> UpdateOrder(Order order, Guid orderId)
        {
            var existingStory = await GetOrderById(orderId);
            if (existingStory != null)
            {
                return await _orderWriteRepository.Update(order);
            }
            else
            {
                throw new InvalidOperationException("The order ID provided does not exist");
            }
        }
    }
}
