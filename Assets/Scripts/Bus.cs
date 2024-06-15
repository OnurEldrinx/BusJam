using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Bus : MonoBehaviour
{
    [SerializeField] private ColorOptions color;
    [SerializeField] private List<Transform> seats;
    [SerializeField] private float speed;
    private int _seatPointer = -1;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private bool _inStation;
    
    
    private void Awake()
    {
        meshRenderer.materials[0].color = Resources.Load<Material>("Colors/"+color).color;
    }

    public ColorOptions GetColor()
    {
        return color;
    }

    private void IncreaseSeatPointer()
    {
        _seatPointer++;

        if (IsFull())
        {
            print("Bus is full");
            MoveOnZ(6,0.75f).OnComplete(() =>
            {
                if (GameManager.Instance.levelSucceed)
                {
                    VFXManager.Instance.PlayConfettiBlast();
                    UIManager.Instance.OnLevelEnd(false);
                }
                Station.Instance.SetNewBus();

            });
            Station.Instance.IncreaseBusPointer();
        }
        
    }

    public Tweener MoveOnZ(float target,float delay)
    {
        var distance = Mathf.Abs(target - transform.position.z);
        return transform.DOMoveZ(target, distance/speed).SetDelay(delay);
    }

    public int GetSeatPointer()
    {
        return _seatPointer;
    }

    private bool IsFull()
    {
        return _seatPointer >= 2;
    }

    public Transform GetEmptySeat()
    {
        IncreaseSeatPointer();
        return _seatPointer > 2 ? null : seats[_seatPointer];
    }

    public void SetColor(ColorOptions c)
    {
        color = c;
    }

    public void SetInStation(bool b)
    {
        _inStation = b;
        if (_inStation)
        {
            Invoke(nameof(ReCheckWaitingTiles),0.1f);
        }
    }

    public bool IsInStation()
    {
        return _inStation;
    }

    private void ReCheckWaitingTiles()
    {
        Station.Instance.SendWaitersToBus();
    }
}
