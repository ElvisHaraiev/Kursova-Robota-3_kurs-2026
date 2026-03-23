using System;
using System.Collections.Generic;

namespace Курсова
{
    public class ReservationData
    {
        public string CustomerName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int GuestCount { get; set; }
        public DateTime ResDate { get; set; }
    }

    public static class GlobalData
    {
        public static Dictionary<string, ReservationData> ReservedTables = new Dictionary<string, ReservationData>();
    }
}