using CakeCompany.ENums;
using CakeCompany.Models;
using CakeCompany.Provider.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Provider.Implementations;

public class CakeProvider : ICakeProvider
{
    private readonly ILogger<CakeProvider> _logger;
    public CakeProvider(ILogger<CakeProvider> logger)
    {
        _logger = logger;
    }
    public DateTime Check(Order order)
    {
        if (order.Name == Cake.Chocolate)
        {
            return DateTime.Now.Add(TimeSpan.FromMinutes(30));
        }

        if (order.Name == Cake.RedVelvet)
        {
            return DateTime.Now.Add(TimeSpan.FromMinutes(60));
        }

        return DateTime.Now.Add(TimeSpan.FromHours(15));
    }

    public Product Bake(Order order)
    {
        if (order.Name == Cake.Chocolate)
        {
            return new()
            {
                Cake = Cake.Chocolate,
                Id = new Guid(),
                Quantity = order.Quantity
            };
        }

        if (order.Name == Cake.RedVelvet)
        {
            return new()
            {
                Cake = Cake.RedVelvet,
                Id = new Guid(),
                Quantity = order.Quantity
            };
        }

        return new()
        {
            Cake = Cake.Vanilla,
            Id = new Guid(),
            Quantity = order.Quantity
        }; ;
    }
}