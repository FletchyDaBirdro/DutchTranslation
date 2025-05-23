﻿using RWCustom;
using System;
using MonoMod.Cil;
using Mono.Cecil.Cil;

namespace DutchTranslation
{
    public class WatcherStuff
    {
        public static void ApplyHooks() 
        {
            On.Watcher.SpinningTop.SpinningTopConversation.SpinningTopLaughLevel += SpinningTopConversation_SpinningTopLaughLevel;           
            IL.Watcher.SpinningTop.SpinningTopConversation.Update += new ILContext.Manipulator(SpinningTopConversation_Update);
        }        

        private static int SpinningTopConversation_SpinningTopLaughLevel(On.Watcher.SpinningTop.SpinningTopConversation.orig_SpinningTopLaughLevel orig, Watcher.SpinningTop.SpinningTopConversation self, string dialog)
        {
            InGameTranslator.LanguageID currLang = Custom.rainWorld.inGameTranslator.currentLanguage;

            if (currLang == LangID.LanguageID.Dutch)
            {
                if (dialog.Contains("OAOA"))
                {
                    return 2;
                }
                if (dialog.Contains("OA!") || dialog.Contains("OA "))
                {
                    return 1;
                }
            }
            return orig(self, dialog);
        }

        public InGameTranslator.LanguageID GetLang
        {
            get
            {
                return Custom.rainWorld.options.language;
            }
        }

        static bool IsEngItEspPorNed(bool isEngItEspPor, WatcherStuff self) => isEngItEspPor || self.GetLang == LangID.LanguageID.Dutch;

        private static void SpinningTopConversation_Update(ILContext il) 
        { 
            ILCursor c = new ILCursor(il);

            try 
            {
                c.GotoNext(
                    MoveType.After,
                    x => x.MatchLdsfld(typeof(InGameTranslator.LanguageID).GetField("Portuguese")),
                    x => x.MatchCallOrCallvirt(typeof(ExtEnum<InGameTranslator.LanguageID>).GetMethod("op_Equality"))
                    );

                c.MoveAfterLabels();
                c.Emit(OpCodes.Ldloc, 1);
                c.EmitDelegate(IsEngItEspPorNed);
                
                //MainPlugIn.TransLogger.LogDebug(il);
            }
            catch (Exception ex) 
            { 
                MainPlugIn.TransLogger.LogError(ex);
            }
        }

        public static void UnapplyHooks() 
        {
            On.Watcher.SpinningTop.SpinningTopConversation.SpinningTopLaughLevel -= SpinningTopConversation_SpinningTopLaughLevel;            
            IL.Watcher.SpinningTop.SpinningTopConversation.Update -= new ILContext.Manipulator(SpinningTopConversation_Update);
        }
    }
}
