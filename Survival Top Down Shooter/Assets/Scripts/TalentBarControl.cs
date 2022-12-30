using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TalentBarControl : MonoBehaviour
{

    public TalentBarData Data;
    [SerializeField] private TMP_Text _pointsAvailableTxt; // Panel which shows the information about the skill


    // Start is called before the first frame update
    private void Start()
    {
        if (Data != null)
        {
            LoadTalentBarData(Data);
        }
    }


    private void Update()
    {
        _pointsAvailableTxt.SetText($"{StatsManager.Instance.AvailableTalentPoints}");

    }


    private void LoadTalentBarData(TalentBarData data)
    {
        GameObject visuals = Instantiate(data.barPrefab, this.transform); // Load bar prefab
        visuals.GetComponentInChildren<TMP_Text>().SetText($"{data.upgradeTitle}"); // Set the title of the upgrade
        visuals.GetComponent<UpgradeBar>()._iD = Data.iD;
    }


    public void ResetButton()
    {
        Data.pointsUsed = 1;
        for (int i = 0; i < StatsManager.Instance.PointsSpentList.Count; i++)
        {
            StatsManager.Instance.PointsSpentList[i] = 1;
        }
        StatsManager.Instance.UsedTalentPoints = 0;
        StatsManager.Instance.SetLevelStats();
        GetComponentInChildren<UpgradeBar>().Start();
    }


    public void SaveUpgradeData()
    {
        Data.pointsUsed = GetComponentInChildren<UpgradeBar>().UniquePointsUsed;
    }
}
