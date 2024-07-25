#if UNITY_EDITOR && !COMPILER_UDONSHARP
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Gun)), CanEditMultipleObjects]
public class GunEditor : Editor
{
    #region Get Serialized Properties
    SerializedProperty maxDistance;
    SerializedProperty firePosition;
    SerializedProperty pelletCount;
    SerializedProperty shotgun;
    SerializedProperty infiniteAmmo;
    SerializedProperty RaycastDamageAmount;
    SerializedProperty BulletSpread;
    SerializedProperty AmmoCount;
    SerializedProperty MagSize;
    SerializedProperty MaxAmmo;
    
    SerializedProperty fireSelection;
    SerializedProperty BaseCycleOffAnimation;
    SerializedProperty CycleTime;
    SerializedProperty BurstCycleTime;
    SerializedProperty BurstCount;
    SerializedProperty currentBurstCount;

    bool editorGroup;
    bool editorGroup2;
    bool editorGroup3;
    bool editorGroup4;
    bool editorGroup5;
    bool editorGroup6;
    bool editorGroup7;
    bool editorGroup8;
    bool editorGroup9;
    bool editorGroup10;
    bool editorGroup11;
    bool editorGroup12;
    bool editorGroup13;
    bool editorGroup14;
    SerializedProperty barrelAudioSource;
    SerializedProperty magazineAudioSource;
    SerializedProperty fireMechanismAudioSource;
    SerializedProperty gunShot;
    SerializedProperty MagPull;
    SerializedProperty MagInsert;
    SerializedProperty GunCock;
    SerializedProperty GunEmpty;

    SerializedProperty reloadTime;
    SerializedProperty BaseReloadTimeOffAnimation;
    SerializedProperty reloadOnMagEmpty;

    SerializedProperty GunAnimator;
    SerializedProperty CycleAnimation;
    SerializedProperty ReloadAnimation;
    SerializedProperty magazineAnimator;
    SerializedProperty MagazineAnimatorVariable;

    SerializedProperty Display;
    SerializedProperty ShellParticle;
    SerializedProperty gunShotParticle;
    SerializedProperty secondaryGripPickup;
    SerializedProperty pickup;

    SerializedProperty RaycastDamage;
    SerializedProperty RaycastBulletDrop;
    SerializedProperty desktopFaceFiring;
    SerializedProperty playerRaycastSpark;
    SerializedProperty players;
    SerializedProperty terrainRaycastSpark;
    SerializedProperty sparkable;

    SerializedProperty hapticFeedback;
    SerializedProperty hapticFeedbackDuration;
    SerializedProperty hapticFeedbackAmplitude;
    SerializedProperty hapticFeedbackFrequency;
    SerializedProperty hapticFeedbackOnManualReload;

    SerializedProperty bullet;
    SerializedProperty fireVelocity;

    SerializedProperty barrels;
    SerializedProperty fireAllBarrelsAtOnce;
    SerializedProperty ammoCountDependsOnBarrels;

    SerializedProperty targetRigidbody;
    SerializedProperty forceFromBarrel;

    SerializedProperty useVirtualStock;
    SerializedProperty stockTransform;
    SerializedProperty rotationPointTransform;
    SerializedProperty relativeVirtualStockActivationDistance;
    SerializedProperty rotationRange;

    SerializedProperty finalCycleAnimation;
    SerializedProperty finalGunShotClip;

    SerializedProperty perShellReload;
    SerializedProperty ReloadAnimation1;
    SerializedProperty ReloadAnimation2;
    SerializedProperty ReloadAnimation3;
    SerializedProperty ReloadAnimation4;
    SerializedProperty ReloadAnimation5;
    SerializedProperty ReloadAnimation6;
    SerializedProperty ReloadAnimation7;
    SerializedProperty ReloadAnimation8;
    SerializedProperty ReloadAnimation9;
    SerializedProperty ReloadAnimation10;
    SerializedProperty ReloadAnimation11;
    SerializedProperty ReloadAnimation12;
    SerializedProperty reloadAnimation1Offset;
    SerializedProperty reloadAnimation2Offset;
    SerializedProperty reloadAnimation3Offset;
    SerializedProperty reloadAnimation4Offset;
    SerializedProperty reloadAnimation5Offset;
    SerializedProperty reloadAnimation6Offset;
    SerializedProperty reloadAnimation7Offset;
    SerializedProperty reloadAnimation8Offset;
    SerializedProperty reloadAnimation9Offset;
    SerializedProperty reloadAnimation10Offset;
    SerializedProperty reloadAnimation11Offset;
    SerializedProperty reloadAnimation12Offset;

