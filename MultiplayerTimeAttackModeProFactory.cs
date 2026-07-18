namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeProFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 45),
                startSpell = "BUBBLE",
                brickLimit = 45,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
        }
    }
}
