using System;
using System.Reflection;
using HarmonyLib;
using BepInEx.Logging;

namespace TimeAttackMod.Patches
{
    [HarmonyPatch(typeof(AbstractEffect))]
    internal static class AbstractEffectDebugPatch
    {
        // Lấy field private "complete" và "_completed" của AbstractEffect qua reflection
        private static readonly FieldInfo _completeField =
            AccessTools.Field(typeof(AbstractEffect), "complete");
        private static readonly FieldInfo _completedField =
            AccessTools.Field(typeof(AbstractEffect), "_completed");

        // Ghi log ra, dùng chung logger của plugin bạn (thay bằng logger BepInEx bạn đã có)
        internal static ManualLogSource Log;

        [HarmonyPatch("_OnComplete")]
        [HarmonyPrefix]
        private static bool Prefix_OnComplete(AbstractEffect __instance)
        {
            _completedField.SetValue(__instance, true);

            var del = _completeField.GetValue(__instance) as MulticastDelegate;
            if (del != null)
            {
                foreach (Delegate handler in del.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(__instance);
                    }
                    catch (Exception e)
                    {
                        Log?.LogError(
                            $"[AbstractEffect._OnComplete] Effect id={__instance.id} " +
                            $"type={__instance.GetType().Name} " +
                            $"handler={handler.Method.DeclaringType}.{handler.Method.Name} threw: {e}"
                        );
                    }
                }
            }

            return false; // bỏ qua method gốc vì đã tự chạy thay
        }
    }
}