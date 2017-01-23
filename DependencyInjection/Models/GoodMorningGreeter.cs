﻿using DependencyInjection.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependencyInjection.Models
{
    public class GoodMorningGreeter : IGreeter
    {
        public string SendGreeting()
        {
            int now = DateTime.Now.Hour;
            if (now < 12)
            {
                return "Good morning";
            }
            else if(now >= 12 && now < 18)
            {
                return "Good afternoon";
            }
            else 
            {
                return "Good evening";
            }
        }
    }
}
