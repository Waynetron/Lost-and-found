using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kino;

public class WeatherEffects : MonoBehaviour
{
    [SerializeField]
    AnalogGlitch analogGlitch;

    public void updateWeatherState(object newValue)
    {
        int stormTurnsRemaining = (int) newValue;
        if(stormTurnsRemaining > 0)
        {
            analogGlitch.scanLineJitter = 0.4f;
            analogGlitch.verticalJump = 0.01f;
            analogGlitch.horizontalShake = 0f;
            analogGlitch.colorDrift = 0.03f;
        }
        else
        {
            analogGlitch.scanLineJitter = 0.004f;
            analogGlitch.verticalJump = 0.006f;
            analogGlitch.horizontalShake = 0f;
            analogGlitch.colorDrift = 0f;
        }
    }
}
