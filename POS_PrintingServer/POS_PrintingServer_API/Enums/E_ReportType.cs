using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS_PrintingServer_API.Enums
{
    public enum E_ReportType
    {
        X = 0,
        Y = 1,
        Z = 2
    }

    public enum E_ActionLog
    {
        PrintInvoice = 0,
        ReclaimInvoice = 1

    }
}
