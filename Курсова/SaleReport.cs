using System;

namespace Курсова
{
    public class SaleReport
    {
        public string TableNo { get; set; }    
        public string Waiter { get; set; }   
        public string Products { get; set; }   
        public double TotalAmount { get; set; }
        public string PayType { get; set; }  
        public DateTime Date { get; set; }  
    }
}