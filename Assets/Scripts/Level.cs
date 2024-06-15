using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Level : MonoBehaviour
{
    [SerializeField] private int levelID;
    [SerializeField] private List<Bus> busQueue = new();
    [SerializeField] private List<Tile> tiles = new();

    private void Awake()
    {
        foreach (var t in busQueue)
        {
            t.transform.parent = null;
        }
    }

    public void InsertBus(Bus bus)
    {
        busQueue.Add(bus);
    }

    public List<Bus> BusQueue()
    {
        return busQueue;
    }

    public void InsertTile(Tile tile)
    {
        tiles.Add(tile);
    }

    public List<Tile> Tiles()
    {
        return tiles;
    }

    public void SetLevelToCenter()
    {
        var firstTileZ = tiles.First().transform.position.z;
        var lastTileZ = tiles.Last().transform.position.z;
        var center = (firstTileZ + lastTileZ) / 2;

        transform.position = new Vector3(2,transform.position.y,-center);

    }
    
}
