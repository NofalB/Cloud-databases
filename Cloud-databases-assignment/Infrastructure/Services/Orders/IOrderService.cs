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

        Task<Order> GetOrderById(Guid orderId);

        Task<Order> AddOrder(OrderDTO order);

        Task<Order> UpdateOrder(Order order, Guid orderId);
        
    }
}
