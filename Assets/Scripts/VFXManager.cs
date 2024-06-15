using System.Linq;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] private ParticleSystem confettiBlast;
    [SerializeField] private ParticleSystem[] popEffects;

    public void PlayConfettiBlast()
    {
        confettiBlast.Play();
    }
    
    public void PlayPopEffect(Transform t)
    {
        var p = popEffects.First(e => !e.gameObject.activeInHierarchy);
        p.gameObject.SetActive(true);
        p.Stop();
        var pTransform = p.transform;
        pTransform.parent = t;
        pTransform.localPosition = new Vector3(0, 0.25f, 0);
        p.Play();
    }
}
