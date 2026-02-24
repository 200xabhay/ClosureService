using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class PaymentDTo
    {
        public int PaymentId { get; set; }
        public int LoanId { get; set; }

        public decimal PaymentAmount { get; set; }

        public decimal PrincipalPaid { get; set; }
        public decimal InterestPaid { get; set; }
        public decimal PenaltyPaid { get; set; }
        public decimal AdvancePayment { get; set; }
        public DateTime PaymentDate { get; set; }
    }
    public class CreatePaymentDTO
    {
        public int LoanId { get; set; }
        public int? Scheduled { get; set; }
        public decimal PaymentAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

    }
    public enum PaymentMethod
    {
        UPI = 1,
        NetBanking = 2,
        AutoDebit = 3,
        Cash = 4,
        Cheque = 5,
        NEFT = 6,
        RTGS = 7
    }
}
