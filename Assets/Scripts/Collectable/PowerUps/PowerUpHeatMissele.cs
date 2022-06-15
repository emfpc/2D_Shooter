using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpHeatMissele : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.StartHeatMisseleCoroutine();
        base.ActivatePowerUp();
    }
}
