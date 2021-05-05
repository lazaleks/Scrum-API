using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Core.Validations.ValidationModels
{
    public class ValidationResultModel
    {
        public string Instance { get; private set; }
        public bool IsTechnical { get; private set; }

        public List<ValidationError> ResultMessages { get; }
        public string Message { get; set; }
        public int? ErrorCode { get; set; }

        public ValidationResultModel()
        {
            ResultMessages = new List<ValidationError>();
            Instance = $"urn:thinker:error:{Guid.NewGuid()}";
        }

        public ValidationResultModel(string message, int errorCode = 0) : this()
        {
            Message = message;
            ErrorCode = errorCode;
        }

        public ValidationResultModel(List<ValidationError> messages, string message) : this()
        {
            ResultMessages = messages;
            Message = message;
        }

        public void AddValidationMessage(string message, int? errorCode)
        {
            this.ResultMessages.Add(new ValidationError
            {
                Message = message,
                ErrorCode = errorCode,
            });
        }
        public void SetTechnicalError(string message)
        {
            this.Message = message;
            IsTechnical = true;
        }
    }
}
