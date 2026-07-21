using System;

namespace TrickyMultiplayerPlus
{
    //=======================================Time Attack=============================================
    public class TimeAttackGameModeFactory : AbstractSinglePlayerGameModeFactory
    {
        public TimeAttackGameModeFactory()
        {
            this.ambientAudio = new string[]
            {
                "AMBIENCE_WATER"
            };
            this.musicAudio = new MusicStruct[]
            {
                new MusicStruct("MUSIC_RACE", 1f)
            };
            this.backgroundFactory = new BackgroundsFactory(new Type[]
            {
                typeof(RaceBackground),
                typeof(RaceForeground),
            });
            this.worldId = 0;
            this.floorFactory = new FloorFactory("FLOOR_LWS", 12.5f);
        }

        public float matchDuration = 300f;
        protected override AbstractGameMode _CreateGameMode()
        {
            return new TimeAttackGameMode
            {
                brickLimit = this.brickLimit,
                startSpell = this.startSpell,
                ambientAudio = this.ambientAudio
            };
        }

        public string startSpell;
        public int brickLimit;
    }
}