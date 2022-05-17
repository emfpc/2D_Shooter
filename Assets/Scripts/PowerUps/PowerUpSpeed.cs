using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpeed : PowerUps
{
    protected override void ActivatePowerUp()
    {
        _player.StartSpeedCoroutine();
        base.ActivatePowerUp();
    }
}
