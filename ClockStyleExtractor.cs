using UnityEngine;
using TMPro;

namespace TrickyMultiplayerPlus
{
    public static class ClockStyleExtractor
    {
        public static void ApplyPaceLineStyle(TextMeshProUGUI targetText)
        {
            GameObject temp = null;
            try
            {
                temp = Singleton<ResourceManager>.instance.InstantiateByName(
                    "PACE_LINE_PERSONAL_BEST", Vector3.zero, null);

                if (temp == null)
                {
                    UnityEngine.Debug.LogError("[TA-DEBUG-STYLE] Could not instantiate PACE_LINE_PERSONAL_BEST");
                    return;
                }

                GameObject textChild = WBTools.FindChildByName(temp, "Graphic/TextLeft");
                TextMeshPro sourceTmp = textChild != null ? textChild.GetComponent<TextMeshPro>() : null;

                if (sourceTmp != null)
                {
                    targetText.font = sourceTmp.font;
                    targetText.fontSharedMaterial = sourceTmp.fontSharedMaterial;
                    targetText.color = sourceTmp.color;
                    UnityEngine.Debug.Log("[TA-DEBUG-STYLE] Applied pace-line font/material successfully");
                }
                else
                {
                    UnityEngine.Debug.LogError("[TA-DEBUG-STYLE] TextMeshPro child not found on pace line prefab");
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError("[TA-DEBUG-STYLE] Exception: " + e);
            }
            finally
            {
                if (temp != null)
                {
                    UnityEngine.Object.Destroy(temp);
                }
            }
        }
    }
}