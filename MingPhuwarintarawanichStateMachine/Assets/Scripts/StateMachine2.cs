using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static StateMachine1;

public class StateMachine2 : MonoBehaviour
{
    public enum StateMachine2State
    {
        FindingMedicine, //had more then 3 tomatoes 
        WarningRaining,
        Cooking,
        Hiding,
        //nothing to do
        EatingTomato //
    }

    public StateMachine2State currentState;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
