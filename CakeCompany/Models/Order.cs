using CakeCompany.ENums;

namespace CakeCompany.Models;
public record Order(string ClientName, string EmailId, DateTime EstimatedDeliveryTime, int Id, Cake Name, int Quantity);