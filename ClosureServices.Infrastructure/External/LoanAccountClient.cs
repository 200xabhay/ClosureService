using ClosureServices.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Infrastructure.External
{
    public class LoanAccountClient
    {
        private readonly HttpClient _client;
        public LoanAccountClient(HttpClient client)
        {
            _client = client;
            
        }
        public async Task<LoanAccountForeclosureDTO> GetLoan(int loanId)
        {
            var response = await _client.GetAsync($"/api/LoanAccount/{loanId}");
            if(!response.IsSuccessStatusCode)
            {
                throw new Exception("LoanAccount Service Error");
            }
            var result = await response.Content.ReadFromJsonAsync<LApiResponse<LoanAccountForeclosureDTO>>();
            if (result == null || !result.Success)
            {
                throw new Exception("LoanAccount service returned failure response");
            }
            return result?.Data;




        }
    }
}
