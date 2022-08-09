using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.Helper
{
    public class ApplicationHelper
    {

        public static HttpClient CurrentHttpClient
        {
            get
            {
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11 | System.Net.SecurityProtocolType.Tls12;
                HttpClient client = new HttpClient();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                return client;
            }
        }


        public static string ReportCallBackURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["ReportCallBackURL"] != null)
                {
                    return ConfigurationManager.AppSettings["ReportCallBackURL"].ToString();
                }
                return string.Empty;
            }
        }

        public static string LoginCallBackURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["LoginCallBackURL"] != null)
                {
                    return ConfigurationManager.AppSettings["LoginCallBackURL"].ToString();
                }
                return string.Empty;
            }
        }

        public static string CashInOutCallBackURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["CashInOutCallBackURL"] != null)
                {
                    return ConfigurationManager.AppSettings["CashInOutCallBackURL"].ToString();
                }
                return string.Empty;
            }
        }
        public static string InvoicePrintCallBackURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["InvoicePrintCallBackURL"] != null)
                {
                    return ConfigurationManager.AppSettings["InvoicePrintCallBackURL"].ToString();
                }
                return string.Empty;
            }
        }

        public static string ReclaimInvoicePrintCallBackURL
        {
            get
            {
                if (ConfigurationManager.AppSettings["ReclaimInvoicePrintCallBackURL"] != null)
                {
                    return ConfigurationManager.AppSettings["ReclaimInvoicePrintCallBackURL"].ToString();
                }
                return string.Empty;
            }
        }

        public static string DatecsFolderPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["DatecsFolderPath"] != null)
                {
                    return ConfigurationManager.AppSettings["DatecsFolderPath"].ToString();
                }
                return string.Empty;
            }
        }
        public static string DatecsFolderPathAnswer
        {
            get
            {
                if (ConfigurationManager.AppSettings["DatecsFolderPathAnswer"] != null)
                {
                    return ConfigurationManager.AppSettings["DatecsFolderPathAnswer"].ToString();
                }
                return string.Empty;
            }
        }
        public static string DatecsArticlesFolderPath
        {
            get
            {
                if (ConfigurationManager.AppSettings["DatecsArticlesFolderPath"] != null)
                {
                    return ConfigurationManager.AppSettings["DatecsArticlesFolderPath"].ToString();
                }
                return string.Empty;
            }
        }




    }
}
