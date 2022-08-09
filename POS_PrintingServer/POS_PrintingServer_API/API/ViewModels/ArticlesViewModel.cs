using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class ArticlesViewModel
    {
        public int id { get; set; }
        public string created { get; set; }
        public string modified { get; set; }

        public string date { get; set; }
        public string Item { get; set; }
        public string article_name { get; set; }
        public string article_code { get; set; }
        public string article_unit_id { get; set; }
        public string article_barcode { get; set; }
        public string article_plu { get; set; }
        public string article_group_id { get; set; }
        public string desc { get; set; }

        public double quantity { get; set; }
        public double price { get; set; }
        public string price_without_vat { get; set; }
        public double discount { get; set; }
        public string discount_total { get; set; }
        public string discount_total_vat { get; set; }
        public string vat { get; set; }
        public string vat_percentage { get; set; }
        public string vat_category { get; set; }

        public string vat_total { get; set; }
        public string total_with_vat_without_discount { get; set; }
        public string total_without_vat { get; set; }
        public string total { get; set; }
        public int article { get; set; }


    }
}
