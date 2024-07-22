using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialEffects : MonoBehaviour
{
    public ParticleSystem buffCarrot;
    public ParticleSystem buffCucumber;
    public ParticleSystem buffPepper;
    public ParticleSystem buffEgg;
    public ParticleSystem buffBread;
    public ParticleSystem debuffPepper;
    public ParticleSystem debuffCucumber;
    public ParticleSystem debuffEgg;
    public ParticleSystem debuffBread;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            EatCarrot();
        }
        if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            EatCucumber();
        }
        if (Input.GetKeyDown(KeyCode.Keypad2))
        {
            EatPepper();
        }
        if (Input.GetKeyDown(KeyCode.Keypad3))
        {
            EatEgg();
        }
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            EatBread();
        }
        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            HitCumcumber();
        }
        if (Input.GetKeyDown(KeyCode.Keypad6))
        {
            HitPepper();
        }
        if (Input.GetKeyDown(KeyCode.Keypad7))
        {
            HitEgg();
        }
        if (Input.GetKeyDown(KeyCode.Keypad8))
        {
            HitBread();
        }
    }
    public void EatCarrot()
    {
        if (!buffCarrot.isPlaying)
            buffCarrot.Play();
        else
        {
            buffCarrot.Stop();
        }
    }

    public void EatCucumber()
    {
        if (!buffCucumber.isPlaying)
            buffCucumber.Play();
        else
        {
            buffCucumber.Stop();
            buffCucumber.Play();
        }
    }

    public void EatPepper()
    {
        if (!buffPepper.isPlaying)
            buffPepper.Play();
        else
        {
            buffPepper.Stop();
            buffPepper.Play();
        }
    }

    public void EatEgg()
    {
        if (!buffEgg.isPlaying)
            buffEgg.Play();
        else
        {
            buffEgg.Stop();
            buffEgg.Play();
        }
    }
    public void EatBread()
    {
        if (!buffBread.isPlaying)
            buffBread.Play();
        else
        {
            buffBread.Stop();
            buffBread.Play();
        }
    }

    public void HitCumcumber()
    {
        if (!debuffCucumber.isPlaying) 
            debuffCucumber.Play();
        else
        {
            debuffCucumber.Stop();
            debuffCucumber.Play();
        }
    }

    public void HitPepper()
    {
        if (!debuffPepper.isPlaying)
            debuffPepper.Play();
        else
        {
            debuffPepper.Stop();
            debuffPepper.Play();
        }
    }

    public void HitEgg()
    {
        if (!debuffEgg.isPlaying)
            debuffEgg.Play();
        else
        {
            debuffEgg.Stop();
            debuffEgg.Play();
        }
    }

    public void HitBread()
    {
        if (!debuffBread.isPlaying)
            debuffBread.Play();
        else
        {
            debuffBread.Stop();
            debuffBread.Play();
        }
    }
}
