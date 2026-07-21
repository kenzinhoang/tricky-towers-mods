using System.Collections.Generic;

namespace TrickyMultiplayerPlus
{
    /// <summary>
    /// Dark spell mapping (spell phụ đi kèm khi 1 spell chính được roll trúng) dùng CHUNG
    /// cho mọi game mode (Race, TimeAttack, Tallest, mod tương lai). Build 1 lần duy nhất
    /// thay vì mỗi mode tự new lại dictionary như AbstractMulitplayerRaceModeFactory gốc.
    /// </summary>
    public static class SharedSpellMappings
    {
        public static readonly Dictionary<string, SpellSet> Normal = BuildNormal();
        public static readonly Dictionary<string, SpellSet> Behind = BuildBehind();
        public static readonly Dictionary<string, SpellSet> FarBehind = BuildFarBehind();

        private static Dictionary<string, SpellSet> BuildNormal()
        {
            var map = new Dictionary<string, SpellSet>();
            map["IVY"] = new SpellSet(new SpellStruct[]
            {
                new SpellStruct("LARGE", 10),
                new SpellStruct("GRASSY", 10),
                new SpellStruct("BUBBLE", 10),
                new SpellStruct("MYSTERY", 10),
                new SpellStruct("AUTO_ROTATE", 10),
                new SpellStruct("SLOW", 6)
            });
            map["UNDO"] = map["IVY"];
            map["PETRIFY"] = new SpellSet(new SpellStruct[]
            {
                new SpellStruct("SLOW", 10),
                new SpellStruct("ICE", 10)
            });
            map["MINIBASE"] = map["PETRIFY"];
            return map;
        }

        private static Dictionary<string, SpellSet> BuildBehind()
        {
            var map = new Dictionary<string, SpellSet>();
            map["PETRIFY"] = new SpellSet(new SpellStruct[]
            {
                new SpellStruct("SLOW", 10),
                new SpellStruct("ICE", 10),
                new SpellStruct("LARGE_MYSTERY", 4),
                new SpellStruct("LARGE_AUTO_ROTATE", 4),
                new SpellStruct("LARGE_ICE_SINGLE", 4)
            });
            map["MINIBASE"] = map["PETRIFY"];
            return map;
        }

        private static Dictionary<string, SpellSet> BuildFarBehind()
        {
            var map = new Dictionary<string, SpellSet>();
            map["PETRIFY"] = new SpellSet(new SpellStruct[]
            {
                new SpellStruct("SLOW", 4),
                new SpellStruct("ICE", 4),
                new SpellStruct("LARGE_MYSTERY", 10),
                new SpellStruct("LARGE_AUTO_ROTATE", 10),
                new SpellStruct("LARGE_ICE_SINGLE", 10),
                new SpellStruct("ICE_AUTO_ROTATE_MYSTERY", 3)
            });
            map["MINIBASE"] = map["PETRIFY"];
            map["AUTO_BUILD"] = new SpellSet(new SpellStruct[]
            {
                new SpellStruct("LARGE_MYSTERY", 1),
                new SpellStruct("LARGE_AUTO_ROTATE", 1),
                new SpellStruct("LARGE_ICE_SINGLE", 1),
                new SpellStruct("ICE_AUTO_ROTATE_MYSTERY", 1)
            });
            return map;
        }
    }
}