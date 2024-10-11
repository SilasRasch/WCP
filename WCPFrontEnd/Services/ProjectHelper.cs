namespace WCPAdminFrontEnd.Services
{
    public static class ProjectHelper
    {
        public static string GetStatusString(int status)
        {
            switch (status)
            {
                case 0: return "Ubekræftet";
                case 1: return "I kø";
                case 2: return "Planlægning";
                case 3: return "Creator magi";
                case 4: return "I klipperummet";
                case 5: return "Feedback";
                case 6: return "Færdig";
                default: return "Ukendt";
            }
        }
    }
}
