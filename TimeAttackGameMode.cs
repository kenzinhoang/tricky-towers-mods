using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace TrickyMultiplayerPlus
{
    public class TimeAttackGameMode : AbstractMultiPlayerGameMode
    {
        public TimeAttackGameMode()
        {
            this._winningPlayerXPosModel = new DataModelFloat(false);
        }

        // Thay vì brickLimit, chế độ này nhận timeLimit (mặc định 300 giây)
        public float timeLimit { private get; set; } = 300f;
        public string startSpell { private get; set; }

        public string[] ambientAudio { private get; set; }


        private float _currentTimer;
        private bool _isMatchEnded = false;

        protected override void _Init()
        {
            base._Init();
            Shader.SetGlobalFloat("_WaterCutoff", -8.5f);
            Shader.SetGlobalColor("_WaterColor", ColorUtil.FromHex(2768553U));
            Shader.SetGlobalColor("_WaterLineColor", ColorUtil.FromHex(12106473U));
            Shader.EnableKeyword("WATER_ON");

            // Sử dụng một điều kiện kết thúc chung của game
            this._endCondition = new FirstCompoundCondition();
        }

        public override void Setup()
        {
            base.Setup();
            _currentTimer = timeLimit;
            _isMatchEnded = false;
        }

        protected override void _InitStateControllers()
        {
            base._InitStateControllers();
            // Ở đây mượn tạm PlayController mặc định, chúng ta sẽ ép kết thúc từ vòng lặp Update
            this._gameModePlayController = new MultiPlayerTallestGameModePlayController(this._winningPlayerXPosModel, this._highestTowerModel, this._lowestTowerModel, this._dropSpeedController, 9999); // Truyền giới hạn gạch cực lớn để không bao giờ hết gạch trước thời gian
            this._gameModePlayController.countDownComplete += this._HandleCountDownComplete;
            this._gameModePlayController.countDownStarted += this._HandleCountDownStarted;

            this.AddStateController("EXPLANATION", new GameModeExplanationController(this._explanationId, this._showControls, "INTRO", this.skipExplanation));
            this.AddStateController("INTRO", new MultiPlayerTallestGameModeIntroController("TIME_ATTACK", this.skipIntroduction, this.skipModeTitle));
            this.AddStateController("COUNTDOWN", new RaceGameModeCountDownController(this._musicResources));
            this.AddStateController("PLAY", this._gameModePlayController);
        }

        // Vòng lặp đếm ngược thời gian 300s
        public new void Update()
        {
            base.Update();

            // Chỉ đếm ngược khi trạng thái game đang ở màn chơi chính (PLAY)
            if (this.stateMachine.state == "PLAY" && !_isMatchEnded)
            {
                if (_currentTimer > 0)
                {
                    _currentTimer -= Time.deltaTime;

                    // (Mẹo hiển thị UI) Tricky Towers sử dụng các HUD, bạn có thể truyền thời gian ra màn hình tại đây.
                }
                else
                {
                    _isMatchEnded = true;
                    _currentTimer = 0;
                    TriggerTimeAttackEnd();
                }
            }
        }

        // Hàm kích hoạt kết thúc trận khi hết 300 giây
        private void TriggerTimeAttackEnd()
        {
            UnityEngine.Debug.Log("Time Attack: 300s over! Triggering match end.");

            // Ép tất cả các controller của người chơi chuyển sang trạng thái kết thúc 
            foreach (AbstractGameController abstractGameController in this._gameControllers)
            {
                this._gameControllersToEndRequest.Add(abstractGameController.id);
            }
            this._gameModeEnded = true;

            // Gọi hàm kết thúc và tìm người cao nhất dựa trên thuật toán có sẵn của tác giả 
            base._OnGameModeEndRequest(this._GetWinners());
        }

        protected override void _Cleanup()
        {
            if (this._gameModePlayController != null)
            {
                this._gameModePlayController.countDownComplete -= this._HandleCountDownComplete;
                this._gameModePlayController.countDownStarted -= this._HandleCountDownStarted;
            }
            if (this._gameControllers != null)
            {
                foreach (AbstractGameController abstractGameController in this._gameControllers)
                {
                    abstractGameController.stateChange -= this._HandleGameControllerStateChanged;
                }
            }
            base._Cleanup();
        }

        public override float ModifyHorizontalMoveLimit(float limit)
        {
            return limit * 1.2f;
        }

        protected override void _AddGameController(AbstractGameController gameController)
        {
            gameController.stateChange += this._HandleGameControllerStateChanged;
        }

        protected override void _FillGameModel(GameModel gameModel, AbstractGameController gameController)
        {
            base._FillGameModel(gameModel, gameController);
            gameModel.AddDataModel("WINNING_X_POS", this._winningPlayerXPosModel);
            gameModel.GetDataModel<DataModelString>("SPELL").value = this.startSpell;
            gameModel.AddDataModel("RANK", new DataModelInt(false));

            // Giữ lại mô hình tính toán chiều cao tháp để game biết ai cao hơn 
            TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
            DataModelFloat dummyTargetHeightModel = new DataModelFloat(false);
            dummyTargetHeightModel.value = 9999; // Đặt vạch đích giả lên cực cao để không ai vô tình chạm trúng
            CompareConditionFloat value = new CompareConditionFloat(dataModel, dummyTargetHeightModel, ComparisonType.GREATER_THAN_OR_EQUAL, ValueDirection.FREE);
            this._towerHeightModels.Add(gameController.id, value);

            DataModelInt dataModelInt2 = new DataModelInt(false);
            dataModelInt2.value = 0;
            dataModelInt2.minValue = 0;
            gameModel.AddDataModel("BRICKS_USED", dataModelInt2);
        }

        protected override void _SetCustomGameStateControllers(AbstractGameController gameController)
        {
            Dictionary<string, AbstractStateController> dictionary = new Dictionary<string, AbstractStateController>();
            GameFinishController value = new GameFinishController(false, true, true);
            dictionary.Add("FINISH", value);
            dictionary.Add("WIN_REQUESTED", value);
            dictionary.Add("ROOF", new LocalRoofController(this._roofResource, false));
            dictionary.Add("BASK", new GameBaskController(false));
            dictionary.Add("GAME", new LocalGamePlayController(false));
            gameController.customStateControllers = dictionary;
        }

        protected override void _CreateHud(GameModel gameModel, AbstractGameController gameController, Rect viewPort)
        {
            // Sử dụng SurvivalHUD hoặc RaceHUD tùy sở thích hiển thị của bạn
            AbstractHUD hud = new SurvivalHUD(gameModel, viewPort, gameController.id);
            gameController.SetHud(hud);
        }

        protected override BrickGuide _CreateBrickGuide(GameModel gameModel)
        {
            return new BrickGuide(new Color(1f, 0.8666667f, 1f, 0.5f))
            {
                minBottom = -12.5f
            }
            ;
        }

        // Kế thừa nguyên vẹn hàm tìm người thắng cuộc có tháp cao nhất từ tác giả cũ 
        private string[] _GetWinners()
        {
            string winner = "";
            float num = -1;
            foreach (string text in this._gameModels.Keys)
            {
                GameModel gameModel = this._gameModels[text];
                TowerHeightModel dataModel = gameModel.GetDataModel<TowerHeightModel>("TOWER_HEIGHT");
                float value = dataModel.value;
                if (value > num)
                {
                    num = value;
                    winner = text;
                }
            }
            return new string[] { winner };
        }

        protected override void _HandleGameStates(Dictionary<AbstractGameController, string> gameStates)
        {
            List<AbstractGameController> list = new List<AbstractGameController>();
            List<AbstractGameController> list2 = new List<AbstractGameController>();
            foreach (AbstractGameController abstractGameController in gameStates.Keys)
            {
                string a = gameStates[abstractGameController];
                if (a == "FINISH" || a == "WIN_REQUESTED")
                {
                    list.Add(abstractGameController);
                }
                else if (a == "FINISH_REQUESTED")
                {
                    list2.Add(abstractGameController);
                }
            }
            foreach (AbstractGameController abstractGameController2 in list2)
            {
                string state = abstractGameController2.stateMachine.state;
                if (state != "FINISH" && state != "BASK" && state != "ROOF")
                {
                    base._OnGameEnd(abstractGameController2.id, true);
                }
            }
            if (list.Count == gameStates.Count)
            {
                this._gameModeEnded = true;
                base._OnGameModeEnd(this._GetWinners(), true);
            }
        }

        private void _HandleCountDownComplete(AbstractGameController gameController)
        {
            base._OnGameEndRequest(gameController.id);
        }

        private void _HandleGameControllerStateChanged(string stateName, string prevStateName)
        {
            if (stateName == "FINISH")
            {
                bool flag = true;
                foreach (AbstractGameController abstractGameController in this._gameControllers)
                {
                    if (!abstractGameController.finished)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    this._gameModeEnded = true;
                    base._OnGameModeEndRequest(this._GetWinners());
                }
            }
        }

        private MultiPlayerTallestGameModePlayController _gameModePlayController;
        private Dictionary<string, AbstractCondition> _towerHeightModels = new Dictionary<string, AbstractCondition>();
        private DataModelFloat _winningPlayerXPosModel;
    }
}