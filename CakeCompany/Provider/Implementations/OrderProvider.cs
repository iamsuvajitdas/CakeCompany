using CakeCompany.ENums;
using CakeCompany.Models;
using CakeCompany.Provider.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Provider.Implementations;

public class OrderProvider : IOrderProvider
{
    private readonly ILogger<OrderProvider> _logger;
    public OrderProvider(ILogger<OrderProvider> logger)
    {
        _logger = logger;
    }
    public Order[] GetLatestOrders()
    {
        try
        {
            _logger.LogInformation("Entry to GetLatestOrders Method");
            return new Order[]
        {
            new("CakeBox", "cakebox.orderer@gmail.com", DateTime.Now.AddHours(3), 1, Cake.RedVelvet, 120),
            new("ImportantCakeShop","importantcakeshop.orderer@gmail.com", DateTime.Now.AddHours(2), 1, Cake.RedVelvet, 120)
        };
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occured at GetLatestOrders method.{ex}", ex);
            throw new Exception(ex.Message);
        }
        finally
        {
            _logger.LogInformation("Exit from GetLatestOrders Method");
        }

    }

    public void UpdateOrders(Order[] orders)
    {
    }
}


