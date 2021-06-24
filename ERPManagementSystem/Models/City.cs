﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Models
{
    public class City
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CityStatus { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
    }
}
