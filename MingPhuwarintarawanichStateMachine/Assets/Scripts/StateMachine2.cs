using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static StateMachine1;

public class StateMachine2 : MonoBehaviour
{
    public enum StateMachine2State
    {
        FindingAntidote, 
        Cooking,
        DancingInTheRain,
        GivingUmbrella,
        EatingTomato
    }

    [SerializeField] TextMeshProUGUI st2Text;

    [SerializeField] GameObject tomato;
    [SerializeField] GameObject umbrella;
    public GameObject cookingLocation;
    [SerializeField] GameObject antidote;

    [SerializeField] RainController raining;

    NavMeshAgent st2;

    [SerializeField] StateMachine1 st1;

    public int numberOfEatenTomatoes;

    public StateMachine2State currentState;

    public bool isInteracting;

    [SerializeField] TextMeshProUGUI eatenText;

    private bool isEating;

    // Start is called before the first frame update
    void Start()
    {
        umbrella.SetActive(false);
        tomato.SetActive(false);
        currentState = StateMachine2State.Cooking;
        st2 = GetComponent<NavMeshAgent>();
        numberOfEatenTomatoes = 0;
        isInteracting = false;
        isEating = false;
    }

    // Update is called once per frame
    void Update()
    {
        eatenText.text = "Eaten Tomato: " + numberOfEatenTomatoes;
        switch (currentState)
        {
            case StateMachine2State.Cooking:
                Cooking();
                if (st1.isInteracting)
                {
                    st2.transform.LookAt(st1.transform);
                    currentState = StateMachine2State.EatingTomato;
                }
                else
                {
                    if (CheckRaining())
                    {
                        currentState = StateMachine2State.GivingUmbrella;
                    }
                    else
                    {
                        currentState = StateMachine2State.Cooking;
                    }
                }
                // code block
                break;
            //LookingForTomatos
            case StateMachine2State.EatingTomato:
                EatingTomato();
                break;
            //Sharing
            case StateMachine2State.FindingAntidote:
                FindAntidote();
                break;
            //Running
            case StateMachine2State.GivingUmbrella:
                GivingUmbrellaWithSt1();
                break;
            //Hiding
            case StateMachine2State.DancingInTheRain:
                if (raining.isRaining)
                {
                    DancingInTheRain();
                }
                else
                {
                    currentState = StateMachine2State.Cooking;
                }
                break;
            default:
                Cooking();
                break;
        }
    }

    void DancingInTheRain()
    {
        st2Text.text = "Dancing in the rain...";
        st2.transform.Rotate(0f, 250f * Time.deltaTime, 0f, Space.Self);
    }

    IEnumerator CountDownGivingUmbrella()
    {
        isInteracting = true;
        umbrella.SetActive(true);
        yield return new WaitForSeconds(3);
        st1.isHaveUmbrella = true;
        umbrella.SetActive(false);
        st2.isStopped = false;
        isInteracting = false;
        currentState = StateMachine2State.DancingInTheRain;
    }

    public bool IsSt2Stopped()
    {
        return st2.isStopped;
    }

    void GivingUmbrellaWithSt1()
    {
        st2Text.text = "Giving an umbrella...";
        if (Vector3.Distance(st2.transform.position, st1.transform.position) < 5)
        {
            st2.isStopped = true;
            umbrella.SetActive(true);
            StartCoroutine(CountDownGivingUmbrella());
        }
        else
        {
            st2.SetDestination(st1.transform.position);
        }
        //transform.LookAt(st2.transform);
    }

    bool CheckRaining()
    {
        return raining.isRaining;
    }

    void FindAntidote()
    {
        st2Text.text = "Getting an antidote...";
        if (Vector3.Distance(st2.transform.position, antidote.transform.position) < 1.1f)
        {
            numberOfEatenTomatoes = 0;
            currentState = StateMachine2State.Cooking;
        }
        st2.SetDestination(antidote.transform.position);
    }

    void EatingTomato()
    {
        st2Text.text = "Eating a tomato...";
        if (!isEating)
        {
            StartCoroutine(DelaySwallowTomato());
        }
    }

    IEnumerator DelaySwallowTomato()
    {
        isEating = true;
        tomato.SetActive(true);
        yield return new WaitForSeconds(4);
        tomato.SetActive(false);
        numberOfEatenTomatoes += 1;
        if (numberOfEatenTomatoes >= 2)
        {
            currentState = StateMachine2State.FindingAntidote;
        }
        else
        {
            currentState = StateMachine2State.Cooking;
        }
        isEating = false;
    }

    void Cooking()
    {
        st2Text.text = "Enjoy cooking...";
        st2.SetDestination(cookingLocation.transform.position);
        if(Vector3.Distance(st2.transform.position, cookingLocation.transform.position) < 2)
        {
            st2.transform.rotation = Quaternion.Lerp(st2.transform.rotation, cookingLocation.transform.rotation, Time.deltaTime);
        }
    }
}
