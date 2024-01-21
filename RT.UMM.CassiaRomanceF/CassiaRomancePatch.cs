using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.JsonSystem.Helpers;
using Kingmaker.Controllers.Dialog;
using Kingmaker.Designers.EventConditionActionSystem.Conditions;
using Kingmaker.ElementsSystem;
using Kingmaker.ElementsSystem.Interfaces;
using UnityEngine;

namespace RT.UMM.CassiaRomanceF;

[HarmonyPatch(typeof(ConditionsChecker), nameof(ConditionsChecker.Check), [typeof(IConditionDebugContext)])]
internal static class CassiaRomancePatch
{
    private static readonly HashSet<string> TargetIds =
    [
        "17b34e1ae36443408805af3a3c2866f7", // Blueprints\World\Dialogs\Ch3\Chasm\PitCassia\Answer_6.jbp
        "c051d0c9f2ba4c23bff1d1e6f2cfe13d", // Blueprints\World\Dialogs\Ch3\Chasm\PitCassia\Answer_11.jbp
        "3d24df76aacf4e2db047cf47ef3474d5", // Blueprints\World\Dialogs\Ch3\Chasm\PitCassia\Answer_12.jbp
        "b3601cd9e84d43dbb4078bf77c89d728", // Blueprints\World\Dialogs\Ch3\Chasm\PitCassia\Answer_19.jbp
        "7f71e0b93dd9420d87151fc3e7114865", // Blueprints\World\Dialogs\Ch3\Chasm\PitCassia\Cue_29.jbp
        "a903589840ba4ab683d6e6b9f985d458", // Blueprints\World\Dialogs\Companions\CompanionDialogues\Navigator\Cue_24.jbp
        "588a3c2e96c6403ca2c7104949b066e4", // Blueprints\World\Dialogs\Companions\CompanionDialogues\Navigator\Cue_47.jbp
        "966f0cc2defa42bd836950aa1ebcde72", // Blueprints\World\Dialogs\Companions\CompanionQuests\Navigator\Navigator_Q1\CassiaSeriousTalk\Answer_8.jbp
        "bf7813b4ee3d49cdbc6305f454479db3", // Blueprints\World\Dialogs\Companions\CompanionQuests\Navigator\Navigator_Q2\Cassia_Q2_BE\Cue_0037.jbp
        "eb76f93740824d16b1e1f54b82de21e0", // Blueprints\World\Dialogs\Companions\Romances\Cassia\StartingEvent\Answer_5.jbp
        "c292b399f4344a639ccb4df9ba66329e", // Blueprints\World\Dialogs\Companions\Romances\Cassia\StartingEvent\Answer_8.jbp
        "56bbf1612e05489ba44bb4a52718e222", // Blueprints\World\Dialogs\Companions\Romances\Cassia\StartingEvent\Answer_10.jbp
        "85b651edb4f74381bbe762999273c6ec", // Blueprints\World\Dialogs\Companions\Romances\Cassia\StartingEvent\Answer_0017.jbp
        "95b0ba7d08e34f6c895b2fbeb53ea404", // Blueprints\World\Dialogs\Companions\Romances\Cassia\StartingEvent\CassFirstTimeBlushing_a.jbp
    ];
    
    static bool Prefix(ConditionsChecker __instance, IConditionDebugContext debugContext)
    {
        if (__instance is null || !__instance.HasConditions)
            return true;

        if (debugContext is not IScriptableObjectWithAssetId bp)
            return true;

        if (!TargetIds.Contains(bp.AssetGuid))
            return true;

        if (__instance.Conditions.All(c => c is not PcMale))
            return true;

        DialogDebug.Add((BlueprintScriptableObject)debugContext, "patching conditions to remove PcMale check", Color.magenta);
        Main.log.Log("Patching conditions to remove PcMale check");
        var newConditions = __instance.Conditions.Where(c => c is not PcMale).ToArray();
        __instance.Conditions = newConditions;
        Main.log.Log($"New condition count: {newConditions.Length}");
        return true;
    }
}