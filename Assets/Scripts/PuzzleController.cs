using UnityEngine;

public abstract class PuzzleController : MonoBehaviour
{
    [SerializeField] private PuzzleFlag PuzzleType;

    protected void Complete() {
        GameManager.Instance.CompletePuzzle(PuzzleType);
    }
}
