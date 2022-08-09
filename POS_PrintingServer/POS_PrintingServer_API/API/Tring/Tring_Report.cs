using POS_PrintingServer_API.API.ViewModels;
using POS_PrintingServer_API.Enums;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tring.Fiscal.Driver;

namespace POS_PrintingServer_API.API.Tring
{
   public class Tring_Report
    {
        public static bool CheckIfPrinterOnline()
        {
            TringFiskalniPrinter printer = new TringFiskalniPrinter();
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                return init;
            }
            return false;
        }
        public static KasaOdgovor Print(E_ReportType type, DateTime? from, DateTime? to)
        {
            ClientDetailsViewModel _details = ClientDetailsViewModel.GetClientDetails();
            if (_details != null)
            {
                TringFiskalniPrinter printer = new TringFiskalniPrinter();
                bool init = printer.Inicijalizacija(_details.pos_host, int.Parse(_details.pos_port), 0, "0");
                switch (type)
                {
                    case E_ReportType.X:
                        return printer.StampatiPresjekStanja();
                    case E_ReportType.Y:
                        return printer.StampatiPeriodicniIzvjestaj(from.Value, to.Value);
                    case E_ReportType.Z:
                        return printer.StampatiDnevniIzvjestaj();
                }
            }
            return new KasaOdgovor() { };
        }
    }
}
