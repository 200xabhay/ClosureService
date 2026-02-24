using ClosureServices.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Application.Interfaces
{
    public interface ILoanClosureRepo
    {
        Task<LoanClosureResponseDTO> Create(CreateClosureDTO dto);

        Task<string> GenerateNoc(int closureId);
        Task<string>GenerateClosureCertificate(int closureId);
        Task<LoanClosureResponseDTO> GetById(int closureId);
        Task<byte[]> DownloadNoc(int closureId);

        Task<byte[]> DownloadCertificate(int closureId);

    }
}
