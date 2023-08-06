using HarmonyLib;
using System;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(SetSlotPortrait), "SetResourceTexture", new Type[] { typeof(string) })]
    class SetSlotPortrait_SetResourceTexture
    {
        static bool Prefix(SetSlotPortrait __instance)
        {
            if (BadgePlugin.LocalPlayerBadge && __instance.rawImage.texture == BadgePlugin.LocalPlayerBadge) return false;
            return true;
        }
    }
}
