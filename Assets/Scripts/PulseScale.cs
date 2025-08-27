using UnityEngine;

public class PulseScale : MonoBehaviour
{
    public float speed = 2f;      
    public float magnitude = 0.08f; 
    Vector3 baseScale;

    void Awake() => baseScale = transform.localScale;

    void Update()
    {
        float s = 1f + Mathf.Sin(Time.unscaledTime * speed) * magnitude;
        transform.localScale = baseScale * s;
    }
}
