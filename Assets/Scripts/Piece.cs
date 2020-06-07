using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField]
    private bool isRed;

    public bool IsRed => isRed;
}
