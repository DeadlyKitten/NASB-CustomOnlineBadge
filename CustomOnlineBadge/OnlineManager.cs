using Nick;
using SlapNetwork;
using SMU.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomOnlineBadge
{
    static class OnlineManager
    {
        private const string CUSTOM_BADGE_KEY = "custom_badge";

        private static OnlineLobby _lobby;

        public static void JoinLobby(OnlineLobby lobby)
        {
            _lobby = lobby;

            if (BadgePlugin.ShareEnabled && !string.IsNullOrEmpty(BadgePlugin.BadgeBase64))
            {
                BadgePlugin.LogInfo("Uploading badge to lobby");
                BadgePlugin.LogDebug($"Badge Data: {BadgePlugin.BadgeBase64}");
                var data = new List<LobbyDataPair>() { new LobbyDataPair(CUSTOM_BADGE_KEY, BadgePlugin.BadgeBase64) };
                lobby.BaseLobby.SetUserData(data);
            }
            else BadgePlugin.LogInfo("Sharing badge online disabled! Skipping...");
        }

        public static bool TryGetBadgeForUser(IUser user, out Texture2D badge)
        {
            badge = null;

            if (BadgePlugin.DownloadEnabled)
            {
                var badgeBase64 = _lobby.BaseLobby.GetUserData(user, CUSTOM_BADGE_KEY);

                if (!string.IsNullOrEmpty(badgeBase64))
                {
                    BadgePlugin.LogInfo($"Downloaded badge for user: {user.GetUsername()}");
                    BadgePlugin.LogDebug($"Badge Data: {badgeBase64}");
                    var bytes = Convert.FromBase64String(badgeBase64);
                    badge = ImageHelper.LoadTextureRaw(bytes);
                    badge.wrapMode = TextureWrapMode.Clamp;
                    badge.filterMode = FilterMode.Trilinear;
                }
            }
            else BadgePlugin.LogInfo("Downloading online badges disabled! Skipping...");

            return badge != null;
        }
    }
}
