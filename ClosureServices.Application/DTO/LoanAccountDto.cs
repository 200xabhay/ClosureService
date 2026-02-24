using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class LoanAccountForeclosureDTO
    {
        public int LoanId { get; set; }
        public decimal OutstandingPrincipal { get; set; }
        public decimal OutstandingInterest { get; set; }
        public decimal DisbursedAmount { get; set; }
        public decimal TotalOutstanding { get; set; }
        public int AccountStatus { get; set; }
        public DateTime MaturityDate { get; set; }



    }
    public class LApiResponse<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }


}
