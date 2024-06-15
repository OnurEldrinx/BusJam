using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private TileContent content;
    private BoxCollider _boxCollider;
    
    
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        if (content == null) return;
        var position = transform.position;
        var contentTransform = content.transform;
        contentTransform.position = new Vector3(position.x, contentTransform.position.y, position.z);
    }

    public void OnTap()
    {
        if (content is null) return;
        content.OnTap(this);
    }

    public void DisableCollider()
    {
        _boxCollider.enabled = false;
    }

    public TileContent GetContent()
    {
        return content;
    }

    public void SetContent(TileContent c)
    {
        content = c;
    }

    public string GetContentInfo()
    {
        switch (content.type)
        {
            case TileContentTypes.Passenger:
                return ((Passenger)content).GetColor() + " Passenger";
            case TileContentTypes.Obstacle:
                return "Obstacle";
        }

        return "Empty";

    }
    
}
