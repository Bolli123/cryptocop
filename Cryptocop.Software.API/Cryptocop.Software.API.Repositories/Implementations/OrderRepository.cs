using System;
using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;
using Cryptocop.Software.API.Repositories.Helpers;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CryptocopDbContext _dbContext;
        private readonly IShoppingCartRepository _cartRepository;

        public OrderRepository(CryptocopDbContext dbContext, IShoppingCartRepository cartRepository)
        {
            _dbContext = dbContext;
            _cartRepository = cartRepository;
        }

        private List<OrderItemDto> GetOrderItems(int orderId)
        {
            var orders = _dbContext
                .OrderItems
                .Where(o => o.OrderId == orderId)
                .Select(a => new OrderItemDto
                {
                    Id = a.Id,
                    ProductIdentifier = a.ProductIdentifier,
                    Quantity = a.Quantity,
                    UnitPrice = a.UnitPrice,
                    TotalPrice = a.UnitPrice * a.Quantity
                }).ToList();
            Console.Write(orders);
            return orders;
        }
        public IEnumerable<OrderDto> GetOrders(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user == null) { throw new NotFoundException();}
            var orders = _dbContext
                .Orders
                .Where(o => o.Email == user.Email)
                .Select(a => new OrderDto
                {
                    Id = a.Id,
                    Email = a.Email,
                    FullName = a.FullName,
                    StreetName = a.StreetName,
                    HouseNumber = a.HouseNumber,
                    ZipCode = a.ZipCode,
                    Country = a.Country,
                    City = a.City,
                    CardholderName = a.CardHolderName,
                    CreditCard = PaymentCardHelper.MaskPaymentCard(a.MaskedCreditCard),
                    OrderDate = a.OrderDate.ToString(),
                    TotalPrice = a.TotalPrice
                }).ToList();
            foreach (var order in orders)
            {
                order.OrderItems = GetOrderItems(order.Id);
            }
            return orders;
        }

        public OrderDto CreateNewOrder(string email, OrderInputModel order)
        {
            var cartItems = _cartRepository.GetCartItems(email);
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var address = _dbContext.Addresses.FirstOrDefault(a => a.Id == order.AddressId);
            var card = _dbContext.PaymentCards.FirstOrDefault(c => c.Id == order.PaymentCardId);
            float totalPrice = 0;
            if (user == null || address == null || card == null) { throw new NotFoundException();}
            foreach (var item in cartItems)
            {
                totalPrice += item.TotalPrice;
            }
            var entity = new Order
            {
                Email = user.Email,
                FullName = user.FullName,
                StreetName = address.StreetName,
                HouseNumber = address.HouseNumber,
                ZipCode = address.ZipCode,
                Country = address.Country,
                City = address.City,
                CardHolderName = card.CardholderName,
                MaskedCreditCard = PaymentCardHelper.MaskPaymentCard(card.CardNumber),
                OrderDate = DateTime.Now,
                TotalPrice = totalPrice,
            };
            _dbContext.Orders.Add(entity);
            _dbContext.SaveChanges();
            foreach (var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    OrderId = entity.Id,
                    ProductIdentifier = item.ProductIdentifier,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                };
                _dbContext.OrderItems.Add(orderItem);
            }
            _dbContext.SaveChanges();
            return new OrderDto
            {
                Id = entity.Id,
                Email = entity.Email,
                FullName = entity.FullName,
                StreetName = entity.StreetName,
                HouseNumber = entity.HouseNumber,
                ZipCode = entity.ZipCode,
                Country = entity.Country,
                City = entity.City,
                CardholderName = entity.CardHolderName,
                CreditCard = card.CardNumber,
                OrderDate = entity.OrderDate.ToString(),
                TotalPrice = entity.TotalPrice,
                OrderItems = GetOrderItems(entity.Id)
            };
        }
    }
}