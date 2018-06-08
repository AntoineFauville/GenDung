using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explo_Status {

    private int maxDuration;
    private int duration;
    private float animationDuration;
    private float tickValue;
    private string effectAnim;
    private Entities entity;

    public Explo_Status()
    {
  
    }

    public void DoSomething(int statusIndex)
    {
        Debug.Log(entity.Name + " has been affected by " + effectAnim + " for a duration of" + duration + " turns");

        if (CheckDuration( statusIndex))
        {
            EffectLayerAnimation();
            entity.ChangeHealth(tickValue,false);
            duration--;
            Debug.Log("my duration is now " + duration);
        }
    }

    public bool CheckDuration( int statusIndex)
    {
        if (duration <= 0)
        {
            entity.EntitiesStatus.RemoveAt(statusIndex);
            Debug.Log("my duration is zero for " + effectAnim + " and has been removed");
            return false;
        }
        else
        {
            return true;
        }
    }

    public void EffectLayerAnimation()
    {
        entity.EntitiesEffectAnimator.Play(EffectAnim);
    }

    IEnumerator WaitForSwitchBackAnimation()
    {
        yield return new WaitForSeconds(animationDuration);
        entity.EntitiesEffectAnimator.Play("Effect_None");
    }

    public int MaxDuration
    {
        get
        {
            return maxDuration;
        }

        set
        {
            maxDuration = value;
        }
    }

    public int Duration
    {
        get
        {
            return duration;
        }

        set
        {
            duration = value;
        }
    }

    public float TickValue
    {
        get
        {
            return tickValue;
        }

        set
        {
            tickValue = value;
        }
    }

    public string EffectAnim
    {
        get
        {
            return effectAnim;
        }

        set
        {
            effectAnim = value;
        }
    }

    public float AnimationDuration
    {
        get
        {
            return animationDuration;
        }

        set
        {
            animationDuration = value;
        }
    }

    public Entities Entity
    {
        get
        {
            return entity;
        }

        set
        {
            entity = value;
        }
    }
}
