using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


[System.Serializable]
public class StatsManager : MonoBehaviour, ISaveable
{
    public static StatsManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject _playerGameObject;
    [SerializeField] private GameObject _playerBulletGameObject;
    [SerializeField] private GameObject _upgradesParentGameObject;

    [Header("Player Stats")]
    public float PlayerHealth;
    public int PlayerDefence;
    public float MovementSpeed;

    [Header("Bullet Stats")]
    public int BulletClipSize;
    public int BulletDamage;
    public float BulletFireRate;
    public float BulletRandomness;
    public int BulletVelocity;

    // Take these from the score manager!
    [Header("Score Stats")]
    public int sm_ScorePerKill;
    public int sm_ScorePerSecond;
    public float sm_SPSTimer;


    // Take these from the LevelSystem!
    [Header("Level Up Stats")]
    public int ls_Level;
    public float ls_CurrentXp;
    public float ls_RequiredXp;
    public int ls_TalentPoint;
    // New Variables for the talent point system
    public int UsedTalentPoints;
    public int AvailableTalentPoints; // Updated in the UpgradeBar script
    public List<int> PointsSpentList = new List<int>();

    [Header("TextBoxes")]
    [SerializeField] private TMP_Text txt_health;
    [SerializeField] private TMP_Text txt_defence;
    [SerializeField] private TMP_Text txt_moveSpeed;
    [SerializeField] private TMP_Text txt_clipSize;
    [SerializeField] private TMP_Text txt_damage;
    [SerializeField] private TMP_Text txt_fireRate;
    [SerializeField] private TMP_Text txt_randomness;
    [SerializeField] private TMP_Text txt_velocity;


    private void Awake()
    {
        Instance = this;
        SetPlayerStats();
    }


    private void Start()
    {
        SetBulletStats();
        SetLevelStats();


        // Loop through the upgrades parent to add points used in each skill to a list
        if (PointsSpentList.Count == 0)
        {
            int i = 0;
            foreach (Transform child in _upgradesParentGameObject.transform)
            {
                PointsSpentList.Add(child.GetComponent<TalentBarControl>().Data.pointsUsed);
                PointsSpentList[i] = (child.GetComponent<TalentBarControl>().Data.pointsUsed);
                i++;
            }
        }
    }


    public void SetPlayerStats()
    {
        // Get Player Stats
        PlayerHealth = _playerGameObject.GetComponent<Health>().MaxHp;
        // PlayerDefence = ; 
        _playerGameObject.GetComponent<PlayerMovement>()._speed = MovementSpeed; // Set the speed value from here
    }


    public void SetBulletStats()
    {
        // Get Bullet Stats
        ObjectPool.SharedInstance._playerBulletPoolSize = BulletClipSize; // Set Bullet Pool Size from here
        _playerBulletGameObject.GetComponent<Projectile>().DmgValue = BulletDamage; // Set Bullet Damage from here
        _playerGameObject.GetComponent<ShootScript>().FireRate = BulletFireRate; // Set Bullet Fire Rate from here
                                                                                 // BulletRandomness = ; 
        _playerGameObject.GetComponent<ShootScript>().BulletSpeed = BulletVelocity; // Set Bullet Velocity From here
    }


    public void SetLevelStats()
    {
        ls_Level = LevelSystem.Instance.Level;
        ls_CurrentXp = LevelSystem.Instance.CurrentXp;
        ls_RequiredXp = LevelSystem.Instance.RequiredXp;
        ls_TalentPoint = LevelSystem.Instance.TalentPoint;
        AvailableTalentPoints = Mathf.Clamp(ls_TalentPoint - UsedTalentPoints, 0, 100);
    }


    // Being called from the 'Talents' menu button OnClick()
    public void SetStatsText()
    {
        // Player Text
        txt_health.SetText(($"{PlayerHealth:#0}"));
        //txt_defence.SetText(($"{PlayerHealth:#0}"));
        txt_moveSpeed.SetText(($"{MovementSpeed:#0}"));

        // Bullet Text
        txt_clipSize.SetText(($"{BulletClipSize:#0}"));
        txt_damage.SetText(($"{BulletDamage:#0}"));
        txt_fireRate.SetText(($"{BulletFireRate:#0}"));
        //txt_randomness.SetText(($"{PlayerHealth:#0}"));
        txt_velocity.SetText(($"{BulletVelocity:#0}"));
    }


    ////////////////////////
    // SAVING AND LOADING //
    public void PopulateSaveData(SaveData a_SaveData)
    {
        a_SaveData.m_AvailableTalentPoint = AvailableTalentPoints;
        a_SaveData.m_UsedTalentPoint = UsedTalentPoints;
        a_SaveData.m_AssignedPointsList = PointsSpentList;

        // List for storing points assigned to each skill
        if (a_SaveData.m_AssignedPointsList.Count < PointsSpentList.Count)
        {
            for (int i = 0; i < PointsSpentList.Count; i++)
            {
                a_SaveData.m_AssignedPointsList.Add(PointsSpentList[i]);
            }
        }
        else
        {
            for (int j = 0; j < 5; j++)
            {
                a_SaveData.m_AssignedPointsList[j] = PointsSpentList[j];
            }
        }
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        sm_ScorePerKill = a_SaveData.m_ScorePerKill;
        sm_ScorePerSecond = a_SaveData.m_ScorePerSecond;
        sm_SPSTimer = a_SaveData.m_SPSTimer;

        ls_Level = a_SaveData.m_PlayerLevel;
        ls_CurrentXp = a_SaveData.m_PlayerCurrentXp;
        ls_RequiredXp = a_SaveData.m_PlayerRequiredXp;
        ls_TalentPoint = a_SaveData.m_PlayerTalentPoint;
        AvailableTalentPoints = a_SaveData.m_AvailableTalentPoint;
        UsedTalentPoints = a_SaveData.m_UsedTalentPoint;

        PointsSpentList = a_SaveData.m_AssignedPointsList;

        // List for loading points assigned to each skill
        if (a_SaveData.m_AssignedPointsList.Count == 0)
        {
            // Loop through the upgrades parent to add points used in each skill to a list
            if (PointsSpentList.Count == 0)
            {
                foreach (Transform child in _upgradesParentGameObject.transform)
                {
                    PointsSpentList.Add(child.GetComponent<TalentBarControl>().Data.pointsUsed);
                }
            }

            int i = 0;

            foreach (Transform child in _upgradesParentGameObject.transform)
            {
                PointsSpentList[i] = (child.GetComponent<TalentBarControl>().Data.pointsUsed);
                i++;
            }
        }
        else
        {
            for (int j = 0; j < PointsSpentList.Count; j++)
            {
                PointsSpentList[j] = a_SaveData.m_AssignedPointsList[j];
            }
        }
    }
    // SAVING AND LOADING //
    ////////////////////////
}
