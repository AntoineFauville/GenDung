﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Status_TemporaryLife : Explo_Status {

    public Explo_Status_TemporaryLife(Entities entities)
    {
        base.TickValue = 3;
        base.MaxDuration = 3;
        base.AnimationDuration = 1.5f;
        base.Duration = MaxDuration;
        base.EffectAnim = "";
        base.Entity = entities;
        base.EffectLayerAnimation();
    }
}
