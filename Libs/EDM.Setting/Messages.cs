using System;
using System.Collections;

namespace EDM.Setting
{
    public static class Messages
    {
        public static String NoRecord { get { return "No records to display."; } }
        public static String RecordSaved { get { return "Record saved successfully."; } }
        public static String RecordNotSaved { get { return "Error saving record, please contact technical support."; } }
        public static String ConfirmArchived { get { return "Are you sure you want to delete this record?"; } }
        public static String NotLoggedIn { get { return "<p>*** User not logged-in! ***</p>"; } }
        public static String NoModifyPerm { get { return "Sorry, you do not have permission to modify this page."; } }
        public static String NoArchivePerm { get { return "Sorry, you do not have permission to delete this record."; } }
        public static String ViewAllAdvisors { get { return "To view the list of all advisors, please click on 'View All Advisors' link."; } }
    }
}
