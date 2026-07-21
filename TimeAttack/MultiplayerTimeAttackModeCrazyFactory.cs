namespace TrickyMultiplayerPlus
{
    class MultiplayerTimeAttackModeCrazyFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                matchDuration = 300f,
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedGroupBrickPickerFactory(null, -1, 4, 9999),
                startSpell = "IVY",
                brickLimit = 9999,
                floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 5f),
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
