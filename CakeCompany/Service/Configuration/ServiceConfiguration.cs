using CakeCompany.Factory.Implementations;
using CakeCompany.Factory.Interfaces;
using CakeCompany.Provider.Implementations;
using CakeCompany.Provider.Interfaces;
using CakeCompany.Service.Implementations;
using CakeCompany.Service.Interfaces;
using Microsoft.Extensions.DependencyInjection;


namespace CakeCompany.Service.Configuration;
public static class ServiceConfiguration
{
   public static void AddServices(this IServiceCollection services)
    {
        #region Provider Injections
        services.AddSingleton<Executor>();
        services.AddSingleton<ICakeProvider, CakeProvider>();
        services.AddSingleton<IOrderProvider, OrderProvider>();
        services.AddSingleton<IPaymentProvider, PaymentProvider>();
        services.AddSingleton<IShipmentService, ShipmentService>();
        services.AddSingleton<ITransportProvider, TransportProvider>();
        #endregion
    }


}

