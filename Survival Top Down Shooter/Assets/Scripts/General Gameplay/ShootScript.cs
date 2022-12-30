using UnityEngine;


public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float FireRate = 1f; //Base value of 1
    private float _timeBetweenShots;
    [SerializeField] public int BulletSpeed = 15; // Base value of 15

    [Header("References")]
    [SerializeField] public GameObject _bulletPrefab;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private GameObject _player;
    private bool _playerBool;
    private bool _enemyBool;


    void Start()
    {
        _timeBetweenShots = FireRate;
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
                Enemy me = this.gameObject.GetComponent<Enemy>();
                if (me.finalDistance < (me.maximumDistance - 15))
                {
                    Shoot();
                }
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

            GameObject obj = ObjectPool.SharedInstance.GetEnemyBulletFromPool();
            if (obj == null) return;
            obj.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
            obj.SetActive(true);

            // Projectile Velocity
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * BulletSpeed, ForceMode2D.Impulse);
        }

        if (CompareTag("Player"))
        {
            GameObject obj = ObjectPool.SharedInstance.GetPlayerBulletFromPool();
            if (obj == null) return;
            obj.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
            obj.SetActive(true);

            // Projectile Velocity
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            rb.AddForce(_firePoint.up * BulletSpeed, ForceMode2D.Impulse);

        }

        _timeBetweenShots = FireRate;
    }
}
