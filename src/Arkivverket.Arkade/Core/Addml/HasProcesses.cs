﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arkivverket.Arkade.Core.Addml
{
    public interface HasProcesses
    {
        string GetName();

        List<string> GetProcesses();

    }
}
