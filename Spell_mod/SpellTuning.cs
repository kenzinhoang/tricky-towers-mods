using System.Collections.Generic;

namespace TrickyMultiplayerPlus
{
    /// <summary>Difficulty áp dụng cho MỌI mode. Thêm mode mới không cần thêm enum mới.</summary>
    public enum ModeDifficulty
    {
        Normal,
        Pro,
        Heroic,
        Crazy
    }

    /// <summary>
    /// RaceStyle: có rule dùng "TARGET_HEIGHT" (chỉ Race mới có khái niệm mục tiêu chiều cao).
    /// HeightOnly: chỉ dùng "HIGHEST_TOWER" vs "TOWER_HEIGHT" (an toàn cho TimeAttack, Tallest,
    /// và mọi mode không có target height) - ai thấp hơn đối thủ thì được ưu ái spell tốt hơn.
    /// </summary>
    public enum SpellProfileKind
    {
        RaceStyle,
        HeightOnly
    }

    /// <summary>
    /// Mô tả 1 rule so sánh điều kiện (giống DifferenceCompareConditionFloatFactory trong code gốc)
    /// nhưng khai báo dữ liệu thuần, không hard-code trong từng factory.
    /// </summary>
    public struct SpellRuleSpec
    {
        public string LeftVar;
        public string RightVar;
        public int Threshold;
        public ComparisonType Comparison;
        public SpellStruct[] Spells;
        public string DarkMappingKey; // "Normal" | "Behind" | "FarBehind"

        public SpellRuleSpec(string leftVar, string rightVar, int threshold, ComparisonType comparison, SpellStruct[] spells, string darkMappingKey)
        {
            LeftVar = leftVar;
            RightVar = rightVar;
            Threshold = threshold;
            Comparison = comparison;
            Spells = spells;
            DarkMappingKey = darkMappingKey;
        }
    }

    /// <summary>Toàn bộ tham số spell của 1 difficulty. Đây là nơi DUY NHẤT cần sửa số.</summary>
    public class SpellTuningProfile
    {
        public List<SpellRuleSpec> Rules = new List<SpellRuleSpec>();
    }

    public static class SpellTuningTable
    {
        private static readonly Dictionary<ModeDifficulty, SpellTuningProfile> _raceStyle = BuildRaceStyleTable();
        private static readonly Dictionary<ModeDifficulty, SpellTuningProfile> _heightOnly = BuildHeightOnlyTable();

        public static SpellTuningProfile Get(SpellProfileKind kind, ModeDifficulty difficulty)
        {
            var table = kind == SpellProfileKind.RaceStyle ? _raceStyle : _heightOnly;
            return table[difficulty];
        }

        private static Dictionary<ModeDifficulty, SpellTuningProfile> BuildRaceStyleTable()
        {
            var table = new Dictionary<ModeDifficulty, SpellTuningProfile>();

            table[ModeDifficulty.Normal] = new SpellTuningProfile
            {
                Rules = new List<SpellRuleSpec>
                {
                    new SpellRuleSpec("TARGET_HEIGHT", "TOWER_HEIGHT", 15, ComparisonType.LESS_THAN_OR_EQUAL,
                        new[] { new SpellStruct("IVY", 1), new SpellStruct("UNDO", 1) }, "Normal"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 15, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("AUTO_BUILD", 2), new SpellStruct("PETRIFY", 1), new SpellStruct("MINIBASE", 1) }, "FarBehind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("PETRIFY", 1), new SpellStruct("MINIBASE", 1) }, "Behind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN,
                        new[] { new SpellStruct("IVY", 10), new SpellStruct("UNDO", 10), new SpellStruct("PETRIFY", 3) }, "Normal"),
                }
            };

            table[ModeDifficulty.Pro] = new SpellTuningProfile
            {
                Rules = new List<SpellRuleSpec>
                {
                    new SpellRuleSpec("TARGET_HEIGHT", "TOWER_HEIGHT", 15, ComparisonType.LESS_THAN_OR_EQUAL,
                        new[] { new SpellStruct("IVY", 1), new SpellStruct("UNDO", 1) }, "Normal"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 20, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("AUTO_BUILD", 2), new SpellStruct("PETRIFY", 1), new SpellStruct("MINIBASE", 1) }, "FarBehind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("PETRIFY", 1), new SpellStruct("MINIBASE", 1) }, "Behind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN,
                        new[] { new SpellStruct("IVY", 10), new SpellStruct("UNDO", 10), new SpellStruct("PETRIFY", 3), new SpellStruct("MINIBASE", 3) }, "Normal"),
                }
            };

            table[ModeDifficulty.Heroic] = new SpellTuningProfile
            {
                Rules = new List<SpellRuleSpec>
                {
                    new SpellRuleSpec("TARGET_HEIGHT", "TOWER_HEIGHT", 10, ComparisonType.LESS_THAN_OR_EQUAL,
                        new[] { new SpellStruct("IVY", 2), new SpellStruct("UNDO", 1) }, "Normal"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 10, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("AUTO_BUILD", 3), new SpellStruct("PETRIFY", 2), new SpellStruct("MINIBASE", 2) }, "FarBehind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 5, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("PETRIFY", 2), new SpellStruct("MINIBASE", 2) }, "Behind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 5, ComparisonType.LESS_THAN,
                        new[] { new SpellStruct("IVY", 6), new SpellStruct("UNDO", 6), new SpellStruct("PETRIFY", 5) }, "Normal"),
                }
            };

            table[ModeDifficulty.Crazy] = new SpellTuningProfile
            {
                Rules = new List<SpellRuleSpec>
                {
                    new SpellRuleSpec("TARGET_HEIGHT", "TOWER_HEIGHT", 8, ComparisonType.LESS_THAN_OR_EQUAL,
                        new[] { new SpellStruct("IVY", 2), new SpellStruct("UNDO", 2) }, "Normal"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 8, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("AUTO_BUILD", 4), new SpellStruct("PETRIFY", 3), new SpellStruct("MINIBASE", 2) }, "FarBehind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 4, ComparisonType.GREATER_THAN_OR_EQUAL,
                        new[] { new SpellStruct("PETRIFY", 3), new SpellStruct("MINIBASE", 2) }, "Behind"),
                    new SpellRuleSpec("HIGHEST_TOWER", "TOWER_HEIGHT", 4, ComparisonType.LESS_THAN,
                        new[] { new SpellStruct("IVY", 5), new SpellStruct("UNDO", 5), new SpellStruct("PETRIFY", 6) }, "Normal"),
                }
            };

            return table;
        }

        // HeightOnly: giống RaceStyle nhưng bỏ hẳn rule TARGET_HEIGHT (TimeAttack/Tallest không có
        // khái niệm mục tiêu chiều cao). Ngưỡng lấy tương tự phần "HIGHEST_TOWER" của RaceStyle.
        private static Dictionary<ModeDifficulty, SpellTuningProfile> BuildHeightOnlyTable()
        {
            var table = new Dictionary<ModeDifficulty, SpellTuningProfile>();
            var raceTable = BuildRaceStyleTable();

            foreach (var kvp in raceTable)
            {
                var profile = new SpellTuningProfile();
                foreach (var rule in kvp.Value.Rules)
                {
                    if (rule.LeftVar == "TARGET_HEIGHT") continue; // bỏ rule không áp dụng được
                    profile.Rules.Add(rule);
                }
                table[kvp.Key] = profile;
            }
            return table;
        }
    }
}