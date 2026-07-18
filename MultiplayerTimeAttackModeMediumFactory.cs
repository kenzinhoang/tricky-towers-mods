namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeMediumFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 35),
                startSpell = "LARGE_SELF",
                brickLimit = 35,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
