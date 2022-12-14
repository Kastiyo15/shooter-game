using UnityEngine;


public class GameManager : MonoBehaviour
{


    public GameObject Player01;
    [SerializeField] private GameObject _spawnerParent;


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
        }
    }
}

