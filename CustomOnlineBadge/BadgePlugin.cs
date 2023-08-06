using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using SMU.Utilities;
using System;
using System.IO;
using UnityEngine;

namespace CustomOnlineBadge
{
    [BepInPlugin("com.steven.nasb.custombadge", "Custom Online Badge", "1.0.1")]
    public class BadgePlugin : BaseUnityPlugin
    {
        internal static BadgePlugin Instance { get; private set; }
        internal static Texture2D LocalPlayerBadge { get; private set; }
        internal static string BadgeBase64 { get; private set; }

        private ConfigEntry<bool> configEnable;
        internal static bool ModEnabled => Instance.configEnable.Value;
        private ConfigEntry<bool> configShare;
        internal static bool ShareEnabled => Instance.configShare.Value;
        private ConfigEntry<bool> configDownload;
        internal static bool DownloadEnabled => Instance.configDownload.Value;

        private void Awake()
        {
            if (Instance)
            {
                DestroyImmediate(this);
                return;
            }
            Instance = this;

            LogInfo("Starting up...");

            LoadConfig();

            if (!ModEnabled)
            {
                LogWarning("Mod disabled!");
                return;
            }

            LoadLocalPlayerBadge();


            if ((LocalPlayerBadge && ShareEnabled) || DownloadEnabled)
            {
                LogInfo("Patching...");
                var harmony = new Harmony("com.steven.nasb.custombadge");
                harmony.PatchAll();
                LogInfo("Patched successfully!");
            }

            LogInfo("Finished loading!");
        }

        private void LoadLocalPlayerBadge()
        {
            var filePath = Path.Combine(Paths.BepInExRootPath, "CustomBadge.png");

            if (File.Exists(filePath))
            {
                Logger.LogInfo("Found custom badge!");
                LocalPlayerBadge = ImageHelper.LoadTextureFromFile(filePath);
                LocalPlayerBadge.wrapMode = TextureWrapMode.Clamp;
                LocalPlayerBadge.filterMode = FilterMode.Trilinear;

                if (!LocalPlayerBadge)
                {
                    LogWarning("Failed to load badge!");
                    return;
                }
                else LogInfo("Badge loaded successfully!");

                var bytes = File.ReadAllBytes(filePath);
                BadgeBase64 = Convert.ToBase64String(bytes);

                // may need to resize image if it's too big for online play (limit: 32767 characters, or short.MaxValue)
                var scaleFactor = 1;
                while (BadgeBase64.Length > short.MaxValue && scaleFactor < 10)
                {
                    if (scaleFactor == 1) LogWarning($"Badge is too big! Attempting to resize... (size: {BadgeBase64.Length} / {short.MaxValue})");

                    scaleFactor++;
                    LogDebug($"Resizing image with scale factor 1 / {scaleFactor}");
                    var newScaleX = LocalPlayerBadge.width / scaleFactor;
                    var newScaleY = LocalPlayerBadge.height / scaleFactor;
                    var resizedTexture = ResizeTexture(LocalPlayerBadge, newScaleX, newScaleY);
                    bytes = resizedTexture.EncodeToPNG();

                    BadgeBase64 = Convert.ToBase64String(bytes);
                }

                if (BadgeBase64.Length > short.MaxValue)
                {
                    LogError($"Unable to resize badge for online play! (size: {BadgeBase64.Length} / {short.MaxValue})");
                    BadgeBase64 = string.Empty;
                }
                else LogInfo($"Badge resized successfully! (size: {BadgeBase64.Length} / {short.MaxValue} | scale factor: 1 / {scaleFactor})");
            }
        }

        private void LoadConfig()
        {
            LogInfo("Loading config!");
            configEnable = Config.Bind("General", "Enable Mod", true, "Enable the mod?");
            configShare = Config.Bind("Online", "Share Custom Badge", true, "Share custom badge online?");
            configDownload = Config.Bind("Online", "Download Custom Badges", false, "Download custom badges from online players?");
            LogInfo("Config loaded successfully!");
            LogDebug($"Mod Enabled:      {ModEnabled}");
            LogDebug($"Share Enabled:    {ShareEnabled}");
            LogDebug($"Download Enabled: {DownloadEnabled}");
        }

        public Texture2D ResizeTexture(Texture2D texture, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture, rt);
            Texture2D result = new Texture2D(targetX, targetY);
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }

        #region logging
        internal static void LogDebug(string message) => Instance.Log(message, LogLevel.Debug);
        internal static void LogInfo(string message) => Instance.Log(message, LogLevel.Info);
        internal static void LogWarning(string message) => Instance.Log(message, LogLevel.Warning);
        internal static void LogError(string message) => Instance.Log(message, LogLevel.Error);
        private void Log(string message, LogLevel logLevel) => Logger.Log(logLevel, message);
        #endregion
    }
}
