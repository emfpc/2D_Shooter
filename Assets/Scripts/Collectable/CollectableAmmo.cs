using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableAmmo : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ReplenishAmmo();
        _player.ActivatePowerUpSoudEffect();
        base.ActivatePowerUp();
    }
}
