using ClosureServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Domain.Entities
{
    public class Foreclosure
    {
        public int ForeclosureId { get; set; }

        public int LoanId { get; set; }

        public DateTime RequestDate { get; set; }
        public DateTime? ForeclosureDate { get; set; }

        public int RequestedBy { get; set; }

        public ForeclosureStatus ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }
        public DateTime? ApprovalDate { get; set; }

        public decimal PrincipalOutstanding { get; set; }
        public decimal InterestOutstanding { get; set; }    
        public decimal PenaltyOutstanding { get; set; }

        public decimal ForeclosureCharges { get; set; }
        public decimal RebateAmount { get; set; }
        public decimal TotalPayable { get; set; }
        public decimal SavingsAmount { get; set; }              


        public decimal? AmountPaid { get; set; }
        public DateTime? PaymentDate { get; set; }

        public PaymentMode PaymentMode { get; set; }
        public string? TransactionId { get; set; }

        public ForeclosureType ForeclosureType { get; set; }

        public int RemainingTenure { get; set; }

        public bool? CertificateGenerated { get; set; }
        public string? CertificatePath { get; set; }

        public string? Remarks { get; set; }
        public string? RejectionReason { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public string CreatedBy { get; set; }= string.Empty;
        public string? DeletedBy { get; set; }
    }
}