    SerializedProperty reloadAudioBypass;

    SerializedProperty finalReloadAnimation;
    SerializedProperty finalReloadAnimationOffset;
    SerializedProperty reloadAnimationOffset;
    SerializedProperty randomGunShotClip;

    private void OnEnable()
    {
        infiniteAmmo = serializedObject.FindProperty("infiniteAmmo");
        RaycastDamageAmount = serializedObject.FindProperty("RaycastDamageAmount");
        maxDistance = serializedObject.FindProperty("maxDistance");
        firePosition = serializedObject.FindProperty("firePosition");
        pelletCount = serializedObject.FindProperty("pelletCount");
        shotgun = serializedObject.FindProperty("shotgun");
        BulletSpread = serializedObject.FindProperty("BulletSpread");
        AmmoCount = serializedObject.FindProperty("AmmoCount");
        MagSize = serializedObject.FindProperty("MagSize");
        MaxAmmo = serializedObject.FindProperty("MaxAmmo");
        
        fireSelection = serializedObject.FindProperty("fireSelection");
        BaseCycleOffAnimation = serializedObject.FindProperty("BaseCycleOffAnimation");
        CycleTime = serializedObject.FindProperty("CycleTime");
        BurstCycleTime = serializedObject.FindProperty("BurstCycleTime");
        BurstCount = serializedObject.FindProperty("BurstCount");
        currentBurstCount = serializedObject.FindProperty("currentBurstCount");

        barrelAudioSource = serializedObject.FindProperty("barrelAudioSource");
        magazineAudioSource = serializedObject.FindProperty("magazineAudioSource");
        fireMechanismAudioSource = serializedObject.FindProperty("fireMechanismAudioSource");
        gunShot = serializedObject.FindProperty("gunShot");
        MagPull = serializedObject.FindProperty("MagPull");
        MagInsert = serializedObject.FindProperty("MagInsert");
        GunCock = serializedObject.FindProperty("GunCock");
        GunEmpty = serializedObject.FindProperty("GunEmpty");

        reloadTime = serializedObject.FindProperty("reloadTime");
        BaseReloadTimeOffAnimation = serializedObject.FindProperty("BaseReloadTimeOffAnimation");
        reloadOnMagEmpty = serializedObject.FindProperty("reloadOnMagEmpty");

        GunAnimator = serializedObject.FindProperty("GunAnimator");
        CycleAnimation = serializedObject.FindProperty("CycleAnimation");
        ReloadAnimation = serializedObject.FindProperty("ReloadAnimation");
        magazineAnimator = serializedObject.FindProperty("magazineAnimator");
        MagazineAnimatorVariable = serializedObject.FindProperty("MagazineAnimatorVariable");

        Display = serializedObject.FindProperty("Display");
        ShellParticle = serializedObject.FindProperty("ShellParticle");
        gunShotParticle = serializedObject.FindProperty("gunShotParticle");
        secondaryGripPickup = serializedObject.FindProperty("secondaryGripPickup");
        pickup = serializedObject.FindProperty("pickup");

        RaycastDamage = serializedObject.FindProperty("RaycastDamage");
        RaycastBulletDrop = serializedObject.FindProperty("RaycastBulletDrop");
        desktopFaceFiring = serializedObject.FindProperty("desktopFaceFiring");
        playerRaycastSpark = serializedObject.FindProperty("playerRaycastSpark");
        players = serializedObject.FindProperty("players");
        terrainRaycastSpark = serializedObject.FindProperty("terrainRaycastSpark");
        sparkable = serializedObject.FindProperty("sparkable");

        hapticFeedback = serializedObject.FindProperty("hapticFeedback");
        hapticFeedbackDuration = serializedObject.FindProperty("hapticFeedbackDuration");
        hapticFeedbackAmplitude = serializedObject.FindProperty("hapticFeedbackAmplitude");
        hapticFeedbackFrequency = serializedObject.FindProperty("hapticFeedbackFrequency");
        hapticFeedbackOnManualReload = serializedObject.FindProperty("hapticFeedbackOnManualReload");

        bullet = serializedObject.FindProperty("bullet");
        fireVelocity = serializedObject.FindProperty("fireVelocity");

        barrels = serializedObject.FindProperty("barrels");
        fireAllBarrelsAtOnce = serializedObject.FindProperty("fireAllBarrelsAtOnce");
        ammoCountDependsOnBarrels = serializedObject.FindProperty("ammoCountDependsOnBarrels");

        targetRigidbody = serializedObject.FindProperty("targetRigidbody");
        forceFromBarrel = serializedObject.FindProperty("forceFromBarrel");

        useVirtualStock = serializedObject.FindProperty("useVirtualStock");
        stockTransform = serializedObject.FindProperty("stockTransform");
        rotationPointTransform = serializedObject.FindProperty("rotationPointTransform");
        relativeVirtualStockActivationDistance = serializedObject.FindProperty("relativeVirtualStockActivationDistance");
        rotationRange = serializedObject.FindProperty("rotationRange");

        finalGunShotClip = serializedObject.FindProperty("finalGunShotClip");
        //finalShotAnimationToggle = serializedObject.FindProperty("seperateFinalShotAnimationToggle");
        finalCycleAnimation = serializedObject.FindProperty("finalCycleAnimation");

        perShellReload = serializedObject.FindProperty("perShellReload");
        ReloadAnimation1 = serializedObject.FindProperty("ReloadAnimation1");
        ReloadAnimation2 = serializedObject.FindProperty("ReloadAnimation2");
        ReloadAnimation3 = serializedObject.FindProperty("ReloadAnimation3");
        ReloadAnimation4 = serializedObject.FindProperty("ReloadAnimation4");
        ReloadAnimation5 = serializedObject.FindProperty("ReloadAnimation5");
        ReloadAnimation6 = serializedObject.FindProperty("ReloadAnimation6");
        ReloadAnimation7 = serializedObject.FindProperty("ReloadAnimation7");
        ReloadAnimation8 = serializedObject.FindProperty("ReloadAnimation8");
        ReloadAnimation9 = serializedObject.FindProperty("ReloadAnimation9");
        ReloadAnimation10 = serializedObject.FindProperty("ReloadAnimation10");
        ReloadAnimation11 = serializedObject.FindProperty("ReloadAnimation11");
        ReloadAnimation12 = serializedObject.FindProperty("ReloadAnimation12");
        reloadAnimation1Offset = serializedObject.FindProperty("reloadAnimation1Offset");
        reloadAnimation2Offset = serializedObject.FindProperty("reloadAnimation2Offset");
        reloadAnimation3Offset = serializedObject.FindProperty("reloadAnimation3Offset");
        reloadAnimation4Offset = serializedObject.FindProperty("reloadAnimation4Offset");
        reloadAnimation5Offset = serializedObject.FindProperty("reloadAnimation5Offset");
        reloadAnimation6Offset = serializedObject.FindProperty("reloadAnimation6Offset");
        reloadAnimation7Offset = serializedObject.FindProperty("reloadAnimation7Offset");
        reloadAnimation8Offset = serializedObject.FindProperty("reloadAnimation8Offset");
        reloadAnimation9Offset = serializedObject.FindProperty("reloadAnimation9Offset");
        reloadAnimation10Offset = serializedObject.FindProperty("reloadAnimation10Offset");
        reloadAnimation11Offset = serializedObject.FindProperty("reloadAnimation11Offset");
        reloadAnimation12Offset = serializedObject.FindProperty("reloadAnimation12Offset");

        reloadAudioBypass = serializedObject.FindProperty("reloadAudioBypass");
        finalReloadAnimation = serializedObject.FindProperty("finalReloadAnimation");
        finalReloadAnimationOffset = serializedObject.FindProperty("finalReloadAnimationOffset");
        reloadAnimationOffset = serializedObject.FindProperty("reloadAnimationOffset");
        randomGunShotClip = serializedObject.FindProperty("randomGunShotClip");

    }
    #endregion

