using UnityEngine;


public class ShootScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] public float FireRate; //Base value of 1
    private float _timeBetweenShots;
    [SerializeField] public float BulletSpeed; // Base value of 15
    [SerializeField] public float BulletSpread;
    [SerializeField] public float BulletsAmount;
    [SerializeField] private float startAngle;
    [SerializeField] private float endAngle;
    [SerializeField] private float angleStep;
    [SerializeField] private float angle;

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
            // Randomise bullet direction when only firing 1 bullet
            if (BulletsAmount == 0)
            {
                startAngle = -(_firePoint.eulerAngles.z) + Random.Range(-BulletSpread, BulletSpread);
                endAngle = -(_firePoint.eulerAngles.z) - Random.Range(-BulletSpread, BulletSpread);
            }
            else // if you have more than 1 bullet, then do a shotgun arc
            {
                startAngle = -(_firePoint.eulerAngles.z) + BulletSpread;
                endAngle = -(_firePoint.eulerAngles.z) - BulletSpread;
            }

            angleStep = (endAngle - startAngle) / BulletsAmount;
            angle = startAngle;

            for (int i = 0; i < BulletsAmount + 1; i++)
            {
                float bulDirx = _firePoint.position.x + Mathf.Sin((angle * Mathf.PI) / 180f);
                float bulDiry = _firePoint.position.y + Mathf.Cos((angle * Mathf.PI) / 180f);

                Vector3 bulMoveVector = new Vector3(bulDirx, bulDiry, 0f);
                Vector2 bulDir = (bulMoveVector - _firePoint.position).normalized;

                GameObject bul = ObjectPool.SharedInstance.GetPlayerBulletFromPool();
                if (bul == null) return;
                bul.transform.position = _firePoint.position;
                bul.transform.rotation = _firePoint.rotation;
                bul.SetActive(true);

                Rigidbody2D rb = bul.GetComponent<Rigidbody2D>();
                rb.velocity = (bulDir * BulletSpeed);

                angle += angleStep;
            }


            /*             var RandXAxis = Random.Range(-BulletSpread, BulletSpread);

                        GameObject obj = ObjectPool.SharedInstance.GetPlayerBulletFromPool();
                        if (obj == null) return;
                        obj.transform.SetPositionAndRotation(_firePoint.position, _firePoint.rotation);
                        obj.SetActive(true);

                        // Projectile Velocity
                        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                        rb.velocity = (_firePoint.up * BulletSpeed); */
        }

        _timeBetweenShots = FireRate;
    }
}
