using System;
using System.Collections.Generic;
using System.Linq;
using Cryptocop.Software.API.Models.DTOs;
using Cryptocop.Software.API.Models.Entities;
using Cryptocop.Software.API.Models.Exceptions;
using Cryptocop.Software.API.Models.InputModels;
using Cryptocop.Software.API.Repositories.Contexts;
using Cryptocop.Software.API.Repositories.Interfaces;

namespace Cryptocop.Software.API.Repositories.Implementations
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly CryptocopDbContext _dbContext;

        public ShoppingCartRepository(CryptocopDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ShoppingCartItemDto> GetCartItems(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
            if (user == null || cart == null)
            {
                throw new NotFoundException();
            }
            var items = _dbContext.ShoppingCartItems
                        .Where(i => i.ShoppingCartId == cart.Id)
                        .Select(item => new ShoppingCartItemDto
                        {
                            Id = item.Id,
                            ProductIdentifier = item.ProductIdentifier,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalPrice = item.Quantity * item.UnitPrice
                        }).ToList();
            return items;
        }

        public void AddCartItem(string email, ShoppingCartItemInputModel shoppingCartItemItem, float priceInUsd)
        {
            Console.Write("here");
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
            if (user == null)
            {
                throw new NotFoundException();
            }
            if (cart == null)
            {
                cart = new ShoppingCart
                {  
                   UserId = user.Id 
                };
                _dbContext.ShoppingCarts.Add(cart);
                _dbContext.SaveChanges();
            }
            Console.Write(priceInUsd);
            float? quant = shoppingCartItemItem.Quantity;
            if (quant == null) { quant = (float)0.01;}
            var entity = new ShoppingCartItem
            {
                ShoppingCartId = cart.Id,
                ProductIdentifier = shoppingCartItemItem.ProductIdentifier,
                Quantity = (float)quant,
                UnitPrice = priceInUsd
            };
            _dbContext.Add(entity);
            _dbContext.SaveChanges();
        }

        public void RemoveCartItem(string email, int id)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var item = _dbContext.ShoppingCartItems.FirstOrDefault(c => c.Id == id);
            if (user == null || item == null)
            {
                throw new NotFoundException();
            }
            _dbContext.Remove(item);
            _dbContext.SaveChanges();
        }

        public void UpdateCartItemQuantity(string email, int id, float quantity)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var item = _dbContext.ShoppingCartItems.FirstOrDefault(c => c.Id == id);
            if (user == null || item == null)
            {
                throw new NotFoundException();
            }
            if (quantity > 0)
            {
                item.Quantity = quantity;
                _dbContext.SaveChanges();
            } 
        }

        public void ClearCart(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var items = GetCartItems(email);
            if (user == null || items == null)
            {
                throw new NotFoundException();
            }
            foreach (var item in items)
            {
                RemoveCartItem(email, item.Id);
            }
        }

        public void DeleteCart(string email)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
            var cart = _dbContext.ShoppingCarts.FirstOrDefault(c => c.UserId == user.Id);
            if (user == null || cart == null)
            {
                throw new NotFoundException();
            }
            ClearCart(email);
            _dbContext.ShoppingCarts.Remove(cart);
            _dbContext.SaveChanges();
        }
    }
}