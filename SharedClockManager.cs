using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace TrickyMultiplayerPlus
{
    public static class SharedClockManager
    {
        private static GameObject _canvasObject;
        private static CountdownDisplay _display;

        // Gọi hàm này mỗi khi TimeAttackHUD được tạo.
        // Lần đầu tiên sẽ tạo đồng hồ, các lần sau sẽ bỏ qua vì đã có sẵn.
        public static void EnsureClockExists(GameModel gameModel)
        {
            if (_canvasObject != null)
            {
                // Đã tồn tại rồi, chỉ cập nhật lại gameModel (đề phòng trường hợp trận mới)
                if (_display != null)
                {
                    _display.gameModel = gameModel;
                }
                return;
            }

            UnityEngine.Debug.Log("[TA-DEBUG-SHARED] Creating shared clock canvas");

            // Tạo Canvas riêng, phủ toàn màn hình, không thuộc viewport người chơi nào
            _canvasObject = new GameObject("TA_SharedClockCanvas");
            UnityEngine.Object.DontDestroyOnLoad(_canvasObject);

            Canvas canvas = _canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            CanvasScaler scaler = _canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ConstantPixelSize;

            _canvasObject.AddComponent<GraphicRaycaster>();

            // Tạo chữ số đồng hồ
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

        // Gọi hàm này khi trận đấu kết thúc / thoát về menu, để lần chơi sau tạo lại đồng hồ mới sạch sẽ
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