using AutoMapper;
using ClosureServices.Application.DTO;
using ClosureServices.Application.Interfaces;
using ClosureServices.Domain.Entities;
using ClosureServices.Domain.Enums;
using ClosureServices.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using ClosureServices.Infrastructure.External;



namespace ClosureServices.Infrastructure.Repository
{
    public class ForClosureRepo : IForeclosureRepo
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly ILoanClosureRepo loanClosureRepo;
        private readonly LoanAccountClient _client;
        private readonly EmiClient _eClient;
        private readonly LoanPaymentClient _paymentClient;
        public ForClosureRepo(ApplicationDbContext db,IMapper mapper, ILoanClosureRepo loanClosureRepo, LoanAccountClient _client, EmiClient _eClient, LoanPaymentClient paymentClient)
        {
            this.db = db;
            this.mapper = mapper;
            this.loanClosureRepo = loanClosureRepo;
            this._client = _client;
            this._eClient = _eClient;
            _paymentClient = paymentClient;
        }
        public async Task<ForeclosureResponseDTO> Request(ForeclosureRequestDTO dto)
        {

            var cal=await Calculate(dto.LoanId);
            var data = mapper.Map<Foreclosure>(dto);
            data.PrincipalOutstanding=cal.PrincipalOutstanding;
            data.InterestOutstanding=cal.InterestOutstanding;
            data.PenaltyOutstanding=cal.PenaltyOutstanding;
            data.ForeclosureCharges=cal.ForeclosureCharges;
            data.RebateAmount=cal.RebateAmount;
            data.TotalPayable=cal.TotalPayable;
            data.SavingsAmount=cal.SavingsAmount;
            data.RequestDate=DateTime.UtcNow;
            data.ApprovalStatus = ForeclosureStatus.Pending;
            data.CreatedAt=DateTime.UtcNow;
            await  db.AddAsync(data);
            await  db.SaveChangesAsync();
            return mapper.Map<ForeclosureResponseDTO>(data);
        }
        public async Task<ForeclosureResponseDTO> Approve(int foreclosureId, ForeclosureApprovalDTO dto)
        {
            var data = await db.Foreclosures.FirstOrDefaultAsync(x => x.ForeclosureId == foreclosureId);
            if (data == null)
            {
                throw new Exception("Foreclosure request not found");

            }
            if (data.ApprovalStatus != ForeclosureStatus.Pending)
            {
                throw new Exception("Only pending requests can be approved");
            }
            data.ApprovalStatus = ForeclosureStatus.Approved;
            data.ApprovedBy = dto.ApprovedBy;
            data.ApprovalDate = DateTime.UtcNow;
            data.ModifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<ForeclosureResponseDTO>(data);

        }
        public async Task<ForeclosureResponseDTO> Reject(int foreclosureId, ForeclosureApprovalDTO dto)
        {
            var data=await db.Foreclosures.FirstOrDefaultAsync(x=>x.ForeclosureId==foreclosureId);
            if(data==null)
            {
                throw new Exception("Foreclosure request not found");
            }
            if(data.ApprovalStatus!=ForeclosureStatus.Pending)
            {
                throw new Exception("Only pending requests can be rejected");
            }
            data.ApprovalStatus=ForeclosureStatus.Rejected;
            data.ApprovedBy=dto.ApprovedBy;
            data.ApprovalDate=DateTime.UtcNow;
            data.RejectionReason=dto.RejectionReason;
            data.ModifiedAt=DateTime.UtcNow;
            await db.SaveChangesAsync();
            return mapper.Map<ForeclosureResponseDTO>(data);
        }
        public async Task<ForeclosureResponseDTO> Complete(int foreclosureId, ForeclosureCompleteDTO dto)
        {
            var data = await db.Foreclosures.FirstOrDefaultAsync(x => x.ForeclosureId == foreclosureId);
            if (data == null)
            {
                throw new Exception("Foreclosure request not found");
            }
            if (data.ApprovalStatus != ForeclosureStatus.Approved)
            {
                throw new Exception("Only approved requests can be completed");
            }
            data.AmountPaid = dto.AmountPaid;
            data.PaymentDate = DateTime.UtcNow;
            data.PaymentMode = dto.PaymentMode;
            data.TransactionId = $"TID-{Guid.NewGuid().ToString("N")[..8].ToString()}";
            data.ForeclosureDate = DateTime.UtcNow;
            data.ApprovalStatus = ForeclosureStatus.Completed;
            data.CertificateGenerated = false;
            data.ModifiedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
            await _eClient.CloseAllEmis(data.LoanId);
            var closureDto = new CreateClosureDTO
            {
                LoanId = data.LoanId,
                ClosureType = ClosureType.Foreclosure,

            };
            await loanClosureRepo.Create(closureDto);
            await db.SaveChangesAsync();
            await _paymentClient.CreatePayment(new CreatePaymentDTO
            {
                LoanId= data.LoanId,
                Scheduled=null,
                PaymentAmount=dto.AmountPaid,
                PaymentMethod=(PaymentMethod)dto.PaymentMode

            });

            await GenerateCertificateInternal(data);
            return mapper.Map<ForeclosureResponseDTO>(data);
        }
        private async Task GenerateCertificateInternal(Foreclosure data)
        {
            var fileName = $"ForeclosureCert-{data.ForeclosureId}.pdf";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(),  "FCCERTIFICATE");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

            }

            var filePath = Path.Combine(folderPath, fileName);

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("FORECLOSURE CERTIFICATE").Bold().FontSize(20);
                        col.Item().Text($"Loan ID: {data.LoanId}");
                        col.Item().Text($"Amount Paid: {data.AmountPaid}");
                        col.Item().Text($"Date: {DateTime.UtcNow}");
                    });
                });
            }).GeneratePdf(filePath);

            data.CertificateGenerated = true;
            data.CertificatePath = Path.Combine("FCCERTIFICATE", fileName);
            data.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        
        public async Task<ForeclosureResponseDTO?> GetById(int foreclosureId)
        {
            var data = await db.Foreclosures.FirstOrDefaultAsync(x => x.ForeclosureId == foreclosureId);
            if (data == null)
            {
                throw new Exception("Foreclosure request not found");
            }
            return mapper.Map<ForeclosureResponseDTO>(data);
        }

        public async Task<ForeclosureResponseDTO?> GetByLoanId(int loanId)
        {
            var data = await db.Foreclosures.FirstOrDefaultAsync(x => x.LoanId == loanId);
            if (data == null)
            {
                throw new Exception("Foreclosure request not found for the given loan");
            }
            return mapper.Map<ForeclosureResponseDTO>(data);
        }
        public async Task<ForeclosureCalculationDTO> Calculate(int loanId)
        {
            var loan=await _client.GetLoan(loanId);
            if(loan == null)
            {
                throw new Exception("Invalid Loan");
            }
            decimal principalOutstanding = loan.OutstandingPrincipal;
            decimal interestOutstanding=loan.OutstandingInterest;

            
            decimal penaltyOutstanding = 0;
            decimal foreclosureCharges = principalOutstanding * 0.02m;
            decimal rebateAmount = interestOutstanding * 0.50m;
            decimal totalPayable = principalOutstanding + interestOutstanding + penaltyOutstanding + foreclosureCharges - rebateAmount;
            decimal savingsAmount = rebateAmount;
            var result = new ForeclosureCalculationDTO
            {
                LoanId = loanId,
                PrincipalOutstanding = principalOutstanding,
                InterestOutstanding = interestOutstanding,
                PenaltyOutstanding = penaltyOutstanding,
                ForeclosureCharges = foreclosureCharges,
                RebateAmount = rebateAmount,
                TotalPayable = totalPayable,
                SavingsAmount = savingsAmount,
                CalculatedOn = DateTime.UtcNow
            };
            return result;


        }

        public async  Task<byte[]> DownloadCertificate(int foreclosureId)
        {
            var data=await db.Foreclosures.FirstOrDefaultAsync(x=>x.ForeclosureId== foreclosureId);
            var fullpath = Path.Combine(Directory.GetCurrentDirectory(), data.CertificatePath);
            return await File.ReadAllBytesAsync(fullpath);
        }
    }
}
