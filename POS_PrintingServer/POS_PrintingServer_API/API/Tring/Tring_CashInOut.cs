
using POS_PrintingServer_API.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.Tring
{
    public class Tring_CashInOut
    {
        public static KasaOdgovor CashInOut(string type, double amount)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                if (type == "+")
                {
                    return printer.UnosNovca(VrstePlacanja.Gotovina, amount);
                }
                else
                {
                    return printer.PovratNovca(VrstePlacanja.Gotovina, amount);
                }

            }
            return new KasaOdgovor() { };
        }
    }
}
