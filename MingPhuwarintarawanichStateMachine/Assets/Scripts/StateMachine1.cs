using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class StateMachine1 : MonoBehaviour
{
    [SerializeField] GameObject dogLocation;
    [SerializeField] GameObject dog;
    [SerializeField] TextMeshProUGUI st1Text;
    [SerializeField] TextMeshProUGUI tomatoText;

    [SerializeField] TextMeshProUGUI seftText;

    private NavMeshAgent st1;
    private bool isInteracting;

    [SerializeField] StateMachine2 st2;

    [SerializeField] GameObject raining;

    private int sharingNumber;

    [SerializeField] TomatoController tomatoController;

    TomatoScript currentTomato;

    int tomatoesCount = 0;

    public enum StateMachine1State
    {
        PlayWithDog,
        LookingForTomatoes, //found item
        Sharing, //after picking up item
        Hiding
    }

    public StateMachine1State currentState;
    // Start is called before the first frame update
    void Start()
    {
        isInteracting = false;
        sharingNumber = 1;
        st1 = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        tomatoText.text = "Tomato: " + tomatoesCount;
        switch (currentState)
        {
            case StateMachine1State.PlayWithDog:
                if (tomatoController.IsTomatoShows())
                {
                    currentState = StateMachine1State.LookingForTomatoes;
                }
                else
                {
                    PlayWithDog();
                }
                // code block
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
                    currentState = StateMachine1State.PlayWithDog;
                }
                //&& //tomatoes
                //if (CheckNumberOfTomatoes() >= sharingNumber && //tomatoes) 
                //{
                //    sharingNumber += 3;
                //    //stop
                //}
                //else
                //{
                //    currentState = StateMachine1State.PlayWithDog;
                //}
                // code block
                break;
            //Sharing
            case StateMachine1State.Sharing:
                sharingWithSt2();
                //if (st2.currentState == StateMachine2.StateMachine2State.EatingTomato)
                //{
                //    currentState = StateMachine1State.PlayWithDog;
                //    //stop
                //}
                //else
                //{
                //    sharingWithSt2();
                //}
                // code block
                break;
            //Hiding
            case StateMachine1State.Hiding:
                //if (!raining.activeSelf)
                //{
                //    PlayWithDog();
                //}
                // code block
                break;
            default:
                PlayWithDog();
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if(other.gameObject.tag == "Tomato" && )
        //{
        //    tomatoesCount
        //}
    }

    IEnumerator CountDownSharing()
    {
        yield return new WaitForSeconds(4);
        currentState = StateMachine1State.PlayWithDog;
    }

    void sharingWithSt2()
    {
        st1Text.text = "Sharing a tomato...";
        Debug.Log(Vector3.Distance(transform.position, st2.transform.position));
        if (Vector3.Distance(transform.position, st2.transform.position) < 5)
        {
            st1.isStopped = true;
            //fixxxxxxxxxxxxxx
            tomatoesCount -= 1;
            sharingNumber += 4;
            //Debug.Log(Vector3.Distance(transform.position, st2.transform.position));
            //seftText.text = "Enjoy the tomato!";
            //StartCoroutine(CountDownSharing());
        }
        else
        {
            st1.SetDestination(st2.transform.position);
        }
        //transform.LookAt(st2.transform);
    }

    int CheckNumberOfTomatoes()
    {
        return 0;
    }

    bool CheckForTomatoes()
    {
        return true;
    }

    void GoGetTomato()
    {
        st1Text.text = "Looking for a tomato...";
        currentTomato = tomatoController.getThisTomato();
        Debug.Log(Vector3.Distance(st1.transform.position, currentTomato.transform.position));
        if (Vector3.Distance(st1.transform.position, currentTomato.transform.position) < 1.1f)
        {
            currentTomato.gameObject.SetActive(false);
            tomatoesCount += 1;
        }
        st1.SetDestination(currentTomato.transform.position);
    }

    void PlayWithDog()
    {
        st1Text.text = "Playing with my dog";
        currentState = 0;
        st1.SetDestination(dogLocation.transform.position);
        //transform.LookAt(dog.transform);
    }
}
