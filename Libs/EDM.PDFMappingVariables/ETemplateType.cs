using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EDM.PDFMappingVariables
{
    public enum ETemplateType : int
    {
        _NoticeofIneligibility = 1,
        _30DayNotification = 2,
        _DeclarationofZeroIncomeNotification = 3,
        _IncompleteApplicationNotification = 4,
        _ApprovalLetter = 5,
        _60DayNotification = 6,
        _90DayNotification = 7
    }
}