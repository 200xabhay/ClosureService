using AutoMapper;
using ClosureServices.Application.DTO;
using ClosureServices.Application.Interfaces;
using ClosureServices.Domain.Entities;
using ClosureServices.Domain.Enums;
using ClosureServices.Infrastructure.Data;
using ClosureServices.Infrastructure.External;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Infrastructure.Repository
{
    public class LoanClosureRepo :ILoanClosureRepo
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        private readonly LoanPaymentClient _paymentClient;
        private readonly LoanAccountClient loanAccountClient;
        public LoanClosureRepo(ApplicationDbContext db,IMapper mapper, LoanPaymentClient _paymentClient, LoanAccountClient loanAccountClient)
        {
            this.db = db;
            this.mapper = mapper;
           this. _paymentClient = _paymentClient;
            this.loanAccountClient = loanAccountClient;
        }
        public async Task<LoanClosureResponseDTO> Create(CreateClosureDTO dto)
        {
            var loan = await loanAccountClient.GetLoan(dto.LoanId);
            if (loan == null)
                throw new Exception("Loan not found");

            var payments = await _paymentClient.FetchPayment(dto.LoanId);

            decimal totalPrincipalPaid = payments.Sum(x => x.PrincipalPaid);
            decimal totalInterestPaid = payments.Sum(x => x.InterestPaid);
            decimal totalPenaltyPaid = payments.Sum(x => x.PenaltyPaid);

            decimal totalAmountPaid = totalPrincipalPaid + totalInterestPaid + totalPenaltyPaid;

            var entity = mapper.Map<LoanClosure>(dto);

            entity.TotalDisbursed = loan.DisbursedAmount;
            entity.TotalPrincipalPaid = totalPrincipalPaid;
            entity.TotalInterestPaid = totalInterestPaid;
            entity.TotalPenaltyPaid = totalPenaltyPaid;
            entity.TotalAmountPaid = totalAmountPaid;
            entity.OriginalMaturityDate = loan.MaturityDate;
            entity.ActualMaturityDate = DateTime.UtcNow;
            entity.ClosureStatus = ClosureStatus.Completed;
            entity.CreatedAt = DateTime.UtcNow;

            await db.LoanClosures.AddAsync(entity);
            await db.SaveChangesAsync();

            await GenerateNocInternal(entity);
            await GenerateClosureCertificate(entity);

            return mapper.Map<LoanClosureResponseDTO>(entity);
        }



        public async Task<string> GenerateNoc(int closureId)
        {
            var closure = await db.LoanClosures.FirstOrDefaultAsync(c => c.ClosureId == closureId);
            if (closure == null) {
                throw new Exception("Closure not found");
            }
            if(closure.NocIssued) {
                return await Task.FromResult(closure.NocFilePath);
            }

            closure.NocIssued = true;
            closure.NocIssueDate = DateTime.UtcNow;
            closure.NocNumber = $"NOC-{closure.ClosureId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            string NocFilePath = $"/nocs/{closure.NocNumber}.pdf";

            closure.NocFilePath = NocFilePath;
            closure.ModifiedAt= DateTime.UtcNow;
            db.SaveChanges();
            return NocFilePath;
        }
        public async Task<string> GenerateClosureCertificate(int closureId)
        {
            var closure = await db.LoanClosures.FirstOrDefaultAsync(c => c.ClosureId == closureId);
            if (closure == null)
            {
                throw new Exception("Closure not found");
            }
            if (closure.ClosureStatus != ClosureStatus.Completed)
            {
                throw new Exception("Only approved closures can generate certificate");

            }
            string certificateNumber = $"CERT-{closure.ClosureId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            string certificatePath = $"/certificates/{certificateNumber}.pdf";

            closure.ClosureCertificatePath = certificatePath;
            closure.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();

            return certificatePath;

        }

        public async  Task<LoanClosureResponseDTO> GetById(int closureId)
        {
            var closure = await db.LoanClosures.FirstOrDefaultAsync(c => c.ClosureId == closureId);
            return mapper.Map<LoanClosureResponseDTO>(closure);
        }
        private async Task GenerateNocInternal(LoanClosure data)
        {
            var fileName = $"NOC-{data.ClosureId}.pdf";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() , "NOC");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);  
            }

            var filePath = Path.Combine(folderPath , fileName);

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("NO OBJECTION CERTIFICATE")
                            .Bold().FontSize(20);

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Loan ID: {data.LoanId}");
                        col.Item().Text($"Closure Date: {DateTime.UtcNow:dd-MM-yyyy}");
                        col.Item().Text($"Total Amount Paid: {data.TotalAmountPaid}");

                        col.Item().Text("");
                        col.Item().Text("This is to certify that the above loan has been fully repaid.");
                        col.Item().Text("There are no outstanding dues against this loan.");

                        col.Item().Text("");
                        col.Item().AlignRight().Text("Authorized Signatory");
                    });
                });
            }).GeneratePdf(filePath);

            data.NocIssued = true;
            data.NocIssueDate = DateTime.UtcNow;
            data.NocNumber = $"NOC-{data.ClosureId}-{DateTime.UtcNow:yyyyMMddHHmmss}";
            data.NocFilePath = Path.Combine("NOC" , fileName);
            data.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }
        private async Task GenerateClosureCertificate(LoanClosure data)
        {
            var fileName = $"ClosureCertificate-{data.ClosureId}.pdf";
            var folderPath = Path.Combine(Directory.GetCurrentDirectory() , "ClosureCertificates");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);

            }

            var filePath = Path.Combine(folderPath , fileName);

            QuestPDF.Fluent.Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(40);

                    page.Content().Column(col =>
                    {
                        col.Item().Text("LOAN CLOSURE CERTIFICATE")
                            .Bold().FontSize(20);

                        col.Item().LineHorizontal(1);

                        col.Item().Text($"Loan ID: {data.LoanId}");
                        col.Item().Text($"Original Maturity Date: {data.OriginalMaturityDate:dd-MM-yyyy}");
                        col.Item().Text($"Actual Closure Date: {data.ActualMaturityDate:dd-MM-yyyy}");

                        col.Item().Text("");
                        col.Item().Text("Financial Summary:");

                        col.Item().Text($"Total Disbursed: {data.TotalDisbursed}");
                        col.Item().Text($"Principal Paid: {data.TotalPrincipalPaid}");
                        col.Item().Text($"Interest Paid: {data.TotalInterestPaid}");
                        col.Item().Text($"Penalty Paid: {data.TotalPenaltyPaid}");

                        col.Item().Text("");
                        col.Item().Text("This loan account is officially marked as CLOSED.");

                        col.Item().Text("");
                        col.Item().AlignRight().Text("Authorized Officer");
                    });
                });
            }).GeneratePdf(filePath);

            data.ClosureCertificatePath = Path.Combine("ClosureCertificates" , fileName);
            data.ModifiedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public async  Task<byte[]> DownloadNoc(int closureId)
        {
            var data= await db.LoanClosures.FirstOrDefaultAsync(x=> x.ClosureId==closureId);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(),data.NocFilePath);
            return await File.ReadAllBytesAsync(fullPath);
        }

        public async Task<byte[]> DownloadCertificate(int closureId)
        {
            var data = await db.LoanClosures.FirstOrDefaultAsync(x => x.ClosureId == closureId);
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), data.ClosureCertificatePath);
            return await File.ReadAllBytesAsync(fullPath);
        }
    }
}
