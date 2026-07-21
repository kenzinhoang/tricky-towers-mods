using System.Collections.Generic;
using System.Linq;

namespace TrickyMultiplayerPlus
{
    public static class SpellPickerBuilder
    {
        public static RubberBandingSpellPickerFactory Build(SpellTuningProfile profile)
        {
            var darkMappings = new Dictionary<string, Dictionary<string, SpellSet>>
            {
                ["Normal"] = SharedSpellMappings.Normal,
                ["Behind"] = SharedSpellMappings.Behind,
                ["FarBehind"] = SharedSpellMappings.FarBehind,
            };

            var rules = profile.Rules.Select(spec =>
                new SpellSetRuleFactory(
                    new DifferenceCompareConditionFloatFactory(spec.LeftVar, spec.RightVar, spec.Threshold, spec.Comparison, ValueDirection.FREE),
                    new SpellSet(spec.Spells),
                    darkMappings[spec.DarkMappingKey]
                )
            ).ToArray();

            return new RubberBandingSpellPickerFactory(rules);
        }
    }

    /// <summary>
    /// Extension method DUY NHẤT cần gọi từ bất kỳ factory nào (Race, TimeAttack, Tallest,
    /// mod tương lai - vì mọi factory đều kế thừa AbstractGameModeFactory) để có spell,
    /// không cần khai báo lại rule, không cần đụng vào cơ chế wiring gốc trong AbstractGameMode.
    ///
    /// Cách dùng:
    ///   var factory = new TimeAttackGameModeFactory() { matchDuration = 240f, ... };
    ///   factory.WithSpells(ModeDifficulty.Pro, SpellProfileKind.HeightOnly);
    ///   return factory;
    /// </summary>
    public static class SpellFactoryExtensions
    {
        public static T WithSpells<T>(this T factory, ModeDifficulty difficulty, SpellProfileKind kind = SpellProfileKind.HeightOnly) where T : AbstractGameModeFactory
        {
            SpellTuningProfile profile = SpellTuningTable.Get(kind, difficulty);
            factory.spellPickerFactory = SpellPickerBuilder.Build(profile);

            // spellContainerSpawnerFactory đã có default không-null từ AbstractGameModeFactory,
            // nhưng ta thay bằng bản "rơi xuống dần" giống Race để spell container thực sự
            // xuất hiện & di chuyển trong lúc chơi thay vì đứng yên theo default cơ bản.
            factory.spellContainerSpawnerFactory = new MoveDownSpellContainerSpawnerFactory(0.25f, 0.5f, 18f, 9f);

            return factory;
        }
    }
}