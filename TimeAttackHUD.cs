namespace TrickyMultiplayerPlus
{
    using UnityEngine;
    using UnityEngine.UI;

    public class TimeAttackHUD : AbstractHUD
    {
        public TimeAttackHUD(GameModel gameModel, Rect viewPort, string id)
            : base("LAST_WIZARD_STANDING_HUD", gameModel, viewPort)
        {
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD ctor START, id=" + id);

            GameObject gameObject = Singleton<ResourceManager>.instance.InstantiateByName("HUD_NEXT_BRICK_VIEW", Vector3.zero, base.skin);
            gameObject.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
            base._AddView(new NextBrickView(gameModel.GetDataModel<DataModelString>("NEXT_BRICK"), gameObject)
            {
                hidePosition = new Vector2(0f, 150f)
            });
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD: NextBrickView OK");

            GameObject clockObject = new GameObject("TA_CountdownClock", typeof(RectTransform), typeof(Text), typeof(CountdownDisplay));
            clockObject.transform.SetParent(base.skin.transform, false);
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD: clockObject created");

            RectTransform rt = clockObject.GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -20f);
            rt.sizeDelta = new Vector2(200f, 60f);

            Text text = clockObject.GetComponent<Text>();
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 36;
            text.alignment = TextAnchor.MiddleCenter;
            text.color = Color.white;
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD: Text configured");

            CountdownDisplay display = clockObject.GetComponent<CountdownDisplay>();
            display.gameModel = gameModel;
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD: CountdownDisplay wired");

            base.Hide(true);
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD ctor END, id=" + id);
        }
    }
}