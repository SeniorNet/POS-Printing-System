using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class ItemViewModel
    {
        public string index { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string code { get; set; }
        public string unit_id { get; set; }


        public string price { get; set; }
        public string sold_by { get; set; }
        public string barcode_sku { get; set; }
        public string price_without_vat { get; set; }
        public string plu { get; set; }
        public string needs_sync { get; set; }
        public string group { get; set; }
        public string vat { get; set; }
        public string vat_category { get; set; }
        public string vat_percentage { get; set; }
        public string article_id { get; set; }


        public static List<ItemViewModel> LoadItems(string itemURL, string uuid)
        {
            string _retval = string.Empty;
            string urlParameters = "?uuid=" + uuid;
            HttpClient client = ApplicationHelper.CurrentHttpClient;
            client.BaseAddress = new Uri(itemURL + "/?uuid=" + uuid);
            // List data response.
            HttpResponseMessage response = client.GetAsync(urlParameters).Result;
            if (response.IsSuccessStatusCode)
            {
                List<ItemViewModel> _obj = response.Content.ReadAsAsync<List<ItemViewModel>>().Result;
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
        public static string LoadItemsAsString(string itemURL, string uuid)
        {
            string _retVal = string.Empty;
            List<ItemViewModel> _items = LoadItems(itemURL, uuid);
            foreach (var item in _items)
            {
                string _vatNumber = string.Empty;
                if (item.vat_category.ToUpper().Trim() == "A")
                {
                    _vatNumber = "1";
                }
                else if (item.vat_category.ToUpper().Trim() == "E")
                {
                    _vatNumber = "2";
                }
                else if (item.vat_category.ToUpper().Trim() == "J")
                {
                    _vatNumber = "3";
                }
                else if (item.vat_category.ToUpper().Trim() == "K")
                {
                    _vatNumber = "4";
                }
                else if (item.vat_category.ToUpper().Trim() == "M")
                {
                    _vatNumber = "5";
                }
                _retVal += _vatNumber + ";" + item.plu + ";" + item.price + ";" + item.name + ";" + Environment.NewLine;
            }
            return _retVal;
        }
    }
}
