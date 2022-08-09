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
    public class CashInOutViewModel
    {
        public string user_id { get; set; }
        public string type { get; set; }
        public double amount { get; set; }

      

    }
}
