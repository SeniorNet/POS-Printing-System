using Newtonsoft.Json;
using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class InvoiceViewModel
    {
        public string user_id { get; set; }
        public string destination { get; set; }
        public int? id { get; set; }
        public int? index { get; set; }
        public string date { get; set; }
        public List<ArticlesViewModel> articles { get; set; }
        public string created { get; set; }
        public string modified { get; set; }
        public string deadline { get; set; }
        public string fiscal_date { get; set; }
        public string fiscal_time { get; set; }
        public string fiscal_number { get; set; }
        public string fiscal_amount { get; set; }
        public string stormed_number { get; set; }
        public string stormed_date { get; set; }
        public string stormed_time { get; set; }
        public string partner_name { get; set; }
        public string partner_address { get; set; }
        public string partner_postal_code { get; set; }
        public string partner_uin { get; set; }
        public string partner_tin { get; set; }
        public string partner_city { get; set; }
        public string ibfm { get; set; }
        public string ibfu { get; set; }
        public string total { get; set; }
        public double cash { get; set; }
        public double check { get; set; }
        public double card { get; set; }
        public double virman { get; set; }
        public string notes { get; set; }
        public string footer { get; set; }
        public string operator_name { get; set; }
        public string uuid { get; set; }
        public string doc_type { get; set; }
        public int? partner { get; set; }
        public string device { get; set; }
        public string stormed { get; set; }
        public string stormed_amount { get; set; }






    }
   
}
