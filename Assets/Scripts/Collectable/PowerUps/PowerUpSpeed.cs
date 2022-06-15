using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.StartSpeedCoroutine();
        base.ActivatePowerUp();
    }
}
