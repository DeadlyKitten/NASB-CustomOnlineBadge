using HarmonyLib;
using System;
using System.Reflection;
using Nick;
using SMU.Reflection;
using System.Collections;
using UnityEngine;
using SlapNetwork;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch]
    class VersusPlayerSlot_Setup_FoundAvatar
    {
        public static MethodBase TargetMethod()
        {
            return Type.GetType("VersusPlayerSlot+<>c__DisplayClass10_0,Assembly-CSharp", false, true)
                .GetMethod("<Setup>g__FoundAvatar|1", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        public static void Postfix(UserData userData, OnlineMatchInfoListItem ___onlineMatchInfoList, VersusPlayerSlot ___0)
        {
            BadgePlugin.LogInfo("Found Avatar!");

            BadgePlugin.LogInfo($"{___onlineMatchInfoList.currUser.IsLocal}");

            if (OnlineManager.TryGetBadgeForUser(___onlineMatchInfoList.currUser.Id, out var badge))
            {
                var playerInformation = ___0.GetField<VersusPlayerSlot, PlayerInformation>("playerInformation");
                var avatar = playerInformation.Avatar;

                avatar.rawImage.texture = badge;
                avatar.SetField("seekTex", false);
                avatar.rawImage.enabled = true;
            }
        }

        static IEnumerator ReenableRawImageDelayed(SetSlotPortrait onlineBadge)
        {
            yield return new WaitUntil(() => !onlineBadge || onlineBadge.rawImage.enabled == false);
            onlineBadge.rawImage.enabled = true;
        }
    }
}
