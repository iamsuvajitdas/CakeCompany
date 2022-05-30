using CakeCompany.Factory.Interfaces;
using CakeCompany.Models;

namespace CakeCompany.Factory.Implementations;

public class Truck : IVehicle
{
    public bool Deliver(List<Product> products)
    {
        return true;
    }
}