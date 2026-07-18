using HarmonyLib;

namespace TrickyMultiplayerPlus
{
	[HarmonyPatch(typeof(LanguageManager))]
	[HarmonyPatch("LoadLanguage")]
	class RegisterNewStringsPatch
	{
		static void Postfix(LanguageManager __instance)
		{
			UnityEngine.Debug.Log("Patching strings accurately to prevent NullReferenceException.");

			// Tên World (tab lớn) - key theo quy tắc "MODE_" + worldId
			__instance.RegisterLanguage("MODE_TALLEST", "Cao vcl", false, true);
			__instance.RegisterLanguage("MODE_RACE_CUSTOM", "Race Custom", false, true);
			__instance.RegisterLanguage("MODE_TIME_ATTACK", "Map Dit", false, true);

			// Explanation title/mô tả cho các world mới
			__instance.RegisterLanguage("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "Tallest Battle", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_TALLEST", "Tallest tower with limited blocks wins!", false, true);
			__instance.RegisterLanguage("EXPLANATION_TITLE_MULTIPLAYER_TIME_ATTACK", "Time Attack", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_TIME_ATTACK", "Race against the clock!", false, true);

			// Tên difficulty mới (global)
			__instance.RegisterLanguage("DIFFICULTY_HEROIC", "Heroic", false, true);
			__instance.RegisterLanguage("DIFFICULTY_CRAZY", "Crazy", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_RACE_CRAZY_SUBTITLE", "Bridge the gap!", false, true);

			// Explanation chi tiết theo từng world/difficulty (tooltip)
			__instance.RegisterLanguage("MULTIPLAYER_RACE_NORMAL", "Custom Normal", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_PRO", "Custom Pro", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_HEROIC", "Custom Heroic", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_CRAZY", "Custom Crazy", false, true);

			__instance.RegisterLanguage("MULTIPLAYER_TALLEST_NORMAL", "T-Custom Normal", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_TALLEST_PRO", "T-Custom Pro", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_TALLEST_HEROIC", "T-Custom Heroic", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_TALLEST_CRAZY", "T-Custom Crazy", false, true);

			return;
		}
	}
}