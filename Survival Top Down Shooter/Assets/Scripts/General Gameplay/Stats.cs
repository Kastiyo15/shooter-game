using UnityEngine;


public class Stats : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CameraShake _cameraShake;
    [SerializeField] private Enemy _enemyScore;

    // Used to calculate SPK * Enemy value
    private int _killValue;


    // Run before anything else
    void Awake()
    {
        Camera _cam = Camera.main;
        _cameraShake = _cam.GetComponent<CameraShake>();
    }


    void Start()
    {
        if (CompareTag("Enemy"))
        {
            _enemyScore.GetComponent<Enemy>();
            _killValue = _enemyScore._scoreValue * (ScoreManager.Instance.ScorePerKill);
        }
    }


    // Death function
    public void Die()
    {
        _cameraShake._start = true;
        if (CompareTag("Enemy"))
        {
            Destroy(gameObject);
            ScoreManager.Instance.IncreaseScore(_killValue);
            ScoreManager.Instance.KillScore(_killValue);
        }
    }
}
