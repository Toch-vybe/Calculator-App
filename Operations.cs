using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    /// <summary>
    /// Holds information about a single calculation operation
    /// </summary>
    internal class Operations
    {

        #region Public Properties
        /// <summary>
        /// the left side of the operation
        /// </summary>
        public string LeftSide { get; set; }

        /// <summary>
        /// the right side of the operation
        /// </summary>
        public string RightSide { get; set; }

        /// <summary>
        /// The type of operation to be performed
        /// </summary>
        public OperationType OperationType { get; set; }

        /// <summary>
        /// The type of operation to be performed first
        /// </summary>
        public OperationType innerOperation { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public Operations()
        {
            //Creates empty string instead of having nulls
            this.LeftSide = String.Empty;
            this.RightSide = String.Empty;
        }

        #endregion 

    }
}
