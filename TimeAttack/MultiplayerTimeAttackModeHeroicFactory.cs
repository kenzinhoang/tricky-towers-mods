namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeHeroicFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                matchDuration = 180f,
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "IVY",
                brickLimit = 9999,
                floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 5f),
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
