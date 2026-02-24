using ClosureServices.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.DTO
{
    public class CreateClosureDTO
    {
        [Range(1, int.MaxValue)]
        public int LoanId { get; set; }

        public ClosureType ClosureType { get; set; }

        public string? Remarks { get; set; }

        [Range(1, int.MaxValue)]
        public int ClosureApprovedBy { get; set; }
    }
    public class LoanClosureResponseDTO
    {
        public int ClosureId { get; set; }
        public int LoanId { get; set; }

        public ClosureType ClosureType { get; set; }
        public ClosureStatus ClosureStatus { get; set; }

        public DateTime ClosureDate { get; set; }

        public decimal TotalDisbursed { get; set; }
        public decimal TotalPrincipalPaid { get; set; }
        public decimal TotalInterestPaid { get; set; }
        public decimal TotalPenaltyPaid { get; set; }

        public decimal TotalAmountPaid { get; set; }

        public bool NocIssued { get; set; }
        public string? NocNumber { get; set; }

        public string? Remarks { get; set; }
    }

}
