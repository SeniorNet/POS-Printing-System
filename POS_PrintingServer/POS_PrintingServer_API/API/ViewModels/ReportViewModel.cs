using POS_PrintingServer_API.Enums;
using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class ReportViewModel
    {
        public string user_id { get; set; }
        public string reportType { get; set; }
        public DateTime? date_from { get; set; }
        public DateTime? date_to { get; set; }
    }
}