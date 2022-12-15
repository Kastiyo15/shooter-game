using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public int DmgValue;

    private GameObject _projectile;
    private GameObject _projectileFX;


    private void OnEnable()
    {
        Invoke("Disable", 3f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Damage using the new Health Script
        if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
        {
            if (hitInfo.tag != tag)
            {
                health.Damage(DmgValue);
            }
        }

        CreateHitEffect();
    }


    public void CreateHitEffect()
    {
        if (CompareTag("Player"))
        {
            //GameObject hitEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity);

            _projectileFX = ObjectPool.SharedInstance.GetPlayerBulletFXFromPool();
            if (_projectileFX == null) return;
            _projectileFX.transform.position = transform.position;
            _projectileFX.SetActive(true);
        }
        else if (CompareTag("Enemy"))
        {
            _projectileFX = ObjectPool.SharedInstance.GetEnemyBulletFXFromPool();
            if (_projectileFX == null) return;
            _projectileFX.transform.position = transform.position;
            _projectileFX.SetActive(true);
        }

        Disable();
    }


    void Disable()
    {
        gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        CancelInvoke();
    }
}
