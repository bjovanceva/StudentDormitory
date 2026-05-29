using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentDormitoryApp.Domain.DomainModels
{
    public class StripeSettings
    {
        public String? PublishableKey { get; set; }
        public String? SecretKey { get; set; }
    }
}
