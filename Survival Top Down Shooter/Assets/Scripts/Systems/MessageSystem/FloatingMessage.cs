using UnityEngine;
using System.Collections;
using TMPro;
using Random = UnityEngine.Random;


namespace Scripts.Systems.MessageSystem
{

    public class FloatingMessage : MonoBehaviour, IInGameMessage
    {

        private Rigidbody2D _rigidbody;
        private TMP_Text _value;

        [SerializeField] private float InitialYVelocity;
        [SerializeField] private float InitialXVelocityRange;
        [SerializeField] private float LifeTime;
        private float _halfLife;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _value = GetComponentInChildren<TMP_Text>();
        }

        private void Start()
        {
            _halfLife = LifeTime / 2f;

            if (InitialXVelocityRange < 1f)
            {
                _rigidbody.velocity = new Vector2(InitialXVelocityRange, InitialYVelocity);
            }
            else
            {
                _rigidbody.velocity = new Vector2(Random.Range(-InitialXVelocityRange, InitialXVelocityRange), InitialYVelocity);
            }

            StartCoroutine(Fadeout());

            Destroy(gameObject, LifeTime);
        }



        private IEnumerator Fadeout()
        {
            yield return new WaitForSeconds(_halfLife);
            _value.CrossFadeAlpha(0.0f, _halfLife, false);

        }


        public void SetMessage(string msg)
        {
            _value.SetText(msg);
        }
    }
}
