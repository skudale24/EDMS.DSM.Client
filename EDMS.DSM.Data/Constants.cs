using System;

namespace EDMS.Data.Constants
{
    public static class SQLConstants
    {
        //TODO: Fetch from DB
        public static string ConnectionString = "";
        public const string AdminPortal = "AP";
    }

    public static class GridParams
    {
        public static int ProgramID { get; set; }
        public static int UserID { get; set; }
    }
}
