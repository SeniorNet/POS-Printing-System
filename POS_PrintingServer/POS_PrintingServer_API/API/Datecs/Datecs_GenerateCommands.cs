using POS_PrintingServer_API.API.ViewModels;
using POS_PrintingServer_API.Enums;
using POS_PrintingServer_API.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.Datecs
{
    public class Datecs_GenerateCommands
    {
        public static string GetCashInOutCommand(string printerType, string type, double amount)
        {
            string _retVal = string.Empty;
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString() || printerType == E_PrinterType.datecsv3.ToString())
            {
                if (type == "+")
                {
                    _retVal = "70,1,______,_,__;" + amount + ";";
                }
                else if (type == "-")
                {
                    _retVal = "70,1,______,_,__;" + amount * -1 + ";";
                }
            }
            return _retVal;
        }
        public static string Print(string printerType, E_ReportType type, DateTime? from, DateTime? to)
        {
            string _retVal = string.Empty;
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString() || printerType == E_PrinterType.datecsv3.ToString())
            {
                if (type == E_ReportType.Z)
                {
                    _retVal = "69,1,______,_,__;0";
                }
                else if (type == E_ReportType.X)
                {
                    _retVal = "69,1,______,_,__;2";
                }
                else if (type == E_ReportType.Y)
                {
                    _retVal = "79,1,______,_,__;" + from.Value.ToString("ddMMyy") + ";" + to.Value.ToString("ddMMyy") + ";;;";
                }
            }

            return _retVal;
        }
        public static string GenerateLoadItems(string url, string uuid)
        {
            string _retVal = string.Empty;
            _retVal = ItemViewModel.LoadItemsAsString(url, uuid);

            return _retVal;
        }
        public static string LoadItemCommand(string printerType, string path)
        {
            string _retVal = string.Empty;

            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString())
            {
                _retVal = "300,1,______,_,__;0;0;Loading items.      Please wait...;" + path + ";";
            }
            else if (printerType == E_PrinterType.datecsv3.ToString())
            {
                _retVal = "300,1,______,_,__;0;;Loading items.      Please wait...;" + path + ";";
            }


            return _retVal;
        }

        public static string PrintInvoice(string printerType, string companyUin, string posUserName, string pos_Password, InvoiceViewModel invoice)
        {
            string _retVal = string.Empty;
            companyUin = "1234567890123456";
            foreach (var lineItem in invoice.articles)
            {
                int _vat = VatCategoryMapping(lineItem.vat_category);
                _retVal += "107,1,______,_,__;2;" + _vat + ";" + lineItem.article_plu + ";" + lineItem.price + ";" + lineItem.article_name + ";" + Environment.NewLine;
                _retVal += "107,1,______,_,__;4;" + lineItem.article_plu + ";" + lineItem.price + ";" + Environment.NewLine;
            }
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString())
            {

                //''48'' – open fiscal receipt
                //48,1,______,_,__;< IOSA >;< Operator >;< Password >;< TillNumber >;[ReceiptNumber];


                _retVal += "48,1,______,_,__;" + companyUin + ";" + posUserName + ";" + pos_Password + ";" + Environment.NewLine;

                //''52'' – sell items and show on the display
                //I. 52,1,______,_,__;[+-]< PLU >;[quantity];[percent];
                foreach (var lineItem in invoice.articles)
                {
                    _retVal += "52,1,______,_,__;" + lineItem.article_plu + ";" + lineItem.quantity + ";-" + lineItem.discount + ";" + Environment.NewLine;
                }

                //''53'' – payment
                //53,1,______,_,__;[flag];[amount];
                if (invoice.cash > 0)
                {
                    _retVal += "53,1,______,_,__;0;" + invoice.cash + ";" + Environment.NewLine;
                }
                if (invoice.card > 0)
                {
                    _retVal += "53,1,______,_,__;1;" + invoice.card + ";" + Environment.NewLine;
                }
                if (invoice.check > 0)
                {
                    _retVal += "53,1,______,_,__;2;" + invoice.check + ";" + Environment.NewLine;
                }
                if (invoice.virman > 0)
                {
                    _retVal += "53,1,______,_,__;3;" + invoice.virman + ";" + Environment.NewLine;
                }

                //''55'' – enter client information(invoice)
                //55,1,______,_,__;< IBK >;< Line 1 >;< Line 2 >;< Line 3 >;[Line 4];[Line 5];[Line 6];
                _retVal += "55,1,______,_,__;" + invoice.partner_uin + ";" + invoice.partner_name + ";" + invoice.partner_address + ";" + invoice.partner_city + ";" + Environment.NewLine;

                //''56'' – close fiscal receipt
                // 56,1,______,_,__;
                _retVal += "56,1,______,_,__;";
            }
            else if (printerType == E_PrinterType.datecsv3.ToString())
            {
                //''48'' – open fiscal receipt
                //48,1,______,_,__;< IOSA >;< Operator >;< Password >;< TillNumber >;[ReceiptNumber];
                _retVal += "48,1,______,_,__;" + companyUin + ";" + posUserName + ";" + pos_Password + ";" + Environment.NewLine;

                //''52'' – sell items and show on the display
                //I. 52,1,______,_,__;[+-PLU];[quantity];[percent];[price];
                foreach (var lineItem in invoice.articles)
                {
                    _retVal += "52,1,______,_,__;" + lineItem.article_plu + ";" + lineItem.quantity + ";-" + lineItem.discount + ";" + lineItem.price + ";" + Environment.NewLine;
                }

                //''53'' – payment
                //53,1,______,_,__;[flag];[amount];
                if (invoice.cash > 0)
                {
                    _retVal += "53,1,______,_,__;0;" + invoice.cash + ";" + Environment.NewLine;
                }
                if (invoice.card > 0)
                {
                    _retVal += "53,1,______,_,__;1;" + invoice.card + ";" + Environment.NewLine;
                }
                if (invoice.check > 0)
                {
                    _retVal += "53,1,______,_,__;2;" + invoice.check + ";" + Environment.NewLine;
                }
                if (invoice.virman > 0)
                {
                    _retVal += "53,1,______,_,__;3;" + invoice.virman + ";" + Environment.NewLine;
                }

                //''55'' – enter client information(invoice)
                //55,1,______,_,__;< IBK >;< Line 1 >;< Line 2 >;< Line 3 >;[Line 4];[Line 5];[Line 6];
                _retVal += "55,1,______,_,__;" + invoice.partner_uin + ";" + invoice.partner_name + ";" + invoice.partner_address + ";" + invoice.partner_city + ";" + Environment.NewLine;

                //''56'' – close fiscal receipt
                // 56,1,______,_,__;
                _retVal += "56,1,______,_,__;";
            }
            return _retVal;
        }
        public static string InvoicePrintDuplicate(string printerType, string fiscal_number)
        {
            string _retVal = string.Empty;
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString() || printerType == E_PrinterType.datecsv3.ToString())
            {
                _retVal = "109,1,______,_,__;F;" + fiscal_number + ";" + fiscal_number + ";1;";

            }
            return _retVal;
        }



        public static string PrintInvoiceReclaim(string printerType, string companyUin, string posUserName, string pos_Password, InvoiceViewModel invoice)
        {
            string _retVal = string.Empty;
            companyUin = "1234567890123456";
            foreach (var lineItem in invoice.articles)
            {
                int _vat = VatCategoryMapping(lineItem.vat_category);
                _retVal += "107,1,______,_,__;2;" + _vat + ";" + lineItem.article_plu + ";" + lineItem.price + ";" + lineItem.article_name + ";" + Environment.NewLine;
                _retVal += "107,1,______,_,__;4;" + lineItem.article_plu + ";" + lineItem.price + ";" + Environment.NewLine;
            }
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString())
            {

                //''48'' – open fiscal receipt
                //48,1,______,_,__;< IOSA >;< Operator >;< Password >;< TillNumber >;[ReceiptNumber];


                _retVal += "48,1,______,_,__;" + companyUin + ";" + posUserName + ";" + pos_Password + ";1;" + invoice.fiscal_number + ";" + Environment.NewLine;

                //''52'' – sell items and show on the display
                //I. 52,1,______,_,__;[+-]< PLU >;[quantity];[percent];
                foreach (var lineItem in invoice.articles)
                {
                    _retVal += "52,1,______,_,__;" + lineItem.article_plu + ";" + lineItem.quantity + ";-" + lineItem.discount + ";" + Environment.NewLine;
                }

                //''53'' – payment
                //53,1,______,_,__;[flag];[amount];
                if (invoice.cash > 0)
                {
                    _retVal += "53,1,______,_,__;0;" + invoice.cash + ";" + Environment.NewLine;
                }
                if (invoice.card > 0)
                {
                    _retVal += "53,1,______,_,__;1;" + invoice.card + ";" + Environment.NewLine;
                }
                if (invoice.check > 0)
                {
                    _retVal += "53,1,______,_,__;2;" + invoice.check + ";" + Environment.NewLine;
                }
                if (invoice.virman > 0)
                {
                    _retVal += "53,1,______,_,__;3;" + invoice.virman + ";" + Environment.NewLine;
                }

                //''55'' – enter client information(invoice)
                //55,1,______,_,__;< IBK >;< Line 1 >;< Line 2 >;< Line 3 >;[Line 4];[Line 5];[Line 6];
                _retVal += "55,1,______,_,__;" + invoice.partner_uin + ";" + invoice.partner_name + ";" + invoice.partner_address + ";" + invoice.partner_city + ";" + Environment.NewLine;

                //''56'' – close fiscal receipt
                // 56,1,______,_,__;
                _retVal += "56,1,______,_,__;";
            }
            else if (printerType == E_PrinterType.datecsv3.ToString())
            {
                //''48'' – open fiscal receipt
                //48,1,______,_,__;< IOSA >;< Operator >;< Password >;< TillNumber >;[ReceiptNumber];
                _retVal += "48,1,______,_,__;" + companyUin + ";" + posUserName + ";" + pos_Password + ";1;" + invoice.fiscal_number + ";" + Environment.NewLine;

                //''52'' – sell items and show on the display
                //I. 52,1,______,_,__;[+-PLU];[quantity];[percent];[price];
                foreach (var lineItem in invoice.articles)
                {
                    _retVal += "52,1,______,_,__;" + lineItem.article_plu + ";" + lineItem.quantity + ";-" + lineItem.discount + ";" + lineItem.price + ";" + Environment.NewLine;
                }

                //''53'' – payment
                //53,1,______,_,__;[flag];[amount];
                if (invoice.cash > 0)
                {
                    _retVal += "53,1,______,_,__;0;" + invoice.cash + ";" + Environment.NewLine;
                }
                if (invoice.card > 0)
                {
                    _retVal += "53,1,______,_,__;1;" + invoice.card + ";" + Environment.NewLine;
                }
                if (invoice.check > 0)
                {
                    _retVal += "53,1,______,_,__;2;" + invoice.check + ";" + Environment.NewLine;
                }
                if (invoice.virman > 0)
                {
                    _retVal += "53,1,______,_,__;3;" + invoice.virman + ";" + Environment.NewLine;
                }

                //''55'' – enter client information(invoice)
                //55,1,______,_,__;< IBK >;< Line 1 >;< Line 2 >;< Line 3 >;[Line 4];[Line 5];[Line 6];
                _retVal += "55,1,______,_,__;" + invoice.partner_uin + ";" + invoice.partner_name + ";" + invoice.partner_address + ";" + invoice.partner_city + ";" + Environment.NewLine;

                //''56'' – close fiscal receipt
                // 56,1,______,_,__;
                _retVal += "56,1,______,_,__;";
            }
            return _retVal;
        }

        public static string ReclaimPrintDuplicate(string printerType, string stormed_number)
        {
            string _retVal = string.Empty;
            if (printerType == E_PrinterType.datecsv1.ToString() || printerType == E_PrinterType.datecsv2.ToString() || printerType == E_PrinterType.datecsv3.ToString())
            {
                _retVal = "109,1,______,_,__;R;" + stormed_number + ";" + stormed_number + ";1;";
            }
            return _retVal;
        }
        static int VatCategoryMapping(string val)
        {
            int _retVal = 0;
            switch (val)
            {
                case "A":
                    _retVal = 1;
                    break;
                case "E":
                    _retVal = 2;
                    break;
                case "J":
                    _retVal = 3;
                    break;
                case "K":
                    _retVal = 4;
                    break;
                case "M":
                    _retVal = 5;
                    break;
                default:
                    break;
            }
            return _retVal;
        }

        public static string ReadInvoiceResponse()
        {
            string _retVal = string.Empty;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(ApplicationHelper.DatecsFolderPathAnswer);
                string _targetline = lines.Where(a => a.StartsWith("48")).FirstOrDefault();
                if (_targetline.Contains("Ok"))
                {
                    _retVal = _targetline.Split(';').ElementAt(1).Split(',').ElementAt(1);  // fiscal_number ;
                }
            }
            catch { }

            return _retVal;
        }
        public static string ReadReclamResponse()
        {
            string _retVal = string.Empty;
            try
            {
                string[] lines = System.IO.File.ReadAllLines(ApplicationHelper.DatecsFolderPathAnswer);
                string _targetline = lines.Where(a => a.StartsWith("48")).FirstOrDefault();
                if (_targetline.Contains("Ok"))
                {
                    _retVal = _targetline.Split(';').ElementAt(1).Split(',').ElementAt(2);  // stormed_number ;
                }
            }
            catch { }

            return _retVal;
        }
    }
}
