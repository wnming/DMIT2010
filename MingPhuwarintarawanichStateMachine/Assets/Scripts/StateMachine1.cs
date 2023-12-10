using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine1 : MonoBehaviour
{
    enum StateMachine1State
    {
        Walking,
        PickupItem,
        Attacking, //if the state machine2 is in radius
        PlayWithDog
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
