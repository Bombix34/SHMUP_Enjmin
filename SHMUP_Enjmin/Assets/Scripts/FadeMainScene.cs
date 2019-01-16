﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeMainScene : MonoBehaviour
{

    [SerializeField] FadeScene fade;

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            fade.FadeOut();
        }
    }
}
