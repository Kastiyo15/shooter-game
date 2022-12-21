using UnityEngine;
using System.Collections;


public class GameManager : MonoBehaviour
{


    public GameObject Player01;
    [SerializeField] private GameObject _spawnerParent;
    [SerializeField] private PauseMenu _deathScreen;

    public int KillCounter = 0;


    private void Awake()
    {
        Player01.SetActive(true);
    }


    private void Update()
    {
        // Inactivate spawner if player dies
        if (!Player01.activeInHierarchy)
        {
            _spawnerParent.SetActive(false);
            StartCoroutine(StartDeathMenu());
        }
    }


    private IEnumerator StartDeathMenu()
    {
        yield return new WaitForSeconds(2f);
        _deathScreen.GetComponent<PauseMenu>().DeathMenu();
    }
}

