
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class ClientDetailsViewModel
    {

        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public string pos_destination { get; set; }
        public string pos_type { get; set; }
        public string pos_username { get; set; }
        public string pos_password { get; set; }
        public string pos_host { get; set; }
        public string pos_port { get; set; }
        public string host { get; set; }
        public string port { get; set; }
        public string ibfm { get; set; }
        public bool locked { get; set; }
        public bool needs_sync { get; set; }
        public string os { get; set; }
        public string os_version { get; set; }

        public string local_ip { get; set; }
        public string public_ip { get; set; }
        public string architecture { get; set; }
        public bool installed { get; set; }

        public DateTime CreationDate { get; set; }

        public string uuid { get; set; }

        public ClientCallBackURLViewModel pos_destination_urls = new ClientCallBackURLViewModel();
        public ClientActionURLViewModel pos_action_urls = new ClientActionURLViewModel();
        public POS_ConfigViewModel pos_config = new POS_ConfigViewModel();
        public POS_Company company = new POS_Company();

        public static bool InsertClientDetails(ClientDetailsViewModel obj)
        {
            PrintingServerConfigDBContainer _context = new PrintingServerConfigDBContainer();
            if (_context.ClientDetails.Count() == 0)
            {
                ClientDetails _item = new ClientDetails()
                {
                    Architecture = obj.architecture,
                    ClientId = obj.id,
                    CreationDate = DateTime.Now,
                    Name = obj.name,
                    OS = obj.name,
                    PosDestination = obj.pos_destination,
                    PosHost = obj.pos_host,
                    PosPort = obj.pos_port,
                    PosType = obj.pos_type,
                    UUID = obj.uuid,
                    pos_password = obj.pos_password,
                    pos_userName = obj.pos_username,
                    company_uin = obj.company?.uin,
                    pos_articles_URL = obj.pos_destination_urls.pos_articles
                };
                _context.ClientDetails.Add(_item);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        public static ClientDetailsViewModel GetClientDetails()
        {
            PrintingServerConfigDBContainer _context = new PrintingServerConfigDBContainer();
            ClientDetails obj = _context.ClientDetails.FirstOrDefault();
            if (obj != null)
            {
                ClientDetailsViewModel _retVal = new ClientDetailsViewModel()
                {
                    architecture = obj.Architecture,
                    id = obj.Id,
                    CreationDate = DateTime.Now,
                    name = obj.Name,
                    os = obj.OS,
                    pos_destination = obj.PosDestination,
                    pos_host = obj.PosHost,
                    pos_port = obj.PosPort,
                    pos_type = obj.PosType,
                    uuid = obj.UUID,
                    pos_username = obj.pos_userName,
                    pos_password = obj.pos_password,
                    company = new POS_Company() { uin = obj.company_uin },
                    pos_destination_urls = new ClientCallBackURLViewModel() { pos_articles = obj.pos_articles_URL }
                };
                return _retVal;
            }
            return null;
        }

        public static void DeleteClientDetails()
        {
            PrintingServerConfigDBContainer _context = new PrintingServerConfigDBContainer();
            if (_context.ClientDetails.Count() > 0)
            {
                List<ClientDetails> _result = _context.ClientDetails.ToList();
                for (int i = 0; i < _result.Count; i++)
                {
                    _context.ClientDetails.Remove(_result[i]);
                }
                _context.SaveChanges();
            }
        }
    }
    public class ClientCallBackURLViewModel
    {
        public string invoices { get; set; }
        public string invoice { get; set; }
        public string inventory { get; set; }
        public string inventory_update_article_quantity { get; set; }
        public string inventory_verify_article_quantity { get; set; }
        public string pos_articles { get; set; }
        public string articles { get; set; }
    }
    public class ClientActionURLViewModel
    {
        public string sync { get; set; }
        public string sync_finished { get; set; }
        public string x_report { get; set; }
        public string y_report { get; set; }
        public string z_report { get; set; }
        public string deposit { get; set; }
        public string change_destination { get; set; }
    }
    public class POS_ConfigViewModel
    {
        public string mode { get; set; }
        public DateTime last_update { get; set; }
        public string nonFiscalPrinterFormat { get; set; }
        public bool nonFiscalPrinter { get; set; }
        public string default_payment { get; set; }
        public string footer { get; set; }


    }

    public class POS_Company
    {
        public string uin { get; set; }
    }
}
