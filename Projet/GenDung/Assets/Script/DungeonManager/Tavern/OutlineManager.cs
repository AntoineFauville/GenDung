using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OutlineManager : MonoBehaviour
{
    public CustomOutline CustomOutline;

    private void Start()
    {
        ActivateOutline(false);
    }

    public void ActivateOutline(bool activate)
    {
        CustomOutline.glintEffect = activate;
        if (activate)
        {
            CustomOutline.m_size = 1;
        }
        else
        {
            CustomOutline.m_size = 0;
        }
    }
}
