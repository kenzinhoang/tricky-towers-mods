namespace TrickyMultiplayerPlus
{
    using System.Collections.Generic;
    using UnityEngine;

    public class MultiPlayerTimeAttackGameModePlayController : MultiPlayerGameModePlayController
    {
        public MultiPlayerTimeAttackGameModePlayController(
             DataModelFloat winningPlayerXPosModel,
             DataModelFloat highestTowerModel,
             DataModelFloat lowestTowerModel,
             DropSpeedController dropSpeedController,
             int brickLimit,
             float matchDuration,
             DataModelFloat timeLeftModel)
            : base(highestTowerModel, lowestTowerModel, dropSpeedController)
        {
            this._winningPlayerXPosModel = winningPlayerXPosModel;
            this._highestTowerModel = highestTowerModel;
            this._brickLimit = brickLimit;
            this._timeRemaining = matchDuration;
            this._timeLeftModel = timeLeftModel;
        }

        private bool _loggedFirstPlayUpdate;
        public override void UpdateController()
        {
            base.UpdateController();
            if (!this._loggedFirstPlayUpdate)
            {
                UnityEngine.Debug.Log("[TA-DEBUG] PlayController first UpdateController, timeRemaining=" + this._timeRemaining);
                this._loggedFirstPlayUpdate = true;
            }

            if (!this._timeIsUp)
            {
                this._timeRemaining -= Time.deltaTime;

                if (this._timeLeftModel != null)
                {
                    this._timeLeftModel.value = Mathf.Max(this._timeRemaining, 0f);
                }

                if (this._timeRemaining <= 0f)
                {
                    this._timeRemaining = 0f;
                    this._timeIsUp = true;
                    this._EndAllPlayersNow();
                }
            }
        }

        private void _EndAllPlayersNow()
        {
            foreach (AbstractGameController abstractGameController in this._gameControllers)
            {
                if (!abstractGameController.finished
                    && !this._gameControllersInCountDown.Contains(abstractGameController))
                {
                    if (abstractGameController is LocalGameController)
                    {
                        ((LocalGameController)abstractGameController).DisableBrickSpawning();
                    }
                    this._StartCountDown(abstractGameController, false, false);
                    this._gameControllersInCountDown.Add(abstractGameController);
                }
            }
        }

        protected override void _CheckGamePlayRules()
        {
            base._CheckGamePlayRules();

            foreach (AbstractGameController abstractGameController in this._gameControllers)
            {
                if (!abstractGameController.finished)
                {
                    GameModel gameModel = this._gameModels[abstractGameController.id];
                    TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
                    if (dataModel.value == this._highestTowerModel.value)
                    {
                        this._winningPlayerXPosModel.value =
                            abstractGameController.zoomableCamera.camera.transform.position.x;
                    }
                }
            }

            List<GameModel> list = new List<GameModel>();
            foreach (AbstractGameController abstractGameController2 in this._gameControllers)
            {
                list.Add(this._gameModels[abstractGameController2.id]);
            }

            this._UpdateTowerHeightInner(list);
            this._UpdateRankInner(list);
        }

        protected override void _UpdateMatchRank(int rank = 1) { }

        protected override float _GetMatchResultValue(string id)
        {
            return this._gameModels[id].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value;
        }

        private void _UpdateTowerHeightInner(List<GameModel> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list.Count - 1; j++)
                {
                    if (list[j].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value
                        < list[j + 1].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value)
                    {
                        GameModel value = list[j + 1];
                        list[j + 1] = list[j];
                        list[j] = value;
                    }
                }
            }
        }

        private void _UpdateRankInner(List<GameModel> list)
        {
            int value2 = 0;
            int num = 1;
            float num2 = float.MaxValue;
            for (int k = 0; k < list.Count; k++)
            {
                float value3 = list[k].GetDataModel<TowerHeightModel>("TOWER_HEIGHT").value;
                if (value3 != num2) { value2 = num; }
                num2 = value3;
                list[k].GetDataModel<DataModelInt>("RANK").value = value2;
                list[k].GetDataModel<DataModelInt>("MATCH_RANK").value = value2;
                num++;
            }
        }

        private int _brickLimit;
        private float _timeRemaining;
        private bool _timeIsUp;
        private DataModelFloat _winningPlayerXPosModel;
        private DataModelFloat _timeLeftModel;
        private List<AbstractGameController> _gameControllersInCountDown = new List<AbstractGameController>();
    }
}