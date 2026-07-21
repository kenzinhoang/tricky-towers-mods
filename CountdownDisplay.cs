using UnityEngine;
using TMPro;

namespace TrickyMultiplayerPlus
{
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
}