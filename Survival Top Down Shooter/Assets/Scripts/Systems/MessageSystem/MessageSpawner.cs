using UnityEngine;


namespace Scripts.Systems.MessageSystem
{

    public class MessageSpawner : MonoBehaviour
    {
        [SerializeField]
        private Vector2 _initialPosition;

        [SerializeField]
        private GameObject _messagePrefab;


        // Instantiate a message prefab
        public void SpawnMessage(string msg)
        {
            var msgObj = Instantiate(_messagePrefab, GetSpawnPosition(), Quaternion.identity);
            var inGameMessage = msgObj.GetComponent<IInGameMessage>();
            inGameMessage.SetMessage(msg);
        }


        // Return position where it should spawn
        private Vector3 GetSpawnPosition()
        {
            return transform.position + (Vector3) _initialPosition;
        }
    }
}
