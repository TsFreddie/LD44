using UnityEngine;

public enum WeaponFireType {
    FullAuto,
    SemiAuto
}

public abstract class Weapon : MonoBehaviour
{
    public float FireInterval = 0.03f;
    public int Damage = 1;
    public LayerMask Target;
    public WeaponFireType FireType;
    public AudioClip FireSound;
    private float lastFireTime = 0;
    private new AudioSource audio;
    
    void Start() {
        audio = GetComponentInParent<AudioSource>();
    }

    public void Firing(bool down) {
        if (((FireType == WeaponFireType.SemiAuto && down) || (FireType == WeaponFireType.FullAuto))
            && Time.time - lastFireTime > FireInterval) {
            if (OnFire()) {
                lastFireTime = Time.time;
                if (audio != null && FireSound != null) {
                    audio.PlayOneShot(FireSound);
                }
            }
        }
    }

    protected abstract bool OnFire();
}
