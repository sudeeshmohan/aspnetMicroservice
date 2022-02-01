using Microsoft.Extensions.Logging;
using Ordering.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Presistence
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext,ILogger<OrderContextSeed> logger)
        {
            if (!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetPreConfiguredOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation("Seed data associated with context {DbContextName}", typeof(OrderContext).Name);
            }
        }
        private static IEnumerable<Order> GetPreConfiguredOrders()
        {
            return new List<Order>
            {
                new Order{UserName="swn",FirstName="Sudeesh", LastName="Mohan",
                EmailAddress="idofsudeeshmohan@gmail.com",
                AddressLine="Idukki",
                Country="India",
                TotalPrice=350}
            };
        }
    }
}
