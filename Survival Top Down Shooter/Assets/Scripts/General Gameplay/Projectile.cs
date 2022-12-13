using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    private int _damage = 100;

    [Header("References")]
    [SerializeField]
    private GameObject _effectPrefab;
    [SerializeField]
    private Rigidbody2D _rb;


    private void OnEnable()
    {
        Invoke("Disable", 2f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Damage using the new Health Script
        if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
        {
            if (hitInfo.tag != tag)
            {
                health.Damage(_damage);
            }
        }

        CreateHitEffect();
    }


    void CreateHitEffect()
    {
        if (CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        GameObject hitEffect = Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Disable();
        //Destroy(hitEffect, 1f);
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
