using FrooxEngine;
using FrooxEngine.CommonAvatar;

using Elements.Core;

using HarmonyLib;
using ResoniteModLoader;
using FrooxEngine.FinalIK;

namespace AvatarPoseRotationConstraintFix;

public class AvatarPoseRotationConstraintFix : ResoniteMod {
	internal const string VERSION_CONSTANT = "1.0.0";
	public override string Name => "AvatarPoseRotationConstraintFix";
	public override string Author => "mpmxyz";
	public override string Version => VERSION_CONSTANT;
	public override string Link => "https://github.com/mpmxyz/AvatarPoseRotationConstraintFix/";

	public override void OnEngineInit() {
		Harmony harmony = new Harmony("mpmxyz.AvatarPoseRotationConstraintFix");
		harmony.PatchAll();
	}

	
	[HarmonyPatch(typeof(AvatarPoseRotationConstraint), "ProcessPose")]
	class ClassNameHere_MethodNameHere_Patch {
		static bool Prefix(AvatarPoseRotationConstraint __instance, AvatarObjectSlot avatarSlot, Slot space, ref float3 position, ref floatQ rotation, ref bool isTracking) {
			Slot slot = __instance.Slot;
			rotation = slot.SpaceRotationToLocal(in rotation, space);
			float3 axis = __instance.Axis;
			float3 orthoAxis = __instance.TwistReferenceAxis;
			rotation = MathX.LimitSwing(in rotation, in axis, __instance.MaxSwing);
			rotation = MathX.LimitTwist(in rotation, in axis, in orthoAxis, __instance.MaxTwist);
			rotation = slot.LocalRotationToSpace(in rotation, space);
			return false;
		}
	}
}
