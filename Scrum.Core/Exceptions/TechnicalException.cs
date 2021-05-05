using Scrum.Core.Validations.ValidationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Core.Exceptions
{
    public class TechnicalException : Exception
    {
        public ValidationResultModel Result;
        public int? StatusCode { get; set; }
        public TechnicalException(ValidationResultModel result)
        {
            Result = result;

        }
        public TechnicalException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
