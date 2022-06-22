using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableRandomNegativeEffect : Collectable
{
    protected override void ActivatePowerUp()
    {
        _player.ActivatePowerUpSoudEffect();
        _player.StartNegativeEffect();
        base.ActivatePowerUp();
    }
}
