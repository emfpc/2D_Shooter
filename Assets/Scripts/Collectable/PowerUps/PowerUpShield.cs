using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.ActivateShields(true);
        base.ActivatePowerUp();
    }
}
