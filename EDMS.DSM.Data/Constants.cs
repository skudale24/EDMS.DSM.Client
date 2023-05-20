﻿using System;

namespace EDMS.Data.Constants
{
    public static class SQLConstants
    {
        public const string ConnectionString = "Server=10.2.3.18\\SQL2016,1435;Database=EDMSTVA;User Id=qauser;Password=qauser123;TrustServerCertificate=True";
        public const string AdminPortal = "AP";
        public const string GemBoxKey = "EMPZ-O95K-ELRO-I5T1";
    }

    public static class GridParams
    {
        public static int ProgramID { get; set; }
        public static int UserID { get; set; }
    }
}
