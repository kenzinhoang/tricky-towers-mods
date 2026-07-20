namespace TrickyMultiplayerPlus
{
    //=======================================Time Attack=============================================s
    public class MultiplayerTimeAttackModeProFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            return new TimeAttackGameModeFactory()
            {
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
