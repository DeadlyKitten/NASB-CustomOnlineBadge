using HarmonyLib;
using Nick;
using SMU.Reflection;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(ResultUImanager), "MenuOpen")]
    class ResultUImanager_MenuOpen
    {
        static void Postfix(ref PlayerInformation ____playerInformation)
        {
            if (BadgePlugin.LocalPlayerBadge)
            {
                var avatar = ____playerInformation.GetField<PlayerInformation, SetSlotPortrait>("avatar");

                BadgePlugin.LogDebug("OnlineMenuContent.MenuOpen: Setting badge for local player");
                avatar.rawImage.texture = BadgePlugin.LocalPlayerBadge;
                avatar.SetField("seekTex", false);
                avatar.rawImage.enabled = true;
            }
        }
    }
}
