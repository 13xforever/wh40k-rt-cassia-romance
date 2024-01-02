using HarmonyLib;
using System.Reflection;
using UnityModManagerNet;

namespace RT.UMM.CassiaRomanceF;

#if DEBUG
[EnableReloading]
#endif
static class Main
{
    internal static Harmony HarmonyInstance;
    internal static UnityModManager.ModEntry.ModLogger log;
    internal static bool enabled;

    static bool Load(UnityModManager.ModEntry modEntry)
    {
        log = modEntry.Logger;
#if DEBUG
        modEntry.OnUnload = OnUnload;
        modEntry.OnToggle = OnToggle; 
#endif
        modEntry.OnGUI = OnGUI;
        HarmonyInstance = new(modEntry.Info.Id);
        HarmonyInstance.PatchAll(Assembly.GetExecutingAssembly());
        return true;
    }

    private static bool OnToggle(UnityModManager.ModEntry modEntry, bool state)
    {
        enabled = state;
        return true;
    }

    static void OnGUI(UnityModManager.ModEntry modEntry)
    {
    }

#if DEBUG
    static bool OnUnload(UnityModManager.ModEntry modEntry)
    {
        HarmonyInstance.UnpatchAll(modEntry.Info.Id);
        return true;
    }
#endif
}