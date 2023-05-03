namespace EDM.PDFMappingVariables
{ 
    /// <summary>
     /// SPName used to fetch sets of SP based on letter type.
    /// </summary>
    public class TemplateSP
    {
        public const string ApprovalLetterSP = "p_Get_HUP_Approvals4CustomerCommunications";
        public const string ThirtyDayNotificationSP = "p_GET_HUP_30DayNotices4CustomerCommunications";
        public const string IncompleteApplicationNotificationSP = "p_GET_HUP_IncompleteApplicationNotices4CustomerCommunications";
        public const string DeclarationofZeroIncomeNotification = "p_GET_HUP_ZeroIncome4CustomerCommunications";
        public const string NoticeOfIneligibilitySP = "p_GET_HUP_IneligibilityNotices4CustomerCommunications";
        public const string SixtyDayNotificationSP = "p_GET_HUP_60DayNotices4CustomerCommunications";
        public const string NinetyDayNotificationSP = "p_GET_HUP_90DayNotices4CustomerCommunications";
    }
    /// <summary>
    /// ControlType used to fetch control type of the letter type.
    /// </summary>
    public class ControlType
    {
        public const string CheckBox = "Check";
        public const string RadioButton = "Radio";
        public const string Image = "Image";
    }
}
