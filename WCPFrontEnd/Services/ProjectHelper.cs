using MudBlazor;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

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
                case 2: return "Scripting";
                case 3: return "Planlægning";
                case 4: return "Creator magi";
                case 5: return "I klipperummet";
                case 6: return "Feedback";
                case 7: return "Færdig";
                case -1: return "Annulleret";
                default: return "Ukendt";
            }
        }

        public static string GetStatusString(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Unconfirmed: return "Ubekræftet";
                case ProjectStatus.Queued: return "I kø";
                case ProjectStatus.Scripting: return "Scripting";
                case ProjectStatus.Planned: return "Planlægning";
                case ProjectStatus.CreatorFilming: return "Creator magi";
                case ProjectStatus.Editing: return "I klipperummet";
                case ProjectStatus.Feedback: return "Feedback";
                case ProjectStatus.Finished: return "Færdig";
                case ProjectStatus.Cancelled: return "Annulleret";
                default: return "Ukendt";
            }
        }

        public static string GetStatusColor(ProjectStatus status)
        {
            string colorClass = string.Empty;

            switch (status)
            {
                case ProjectStatus.Unconfirmed:
                    colorClass = "bg-red-500";
                    break;
                case ProjectStatus.Queued:
                    colorClass = "bg-red-500";
                    break;
                case ProjectStatus.Scripting:
                    colorClass = "bg-yellow-600";
                    break;
                case ProjectStatus.Planned:
                    colorClass = "bg-yellow-600";
                    break;
                case ProjectStatus.CreatorFilming:
                    colorClass = "bg-blue-600";
                    break;
                case ProjectStatus.Editing:
                    colorClass = "bg-blue-600";
                    break;
                case ProjectStatus.Feedback:
                    colorClass = "bg-green-600";
                    break;
                case ProjectStatus.Finished:
                    colorClass = "bg-red-500";
                    break;
                case ProjectStatus.Cancelled:
                    colorClass = "bg-red-600";
                    break;
                default:
                    colorClass = "bg-red-500";
                    break;
            }

            return colorClass + " text-white p-2 rounded-lg";
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
                case 3: colorClass = "bg-yellow-600";
                    break;
                case 4: colorClass = "bg-blue-600";
                    break;
                case 5: colorClass = "bg-blue-600";
                    break;
                case 6: colorClass = "bg-green-600";
                    break;
                case 7: colorClass = "bg-red-500";
                    break;
                case -1:
                    colorClass = "bg-red-600";
                    break;
                default: colorClass = "bg-red-500";
                    break;
            }

            return colorClass + " text-white p-2 rounded-lg";
        }



        public static int CalculateAge(DateTime birthday)
        {
            var now = DateTime.Now;
            int age = now.Year - birthday.Year;

            if (now.Month < birthday.Month || (now.Month == birthday.Month && now.Day < birthday.Day))
                age--;

            return age;
        }

        public static string CountryStringToFlag(string country)
        {
            if (country == "DAN") return "🇩🇰";
            if (country == "ENG") return "🇬🇧";
            if (country == "SPA") return "🇪🇸";
            if (country == "GER") return "🇩🇪";
            if (country == "SWE") return "🇸🇪";
            if (country == "NOR") return "🇳🇴";

            return "";
        }

        public static string LanguageToStringFlag(Language lang) => lang is not null ? CountryStringToFlag(lang.Name) : "";
    }
}
