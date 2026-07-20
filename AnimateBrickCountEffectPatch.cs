using HarmonyLib;
using UnityEngine;

namespace TrickyMultiplayerPlus.Patches
{
    [HarmonyPatch(typeof(AnimateBrickCountEffect))]
    internal static class AnimateBrickCountEffectPatch
    {
        [HarmonyPatch("_HandleBrickCountChanged")]
        [HarmonyPrefix]
        private static bool Prefix_HandleBrickCountChanged(
            AnimateBrickCountEffect __instance,
            object ____bricksToPlaceView) // field private, Harmony inject theo naming convention ____fieldName
        {
            if (____bricksToPlaceView == null)
            {
                Debug.Log("[TA-DEBUG-FIX] _bricksToPlaceView is NULL, skipping ForceShowValue to avoid NRE");
                return false; // bỏ qua method gốc, tránh crash tween
            }
            return true; // có view thật thì chạy bình thường
        }
    }
}