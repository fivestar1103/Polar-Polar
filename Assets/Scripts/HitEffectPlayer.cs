using UnityEngine;

public class HitEffectPlayer : MonoBehaviour
{
    public ParticleSystem lightEffect;
    public ParticleSystem sparkEffect;

    void Start()
    {
        if (sparkEffect != null && !sparkEffect.isPlaying)
        {
            sparkEffect.Play();
        }

        if (lightEffect != null && !lightEffect.isPlaying)
        {
            lightEffect.Play();
        }

        Destroy(gameObject, 1.5f);
    }
}