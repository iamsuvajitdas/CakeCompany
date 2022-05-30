using CakeCompany.ENums;
using CakeCompany.Models;
using CakeCompany.Provider.Interfaces;
using CakeCompany.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Service.Implementations;

/**
    * Date             Author              PBI         Description
    * ------------------------------------------------------------------------------------------------------------------------------------
    * 30-05-2021       Suvajit D.                      Renamed GetShipment to Shipment and refactored the method for better readability, maintanibility
    * 30-05-2021       Suvajit D.                      Added ValidateOrder to validate order details and delivery time.
    * 
*/
public class ShipmentService : IShipmentService
{
    private readonly ILogger<ShipmentService> _logger;
    private readonly IOrderProvider _orderProvider;
    private readonly ICakeProvider _cakeProvider;
    private readonly ITransportProvider _transportProvider;
    private readonly IPaymentProvider _paymentProvider;

    public ShipmentService(ILogger<ShipmentService> logger, IOrderProvider orderProvider, ICakeProvider cakeProvider, ITransportProvider transportProvider, IPaymentProvider paymentProvider)
    {
        _logger = logger;
        _orderProvider = orderProvider;
        _cakeProvider = cakeProvider;
        _transportProvider = transportProvider;
        _paymentProvider = paymentProvider;
    }

    /// <summary>
    /// Search for latest orders, validate and send for shipment
    /// </summary>

    public void Shipment()
    {
        try
        {
            _logger.LogInformation("Entry to Shipment Method");

            var orders = _orderProvider.GetLatestOrders();

            if (!orders.Any())
            {
                _logger.LogWarning("No Order");
                throw new Exception("No Order");
            }

            var products = new List<Product>();

            foreach (var order in orders)
            {
                if (ValidateOrder(order))
                {
                    var product = _cakeProvider.Bake(order);
                    products.Add(product);
                }
                else
                {
                    //Notify cancellation. Added emial id to the order model for notification.
                    _logger.LogWarning("Order cancelled {@order}", order);
                }

            }

            if (products.Count > 0)
            {
                var transport = _transportProvider.CheckForAvailability(products);
                var vehicle = _transportProvider.GetDeliveryVehicle(transport);
                vehicle.Deliver(products);
            }
            else
            {
                _logger.LogInformation("No valid product to deliver {@products}", products);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception occured at Shipment method. {@ex}", ex);
            throw;
        }
        finally{
            _logger.LogInformation("Exit from Shipment Method");
        }
    }

    /// <summary>
    /// Validate order by checking delivery time and order payment 
    /// </summary>
    /// <returns></returns>

    public bool ValidateOrder(Order order)
    {
        var estimatedBakeTime = _cakeProvider.Check(order);

        if (estimatedBakeTime > order.EstimatedDeliveryTime)
        {
            return false;
        }

        if (!_paymentProvider.Process(order).IsSuccessful)
        {
            return false;
        }
        return true;
    }
}
