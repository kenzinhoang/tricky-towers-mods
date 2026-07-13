using System;

namespace TrickyMultiplayerPlus
{
    public class TimeAttackGameModeFactory : AbstractSinglePlayerGameModeFactory
    {
        public TimeAttackGameModeFactory()
        {
            this.ambientAudio = new string[] { "AMBIENCE_WATER" };
            this.musicAudio = new MusicStruct[] { new MusicStruct("MUSIC_RACE", 1f) };
            this.backgroundFactory = new BackgroundsFactory(new Type[]
            {
                typeof(RaceBackground),
                typeof(RaceForeground)
            });
            this.worldId = 0;
            this.floorFactory = new FloorFactory("FLOOR_LWS", 12.5f);
        }

        protected override AbstractGameMode _CreateGameMode()
        {
            return new TimeAttackGameMode
            {
                timeLimit = this.timeLimit,
                startSpell = this.startSpell,
                ambientAudio = this.ambientAudio
            };
        }

        public string startSpell;
        public float timeLimit = 300f; // Đặt mặc định là 300 giây
    }
}