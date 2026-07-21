namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeProFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                matchDuration = 180f,
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "BUBBLE",
                brickLimit = 9999,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
