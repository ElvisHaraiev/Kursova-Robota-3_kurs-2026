using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Курсова
{
    public interface IDiscountStrategy
    {
        decimal CalculateDiscountAmount(decimal totalAmount);
    }

    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        private readonly decimal _percentage;

        public PercentageDiscountStrategy(decimal percentage)
        {
            if (percentage < 0m || percentage > 1m)
                throw new ArgumentOutOfRangeException(nameof(percentage), "Відсоток знижки має бути в межах від 0 до 1.");

            _percentage = percentage;
        }

        public decimal CalculateDiscountAmount(decimal totalAmount)
        {
            if (totalAmount < 0m)
                throw new ArgumentException("Сума замовлення не може бути від'ємною.", nameof(totalAmount));

            return totalAmount * _percentage;
        }
    }

    public class NoDiscountStrategy : IDiscountStrategy
    {
        public decimal CalculateDiscountAmount(decimal totalAmount)
        {
            if (totalAmount < 0m)
                throw new ArgumentException("Сума замовлення не може бути від'ємною.", nameof(totalAmount));

            return 0m;
        }
    }
}