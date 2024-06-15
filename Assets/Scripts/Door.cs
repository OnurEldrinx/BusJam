using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private DoorAnimation[] doors;
    
    private Bus _parent;

    private void Start()
    {
        _parent = transform.root.GetComponent<Bus>();

        foreach (var door in doors)
        {
            door.GetComponent<MeshRenderer>().materials[0].color = Resources.Load<Material>("Colors/" + _parent.GetColor()).color;
        }    
    }

    private void OnEnable()
    {
        Station.BusArrived += OpenDoors;
        Station.BusLeaving += CloseDoors;
    }
    
    private void OnDisable()
    {
        Station.BusArrived -= OpenDoors;
        Station.BusLeaving -= CloseDoors;
    }
    
    private void CloseDoors(Bus bus)
    {
        
        if (_parent != bus)
        {
            return;
        }
        
        foreach (var d in doors)
        {
            d.Close();
        }
    }

    private void OpenDoors(Bus bus)
    {
        if (_parent != bus)
        {
            return;
        }
        
        foreach (var d in doors)
        {
            d.Open();
        }
    }
}
