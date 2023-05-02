using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillScriptableObject", menuName = "ScriptableObjects/Skill")]
public class SkillScriptableObject : ScriptableObject 
{
    // Main Info
    [SerializeField] private string name;
    [SerializeField] private int level = 1;
    [SerializeField] private Sprite skillIcon;

    // Detail Info
    [SerializeField, TextArea] private string description;
    [SerializeField, TextArea] private string levelUpDescription;
    [SerializeField, TextArea] private string maxLevelDescription;
    
    // TODO: PUT THE DAMAGE MULTIPLIERS IN THE SCRIPTABLE OBJECT FOR BETTER CONTROL OF EVERYTHING

    public Sprite GetSkillIcon() {
        return skillIcon;
    }
}
