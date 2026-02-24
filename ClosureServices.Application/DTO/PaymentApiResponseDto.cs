using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class PaymentApiResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<PaymentDTo> Data { get; set; }
        public string Error { get; set; }
    }

}
