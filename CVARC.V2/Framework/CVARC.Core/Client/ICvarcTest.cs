﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface ICvarcTest
    {
        void Run(int port, TestAsyncLock holder, IAsserter asserter);
    }
}