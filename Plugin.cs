using BepInEx;
using HarmonyLib;

namespace BadApple;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        Logger.LogInfo($"Loading Bad Apple support plugin v{PluginInfo.PLUGIN_VERSION}");

        Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        harmony.PatchAll();
    }
}