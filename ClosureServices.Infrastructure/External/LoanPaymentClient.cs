using ClosureServices.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Infrastructure.External
{
    public class LoanPaymentClient
    {
       private readonly HttpClient _client;  

        public LoanPaymentClient(HttpClient client)
        {
            _client = client;   
        }

        public async Task CreatePayment(CreatePaymentDTO dto)
        {
            var response=await _client.PostAsJsonAsync("api/v1/Payment",dto);

        }

       

        public async Task<List<PaymentDTo>> FetchPayment(int id)
        {
            var response = await _client.GetAsync($"api/v1/Payment/loan/{id}");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Payment Service Error");
            }
             
            var apiResponse = await response.Content.ReadFromJsonAsync<PaymentApiResponseDto>();

            if (apiResponse == null || !apiResponse.Success)
            {
                throw new Exception("Payment service returned failure response");
            }

            return apiResponse.Data ?? new List<PaymentDTo>();
        }

    }
}
