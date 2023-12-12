using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine1 : MonoBehaviour
{
    public GameObject dogLocation;
    [SerializeField] GameObject dog;
    [SerializeField] TextMeshProUGUI st1Text;
    [SerializeField] TextMeshProUGUI tomatoText;

    [SerializeField] TextMeshProUGUI seftText;

    [SerializeField] GameObject umbrella;
    [SerializeField] GameObject useUmbrellaLocation;

    private NavMeshAgent st1;

    [SerializeField] StateMachine2 st2;

    [SerializeField] RainController raining;
    [SerializeField] GameObject destination1;
    [SerializeField] GameObject destination2;
    private GameObject currentDestination;

    private int sharingNumber;

    [SerializeField] TomatoController tomatoController;

    TomatoScript currentTomato;

    public int tomatoesCount = 0;

    public bool isHaveUmbrella;
    public bool isOkayToRain;

    public bool isInteracting;

    public enum StateMachine1State
    {
        PlayingWithADog,
        LookingForTomatoes,
        Sharing,
        Running,
        UsingUmbrella
    }

    public StateMachine1State currentState;
    // Start is called before the first frame update
    void Start()
    {
        currentDestination = destination1;
        isHaveUmbrella = false;
        sharingNumber = 2;
        st1 = GetComponent<NavMeshAgent>();
        umbrella.SetActive(false);
        isOkayToRain = false;
        isInteracting = false;
    }

    // Update is called once per frame
    void Update()
    {
        tomatoText.text = "Tomato: " + tomatoesCount;
        switch (currentState)
        {
            //PlayWithMyDog
            case StateMachine1State.PlayingWithADog:
                if (CheckRaining()) 
                {
                    currentState = StateMachine1State.Running;
                }
                else
                {
                    if (tomatoController.IsTomatoShows())
                    {
                        currentState = StateMachine1State.LookingForTomatoes;
                    }
                    else
                    {
                        PlayWithDog();
                    }
                }
                break;
            //LookingForTomatos
            case StateMachine1State.LookingForTomatoes:
                GoGetTomato();
                if (tomatoesCount == sharingNumber)
                {
                    currentState = StateMachine1State.Sharing;
                }
                else
                {
                    currentState = StateMachine1State.PlayingWithADog;
                }
                break;
            //Sharing
            case StateMachine1State.Sharing:
                sharingWithSt2();
                break;
            //Running
            case StateMachine1State.Running:
                if (!isHaveUmbrella)
                {
                    if (st2.isInteracting && st2.currentState == StateMachine2.StateMachine2State.GivingUmbrella)
                    {
                        st1.isStopped = true;
                        st1.transform.LookAt(st2.transform);
                        //st1.transform.rotation = Quaternion.Lerp(st2.transform.rotation, transform.rotation, Time.deltaTime);
                    }
                    Running();
                }
                else
                {
                    st1.isStopped = false;
                    currentState = StateMachine1State.UsingUmbrella;
                }
                break;
            //UsingUmbrella
            case StateMachine1State.UsingUmbrella:
                if (raining.isRaining)
                {
                    UsingUmbrealla();
                }
                else
                {
                    umbrella.SetActive(false);
                    st1.isStopped = false;
                    isHaveUmbrella = false;
                    currentState = StateMachine1State.PlayingWithADog;
                }
                break;
            default:
                PlayWithDog();
                break;
        }
    }

    void UsingUmbrealla()
    {
        st1Text.text = "Using the umbrella...";
        umbrella.SetActive(true);
        if (Vector3.Distance(transform.position, useUmbrellaLocation.transform.position) < 1 && !st1.isStopped)
        {
            st1.transform.rotation = Quaternion.Lerp(st1.transform.rotation, useUmbrellaLocation.transform.rotation, Time.deltaTime);
        }
        else
        {
            st1.SetDestination(useUmbrellaLocation.transform.position);
        }
    }

    bool CheckRaining() 
    {
        return raining.isRaining && !isHaveUmbrella;
    }

    void Running()
    {
        st1Text.text = "Running back and forth...";
        if (Vector3.Distance(st1.transform.position, destination1.transform.position) < 2)
        {
            currentDestination = destination2;
        }
        if (Vector3.Distance(st1.transform.position, destination2.transform.position) < 2)
        {
            currentDestination = destination1;
        }
        st1.SetDestination(currentDestination.transform.position);
    }

    IEnumerator CountDownSharing()
    {
        yield return new WaitForSeconds(4);
        isInteracting = false;
        currentState = StateMachine1State.PlayingWithADog;
        st1.isStopped = false;
        isOkayToRain = true;
    }

    void sharingWithSt2()
    {
        st1Text.text = "Sharing a tomato...";
        if (Vector3.Distance(transform.position, st2.transform.position) < 5 && !st1.isStopped)
        {
            isInteracting = true;
            st1.isStopped = true;
            tomatoesCount -= 1;
            sharingNumber += 2;
            StartCoroutine(CountDownSharing());
        }
        else
        {
            st1.SetDestination(st2.transform.position);
        }
    }

    void GoGetTomato()
    {
        st1Text.text = "Looking for a tomato...";
        currentTomato = tomatoController.getThisTomato();
        if (Vector3.Distance(st1.transform.position, currentTomato.transform.position) < 1.1f)
        {
            currentTomato.gameObject.SetActive(false);
            tomatoesCount += 1;
        }
        st1.SetDestination(currentTomato.transform.position);
    }

    void PlayWithDog()
    {
        st1Text.text = "Playing with my dog...";
        currentState = 0;
        if (Vector3.Distance(st1.transform.position, dogLocation.transform.position) < 2)
        {
            st1.transform.rotation = Quaternion.Lerp(st1.transform.rotation, dogLocation.transform.rotation, Time.deltaTime);
        }
        st1.SetDestination(dogLocation.transform.position);
    }
}
