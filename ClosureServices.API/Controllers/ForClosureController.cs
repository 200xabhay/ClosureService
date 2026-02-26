using ClosureServices.Application.DTO;
using ClosureServices.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClosureServices.API.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    [ApiController]
    public class ForClosureController : ControllerBase
    {
        private readonly IForeclosureRepo foreclosureRepo;
        public ForClosureController(IForeclosureRepo foreclosureRepo)
        {
            this.foreclosureRepo = foreclosureRepo;

        }
        [HttpGet("calculate/{loanId}")]
        public async Task<IActionResult> Calculate(int loanId)
        {
            var result = await foreclosureRepo.Calculate(loanId);
            return Ok(ApiResponse<ForeclosureCalculationDTO>.SuccessResponse(result, "ForClosure Amount Calculated Successfully"));

        }
        [HttpPost("request")]
        public async Task<IActionResult> Request(ForeclosureRequestDTO dto)
        {
            var result = await foreclosureRepo.Request(dto);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "Request for ForClosure Send Successfully"));
        }
        [HttpPost("approve/{foreclosureId}")]
        public async Task<IActionResult> Approve(int foreclosureId, ForeclosureApprovalDTO dto)
        {
            var result = await foreclosureRepo.Approve(foreclosureId, dto);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "ForClosure Approve Successfully"));
        }
        [HttpPost("reject/{foreclosureId}")]
        public async Task<IActionResult> Reject(int foreclosureId, ForeclosureApprovalDTO dto)
        {
            var result = await foreclosureRepo.Reject(foreclosureId, dto);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "ForClosure Rejected"));
        }
        [HttpPost("complete/{foreclosureId}")]
        public async Task<IActionResult> Complete(int foreclosureId, ForeclosureCompleteDTO dto)
        {
            var result = await foreclosureRepo.Complete(foreclosureId, dto);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "ForeClosure Payment Complete"));
        }
        //[HttpGet("certificate/{foreclosureId}")]
        //public async Task<IActionResult> GetCertificate(int foreclosureId)
        //{
        //    var result = await foreclosureRepo.GenerateCertificate(foreclosureId);
        //    return Ok(ApiResponse<string>.SuccessResponse(result,"NOC Certificate Generate Successfully"));
        //}

        [HttpGet("{loanId}")]
        public async Task<IActionResult> GetByLoanId(int loanId)
        {
            var result = await foreclosureRepo.GetByLoanId(loanId);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "ForClosure  Details Fetch Successfully"));
        }
        [HttpGet("details/{foreclosureId}")]
        public async Task<IActionResult> GetById(int foreclosureId)
        {
            var result = await foreclosureRepo.GetById(foreclosureId);
            return Ok(ApiResponse<ForeclosureResponseDTO>.SuccessResponse(result, "ForClosure  Details Fetch Successfully"));
        }
        [HttpGet("Download/{foreclosureId}")]
        public async Task<IActionResult> Download(int foreclosureId)
        {
            var result = await foreclosureRepo.DownloadCertificate(foreclosureId);
            var fileName = $"ForeclosureCert-{foreclosureId}.pdf";
            return File(result,"application/pdf", fileName);

        }
    }
}