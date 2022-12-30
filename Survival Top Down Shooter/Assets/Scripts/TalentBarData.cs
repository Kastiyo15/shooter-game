using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "TalentBarData", menuName = "My Game/Talent Bar Data")]
public class TalentBarData : ScriptableObject
{
    public string upgradeTitle;
    public string description;
    public GameObject barPrefab;
    public float barFillAmount;
    public int pointsUsed;
    public int iD;
}
