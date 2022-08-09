using POS_PrintingServer_API.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.API.ViewModels
{
    public class ApplicationLogViewModel
    {
        public static void AddLog(E_ActionLog action, string json)
        {
            PrintingServerConfigDBContainer _context = new PrintingServerConfigDBContainer();
            _context.ApplicationLogs.Add(new ApplicationLog()
            {
                CreationDate = DateTime.Now,
                JsonObject = json,
                ActionName = action.ToString()
            });
        }
    }
}
