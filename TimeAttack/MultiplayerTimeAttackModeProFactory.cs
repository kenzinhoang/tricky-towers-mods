namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeProFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            var factory = new TimeAttackGameModeFactory()
            {
                matchDuration = 180f,
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "BUBBLE",
                brickLimit = 9999,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };

            factory.WithSpells(ModeDifficulty.Pro, SpellProfileKind.HeightOnly);
            return factory;
        }
    }
}
