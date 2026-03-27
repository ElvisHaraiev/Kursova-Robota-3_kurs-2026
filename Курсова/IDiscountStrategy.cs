using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсова
{

    public interface IDiscountStrategy
    {
        double CalculateDiscount(double totalAmount);
        string GetDiscountName();
    }

    public class NewCustomerDiscount : IDiscountStrategy
    {
        public double CalculateDiscount(double totalAmount) => totalAmount * 0.20;
        public string GetDiscountName() => "Знижка 20% (Новий клієнт)";
    }


    public class NoDiscount : IDiscountStrategy
    {
        public double CalculateDiscount(double totalAmount) => 0;
        public string GetDiscountName() => "Без знижки";
    }

    public class PromoCodeDiscount : IDiscountStrategy
    {
        public double CalculateDiscount(double totalAmount) => totalAmount * 0.20;
        public string GetDiscountName() => "Знижка за промокодом";
    }
}