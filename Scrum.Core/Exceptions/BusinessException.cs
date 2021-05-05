using Scrum.Core.Validations.ValidationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Core.Exceptions
{
    public class BusinessException : Exception
    {
        public ValidationResultModel ValidationResult;
        public BusinessException(ValidationResultModel result)
        {
            ValidationResult = result;
        }
    }
}
