﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ERPManagementSystem.Areas.Admin.Controllers
{
    public class EmployeeSalaryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
