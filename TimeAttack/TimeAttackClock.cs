using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TrickyMultiplayerPlus
{
    // ====== Phần 1: hiển thị số đếm ngược, gắn lên GameObject có TextMeshProUGUI ======
    public class CountdownDisplay : MonoBehaviour
    {
        public GameModel gameModel;
        private DataModelFloat _timeLeftModel;
        private TextMeshProUGUI _text;
        private bool _loggedFirstUpdate;

        private void Awake()
        {
            UnityEngine.Debug.Log("[TA-DEBUG] CountdownDisplay Awake");
            this._text = base.GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            if (!this._loggedFirstUpdate)
            {
                UnityEngine.Debug.Log("[TA-DEBUG] CountdownDisplay first Update");
                this._loggedFirstUpdate = true;
            }

            if (this._timeLeftModel == null)
            {
                if (this.gameModel == null) { return; }
                try
                {
                    this._timeLeftModel = this.gameModel.GetDataModel<DataModelFloat>("TIME_LEFT");
                    UnityEngine.Debug.Log("[TA-DEBUG] CountdownDisplay got TIME_LEFT model");
                }
                catch
                {
                    return;
                }
            }

            if (this._timeLeftModel == null || this._text == null) { return; }

            float t = Mathf.Max(this._timeLeftModel.value, 0f);
            this._text.text = TimeUtil.FormatTime(t, false);
        }
    }

    // ====== Phần 2: lấy font/màu từ đồng hồ có sẵn của game (mode Race) ======
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

    // ====== Phần 3: quản lý việc chỉ tạo 1 đồng hồ chung duy nhất ======
    public static class SharedClockManager
    {
        private static GameObject _canvasObject;
        private static CountdownDisplay _display;

        public static void EnsureClockExists(GameModel gameModel)
        {
            if (_canvasObject != null)
            {
                if (_display != null)
                {
                    _display.gameModel = gameModel;
                }
                return;
            }

            UnityEngine.Debug.Log("[TA-DEBUG-SHARED] Creating shared clock canvas");

            _canvasObject = new GameObject("TA_SharedClockCanvas");
            UnityEngine.Object.DontDestroyOnLoad(_canvasObject);

            Canvas canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            CanvasScaler scaler = _canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            _canvasObject.AddComponent<GraphicRaycaster>();

            GameObject clockObject = new GameObject("TA_CountdownClock", typeof(RectTransform), typeof(TextMeshProUGUI), typeof(CountdownDisplay));
            clockObject.transform.SetParent(_canvasObject.transform, false);

            RectTransform rt = clockObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -20f);
            rt.sizeDelta = new Vector2(200f, 60f);

            TextMeshProUGUI text = clockObject.GetComponent<TextMeshProUGUI>();
            text.fontSize = 36;
            text.alignment = TextAlignmentOptions.Center;
            text.color = Color.white;
            ClockStyleExtractor.ApplyPaceLineStyle(text);

            _display = clockObject.GetComponent<CountdownDisplay>();
            _display.gameModel = gameModel;

            UnityEngine.Debug.Log("[TA-DEBUG-SHARED] Shared clock created successfully");
        }

        public static void DestroyClock()
        {
            if (_canvasObject != null)
            {
                UnityEngine.Object.Destroy(_canvasObject);
                _canvasObject = null;
                _display = null;
                UnityEngine.Debug.Log("[TA-DEBUG-SHARED] Shared clock destroyed");
            }
        }
    }
}