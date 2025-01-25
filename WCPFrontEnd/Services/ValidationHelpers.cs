using WCPShared.Services.StaticHelpers;

namespace WCPFrontEnd.Services
{
    public static class ValidationHelpers
    {
        public static string NameValidation(string arg)
        {
            return !Validation.ValidateDisplayName(arg) ? "Navnet skal være minimum to tegn" : null!;
        }

        public static string URLValidation(string arg)
        {
            return !Validation.ValidateBrandURL(arg) ? "Ugyldigt URL" : null!;
        }

        public static string CvrValidation(string arg)
        {
            return !Validation.ValidateCVR(arg) ? "CVR skal være mindst 8 tal" : null!;
        }

        public static string PhoneValidation(string arg)
        {
            return !Validation.ValidatePhone(arg) ? "Ugyldigt nummer" : null!;
        }

        public static string EmailValidation(string arg)
        {
            return !Validation.ValidateEmail(arg) ? "Ugyldig email" : null!;
        }
    }
}
