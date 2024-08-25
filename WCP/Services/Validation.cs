﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WCPShared.Services
{
    public static class Validation
    {
        public static bool ValidateEmail(string email)
        {
            if (!Regex.IsMatch(email, @"^\S+@\S+\.\w{1,24}$"))
                return false;
            return true;
        }

        public static bool ValidatePhone(string phoneNo)
        {
            if (String.IsNullOrEmpty(phoneNo) || !Regex.IsMatch(phoneNo, @"^\d{8,11}$"))
                return false;
            return true;
        }

        public static bool ValidateCVR(string CVR)
        {
            if (!Regex.IsMatch(CVR, @"^\d{8}$"))
                return false;
            return true;
        }

        public static bool ValidateDisplayName(string name)
        {
            if (name.Length < 2)
                return false;
            return true;
        }
    }
}
