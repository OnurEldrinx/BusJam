using DG.Tweening;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    [SerializeField] private Direction movementDirection;
    [SerializeField] private float xOffset;
    [SerializeField] private float yOffset;
    [SerializeField] private float duration;

    private Vector3 _defaultPosition;

    private void Awake()
    {
        _defaultPosition = transform.localPosition;
    }

    public void Open()
    {

        //Opening
        transform.DOLocalMoveX(_defaultPosition.x + xOffset,duration/2);

        float directionModifier = movementDirection switch
        {
            Direction.Left => -1,
            Direction.Right => 1,
            _ => 1
        };

        transform.DOLocalRotate(new Vector3(0, 0, -10 * directionModifier), duration / 2);
        
        transform.DOLocalRotate(Vector3.zero, duration / 2).SetDelay(duration/2);
        
        transform.DOLocalMoveY(_defaultPosition.y + (directionModifier * yOffset),duration/2).SetDelay(duration/2);
    }

    public void Close()
    {

        //Closing
        transform.DOLocalMoveY(_defaultPosition.y, duration/2);

        transform.DOLocalMoveX(_defaultPosition.x, duration/2).SetDelay(duration/2);
    }
    
}

public enum Direction
{
    Left,
    Right
}
