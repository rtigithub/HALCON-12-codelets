using System;
using System.Collections.Generic;
using System.Text;
using HalconDotNet;

namespace HImageAcquisition
{

    /// <summary>
    /// The class contains the name and the value
    /// of a parameter of an image acquisition interface
    /// </summary>
    class IAParameterEntry
    {
        string parameterName;
        HTuple parameterValue;


        /// <summary>
        /// Constructor
        /// </summary>
        public IAParameterEntry(string paramName, HTuple paramValue)
        {
            this.parameterName  = paramName;
            this.parameterValue = paramValue;               
        }

        /// <summary>
        /// Gets a parameter name. The parameter name doesn't change.
        /// </summary>
        public string ParameterName
        {
            get
            {
                return this.parameterName;
            }
        }

        /// <summary>
        /// Gets a parameter name as a tuple. 
        /// The parameter name doesn't change.
        /// </summary>
        public HTuple ParameterNameTuple
        {
            get
            {
                return new HTuple(this.parameterName);
            }
        }

        /// <summary>
        /// Gets or sets a parameter value
        /// </summary>
        public HTuple ParameterValue
        {
            get
            {
                return this.parameterValue;
            }
            set
            {
                this.parameterValue = value;
            }
        }

    }
}
