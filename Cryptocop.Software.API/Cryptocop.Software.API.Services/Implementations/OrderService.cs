using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Cryptocop.Software.API.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IQueueService _mbClient;
        private readonly string _routingKey;

        public OrderService(IOrderRepository orderRepository, IShoppingCartService shoppingCartService, IQueueService mbClient, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _shoppingCartService = shoppingCartService;
            _mbClient = mbClient;
            _routingKey = configuration.GetSection("MessageBroker").GetSection("RoutingKey").Value;
        }

        public IEnumerable<OrderDto> GetOrders(string email)
        {
            return _orderRepository.GetOrders(email);
        }

        public void CreateNewOrder(string email, OrderInputModel order)
        {
            var newOrder = _orderRepository.CreateNewOrder(email, order);
            var items = _shoppingCartService.GetCartItems(email);
            if (!items.Any())
            {
                throw new NotFoundException();
            }
            _shoppingCartService.ClearCart(email);
            _mbClient.PublishMessage(_routingKey, newOrder);
        }
    }
}