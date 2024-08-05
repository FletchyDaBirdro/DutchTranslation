using MonoMod.Cil;
using System;
using System.Collections.Generic;
using System.IO;

namespace DutchTranslation
{
    public class GeneralStuff
    {
        public static void ApplyHooks()
        {            
            On.MoreSlugcats.ChatlogData.DecryptResult += ChatlogData_DecryptResult;
            On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.GetSliderWidth += ColorSlider_GetSliderWidth;
        }

        public static void ApplyEarlyHooks()
        {
            IL.InGameTranslator.LoadShortStrings += new ILContext.Manipulator(InGameTranslator_LoadShortStrings);
        }
      
        private static void InGameTranslator_LoadShortStrings(ILContext il)
        {
            ILCursor c = new ILCursor(il);
            int target = -1;

            try
            {
                c.GotoNext(
                 x => x.MatchCallOrCallvirt(typeof(InGameTranslator).GetMethod("get_currentLanguage")), //36
                 x => x.MatchCallOrCallvirt(out _), //37
                 x => x.MatchStloc(out target), //38
                 x => x.MatchLdloc(out _), //16
                 x => x.MatchLdcI4(out _), //17
                 x => x.MatchLdelemRef(), //18
                 x => x.MatchCallOrCallvirt(typeof(System.IO.File), "Exists"), //19
                 x => x.MatchBrtrue(out _), //20
                 x => x.MatchRet()); //21
                {
                    //index starts at 36
                    c.Index += 3;
                    c.RemoveRange(6);
                    c.Emit(Mono.Cecil.Cil.OpCodes.Ldloca, target);
                    c.Emit<GeneralStuff>(Mono.Cecil.Cil.OpCodes.Call, nameof(LoadStrings));
                }
            }
            catch (Exception ex)
            {
                MainPlugIn.TransLogger.LogError(ex);
                MainPlugIn.TransLogger.LogMessage("Whoopsie doodles. The ILHook failed!");
            }
        }

        private static void LoadStrings(ref string[] files)
        {
            List<string> list = new List<string>(files);
            list.RemoveAll((string p) => !File.Exists(p));

            if (list.Count != files.Length)
            {
                files = list.ToArray();
            }
        }

        private static string ChatlogData_DecryptResult(On.MoreSlugcats.ChatlogData.orig_DecryptResult orig, string result, string path)
        {
            if (result[0] == '0')
            {
                return result;
            }
            return orig(result, path);
        } 
        
        private static float ColorSlider_GetSliderWidth(On.JollyCoop.JollyMenu.ColorChangeDialog.ColorSlider.orig_GetSliderWidth orig, InGameTranslator.LanguageID lang)
        {            
            if (lang == LangID.LanguageID.Dutch)
            {
                return 160f;
            }
            return orig(lang);
        }
    }
}
