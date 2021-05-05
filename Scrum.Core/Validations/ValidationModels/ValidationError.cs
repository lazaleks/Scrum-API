using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrum.Core.Validations.ValidationModels
{
    public class ValidationError
    {
        public string Message { get; set; }
        public int? ErrorCode { get; set; }
        public dynamic MoreInfo { get; set; }
    }
}
