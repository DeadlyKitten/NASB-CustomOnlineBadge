﻿using HarmonyLib;
using Nick;
using SlapNetwork;
using System;

namespace CustomOnlineBadge.Patches
{
    [HarmonyPatch(typeof(OnlineLobby), MethodType.Constructor, new Type[] { typeof(IClient), typeof(ILobby), typeof(OnlineConnections), typeof(OnlineBlockList), typeof(OnlineLobby.MatchRules) })]
    class OnlineLobby_ctor
    {
        static void Postfix(OnlineLobby __instance) => OnlineManager.JoinLobby(__instance);
    }
}
