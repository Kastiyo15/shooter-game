using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[System.Serializable]
public class SaveData
{
    // Player Stats
    public float m_CareerDamage;
    public float m_CareerBullets;
    public float m_CareerKills;

    // Player Level
    public int m_PlayerLevel;
    public float m_PlayerCurrentXp;
    public float m_PlayerRequiredXp;
    public int m_PlayerTalentPoint;
    public int m_UsedTalentPoint;
    public int m_AvailableTalentPoint;
    public List<int> m_AssignedPointsList = new List<int>();

    // Player Score
    public int m_ScorePerKill;
    public int m_ScorePerSecond;
    public float m_SPSTimer;


    // First method to convert class to Json string
    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }


    // Second method to take the Json string and fill this class
    public void LoadFromJson(string a_Json)
    {
        JsonUtility.FromJsonOverwrite(a_Json, this);
    }
}


public interface ISaveable
{
    void PopulateSaveData(SaveData a_SaveData);
    void LoadFromSaveData(SaveData a_SaveData);
}
