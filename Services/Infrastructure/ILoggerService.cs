﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Infrastructure
{
    public interface ILoggerService
    {
        void LogItem(string message);
    }
}
