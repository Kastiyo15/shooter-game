using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class KillCounter : MonoBehaviour
{

    public int KillCount;
    [SerializeField] private TMP_Text _killCountText; // Score at the top of the screen


    public void UpdateKillCounter()
    {
        KillCount++;
        _killCountText.text = string.Format("<b>Kills:</b> {0}", KillCount);
    }
}
