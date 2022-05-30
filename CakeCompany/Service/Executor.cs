using CakeCompany.Service.Interfaces;
using Microsoft.Extensions.Logging;

namespace CakeCompany.Service;

internal class Executor
{
    private readonly IShipmentService _shipmentService;
    private readonly ILogger<Executor> _logger;
    public Executor(IShipmentService shipmentService, ILogger<Executor> logger)
    {
        _shipmentService = shipmentService;
        _logger = logger;
    }
    public void Execute()
    {
        try
        {
            _logger.LogInformation("Execution Starts");
            _shipmentService.Shipment();
            _logger.LogInformation("Execution Ends");
        }
        catch (Exception ex)
        {
            _logger.LogError("Execution inturrupted", ex);
        }
    }
}
