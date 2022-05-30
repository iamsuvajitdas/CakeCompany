using CakeCompany.ENums;
using CakeCompany.Factory.Implementations;
using CakeCompany.Models;
using CakeCompany.Provider.Interfaces;
using CakeCompany.Service.Implementations;
using CakeCompany.Service.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CakeCompany.UnitTest.Services;

[TestFixture]
public class ShipmentServiceTests
{
    private ShipmentService _sut;
    private Mock<IOrderProvider> _orderProviderMock;
    private Mock<ICakeProvider> _cakeProviderMock;
    private Mock<ITransportProvider> _transportProviderMock;
    private Mock<IPaymentProvider> _paymentProviderMock;
    private Mock<ILogger<ShipmentService>> _loggerMock;
    
    [SetUp]
    public void SetUp()
    {
        _orderProviderMock = new Mock<IOrderProvider>();
        _cakeProviderMock = new Mock<ICakeProvider>();
        _transportProviderMock = new Mock<ITransportProvider>();
        _paymentProviderMock = new Mock<IPaymentProvider>();
        _loggerMock = new Mock<ILogger<ShipmentService>>();
        _sut = new ShipmentService(_loggerMock.Object, _orderProviderMock.Object, _cakeProviderMock.Object, _transportProviderMock.Object, _paymentProviderMock.Object);

    }

    [Test]
    public async Task ShipmentTest()
    {
        //arrange
        Order[] orders = new Order[] {
            new("CakeBox", "cakebox.orderer@gmail.com", DateTime.Now.AddHours(3), 1, Cake.RedVelvet, 100),
            new("ImportantCakeShop","importantcakeshop.orderer@gmail.com", DateTime.Now.AddHours(2), 1, Cake.Vanilla, 120)
        };

        List<Product> products = new List<Product>();

        for(int i=0; i< orders.Length; i++)
        {
            Product product = new Product();
            product.Id = new Guid();
            product.OrderId = i + 1;
            product.Quantity = orders[i].Quantity;
            product.Cake = orders[i].Name;
            products.Add(product);

        }


        _orderProviderMock.Setup(p => p.GetLatestOrders()).Returns(orders);

        _cakeProviderMock.Setup(p => p.Bake(orders[0])).Returns(products[0]);
        _cakeProviderMock.Setup(p => p.Bake(orders[1])).Returns(products[1]);

        _cakeProviderMock.Setup(p => p.Check(orders[0])).Returns(DateTime.Now.AddMinutes(30));
        _cakeProviderMock.Setup(p => p.Check(orders[1])).Returns(DateTime.Now.AddMinutes(15));

        var paymentInfo = new PaymentIn
        {
            HasCreditLimit = true,
            IsSuccessful = true
        };
        _paymentProviderMock.Setup(p => p.Process(orders[0])).Returns(paymentInfo);
        _paymentProviderMock.Setup(p => p.Process(orders[1])).Returns(paymentInfo);

        _transportProviderMock.Setup(x => x.CheckForAvailability(products));

        _transportProviderMock.Setup(p => p.GetDeliveryVehicle(TransportType.Van)).Returns(new Van());

        //act
        _sut.Shipment();

        //assert
        _orderProviderMock.Verify(x => x.GetLatestOrders(), Times.Once());
        _cakeProviderMock.Verify(x => x.Check(orders[0]), Times.Once());
        _cakeProviderMock.Verify(x => x.Check(orders[1]), Times.Once());
        _cakeProviderMock.Verify(x => x.Bake(orders[0]), Times.Once());
        _cakeProviderMock.Verify(x => x.Bake(orders[1]), Times.Once());
        _transportProviderMock.Verify(x => x.CheckForAvailability(products), Times.Once());
        _transportProviderMock.Verify(x => x.GetDeliveryVehicle(TransportType.Van), Times.Once());
    }
    [Test]
    public async Task Shipment_NoOrderTest()
    {
        //arrange
        Order[] orders = new Order[] { };
        _orderProviderMock.Setup(p => p.GetLatestOrders()).Returns(orders);
        //act
        var result = Assert.Throws<Exception>(() => _sut.Shipment());
        //assert
        Assert.That(result.Message, Is.EqualTo("No Order"));

    }

    [Test]
    public async Task Shipment_NoValidOrderForInvalidDeliveryTimeTest()
    {
        //arrange
        Order[] orders = new Order[] {
            new("CakeBox", "cakebox.orderer@gmail.com", DateTime.Now, 1, Cake.RedVelvet, 120),
            new("ImportantCakeShop","importantcakeshop.orderer@gmail.com", DateTime.Now, 1, Cake.Vanilla, 120)
        };
        _orderProviderMock.Setup(p => p.GetLatestOrders()).Returns(orders);
        _cakeProviderMock.Setup(p => p.Check(orders[0])).Returns(DateTime.Now.AddMinutes(30));
        _cakeProviderMock.Setup(p => p.Check(orders[1])).Returns(DateTime.Now.AddMinutes(15));
        List<Product> products = new List<Product>();
        _transportProviderMock.Setup(x => x.CheckForAvailability(products));

        //act
        _sut.Shipment();

        //assert
        _transportProviderMock.Verify(x => x.CheckForAvailability(products), Times.Never());

    }

    [Test]
    public async Task Shipment_NoValidOrderForUnsuccessfullPaymentTest()
    {
        //arrange
        Order[] orders = new Order[] {
            new("CakeBox", "cakebox.orderer@gmail.com", DateTime.Now, 1, Cake.RedVelvet, 120),
            new("ImportantCakeShop","importantcakeshop.orderer@gmail.com", DateTime.Now, 1, Cake.RedVelvet, 120)
        };
        _orderProviderMock.Setup(p => p.GetLatestOrders()).Returns(orders);
        var paymentInfo = new PaymentIn
        {
            HasCreditLimit = false,
            IsSuccessful = false
        };
        _paymentProviderMock.Setup(p => p.Process(orders[0])).Returns(paymentInfo);
        _paymentProviderMock.Setup(p => p.Process(orders[1])).Returns(paymentInfo);

        List<Product> products = new List<Product>();
        _transportProviderMock.Setup(x => x.CheckForAvailability(products));

        //act
        _sut.Shipment();

        //assert
        _transportProviderMock.Verify(x => x.CheckForAvailability(products), Times.Never());

    }
}
