using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace SPA_TemplateHelpers
{
    public static class SpaSettings
    {
        public static string ForceBaseUrl
        {
            get
            {
                string value = ConfigurationManager.AppSettings["ForceBaseUrl"];
                return value;
            }
        }

        public static bool IsRegistrationEnabled
        {
            get
            {
                string conf = ConfigurationManager.AppSettings["IsRegistrationEnabled"];
                if (string.IsNullOrEmpty(conf))
                    return false;

                bool ris = false;
                bool.TryParse(conf, out ris);
                return ris;
            }
        }

        public static bool ShouldSignInAfterRegister
        {
            get
            {
                string conf = ConfigurationManager.AppSettings["ShouldSignInAfterRegister"];
                if (string.IsNullOrEmpty(conf))
                    return false;

                bool ris = false;
                bool.TryParse(conf, out ris);
                return ris;
            }
        }

        public static bool ShouldTraceFullExceptions
        {
            get
            {
                string conf = ConfigurationManager.AppSettings["ShouldTraceFullExceptions"];
                if (string.IsNullOrEmpty(conf))
                    return false;

                bool ris = false;
                bool.TryParse(conf, out ris);
                return ris;
            }
        }
    }
}