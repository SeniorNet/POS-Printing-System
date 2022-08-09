using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class LoginViewModel
    {
        public string uuid { get; set; }


        public static ClientDetailsViewModel LoginCallBackURL(string uuid)
        {
            string _retval = string.Empty;
            string urlParameters = "?uuid=" + uuid ;
            HttpClient client = ApplicationHelper.CurrentHttpClient;

            client.BaseAddress = new Uri(ApplicationHelper.LoginCallBackURL);

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                ClientDetailsViewModel _obj = response.Content.ReadAsAsync<ClientDetailsViewModel>().Result;
                ClientDetailsViewModel.InsertClientDetails(_obj);
                client.Dispose();
                return _obj;
            }
            else
            {
                _retval = response.ReasonPhrase;
                client.Dispose();
                return null;
            }
        }
    }
}
