using HarmonyLib;
using UnityEngine;

namespace BadApple;

[HarmonyPatch(typeof(GameController), nameof(GameController.Update))]
internal class PatchGameControllerUpdate
{
    private const float FPS = 30;

    static void Postfix(GameController __instance)
    {
        if (__instance.beatspermeasure < 10000) return;

        // Sync position to frames
        float trackTime = Mathf.Floor(__instance.musictrack.time * FPS) / FPS;
        float noteHolderX = __instance.zeroxpos + trackTime * -__instance.trackmovemult;
        __instance.noteholderr.anchoredPosition3D = new Vector3(noteHolderX, 0, 0);

        NoteManager.Update(__instance, noteHolderX);
    }
}
