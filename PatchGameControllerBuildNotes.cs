using HarmonyLib;

namespace BadApple;

[HarmonyPatch(typeof(GameController), nameof(GameController.buildNotes))]
internal class PatchGameControllerBuildNotes
{
    static bool Prefix(GameController __instance)
    {
        if (__instance.beatspermeasure < 10000) return true;
        
        // Replace the normal buildNotes process with our efficient version
        NoteManager.Init(__instance.leveldata);
        return false;
    }
}
