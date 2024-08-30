using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Modules.CharacterController
{
	public static class AnimatorExtension
	{
		[Serializable]
		public class OverridePair
		{
			public AnimationClip targetClip, originalClip;
			public void SetClip(Animator animator) => animator.SetCustomAnimationClip(this);
			#if UNITY_EDITOR
			[CustomPropertyDrawer(typeof(OverridePair))]
			public class Drawer : PropertyDrawer
			{
				public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
				{
					return EditorGUIUtility.singleLineHeight;
				}

				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
				{
					float originalWidth = position.width;
					position.width *= 0.7f;
					EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(targetClip)), label);
					position.x += position.width;
					position.width = originalWidth * 0.3f;
					EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(originalClip)), new GUIContent(""));
				}
			}
			#endif
		}
		public static void SetCustomAnimationClip(this Animator animator, OverridePair pair) =>
			animator.SetCustomAnimationClip(pair.targetClip, pair.originalClip);
		public static void SetCustomAnimationClip(this Animator animator, AnimationClip targetClip, AnimationClip originalAnim)
		{
			AnimatorOverrideController aoc = new AnimatorOverrideController(animator.runtimeAnimatorController);	//tbd: how much does this cost - should we cache?
			var anims = new List<KeyValuePair<AnimationClip, AnimationClip>>()
			{
				new KeyValuePair<AnimationClip, AnimationClip>(originalAnim, targetClip)
			};
			aoc.ApplyOverrides(anims);
			animator.runtimeAnimatorController = aoc;
		}

		
	}
}