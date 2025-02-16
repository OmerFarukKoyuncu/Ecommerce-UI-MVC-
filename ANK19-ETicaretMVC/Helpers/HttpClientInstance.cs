namespace ANK19_ETicaretMVC.Helpers
{
    public static class HttpClientInstance
    {
        //public static HttpClient CreateClient()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("");
        //    return client;
        //}

        public static HttpClient CreateClient(HttpContext context)
        {
            HttpClient client = new HttpClient();

            var url = "https://ank19-eticaret20250206213111.azurewebsites.net/api/";
            //var url = "https://localhost:7091/api/";

            client.BaseAddress = new Uri(url);

            // Token'ı Session'dan al
            if (context!=null)
            {
                var token = context.Session.GetString("JwtToken");
                if (!string.IsNullOrEmpty(token))
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                }
            }

          

            return client;
        }

    }
}
