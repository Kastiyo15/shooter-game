using UnityEngine;


public class EffectDisable : MonoBehaviour
{
    private void OnEnable()
    {
        Invoke("Disable", 0.5f);
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
