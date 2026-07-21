namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeMediumFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                matchDuration = 240f,
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "LARGE_SELF",
                brickLimit = 9999,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
