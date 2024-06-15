using System;
using DG.Tweening;
using UnityEngine;

public class Passenger : TileContent
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform moveTarget;
    [SerializeField] private ColorOptions color;
    [SerializeField] private SkinnedMeshRenderer meshRenderer;
    
    private static readonly int MoveKey = Animator.StringToHash("Move");
    private static readonly int SitKey = Animator.StringToHash("Sit");
    private static readonly int StopKey = Animator.StringToHash("Stop");

    public bool canMove = true;

    //public PassengerState state;
    public bool isMoving;
    
    private void Awake()
    {
        animator = GetComponent<Animator>();
        moveTarget = Station.Instance.transform;
        meshRenderer.materials[0].color = Resources.Load<Material>("Colors/"+color).color;
        //state = PassengerState.Waiting;
    }

    /*private void Update()
    {
        switch (state)
        {
            case PassengerState.MovingToWaitingTile:
                if (IsValidBus())
                {
                    MoveTo(moveTarget);
                    state = PassengerState.MovingToBus;
                } 
                break;
        }
    }*/

    public override void OnTap(Tile tile)
    {
        print("Passenger On Tap");
        if (IsValidBus() && Station.Instance.GetCurrentBus().IsInStation())
        {
            MoveTo(moveTarget);
            tile.DisableCollider();
            //state = PassengerState.MovingToBus;
        }else if (Station.Instance.HasEmptyWaitingTile())
        {
            print("Can Wait");
            WaitingTile waitingTile = Station.Instance.GetEmptyWaitingTile();
            waitingTile.SetPassenger(this);
            MoveTo(waitingTile.transform);
            tile.DisableCollider();
            //state = PassengerState.MovingToWaitingTile;

        }
    }
    
    public void MoveTo(Transform target)
    {
        transform.DOLookAt(target.position, 0.1f, AxisConstraint.Y);
        animator.SetTrigger(MoveKey);
    }

    private bool IsValidBus()
    {
        Bus current = Station.Instance.GetCurrentBus();
        if (current is null)
        {
            return false;
        }
        return current.GetColor() == color;
    }

    public void SendToBus()
    {
        transform.localScale = Vector3.zero;

        Transform emptySeat = Station.Instance.GetCurrentBus().GetEmptySeat();

        if (emptySeat is null)
        {
            print("Bus is full");
            return;
        }

        if (Station.Instance.GetCurrentBus().GetColor() != color)
        {
            return;
        }

        transform.parent = emptySeat;

        
        gameObject.SetActive(true);
        animator.SetTrigger(SitKey);
        transform.position = emptySeat.position + new Vector3(0,0,0.2f);
        transform.rotation = Quaternion.Euler(Vector3.zero);
        transform.DOScale(Vector3.one * 0.009f, 0.25f).SetDelay(0.1f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            //transform.parent = emptySeat;
            VFXManager.Instance.PlayPopEffect(transform);
        });
        //state = PassengerState.Sitting;

    }

    public void StopMovement()
    {
        animator.SetTrigger(StopKey);
        //state = PassengerState.Waiting;
    }

    public void SetColor(ColorOptions c)
    {
        color = c;
    }

    public ColorOptions GetColor()
    {
        return color;
    }
    
}

public enum PassengerState
{
    Waiting,
    MovingToBus,
    MovingToWaitingTile,
    Sitting
}
