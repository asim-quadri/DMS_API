namespace DmsApi.Helpers
{

    // Need to replace with string values
    public static class RefApprovalStatus
    {
        public static int Pending = 1;
        public static int Approved = 2;
        public static int Rejected = 3;
        public static int Reviewed = 4;
    }

    public static class RefApprovalStatusU
    {
        public static string Pending = "Pending";
        public static string Approved = "Approved";
        public static string Rejected = "Rejected";
        public static string Reviewed = "Reviewed";
        public static string Forward = "Forward";
    }

    public static class RefApprovalType
    {
        public static int User = 1;
        public static int Role = 2;
        public static int Access = 3;
    }
}
