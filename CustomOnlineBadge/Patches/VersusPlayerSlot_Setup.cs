using HarmonyLib;
using Nick;
using SMU.Reflection;
using System;
using UnityEngine;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(VersusPlayerSlot), "Setup", new Type[] { typeof(OnlineMatchInfoListItem) })]
    class VersusPlayerSlot_Setup
    {
        static void Postfix(OnlineMatchInfoListItem onlineMatchInfoList, PlayerInformation ___playerInformation)
        {
            if (onlineMatchInfoList == null || onlineMatchInfoList.currUser == null) return;

            var avatar = ___playerInformation.Avatar;

            if (onlineMatchInfoList.currUser.IsLocal)
            {
                BadgePlugin.LogDebug("VersusPlayerSlot.Setup: Setting badge for local player");
                SetBadge(avatar, BadgePlugin.LocalPlayerBadge);
            }
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
