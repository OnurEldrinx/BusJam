using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Station : Singleton<Station>
{
    [SerializeField] private List<WaitingTile> waitingTiles;
    [SerializeField] private List<Bus> busQueue;
    [SerializeField] private Bus currentBus;
    [SerializeField] private float offsetBetweenBusses;//4.25f
    private int _busPointer;
    private int _waitingTilePointer = -1;
    
    //Bus Events
    public static Action<Bus> BusArrived;
    public static Action<Bus> BusLeaving;

    private void OnEnable()
    {
        GameManager.LevelStarted += OnLevelStart;
    }

    private void OnDisable()
    {
        GameManager.LevelStarted -= OnLevelStart;
    }

    private void Start()
    {
        var firstZ = -offsetBetweenBusses;
        
        foreach (var bus in busQueue)
        {
            bus.transform.position = new Vector3(-2, -0.25f, firstZ);
            firstZ -= offsetBetweenBusses;
        }
    }

    private void OnLevelStart()
    {
        Bus first = busQueue.First();
        currentBus = first;

        var delayCounter = 0.25f;
        
        foreach (var bus in busQueue)
        {
            bus.MoveOnZ(bus.transform.position.z + offsetBetweenBusses, delayCounter).OnComplete(() =>
            {
                if (bus == first)
                {
                    BusArrived.Invoke(currentBus);
                    bus.SetInStation(true);
                }
            });
            delayCounter += 0.25f;
            
        }
        
    }

    public Bus GetCurrentBus()
    {
        return currentBus;
    }

    public void IncreaseBusPointer()
    {
        _busPointer++;
        BusLeaving.Invoke(currentBus);
        if (_busPointer >= busQueue.Count)
        {
            //GameManager.Instance.OnLevelSucceed();
            GameManager.Instance.levelSucceed = true;
        }
    }

    public void SetNewBus()
    {
        //_busPointer++;
        //BusLeaving.Invoke(currentBus);
        if (_busPointer >= busQueue.Count)
        {
            //GameManager.Instance.OnLevelSucceed();
            GameManager.Instance.levelSucceed = true;
            return;
        }
        currentBus.SetInStation(false);
        currentBus = null;
        Bus next = busQueue[_busPointer];
        next.SetInStation(false);

        foreach (var b in busQueue)
        {
            b.MoveOnZ(b.transform.position.z + offsetBetweenBusses, 0.25f).OnComplete(() =>
            {
                if (b == next)
                {
                    currentBus = next;
                    BusArrived.Invoke(currentBus);
                    currentBus.SetInStation(true);
                    SendWaitersToBus();
                }
            });
        }
        
        /*next.MoveOnZ(0,0.25f).OnComplete(()=>
        {
            currentBus = next;
            BusArrived.Invoke(currentBus);
            currentBus.SetInStation(true);
            SendWaitersToBus();
        });*/
    }

    public WaitingTile GetEmptyWaitingTile()
    {
        return waitingTiles.Find(e => e.IsEmpty());
    }

    public bool HasEmptyWaitingTile()
    {
        return waitingTiles.Find(e => e.IsEmpty()) is not null;
    }

    public void SendWaitersToBus()
    {
        
        foreach (var waitingTile in waitingTiles)
        {
            if (!waitingTile.IsEmpty() && (waitingTile.GetPassenger().GetColor() == currentBus.GetColor()))
            {
                waitingTile.GetPassenger().MoveTo(transform);
                waitingTile.ResetTileState();
            }
        }

    }

    public void SetBusQueue(List<Bus> q)
    {
        busQueue.Clear();
        busQueue = q;
    }

}
