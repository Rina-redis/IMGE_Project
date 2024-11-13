using UnityEngine;

public class GravitySourse : MonoBehaviour
{
    public virtual Vector3 GetGravity(Vector3 position)
    {
        return Physics.gravity;
    }

    void OnEnable()
    {
        CustomGravityManager.Register(this);
    }

    void OnDisable()
    {
        CustomGravityManager.Unregister(this);
    }
}
