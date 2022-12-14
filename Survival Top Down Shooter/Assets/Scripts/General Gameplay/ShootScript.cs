using UnityEngine;


public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private float _fireRate;
    private float _timeBetweenShots;
    [SerializeField] private int _bulletSpeed;

    [Header("References")]
    [SerializeField] public GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _player;
    private bool _playerBool;
    private bool _enemyBool;


    void Start()
    {
        _timeBetweenShots = _fireRate;
        _playerBool = CompareTag("Player");
        _enemyBool = CompareTag("Enemy");
        _player = GameManager.FindObjectOfType<GameManager>().Player01;
    }


    // Update is called once per frame
    void Update()
    {
        if ((_timeBetweenShots <= 0))
        {
            // Run Player Shoot function
            if (_playerBool && Input.GetMouseButton(0))
            {
                Shoot();
            }

            // Run Enemy shoot function
            if (_enemyBool && _player.activeInHierarchy)
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
            /*             GameObject bullet = Instantiate(_bulletPrefab, _firePoint.position, _firePoint.rotation);

                        // Projectile Velocity
                        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                        rb.AddForce(_firePoint.up * _bulletSpeed, ForceMode2D.Impulse); */

            GameObject obj = ObjectPool.SharedInstance.GetPooledObject();
            if (obj == null) return;
            obj.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
            obj.SetActive(true);

            // Projectile Velocity
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * _bulletSpeed, ForceMode2D.Impulse);
        }

        if (CompareTag("Player"))
        {
            GameObject obj = ObjectPool.SharedInstance.GetPooledObject();
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
