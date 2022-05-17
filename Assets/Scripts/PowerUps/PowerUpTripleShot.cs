using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpTripleShot : PowerUps
{
    protected override void ActivatePowerUp()
    {
        _player.StartTripleShotCoroutine();
        base.ActivatePowerUp();
    }
}
