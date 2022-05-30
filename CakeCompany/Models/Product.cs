using CakeCompany.ENums;

namespace CakeCompany.Models;

public class Product
{
    public Guid Id { get; set; }
    public Cake Cake { get; set; }
    public int Quantity { get; set; }
    public int OrderId { get; set; }
}
