using Scrum.Core.Validations.ValidationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Core.Exceptions
{
    public static class ExceptionExtensionMethods
    {
        public static ValidationResultModel BuildValidationResult(this Exception exception, HttpStatusCode statusCode)
        {
            ValidationResultModel validationResult = new ValidationResultModel("Unexpected Error !", (int)statusCode);
            Exception _exception = exception;

            while (true)
            {
                if (_exception == null)
                    break;

                validationResult.ResultMessages.Add(new ValidationError
                {
                    Message = _exception.Message
                });

                _exception = _exception.InnerException;
            }
            return validationResult;
        }
    }
}
