using ClosureServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class ForeclosureRequestDTO   
    {
        [Range(1, int.MaxValue)]
        public int LoanId { get; set; }

        [Range(1, int.MaxValue)]
        public int RequestedBy { get; set; }

        public ForeclosureType ForeclosureType { get; set; }

        public string? Remarks { get; set; }
    }
    public class ForeclosureCalculationDTO
    {
        public int ForeclosureId { get; set; }
        public int LoanId { get; set; }   


        public decimal PrincipalOutstanding { get; set; }

        public decimal InterestOutstanding { get; set; }

        public decimal PenaltyOutstanding { get; set; }

        public decimal ForeclosureCharges { get; set; }

        public decimal RebateAmount { get; set; }

        public decimal TotalPayable { get; set; }

        public int RemainingTenure { get; set; }

        public decimal SavingsAmount { get; set; }
        public DateTime CalculatedOn { get; set; }       


        public ForeclosureStatus ApprovalStatus { get; set; }
    }
    public class ForeclosureApprovalDTO 
    {
        [Range(1, int.MaxValue)]
        public int? ApprovedBy { get; set; }

        public string? Remarks { get; set; }

        public string? RejectionReason { get; set; }
    }
    public class ForeclosureCompleteDTO
    {
        public decimal AmountPaid { get; set; }

        //public DateTime PaymentDate { get; set; }

        public PaymentMode PaymentMode { get; set; }

        //public string TransactionId { get; set; }= string.Empty;

        //public string? Remarks { get; set; }
    }
    public class ForeclosureResponseDTO
    {
        public int ForeclosureId { get; set; }

        public int LoanId { get; set; }

        public int RequestedBy { get; set; }             


        public DateTime RequestDate { get; set; }
        public DateTime? ForeclosureDate { get; set; }   


        public ForeclosureType ForeclosureType { get; set; }

        public ForeclosureStatus ApprovalStatus { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovalDate { get; set; }

        public decimal PrincipalOutstanding { get; set; }

        public decimal InterestOutstanding { get; set; }

        public decimal PenaltyOutstanding { get; set; }

        public decimal ForeclosureCharges { get; set; }

        public decimal RebateAmount { get; set; }

        public decimal TotalPayable { get; set; }

        public decimal? AmountPaid { get; set; }

        public DateTime? PaymentDate { get; set; }
        
        public PaymentMode PaymentMode { get; set; }   
        public string? TransactionId { get; set; } 

        public bool CertificateGenerated { get; set; }

        public string? CertificatePath { get; set; }

        public string? Remarks { get; set; }

        public string? RejectionReason { get; set; }
    }
    




}
