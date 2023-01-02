using UnityEngine;


public class Projectile : MonoBehaviour
{
    [Header("Stats")]
    public int DmgValue;

    // Used to send data to GameManager
    private GameManager _gameManager;

    private GameObject _projectile;
    private GameObject _projectileFX;


    private void OnEnable()
    {
        // Check if they are players bullets
        if (CompareTag("Player"))
        {
            // Add to bullets fired count
            _gameManager = FindObjectOfType<GameManager>().GetComponent<GameManager>();
            _gameManager.BulletsFired++;
        }

        // Once enabled, disable after 3 seconds
        Invoke("Disable", 2.5f);
    }


    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
         // Play Sound effects
        if (hitInfo.tag == "Player" || hitInfo.tag == "Enemy")
        {
            SoundManager.Instance.PlaySoundImpact(SoundManager.Instance.ImpactClips[Random.Range(0, SoundManager.Instance.DeathClips.Length)]);
        }
        else if (hitInfo.tag == "MainCamera")
        {
            //SoundManager.Instance.PlaySoundImpact(SoundManager.Instance.ImpactClips[Random.Range(0, SoundManager.Instance.ImpactClips.Length)]);
        }


        // Damage using the new Health Script
        if (hitInfo.gameObject.TryGetComponent<Health>(out var health))
        {
            // If we hit something with a different tag, cause damage
            if (hitInfo.tag != tag)
            {
                health.Damage(DmgValue);
            }

            // If bullets hits an enemy
            if (hitInfo.tag == "Enemy")
            {
                // Add the damage to the players total damage value
                _gameManager.TotalDamage += DmgValue;
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
