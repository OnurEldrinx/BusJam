using UnityEngine;

public abstract class TileContent:MonoBehaviour
{
    public TileContentTypes type;
    public abstract void OnTap(Tile tile);
}
