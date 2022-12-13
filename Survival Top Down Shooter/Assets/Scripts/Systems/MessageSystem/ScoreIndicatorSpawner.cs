namespace Scripts.Systems.MessageSystem

{
    public class ScoreIndicatorSpawner : MessageSpawner
    {
        public void SpawnMessage(float scoreValue)
        {
            SpawnMessage(scoreValue.ToString());
        }
    }
}
