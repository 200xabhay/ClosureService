using ClosureServices.Application.DTO;
using ClosureServices.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.Interfaces
{
    public interface IForeclosureRepo
    {
        Task<ForeclosureCalculationDTO> Calculate(int loanId);

        Task<ForeclosureResponseDTO> Request(ForeclosureRequestDTO dto);

        Task<ForeclosureResponseDTO> Approve(int foreclosureId, ForeclosureApprovalDTO dto);

        Task<ForeclosureResponseDTO> Reject(int foreclosureId, ForeclosureApprovalDTO dto);

        Task<ForeclosureResponseDTO> Complete(int foreclosureId, ForeclosureCompleteDTO dto);

        Task<ForeclosureResponseDTO?> GetByLoanId(int loanId);

        Task<ForeclosureResponseDTO?> GetById(int foreclosureId);
        Task<byte[]> DownloadCertificate(int foreclosureId);



        //Task<string> GenerateCertificate(int foreclosureId);
    }
}
