using ClosureServices.Application.DTO;
using ClosureServices.Application.Interfaces;
using ClosureServices.Domain.Entities;
using ClosureServices.Infrastructure.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ClosureServices.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanClosuresController : ControllerBase
    {
        private readonly ILoanClosureRepo loanClosureRepo;
        public LoanClosuresController(ILoanClosureRepo loanClosureRepo)
        {
            this.loanClosureRepo = loanClosureRepo;
        }
        [HttpPost("create")]
        public async Task<IActionResult> Create(CreateClosureDTO dto)
        {
            var result = await loanClosureRepo.Create(dto);
            return Ok(ApiResponse<LoanClosureResponseDTO>.SuccessResponse(result, "Loan Closure Request Created Successfully"));
        }
        [HttpGet("noc/{closureId}")]
        public async Task<IActionResult> GenerateNoc(int closureId)
        {
            var result = await loanClosureRepo.GenerateNoc(closureId);
            return Ok(ApiResponse<string>.SuccessResponse(result, "NOC Generated Successfully"));
        }
        [HttpGet("{closureId}")]
        public async Task<IActionResult> GetById(int closureId)
        {
            var result = await loanClosureRepo.GetById(closureId);
            return Ok(ApiResponse<LoanClosureResponseDTO>.SuccessResponse(result, "Loan Closure Details Fetch Successfully"));
        }
        
        [HttpGet("certificate/{closureId}")]
        public async Task <IActionResult> GenCertificate(int closureId)
        {
            var result=await loanClosureRepo.GenerateClosureCertificate(closureId);
            return Ok(ApiResponse<string>.SuccessResponse(result, "Certicate Genearate Successfully.."));
        }
        [HttpGet("DownloadNoc/{closureId}")]
        public async Task<IActionResult> Download(int closureId)
        {
            var result = await loanClosureRepo.DownloadNoc(closureId);
            return File(result, "application/pdf", $"NOC-{closureId}.pdf");
        }
        [HttpGet("DownloadCert/{closureId}")]
        public async Task<IActionResult> DownloadCertificate(int closureId)
        {
            var result = await loanClosureRepo.DownloadCertificate(closureId);
            return File(result, "application/pdf", $"NOC-{closureId}.pdf");
        }




    }
}
