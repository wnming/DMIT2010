using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainController : MonoBehaviour
{
    public GameObject rain;
    [SerializeField] StateMachine1 st1;
    [SerializeField] StateMachine2 st2;

    public bool isRaining = false;

    void Start()
    {
        isRaining = false;
        rain.SetActive(false);
    }

    void Update()
    {
        if(!isRaining && st1.isOkayToRain && Vector3.Distance(st1.transform.position, st1.dogLocation.transform.position) < 3 && st1.currentState == StateMachine1.StateMachine1State.PlayingWithADog && st2.currentState == StateMachine2.StateMachine2State.Cooking && Vector3.Distance(st2.transform.position, st2.cookingLocation.transform.position) < 2)
        {
            StartCoroutine(MakeRains());
        }
    }

    IEnumerator MakeRains()
    {
        yield return new WaitForSeconds(2);
        st1.isOkayToRain = false;
        isRaining = true;
        PlayParticleSystem();
        rain.SetActive(true);
        yield return new WaitForSeconds(35);
        isRaining = false;
        rain.SetActive(false);
    }

    void PlayParticleSystem()
    {
        ParticleSystem[] childScripts = rain.GetComponentsInChildren<ParticleSystem>();
        for (int index = 0; index < childScripts.Length; index++)
        {
            childScripts[index].Play();
        }
    }
}
