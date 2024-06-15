using UnityEngine;

public class WaitingTile:MonoBehaviour
{
    [SerializeField] private Passenger passenger;

    private bool _checkDistance = true;
    
    public bool IsEmpty()
    {
        return passenger is null;
    }

    public void SetPassenger(Passenger p)
    {
        passenger = p;
    }

    public Passenger GetPassenger()
    {
        return passenger;
    }

    private void Update()
    {
        if (passenger is not null && _checkDistance)
        {
            Vector3 p1 = transform.position;
            p1.y = 0;
            Vector3 p2 = passenger.transform.position;
            p2.y = 0;
            float distanceBetween = Vector3.Distance(p1, p2);
            
            if (distanceBetween < 0.5f)
            {
                passenger.StopMovement();
                _checkDistance = false;

                
                
            }
        }

        /*if (passenger is not null && (passenger.transform.localPosition.z >= 2f) && _checkDistance)
        {
            passenger.StopMovement();
            _checkDistance = false;
        }*/
        
    }

    public void ResetTileState()
    {
        passenger = null;
    }
}