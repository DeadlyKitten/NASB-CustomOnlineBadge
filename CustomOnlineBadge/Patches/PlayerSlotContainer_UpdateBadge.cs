using HarmonyLib;
using Nick;
using SMU.Reflection;
using SMU.Utilities;
using System;
using System.Collections;
using UnityEngine;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(PlayerSlotContainer), "UpdateBadge")]
    class PlayerSlotContainer_UpdateBadge
    {
        static bool Prefix(OnlineMatchInfoListItem onlineMatchInfoListItem, ref SetSlotPortrait ___onlineBadge)
        {
            if (onlineMatchInfoListItem.currUser.IsLocal)
            {
                if (!BadgePlugin.LocalPlayerBadge) return true;

                BadgePlugin.LogDebug("PlayerSlotContainer.UpdateBadge: Setting badge for local player");
                SetBadge(___onlineBadge, BadgePlugin.LocalPlayerBadge);
                return false;
            }
            else if (OnlineManager.TryGetBadgeForUser(onlineMatchInfoListItem.currUser.Id, out var badge))
            {
                BadgePlugin.LogDebug($"PlayerSlotContainer.UpdateBadge: Setting badge for player: {onlineMatchInfoListItem.currUser.Id.UserId}");
                SetBadge(___onlineBadge, badge);
                return false;
            }

            return true;
        }

        static void SetBadge(SetSlotPortrait avatar, Texture2D badge)
        {
            avatar.gameObject.SetActive(true);
            avatar.SetNativeSize = false;
            avatar.rawImage.texture = badge;
            avatar.SetField("seekTex", false);
            avatar.rawImage.enabled = true;

            BadgePlugin.Instance.StartCoroutine(KeepBadgeVisible(avatar));
        }

        static IEnumerator KeepBadgeVisible(SetSlotPortrait avatar)
        {
            while (avatar)
            {
                if (!avatar.gameObject.activeSelf)
                    avatar.gameObject.SetActive(true);
                if (!avatar.rawImage.enabled)
                    avatar.rawImage.enabled = true;
                yield return null;
            }
        }
    }
}
