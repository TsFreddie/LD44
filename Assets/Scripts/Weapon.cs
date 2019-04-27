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
    private float lastFireTime = 0;

    public void Firing(bool down) {
        if (((FireType == WeaponFireType.SemiAuto && down) || (FireType == WeaponFireType.FullAuto))
            && Time.time - lastFireTime > FireInterval) {
            if (OnFire()) {
                lastFireTime = Time.time;
            }
        }
    }

    protected abstract bool OnFire();
}
