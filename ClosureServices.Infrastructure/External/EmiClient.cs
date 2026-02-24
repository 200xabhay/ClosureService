using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClosureServices.Infrastructure.External
{
    public class EmiClient
    {
        private readonly HttpClient _client;
        public EmiClient(HttpClient client)
        {
           _client = client;
        }
        public async Task CloseAllEmis(int loanId)
        {
            var response = await _client.PostAsync($"/api/Emi/close-all/{loanId}", null);

        }
    }
}
