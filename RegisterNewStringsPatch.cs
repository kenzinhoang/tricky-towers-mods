using HarmonyLib;

namespace TrickyMultiplayerPlus
{
	[HarmonyPatch(typeof(LanguageManager))]
	[HarmonyPatch("LoadLanguage")]
	class RegisterNewStringsPatch
	{
		static void Postfix(LanguageManager __instance)
		{
			UnityEngine.Debug.Log("Patching strings.");
			__instance.RegisterLanguage("MODE_TALLEST", "Tallest", false, true);
			__instance.RegisterLanguage("EXPLANATION_TITLE_MULTIPLAYER_TALLEST", "Tallest Battle", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_TALLEST", "Tallest tower with limited blocks wins!", false, true);

			__instance.RegisterLanguage("DIFFICULTY_HEROIC", "Heroic", false, true);
			__instance.RegisterLanguage("DIFFICULTY_CRAZY", "Crazy", false, true);
			__instance.RegisterLanguage("EXPLANATION_MULTIPLAYER_RACE_CRAZY_SUBTITLE", "Bridge the gap!", false, true);
			// 1. Đổi tên hiển thị ngoài Menu chính của Thế giới Race Custom (thay vì chữ Race mặc định)
			__instance.RegisterLanguage("MRW", "Race Custom Mode", false, true);

			// 2. Đổi tên hiển thị của các chế độ con bên trong (Các Key này nằm trong MultiplayerGameModeModel của bạn)
			__instance.RegisterLanguage("MULTIPLAYER_RACE_NORMAL", "Custom Normal", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_PRO", "Custom Pro", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_HEROIC", "Custom Heroic", false, true);
			__instance.RegisterLanguage("MULTIPLAYER_RACE_CRAZY", "Custom Crazy", false, true);
			return;
		}
	}
}
