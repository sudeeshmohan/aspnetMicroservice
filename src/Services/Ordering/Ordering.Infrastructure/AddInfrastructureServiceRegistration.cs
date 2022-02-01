using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Contracts.Presistence;
using Ordering.Application.Models;
using Ordering.Infrastructure.Mail;
using Ordering.Infrastructure.Presistence;
using Ordering.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Infrastructure
{
    public static class AddInfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service,IConfiguration configuration)
        {
            service.AddDbContext<OrderContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("OrderingConnectionString")));
            
            service.AddScoped(typeof(IAsyncRepository<>), typeof(RepositoryBase<>));
            service.AddScoped<IOrderRepository, OrderRepository>();

            service.Configure<EmailSettings>(c => configuration.GetSection("EmailSettings"));
            service.AddTransient<IEmailService, EmailService>();

            return service;
        }
    }
}
