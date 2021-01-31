using System;
using System.Collections;
using System.Collections.Generic;
using Arif.Scripts;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource Part1;
    public AudioSource Part2;
    public AudioSource Part3;
    public AnimationCurve Part1Curve;
    public AnimationCurve Part2Curve;
    public AnimationCurve Part3Curve;

    private void Start()
    {
        /*  Part1.volume = 0;
          Part2.volume = 0;
          Part3.volume = 0;*/
        Part1.Play();
        Part2.Play();
        Part3.Play();
    }

    private void OnDisable()
    {
        Shader.SetGlobalFloat("_MTimer_", 0);
    }

    private void Update()
    {
        float maxdaytime = LevelManager.Manager.maxDayTime;
        float daytimer = LevelManager.Manager.dayTimer;
        float MVal = (daytimer / maxdaytime);
        Debug.Log(MVal);
        Part1.volume = Part1Curve.Evaluate(MVal);
        Part2.volume = Part2Curve.Evaluate(MVal);
        Part3.volume = Part3Curve.Evaluate(MVal);
        if (MVal > 0.7)
            Shader.SetGlobalFloat("_MTimer_", MVal);
        else
            Shader.SetGlobalFloat("_MTimer_", 0);
    }
}