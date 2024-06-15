using UnityEngine;

public class DisableTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Passenger passenger))
        {
            passenger.gameObject.SetActive(false);
            passenger.SendToBus();
        }
        
    }
}
