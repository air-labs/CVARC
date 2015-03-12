using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CVARC.V2
{
    public interface IAsserter
    {
        void IsEqual(double expected, double actual, double delta);
        void IsEqual(bool expected, bool actual);
    }

    public static class IAsserterExtensions
    {
        public static void True(this IAsserter asserter, bool expression)
        {
            asserter.IsEqual(true, expression);
        }
    }
}
