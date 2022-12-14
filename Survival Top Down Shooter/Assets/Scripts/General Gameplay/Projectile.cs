using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    public int DmgValue;

    [Header("References")]
    [SerializeField]
    private GameObject _effectPrefab;
    [SerializeField]
    private Rigidbody2D _rb;


    private void OnEnable()
    {
        if (CompareTag("Player"))
        {
            Invoke("Disable", 2f);
        }

        Invoke("DestroyBullet", 3f);
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
        GameObject hitEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity);

        // Destroy if its an enemy bullet
        if (CompareTag("Enemy"))
        {
            DestroyBullet();
        }
        else
        {
            Disable();
        }
    }


    private void DestroyBullet()
    {
        Destroy(gameObject);
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
