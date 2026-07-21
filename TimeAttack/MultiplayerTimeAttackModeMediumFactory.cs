namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeMediumFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            var factory = new TimeAttackGameModeFactory()
            {
                matchDuration = 240f, //game time
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "LARGE_SELF",
                brickLimit = 9999,
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };

            // Dòng DUY NHẤT cần thêm để có spell, tự tinh chỉnh theo độ khó Pro:
            factory.WithSpells(ModeDifficulty.Normal, SpellProfileKind.HeightOnly);

            return factory;
        }
    }
}