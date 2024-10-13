using MudBlazor;

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
                case -1: return "Annulleret";
                default: return "Ukendt";
            }
        }

        public static string GetStatusColor(int status)
        {
            string colorClass = string.Empty;
            
            switch (status)
            {
                case 0: colorClass = "bg-red-500";
                    break;
                case 1: colorClass = "bg-red-500";
                    break;
                case 2: colorClass = "bg-yellow-600";
                    break;
                case 3: colorClass = "bg-blue-600";
                    break;
                case 4: colorClass = "bg-blue-600";
                    break;
                case 5: colorClass = "bg-green-600";
                    break;
                case 6: colorClass = "bg-red-500";
                    break;
                case -1:
                    colorClass = "bg-red-600";
                    break;
                default: colorClass = "bg-red-500";
                    break;
            }

            return colorClass + " text-white p-2 rounded-lg";
        }
    }
}
