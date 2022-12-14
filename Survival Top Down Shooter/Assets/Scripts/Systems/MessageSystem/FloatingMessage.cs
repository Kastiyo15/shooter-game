using UnityEngine;
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


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _value = GetComponentInChildren<TMP_Text>();
        }


        private void Start()
        {
            if (InitialXVelocityRange < 1f)
            {
                _rigidbody.velocity = new Vector2(InitialXVelocityRange, InitialYVelocity);
            }
            else
            {
                _rigidbody.velocity = new Vector2(Random.Range(-InitialXVelocityRange, InitialXVelocityRange), InitialYVelocity);
            }

            Destroy(gameObject, LifeTime);
        }


        public void SetMessage(string msg)
        {
            _value.SetText(msg);
        }
    }
}
