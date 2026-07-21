namespace TrickyMultiplayerPlus
{
    using UnityEngine;

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

            // Đồng hồ giờ dùng chung cho mọi người chơi, chỉ tạo 1 lần duy nhất
            SharedClockManager.EnsureClockExists(gameModel);
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD: shared clock ensured");

            base.Hide(true);
            UnityEngine.Debug.Log("[TA-DEBUG] TimeAttackHUD ctor END, id=" + id);
        }
    }
}