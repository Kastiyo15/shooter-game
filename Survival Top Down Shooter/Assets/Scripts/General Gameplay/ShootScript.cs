using UnityEngine;


public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _fireRate;
    private float _timeBetweenShots;
    [SerializeField] private int _bulletSpeed;

    [Header("References")]
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    private bool _player;
    private bool _enemy;


    void Start()
    {
        _timeBetweenShots = _fireRate;
        _player = CompareTag("Player");
        _enemy = CompareTag("Enemy");
    }


    // Update is called once per frame
    void Update()
    {
        if ((_timeBetweenShots <= 0))
        {
            // Run Player Shoot function
            if (_player && Input.GetMouseButton(0))
            {
                Shoot();
            }

            // Run Enemy shoot function
            if (_enemy)
            {
                Shoot();
            }
        }
        else
        {
            _timeBetweenShots -= Time.deltaTime;
        }
    }


    // Create bullet at firePoint
    void Shoot()
    {
        // Old Shoot Script
        if (CompareTag("Enemy"))
        {
            GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

            // Projectile Velocity
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * _bulletSpeed, ForceMode2D.Impulse);
        }

        if (CompareTag("Player"))
        {
            GameObject obj = ObjectPool.current.GetPooledObject();
            if (obj == null) return;
            obj.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
            obj.SetActive(true);

            // Projectile Velocity
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * _bulletSpeed, ForceMode2D.Impulse);

        }

        _timeBetweenShots = _fireRate;
    }
}
