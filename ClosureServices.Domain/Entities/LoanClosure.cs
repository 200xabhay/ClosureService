using ClosureServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Domain.Entities
{
    public class LoanClosure
    {
        public int ClosureId { get; set; }

        public int LoanId { get; set; }

        public ClosureType ClosureType { get; set; }

        public DateTime ClosureDate { get; set; }
        public string? ClosureReason { get; set; }

        public DateTime OriginalMaturityDate { get; set; }
        public DateTime ActualMaturityDate { get; set; }

        public decimal TotalDisbursed { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal TotalPenaltyPaid { get; set; }

        public decimal TotalAmountPaid { get; set; }
        public decimal OutstandingAtClosure { get; set; }

        public decimal ClosureCharges { get; set; }
        public decimal RebateAmount { get; set; }

        public decimal WriteOffAmount { get; set; }
        public decimal SettlementAmount { get; set; }

        public bool NocIssued { get; set; }
        public DateTime? NocIssueDate { get; set; }
        public string? NocNumber { get; set; }
        public string? NocFilePath { get; set; }

        public string? ClosureCertificatePath { get; set; }

        public int? ClosureApprovedBy { get; set; }

        public ClosureStatus ClosureStatus { get; set; }

        public string? Remarks { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string CreatedBy { get; set; }=string.Empty;
        public string? DeletedBy { get; set; }
    }
}
