using MudBlazor;
using System.Diagnostics.Metrics;
using WCPShared.Models.Entities.UserModels;
using WCPShared.Models.Enums;

namespace WCPAdminFrontEnd.Services
{
    public static class ProjectHelper
    {
        public static string GetStatusString(ProjectStatus status)
        {
            switch (status)
            {
                case ProjectStatus.Unconfirmed: return "Ubekræftet";
                case ProjectStatus.Queued: return "I kø";
                case ProjectStatus.Scripting: return "Scripting";
                case ProjectStatus.Planned: return "Planlægning";
                case ProjectStatus.CreatorFilming: return "Produktion";
                case ProjectStatus.Editing: return "Editing";
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
                    colorClass = "bg-red-50 text-red-500 border-red-500";
                    break;
                case ProjectStatus.Queued:
                    colorClass = "bg-red-50 text-red-500 border-red-500";
                    break;
                case ProjectStatus.Scripting:
                    colorClass = "bg-yellow-50 text-yellow-500 border-yellow-500";
                    break;
                case ProjectStatus.Planned:
                    colorClass = "bg-yellow-50 text-yellow-500 border-yellow-500";
                    break;
                case ProjectStatus.CreatorFilming:
                    colorClass = "bg-blue-50 text-blue-500 border-blue-500";
                    break;
                case ProjectStatus.Editing:
                    colorClass = "bg-blue-50 text-blue-500 border-blue-500";
                    break;
                case ProjectStatus.Feedback:
                    colorClass = "bg-green-50 text-green-500 border-green-500";
                    break;
                case ProjectStatus.Finished:
                    colorClass = "bg-red-50 text-red-500 border-red-500";
                    break;
                case ProjectStatus.Cancelled:
                    colorClass = "bg-red-50 text-red-500 border-red-500";
                    break;
                default:
                    colorClass = "bg-red-50 text-red-500 border-red-500";
                    break;
            }

            return colorClass + " border-2 px-2 p-1 rounded-xl font-medium text-center w-fit";
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
            //throw new ArgumentException("Country does not correspond to any existing country flag");
        }

        public static string LanguageToStringFlag(Language lang) => lang is not null ? CountryStringToFlag(lang.IsoLanguageCode) : "";

        public static string ArrayToStringFunc(int[] array)
        {
            // Check for null or empty array
            if (array == null || array.Length == 0)
                return string.Empty;

            if (array.Length == 2)
                if (array[0] == 0 && array[1] == 0)
                    return string.Empty;

            // Check for a single element in the array
            if (array.Length == 1)
                return $"{array[0]}+";

            // Check for two elements in the array
            if (array.Length == 2)
                return $"{array[0]}-{array[1]}";

            // If the array has more than two elements, return an error message
            return "Invalid input";
        }

        public static string LongArrayToStringFunc(long[] array)
        {
            // Check for null or empty array
            if (array == null || array.Length == 0)
                return string.Empty;

            if (array.Length == 2)
                if (array[0] == 0 && array[1] == 0)
                    return string.Empty;

            // Check for a single element in the array
            if (array.Length == 1)
                return $"{array[0]}+";

            // Check for two elements in the array
            if (array.Length == 2)
                return $"{array[0]}-{array[1]}";

            // If the array has more than two elements, return an error message
            return "Invalid input";
        }
    }
}
