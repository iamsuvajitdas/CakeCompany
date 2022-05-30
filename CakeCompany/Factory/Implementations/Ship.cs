using CakeCompany.Factory.Interfaces;
using CakeCompany.Models;

namespace CakeCompany.Factory.Implementations;

public class Ship : IVehicle
{
    public bool Deliver(List<Product> products)
    {
        return true;
    }
}