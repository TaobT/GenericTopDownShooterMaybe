using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//TODO: Reorganize draw calls
[CustomEditor(typeof(WeaponPreset))]
public class WeaponPresetEditor : Editor
{
    public override void OnInspectorGUI()
    {
        WeaponPreset weaponPreset = (WeaponPreset)target;

        serializedObject.Update();
        //Draw general variables
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponName"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("damage"));

        //General Equip
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponSlot"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("equipTime"));
        
        //General Player Effects
        EditorGUILayout.PropertyField(serializedObject.FindProperty("playerSpeed"));

        //General Firing
        EditorGUILayout.PropertyField(serializedObject.FindProperty("firingType"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("firingPoint"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("firePerSecond"));
        
        //General WeaponGround
        EditorGUILayout.PropertyField(serializedObject.FindProperty("weaponGroundSprite"));

        //General SFX
        EditorGUILayout.PropertyField(serializedObject.FindProperty("equip"));
        serializedObject.ApplyModifiedProperties();

        if(weaponPreset.weaponSlot != WeaponPreset.WeaponSlot.Melee)
        {
            //Draw general weapon variables
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxAmmo"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("ammoReserve"));

            //General Reload
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadingType"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reloadTime"));
            if (weaponPreset.reloadingType == WeaponPreset.ReloadingType.PerBullet)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("bulletsReloaded"));
                serializedObject.ApplyModifiedProperties();
            }

            //General SFX
            EditorGUILayout.PropertyField(serializedObject.FindProperty("fire"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("reload"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("empty"));

            //General Recoil
            EditorGUILayout.PropertyField(serializedObject.FindProperty("minFiringAngle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("maxFiringAngle"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleIncrement"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("angleDecrement"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("movementInaccuracyFactor"));
            serializedObject.ApplyModifiedProperties();
        }
        else
        {
            EditorGUILayout.PropertyField(serializedObject.FindProperty("miss"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("stab"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}