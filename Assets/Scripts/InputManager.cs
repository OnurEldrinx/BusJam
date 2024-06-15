using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{

    public static Action TapEvent;
    private Camera _mainCamera;
    [SerializeField] private LayerMask tappableLayer;
    
    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void Update()
    {

        if (!GameManager.Instance.levelStarted)
        {
            return;
        }
        
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Began)
            {
                DetectTapOnTarget(t.position);
            }

        }
    }

    private void DetectTapOnTarget(Vector2 touchPosition)
    {
        Ray ray = _mainCamera.ScreenPointToRay(touchPosition);

        if (!Physics.Raycast(ray, out var hit,float.MaxValue,tappableLayer)) return;
        
        print("Tapped on " + hit.transform.name);

        if (hit.transform.TryGetComponent(out Tile tile))
        {
            print(tile.name);
            tile.OnTap();
        }
    }
}
