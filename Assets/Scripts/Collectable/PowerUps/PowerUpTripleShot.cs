using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTripleShot : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.StartTripleShotCoroutine();
        base.ActivatePowerUp();
    }
}
