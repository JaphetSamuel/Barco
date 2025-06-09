using Barco.Librairie.Application.Events;
using Barco.Librairie.Application.Services; // Required for IAccountRepository, IUnitOfWork, AccountService
using Barco.Librairie.Domain.Events;
using Barco.Librairie.Infrastructure.Data; // Required for InMemoryAccountRepository, InMemoryUnitOfWork
using Barco.Librairie.Infrastructure.DomainEvents;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Barco.Librairie
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            // Application Services
            services.AddScoped<AccountService>(); // Register AccountService itself

            // Infrastructure Services
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
            services.AddScoped<IAccountRepository, InMemoryAccountRepository>(); // Register in-memory repository
            services.AddScoped<IUnitOfWork, InMemoryUnitOfWork>(); // Register in-memory unit of work

            // Domain Event Handlers
            services.AddScoped<IDomainEventHandler<AccountOpened>, AccountOpenedHandler>();
            services.AddScoped<IDomainEventHandler<MoneyDeposited>, MoneyDepositedHandler>();
            services.AddScoped<IDomainEventHandler<MoneyWithdrawn>, MoneyWithdrawnHandler>();
        }
    }
}
