namespace TrickyMultiplayerPlus
{
    public class MultiplayerTimeAttackModeHeroicFactory : AbstractMulitplayerTimeAttackModeFactory
    {
        public override TimeAttackGameModeFactory Create()
        {
            var factory = new TimeAttackGameModeFactory()
            {
                matchDuration = 180f, //game time
                dropSpeedControllerFactory = new DropSpeedControllerFactory(2f),
                brickPickerFactory = new SharedRandomNamedBrickPickerFactory(null, -1, 9999),
                startSpell = "IVY",
                brickLimit = 9999,
                floorFactory = new ProPuzzleFloorFactory("FLOOR_PUZZLE_PRO", 5f),
                windStrengthMax = 0f,
                windStrengthMin = 0f
            };
            // Dòng DUY NHẤT cần thêm để có spell, tự tinh chỉnh theo độ khó Pro:
            factory.WithSpells(ModeDifficulty.Heroic, SpellProfileKind.HeightOnly);

            return factory;
        }
    }
}
