using Domain;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.Orders
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrders();

        Task<Order> GetOrderById(string orderId);

        Task<Order> AddOrder(OrderDTO order);

        Task<Order> UpdateOrderStatus(OrderStatusDTO order, string orderId);

        Task DeleteOrderAsync(string orderId);


    }
}
