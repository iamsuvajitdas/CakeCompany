using CakeCompany.Models;

namespace CakeCompany.Provider.Interfaces
{
    public interface IPaymentProvider
    {
        public PaymentIn Process(Order order);
    }
}
