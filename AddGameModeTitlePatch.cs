using System;
using TMPro;
using UnityEngine;
using HarmonyLib;
using System.Reflection;

/*
 * Replacing title in mode introduction because of oversight that makes new game modes otherwise say RACE
 */
namespace TrickyMultiplayerPlus
{

	[HarmonyPatch(typeof(ShowModeTitleEffect))]
	[HarmonyPatch("_HandlePreAnimationDelayComplete")]
	class AddGameModeTitlePatch
	{

		static AccessTools.FieldRef<ShowModeTitleEffect, string> gameTypeRef =
		   AccessTools.FieldRefAccess<ShowModeTitleEffect, string>("_gameType");

		static AccessTools.FieldRef<ShowModeTitleEffect, string> difficultyRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, string>("_difficulty");

		static AccessTools.FieldRef<ShowModeTitleEffect, GameObject> modeTitleRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, GameObject>("_modeTitle");

		static AccessTools.FieldRef<ShowModeTitleEffect, bool> overtimeRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, bool>("_overtime");

		static AccessTools.FieldRef<ShowModeTitleEffect, AnimationEventHandler> animationEventHandlerRef =
			AccessTools.FieldRefAccess<ShowModeTitleEffect, AnimationEventHandler>("_animationEventHandler");

		static bool Prefix(ref Timer timer, ShowModeTitleEffect __instance)
		{
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch START");

			MethodInfo handlePreAnimationDelayCompleteMethod = AccessTools.DeclaredMethod(typeof(ShowModeTitleEffect), "_HandlePreAnimationDelayComplete");
			Action<Timer> handlePreAnimationDelayAction = (Action<Timer>)Delegate.CreateDelegate(typeof(Action<Timer>), __instance, handlePreAnimationDelayCompleteMethod);
			timer.complete -= handlePreAnimationDelayAction;
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: unsubscribed timer");

			string id = "MODE_" + gameTypeRef(__instance);
			GameObject parent = GameObject.Find("UI HUD/Container");
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: parent found=" + (parent != null) + ", id=" + id);

			string id2 = difficultyRef(__instance) != null ? "DIFFICULTY_" + difficultyRef(__instance) : "DIFFICULTY_EASY";
			string difficulty = difficultyRef(__instance);
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: difficulty=" + difficulty);

			if (difficulty == "PRO")
			{
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_SPECIAL", new Vector3(0f, -40f), parent);
			}
			else if (difficulty == "NORMAL")
			{
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_" + difficultyRef(__instance), Vector3.zero, parent);
			}
			else
			{
				modeTitleRef(__instance) = Singleton<ResourceManager>.instance.InstantiateByName("MODE_TITLE_" + difficultyRef(__instance), new Vector3(0f, -40f), parent);
			}
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: modeTitle instantiated=" + (modeTitleRef(__instance) != null));

			UnityEngine.Debug.Log("Fetching title for difficulty: " + id2 + " and mode " + id);

			GameObject labelObj = WBTools.FindChildByName(modeTitleRef(__instance), "Background/Label");
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: labelObj found=" + (labelObj != null));

			TextMeshProUGUI component = labelObj.GetComponent<TextMeshProUGUI>();
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: TMP component found=" + (component != null));

			string s1 = Singleton<LanguageManager>.instance.GetById(id);
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: GetById(id) OK, value=" + s1);

			string s2 = Singleton<LanguageManager>.instance.GetById(id2);
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: GetById(id2) OK, value=" + s2);

			component.text = s1 + " " + s2;
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: text set");

			if (!overtimeRef(__instance))
			{
				GameObject overtimeObj = WBTools.FindChildByName(modeTitleRef(__instance), "Background/OvertimeLabel");
				UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: overtimeObj found=" + (overtimeObj != null));
				overtimeObj.SetActive(false);
			}
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: overtime handled");

			animationEventHandlerRef(__instance) = modeTitleRef(__instance).GetComponent<AnimationEventHandler>();
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: animEventHandler found=" + (animationEventHandlerRef(__instance) != null));

			MethodInfo handleAnimationEventMethod = AccessTools.DeclaredMethod(typeof(ShowModeTitleEffect), "_HandleAnimationEvent");
			Action<string> handleAnimationEventAction = (Action<string>)Delegate.CreateDelegate(typeof(Action<string>), __instance, handleAnimationEventMethod);
			animationEventHandlerRef(__instance).animationEvent += handleAnimationEventAction;
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch: animEvent subscribed");

			MonoBehaviourSingleton<AudioManager>.instance.PlaySfx("SFX_COUNTDOWN", 1f, 0f, 1f, false, false);
			UnityEngine.Debug.Log("[TA-DEBUG] ModeTitlePatch END");

			return false;
		}
	}

	[HarmonyPatch(typeof(ShowModeTitleEffect))]
	[HarmonyPatch("_HandleAnimationEvent")]
	class LogAnimationEventPatch
	{
		static void Prefix(string animationEvent)
		{
			Debug.Log("[TA-DEBUG-ANIM] _HandleAnimationEvent fired: " + animationEvent);
		}
	}

	[HarmonyPatch(typeof(AbstractEffect))]
	[HarmonyPatch("_OnComplete")]
	class LogEffectCompletePatch
	{
		static void Prefix(AbstractEffect __instance)
		{
			Debug.Log("[TA-DEBUG-ANIM] AbstractEffect _OnComplete: " + __instance.GetType().Name + " id=" + __instance.id);
		}
	}

	[HarmonyPatch(typeof(AbstractEffect))]
	[HarmonyPatch("_OnFinish")]
	class LogEffectFinishPatch
	{
		static void Prefix(AbstractEffect __instance)
		{
			Debug.Log("[TA-DEBUG-ANIM] AbstractEffect _OnFinish: " + __instance.GetType().Name + " id=" + __instance.id);
		}
	}
}
