using CakeCompany.ENums;
using CakeCompany.Factory.Implementations;
using CakeCompany.Factory.Interfaces;
using CakeCompany.Models;
using CakeCompany.Provider.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Provider.Implementations;

public class TransportProvider : ITransportProvider
{
    private readonly ILogger<TransportProvider> _logger;
    public TransportProvider(ILogger<TransportProvider> logger)
    {
        _logger = logger;
    }
    public TransportType CheckForAvailability(List<Product> products)
    {
        if (products.Sum(p => p.Quantity) < 1000)
        {
            return TransportType.Van;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
        }

        if (products.Sum(p => p.Quantity) > 1000 && products.Sum(p => p.Quantity) < 5000)
        {
            return TransportType.Truck;
        }

        return TransportType.Ship;
    }

    public IVehicle GetDeliveryVehicle(TransportType transport)
    {
        switch (transport)
        {
            case TransportType.Van:
                return new Van();

            case TransportType.Truck:
                return new Truck();

            default:
                return new Ship();
        }

    }
}
