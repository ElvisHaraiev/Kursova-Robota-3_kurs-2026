using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсова
{
    public class PromoCodeDiscountStrategy : IDiscountStrategy
    {
        public double CalculateDiscount(double totalAmount)
        {
            return totalAmount * 0.20;
        }

        public string GetDiscountName()
        {
            return "Знижка 20% (Промокод WELCOME2026)";
        }
    }
}
