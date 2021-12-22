using Domain;
using Domain.DTO;
using DAL.Repositories;
using Infrastructure.Services.Products;
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
        private readonly IProductService _productService;

        public OrderService(ICosmosReadRepository<Order> orderReadRepository, ICosmosWriteRepository<Order> orderWriteRepository, IProductService productService)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
            _productService = productService;
        }

        public async Task<Order> AddOrder(OrderDTO orderDTO)
        {
            try
            {
                if (orderDTO == null)
                {
                    throw new NullReferenceException($"{nameof(orderDTO)} cannot be null.");
                }
                var product = await _productService.GetProductById(orderDTO.ProductId.ToString());

                if (product == null)
                {
                    throw new NullReferenceException($"this product with product id {orderDTO.ProductId} does not exist");

                }
                Order order = new Order( Guid.NewGuid(), orderDTO.ProductId, orderDTO.Quantity, orderDTO.TotalPrice, OrderStatus.ordered, orderDTO.ShippingDate, orderDTO.UserId);
                return await _orderWriteRepository.AddAsync(order);

            }
            catch
            {
                throw new InvalidOperationException("Please check all the fields are filled with correct values");
            }
            

            
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            return await _orderReadRepository.GetAll().ToListAsync();
        }

        public async Task<Order> GetOrderById(string orderId)
        {
            try
            {
                Guid id = !string.IsNullOrEmpty(orderId) ? Guid.Parse(orderId) : throw new ArgumentNullException("No order Id was provided.");

                var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(t => t.OrderId == id);
                return order;
            }
            catch
            {
                throw new InvalidOperationException($"Invalid user Id {orderId} provided.");
            }            
        }

        public async Task<Order> UpdateOrder(OrderUpdateDTO order, string orderId)
        {
            var existingOrder = await GetOrderById(orderId);
            if (existingOrder != null)
            {
                existingOrder.ShippingDate = order.ShippingDate;
                existingOrder.OrderStatus = order.OrderStatus;
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
                throw new InvalidOperationException($"The order ID {orderId} provided is invalid.");
            }
        }


    }
}
