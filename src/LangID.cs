using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTranslation
{
    public static class LangID
    {
        public class LanguageID
        {
            public static global::InGameTranslator.LanguageID Dutch;
        }

        public static void RegisterValues()
        {
            LanguageID.Dutch = new InGameTranslator.LanguageID("Dutch", register : true);
        }

        public static void UnregisterValues() 
        {
            if (LanguageID.Dutch != null)
            {
                LanguageID.Dutch.Unregister();
                LanguageID.Dutch = null;
            } 
        }
    }   
}
