using CakeCompany.Models;

namespace CakeCompany.Factory.Interfaces;

public interface IVehicle
{
    public bool Deliver(List<Product> products);
}
