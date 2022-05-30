using CakeCompany.ENums;
using CakeCompany.Factory.Interfaces;
using CakeCompany.Models;

namespace CakeCompany.Provider.Interfaces
{
    public interface ITransportProvider
    {
        public TransportType CheckForAvailability(List<Product> products);
        public IVehicle GetDeliveryVehicle(TransportType transport);
    }
}
