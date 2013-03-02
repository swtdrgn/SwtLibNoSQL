using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SwtLib
{
    /// <summary>
    /// Filter is more limited than regular boolean expressions.
    /// Motivation to use this limited design is because Amazon uses it and is the weakest link.
    /// Other robust and flexible models will require commitment in the design/implementation artifact.
    /// If design and implementation plan are clear and easy to execute, I will go forward with the plan.
    /// </summary>
    public class NoSQLFilter
    {
        

        public NoSQLFilter() { }
        private void Initialize()
        {
        }
    }
}
