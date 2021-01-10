using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Policy;
using System.Threading.Tasks;

namespace PinBackendSystem.Util
{
    public static class PinrumahConstants
    {
        public const string RoleCodeAdmin = "5413";
        public const string RoleCodeSurveyor = "2258";
        public const string RoleCodeTele = "2387";
        public const string RoleCodeAds = "2413";

        public const string RoleTextAdmin = "admin";
        public const string RoleTextMarketing = "marketing";
        public const string RoleTextSurveyor = "surveyor";
        public const string RoleTextTele = "tele";
        public const string RoleTextAds = "ads";

        public const int SurveyStatus = 0;
        public const int AdsStatus = 1;
        public const int TeleStatus = 5;
    }
}