    public override void OnInspectorGUI()
    {
        GUIStyle helpBox = new GUIStyle(EditorStyles.helpBox);
        GUIStyle richText = new GUIStyle(GUI.skin.label);
        GUIStyle richTextCentered = new GUIStyle(GUI.skin.label);
        richText.richText = true;
        richTextCentered.richText = true;
        richTextCentered.alignment = TextAnchor.UpperCenter;

        GUIStyle foldoutStyle = EditorStyles.foldout;
        FontStyle previousStyle = foldoutStyle.fontStyle;
        foldoutStyle.fontStyle = FontStyle.Bold;

        serializedObject.Update();

        EditorGUILayout.LabelField($"<size=14><b><color={InspectorColors.Color(ThemeColor.Col1)}>----------------- Gun Settings -----------------</color></b></size>", richTextCentered);
        EditorGUILayout.Space(1);

        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col2)}>General Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(GunAnimator);
        /*EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(RaycastDamageAmount);
        EditorGUILayout.PropertyField(shotgun);
        EditorGUILayout.PropertyField(pelletCount);*/
        EditorGUILayout.PropertyField(Display);
        EditorGUILayout.PropertyField(ShellParticle);
        EditorGUILayout.PropertyField(gunShotParticle);
        EditorGUILayout.PropertyField(secondaryGripPickup);
        EditorGUILayout.PropertyField(pickup);
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}>Fire Rate Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(fireSelection);
        editorGroup2 = GUILayout.Toggle(editorGroup2, "Shotgun", foldoutStyle);
        if (editorGroup2)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(shotgun);
            EditorGUILayout.PropertyField(pelletCount);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.PropertyField(CycleTime);
        EditorGUILayout.PropertyField(BurstCycleTime);
        EditorGUILayout.PropertyField(BurstCount);
        editorGroup3 = GUILayout.Toggle(editorGroup3, "Per Shot Animation", foldoutStyle);
        if (editorGroup3)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(BaseCycleOffAnimation);
            EditorGUILayout.PropertyField(CycleAnimation);
            //EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}>--------------------------------------------------------------------</color></b></size>", richText);
            //EditorGUILayout.PropertyField(seperateFinalShotAnimationToggle);
            EditorGUILayout.PropertyField(finalCycleAnimation);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            editorGroup5 = GUILayout.Toggle(editorGroup5, "Magazine Animator (Could physically show you how many bullets left in a mag)", foldoutStyle);
            if (editorGroup5)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PropertyField(magazineAnimator);
                EditorGUILayout.PropertyField(MagazineAnimatorVariable);
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        //EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}>Bullet Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(maxDistance);
        EditorGUILayout.PropertyField(BulletSpread);
        EditorGUILayout.PropertyField(RaycastDamageAmount);
        EditorGUILayout.PropertyField(firePosition);
        //EditorGUILayout.PropertyField(currentBurstCount);
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col4)}>Audio Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(reloadAudioBypass);
        EditorGUILayout.PropertyField(barrelAudioSource);
        EditorGUILayout.PropertyField(magazineAudioSource);
        EditorGUILayout.PropertyField(fireMechanismAudioSource);
        editorGroup = GUILayout.Toggle(editorGroup, "Audio Clips", foldoutStyle);
        if (editorGroup)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(finalGunShotClip);
            //EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col4)}>--------------------------------------------------------------------</color></b></size>", richText);
            EditorGUILayout.PropertyField(gunShot);
            EditorGUILayout.PropertyField(randomGunShotClip);
            EditorGUILayout.PropertyField(MagPull);
            EditorGUILayout.PropertyField(MagInsert);
            EditorGUILayout.PropertyField(GunCock);
            EditorGUILayout.PropertyField(GunEmpty);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col5)}>Reload Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(reloadTime);
        EditorGUILayout.PropertyField(reloadOnMagEmpty);
        EditorGUILayout.PropertyField(AmmoCount);
        EditorGUILayout.PropertyField(MagSize);
        EditorGUILayout.PropertyField(MaxAmmo);
        EditorGUILayout.PropertyField(infiniteAmmo);
        editorGroup4 = GUILayout.Toggle(editorGroup4, "Reload Animation", foldoutStyle);
        if (editorGroup4)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(BaseReloadTimeOffAnimation);
            EditorGUILayout.PropertyField(ReloadAnimation);
            EditorGUILayout.PropertyField(reloadAnimationOffset);
            EditorGUILayout.PropertyField(finalReloadAnimation);
            EditorGUILayout.PropertyField(finalReloadAnimationOffset);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col5)}> </color></b></size>", richText);
            editorGroup14 = GUILayout.Toggle(editorGroup14, "Per Shell Reload", foldoutStyle);
            if (editorGroup14)
            {
                EditorGUILayout.BeginVertical();
                EditorGUILayout.PropertyField(perShellReload);
                EditorGUILayout.PropertyField(ReloadAnimation1);
                EditorGUILayout.PropertyField(reloadAnimation1Offset);
                EditorGUILayout.PropertyField(ReloadAnimation2);
                EditorGUILayout.PropertyField(reloadAnimation2Offset);
                EditorGUILayout.PropertyField(ReloadAnimation3);
                EditorGUILayout.PropertyField(reloadAnimation3Offset);
                EditorGUILayout.PropertyField(ReloadAnimation4);
                EditorGUILayout.PropertyField(reloadAnimation4Offset);
                EditorGUILayout.PropertyField(ReloadAnimation5);
                EditorGUILayout.PropertyField(reloadAnimation5Offset);
                EditorGUILayout.PropertyField(ReloadAnimation6);
                EditorGUILayout.PropertyField(reloadAnimation6Offset);
                EditorGUILayout.PropertyField(ReloadAnimation7);
                EditorGUILayout.PropertyField(reloadAnimation7Offset);
                EditorGUILayout.PropertyField(ReloadAnimation8);
                EditorGUILayout.PropertyField(reloadAnimation8Offset);
                EditorGUILayout.PropertyField(ReloadAnimation9);
                EditorGUILayout.PropertyField(reloadAnimation9Offset);
                EditorGUILayout.PropertyField(ReloadAnimation10);
                EditorGUILayout.PropertyField(reloadAnimation10Offset);
                EditorGUILayout.PropertyField(ReloadAnimation11);
                EditorGUILayout.PropertyField(reloadAnimation11Offset);
                EditorGUILayout.PropertyField(ReloadAnimation12);
                EditorGUILayout.PropertyField(reloadAnimation12Offset);

                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col4)}>Raycast Settings</color></b></size>", richText);
        EditorGUILayout.PropertyField(RaycastDamage);
        EditorGUILayout.PropertyField(RaycastBulletDrop);
        EditorGUILayout.PropertyField(desktopFaceFiring);
        editorGroup6 = GUILayout.Toggle(editorGroup6, "Player Spark Effect", foldoutStyle);
        if (editorGroup6)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(players);
            EditorGUILayout.PropertyField(playerRaycastSpark);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col4)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        editorGroup7 = GUILayout.Toggle(editorGroup7, "Terrain Spark Effect", foldoutStyle);
        if (editorGroup7)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(sparkable);
            EditorGUILayout.PropertyField(terrainRaycastSpark);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);
        EditorGUILayout.BeginVertical(helpBox);
        EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}>Miscellaneous Settings</color></b></size>", richText);
        editorGroup8 = GUILayout.Toggle(editorGroup8, "Haptic Feedback", foldoutStyle);
        if (editorGroup8)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(hapticFeedback);
            EditorGUILayout.PropertyField(hapticFeedbackDuration);
            EditorGUILayout.PropertyField(hapticFeedbackAmplitude);
            EditorGUILayout.PropertyField(hapticFeedbackFrequency);
            EditorGUILayout.PropertyField(hapticFeedbackOnManualReload);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        editorGroup9 = GUILayout.Toggle(editorGroup9, "Rigibody Projectile (Requires Raycast To Be Off)", foldoutStyle);
        if (editorGroup9)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(bullet);
            EditorGUILayout.PropertyField(fireVelocity);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        editorGroup10 = GUILayout.Toggle(editorGroup10, "Multi-Barrel Settings (If The Script Detects Multiple Barrels It Will Use Them)", foldoutStyle);
        if (editorGroup10)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(barrels);
            EditorGUILayout.PropertyField(fireAllBarrelsAtOnce);
            EditorGUILayout.PropertyField(ammoCountDependsOnBarrels);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        editorGroup11 = GUILayout.Toggle(editorGroup11, "Physics Stuff (Only Add If You're Putting This On A Vehicle)", foldoutStyle);
        if (editorGroup11)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(targetRigidbody);
            EditorGUILayout.PropertyField(forceFromBarrel);
            EditorGUILayout.LabelField($"<size=13><b><color={InspectorColors.Color(ThemeColor.Col3)}> </color></b></size>", richText);
            EditorGUILayout.EndVertical();
        }
        editorGroup12 = GUILayout.Toggle(editorGroup12, "Manipulation Settings", foldoutStyle);
        if (editorGroup12)
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.PropertyField(useVirtualStock);
            EditorGUILayout.PropertyField(stockTransform);
            EditorGUILayout.PropertyField(rotationPointTransform);
            EditorGUILayout.PropertyField(relativeVirtualStockActivationDistance);
            EditorGUILayout.PropertyField(rotationRange);
            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Space(2);
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
#endif