﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class AnalyticsViewModel
    {
        public int TotalReservations { get; set; }
        public double TotalRevenue { get; set; }
        public double TotalIncome { get; set;}
    }
}