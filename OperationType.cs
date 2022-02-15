using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    /// <summary>
    /// A type of operation to be performed
    /// </summary>
    public enum OperationType
    {
        /// <summary>
        /// Adds two values together
        /// </summary>
        Add,

        /// <summary>
        /// Subtracts two values
        /// </summary>
        Minus,

        /// <summary>
        /// Divide between two values
        /// </summary>
        Divide,

        /// <summary>
        /// Multiply two values
        /// </summary>
        Multiply,
    }
}
