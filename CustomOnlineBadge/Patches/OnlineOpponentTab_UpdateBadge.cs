using HarmonyLib;
using Nick;
using SMU.Reflection;
using UnityEngine;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(OnlineOpponentTab), "UpdateBadge")]
    class OnlineOpponentTab_UpdateBadge
    {
        static bool Prefix(OnlineMatchInfoListItem onlineMatchInfoListItem, ref SetSlotPortrait ___badgeImage)
        {
            if (onlineMatchInfoListItem.currUser.IsLocal && BadgePlugin.LocalPlayerBadge)
            {
                BadgePlugin.LogDebug("OnlineOpponentTab.UpdateBadge: Setting badge for local player");
                SetBadge(___badgeImage, BadgePlugin.LocalPlayerBadge);
                return false;
            }
            else if (OnlineManager.TryGetBadgeForUser(onlineMatchInfoListItem.currUser.Id, out var badge))
            {
                BadgePlugin.LogDebug($"OnlineOpponentTab.UpdateBadge: Setting badge for player: {onlineMatchInfoListItem.currUser.Id.GetUsername()}");
                SetBadge(___badgeImage, badge);
                return false;
            }

            return true;
        }

        static void SetBadge(SetSlotPortrait avatar, Texture2D badge)
        {
            avatar.gameObject.SetActive(true);
            avatar.rawImage.texture = badge;
            avatar.SetField("seekTex", false);
            avatar.rawImage.enabled = true;
        }
    }
}
