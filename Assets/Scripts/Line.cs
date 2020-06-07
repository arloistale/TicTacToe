using UnityEngine;

public class Line : MonoBehaviour
{
    public void ScaleToLength(Vector2 value)
    {
        transform.localScale = value;
    }
}
