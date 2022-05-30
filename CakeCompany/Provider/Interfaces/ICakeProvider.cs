using CakeCompany.Models;

namespace CakeCompany.Provider.Interfaces
{
    public interface ICakeProvider
    {
        public DateTime Check(Order order);
        public Product Bake(Order order);
    }
}
