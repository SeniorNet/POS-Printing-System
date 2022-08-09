using Newtonsoft.Json;
using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class GlobalCallBackViewModel
    {
        public static string CashInOutCallBackURL(string uuid, string user, double amount, string type)
        {

            string _retval = string.Empty;
            string urlParameters = "?uuid=" + uuid + "&user=" + user;
            HttpClient client = ApplicationHelper.CurrentHttpClient;

            client.BaseAddress = new Uri(ApplicationHelper.CashInOutCallBackURL);
            var data = new { amount = amount, type = type == "+" ? "in" : "out" };
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(urlParameters, content).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                string _success = response.Content.ReadAsStringAsync().Result;

            }
            else
            {
                _retval = response.ReasonPhrase;
            }


            client.Dispose();
            return _retval;



            
        }
        public static string PrintReportCallbackURL(string uuid, string user, string action)
        {
            string _retval = string.Empty;
            string urlParameters = "?uuid=" + uuid + "&user=" + user + "&action=" + action + "-report";

            HttpClient client = ApplicationHelper.CurrentHttpClient;

            client.BaseAddress = new Uri(ApplicationHelper.ReportCallBackURL);

            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                // Parse the response body.
                string _success = response.Content.ReadAsStringAsync().Result;

            }
            else
            {
                _retval = response.ReasonPhrase;
            }


            client.Dispose();
            return _retval;
        }
        public static string InvoicePrintCallBackURL(InvoiceViewModel model, string uuid, string user, string destination, string id, string fiscal_date, string fiscal_time, string fiscal_number, string fiscal_amount)
        {
            string _retval = string.Empty;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                string urlParameters = "?uuid=" + uuid + "&user=" + user + "&destination=" + destination;
                HttpClient client = ApplicationHelper.CurrentHttpClient;

                client.BaseAddress = new Uri(ApplicationHelper.InvoicePrintCallBackURL);


                model.fiscal_date = fiscal_date;
                model.fiscal_time = fiscal_time;
                model.fiscal_number = fiscal_number;
                model.fiscal_amount = fiscal_amount;
                //var data = new { id = id, fiscal_date = fiscal_date, fiscal_time = fiscal_time, fiscal_number = fiscal_number, fiscal_amount = fiscal_amount };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(urlParameters, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    string _success = response.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    // Keep Log here 
                    string _json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    ApplicationLogViewModel.AddLog(Enums.E_ActionLog.ReclaimInvoice, _json);
                    _retval = response.ReasonPhrase;
                }


                client.Dispose();
                return _retval;

            }
            return _retval;

        }
        public static string ReclaimInvoicePrintCallBackURL(InvoiceViewModel model, string uuid, string user, string destination, string id, string stormed_date, string stormed_time, string stormed_number, string stormed_amount)
        {
            string _retval = string.Empty;
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                string urlParameters = "?uuid=" + uuid + "&user=" + user + "&destination=" + destination;
                HttpClient client = ApplicationHelper.CurrentHttpClient;
                client.BaseAddress = new Uri(ApplicationHelper.ReclaimInvoicePrintCallBackURL);


                model.stormed_date = stormed_date;
                model.stormed_time = stormed_time;
                model.stormed_number = stormed_number;
                model.stormed_amount = stormed_amount;
                //var data = new { id = id, stormed_number = stormed_number, stormed_amount = stormed_amount, stormed_date = stormed_date, stormed_time = stormed_time };
                var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                HttpResponseMessage response = client.PostAsync(urlParameters, content).Result;
                if (response.IsSuccessStatusCode)
                {
                    // Parse the response body.
                    string _success = response.Content.ReadAsStringAsync().Result;

                }
                else
                {
                    // Keep Log here 
                    string _json = Newtonsoft.Json.JsonConvert.SerializeObject(model);
                    ApplicationLogViewModel.AddLog(Enums.E_ActionLog.ReclaimInvoice, _json);
                    _retval = response.ReasonPhrase;
                }


                client.Dispose();
                return _retval;

            }
            return _retval;
        }
    }
}
