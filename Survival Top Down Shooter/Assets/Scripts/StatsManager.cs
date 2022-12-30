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

    [Header("Bullet Stats")] // These are base values which will be calculated against a mulitplier to get true value
    public int BulletClipSize = 10;
    public int BulletDamage = 50;
    public float BulletFireRate = 1.000f;
    public float BulletRandomness = 0f;
    public float BulletVelocity = 15f;  // Below are multipliers for respective skills
    public int MultBulletClipSize = 4;
    public int MultBulletDamage = 20;
    public float MultBulletFireRate = 0.800f;
    public float MultBulletRandomness = 2.5f;
    public float MultBulletVelocity = 1.5f;
    public List<float> DefaultBulletValues = new List<float>();
    public List<float> BulletStatsList = new List<float>();
    public List<float> BulletMultipliersList = new List<float>();

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

        SetBulletStats();
        SetLevelStats();

        // Loop through and update stats lists at the start
        for (int i = 0; i < PointsSpentList.Count; i++)
        {
            UpdateBulletStats(i);
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
        // If New player, create a list
        // Get List of base values
        if (DefaultBulletValues.Count == 0)
        {
            DefaultBulletValues.Add((Mathf.Round(BulletClipSize))); // 0 decimal place
            DefaultBulletValues.Add((Mathf.Round(BulletDamage))); // 0 decimal place
            DefaultBulletValues.Add((Mathf.Round(BulletFireRate * 1000.0f)) * 0.001f); // 3 decimal place
            DefaultBulletValues.Add((Mathf.Round(BulletRandomness * 10.0f)) * 0.1f); // 2 decimal place
            DefaultBulletValues.Add((Mathf.Round(BulletVelocity * 10.0f)) * 0.1f); // 1 decimal place
        }
        else
        {
            DefaultBulletValues[0] = (Mathf.Round(BulletClipSize));
            DefaultBulletValues[1] = (Mathf.Round(BulletDamage));
            DefaultBulletValues[2] = (Mathf.Round(BulletFireRate * 1000.0f)) * 0.001f;
            DefaultBulletValues[3] = (Mathf.Round(BulletRandomness * 10.0f)) * 0.1f;
            DefaultBulletValues[4] = (Mathf.Round(BulletVelocity * 10.0f)) * 0.1f;
        }

        // Create a List of BulletStats
        if (BulletStatsList.Count == 0)
        {
            BulletStatsList.Add((Mathf.Round(BulletClipSize)));
            BulletStatsList.Add((Mathf.Round(BulletDamage)));
            BulletStatsList.Add((Mathf.Round(BulletFireRate * 1000.0f)) * 0.001f);
            BulletStatsList.Add((Mathf.Round(BulletRandomness * 10.0f)) * 0.1f);
            BulletStatsList.Add((Mathf.Round(BulletVelocity * 10.0f)) * 0.1f);

            // Set the Default Values for the stats
            ObjectPool.SharedInstance._playerBulletPoolSize = (int)DefaultBulletValues[0]; // Set Bullet Pool Size from here
            _playerBulletGameObject.GetComponent<Projectile>().DmgValue = (int)DefaultBulletValues[1]; // Set Bullet Damage from here
            _playerGameObject.GetComponent<ShootScript>().FireRate = DefaultBulletValues[2]; // Set Bullet Fire Rate from here
            _playerGameObject.GetComponent<ShootScript>().BulletSpread = DefaultBulletValues[3]; // BulletRandomness
            _playerGameObject.GetComponent<ShootScript>().BulletSpeed = DefaultBulletValues[4]; // Set Bullet Velocity From here
        }

        // Create a List of Multipliers
        if (BulletMultipliersList.Count == 0)
        {
            BulletMultipliersList.Add(MultBulletClipSize);
            BulletMultipliersList.Add(MultBulletDamage);
            BulletMultipliersList.Add(MultBulletFireRate);
            BulletMultipliersList.Add(MultBulletRandomness);
            BulletMultipliersList.Add(MultBulletVelocity);
        }
    }


    public void UpdateBulletStats(int iD)
    {
        // If the number of points spent in a skill is 1, set the bullet stat list to base value
        if (PointsSpentList[iD] == 1)
        {
            BulletStatsList[iD] = DefaultBulletValues[iD];
        }
        else if (PointsSpentList[iD] > 1)
        {
            if (iD != 2)
            {
                BulletStatsList[iD] = DefaultBulletValues[iD] + ((PointsSpentList[iD] - 1) * BulletMultipliersList[iD]); // Clip size

            }
            else
            {
                BulletStatsList[iD] = DefaultBulletValues[iD] * (Mathf.Pow(BulletMultipliersList[iD], (PointsSpentList[iD] - 1))); // Fire Rate
            }
        }

        /*             BulletStatsList[0] = DefaultBulletValues[i] + ((PointsSpentList[i] - 1) * BulletMultipliersList[i]); // Clip size
                    BulletStatsList[1] = DefaultBulletValues[i] + ((PointsSpentList[i] - 1) * BulletMultipliersList[i]); // Damage
                    BulletStatsList[2] = DefaultBulletValues[i] * (Mathf.Pow(BulletMultipliersList[i], (PointsSpentList[i] - 1))); // Fire Rate
                    BulletStatsList[3] = DefaultBulletValues[i] + ((PointsSpentList[i] - 1) * BulletMultipliersList[i]); // Randomness
                    BulletStatsList[4] = DefaultBulletValues[i] + ((PointsSpentList[i] - 1) * BulletMultipliersList[i]); // Velocity
         */

        // Update Bullet Stats
        ObjectPool.SharedInstance._playerBulletPoolSize = (int)BulletStatsList[0]; // Set Bullet Pool Size from here
        _playerBulletGameObject.GetComponent<Projectile>().DmgValue = (int)BulletStatsList[1]; // Set Bullet Damage from here
        _playerGameObject.GetComponent<ShootScript>().FireRate = BulletStatsList[2]; // Set Bullet Fire Rate from here
        _playerGameObject.GetComponent<ShootScript>().BulletSpread = BulletStatsList[3]; // BulletRandomness
        _playerGameObject.GetComponent<ShootScript>().BulletSpeed = BulletStatsList[4]; // Set Bullet Velocity From here
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
        txt_clipSize.SetText(($"{BulletStatsList[0]:#0}"));
        txt_damage.SetText(($"{BulletStatsList[1]:#0}"));
        txt_fireRate.SetText(($"{BulletStatsList[2]:#0.000}"));
        txt_randomness.SetText(($"{BulletStatsList[3]:#0.00}"));
        txt_velocity.SetText(($"{BulletStatsList[4]:#0.0}"));
    }


    public void AssignedPointsFunction(int iD)
    {
        SetLevelStats();
        UpdateBulletStats(iD);
        SetStatsText();
    }


    ////////////////////////
    // SAVING AND LOADING //
    public void PopulateSaveData(SaveData a_SaveData)
    {
        // Level Stats
        a_SaveData.m_AvailableTalentPoint = AvailableTalentPoints;
        a_SaveData.m_UsedTalentPoint = UsedTalentPoints;

        // Lists
        a_SaveData.m_AssignedPointsList = PointsSpentList;
        a_SaveData.m_DefaultBulletValues = DefaultBulletValues;
        a_SaveData.m_BulletStatsList = BulletStatsList;
        a_SaveData.m_BulletMultipliersList = BulletMultipliersList;
    }

    public void LoadFromSaveData(SaveData a_SaveData)
    {
        // Score stats
        sm_ScorePerKill = a_SaveData.m_ScorePerKill;
        sm_ScorePerSecond = a_SaveData.m_ScorePerSecond;
        sm_SPSTimer = a_SaveData.m_SPSTimer;

        // Level stats
        ls_Level = a_SaveData.m_PlayerLevel;
        ls_CurrentXp = a_SaveData.m_PlayerCurrentXp;
        ls_RequiredXp = a_SaveData.m_PlayerRequiredXp;
        ls_TalentPoint = a_SaveData.m_PlayerTalentPoint;
        AvailableTalentPoints = a_SaveData.m_AvailableTalentPoint;
        UsedTalentPoints = a_SaveData.m_UsedTalentPoint;

        // Lists
        PointsSpentList = a_SaveData.m_AssignedPointsList;
        DefaultBulletValues = a_SaveData.m_DefaultBulletValues;
        BulletStatsList = a_SaveData.m_BulletStatsList;
        BulletMultipliersList = a_SaveData.m_BulletMultipliersList;
    }
    // SAVING AND LOADING //
    ////////////////////////
}
