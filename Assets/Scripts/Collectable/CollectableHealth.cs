using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHealth : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.ReplenishLives();
        base.ActivatePowerUp();
    }
}
