using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class SpyPath : MonoBehaviour
{
    [SerializeField] GameObject currentNode, nextNode, startNode, destinationNode, previousNode;

    [SerializeField] GameObject jailPosition;

    [SerializeField] float movementSpeed;

    [SerializeField] bool isRedSpy;

    [SerializeField] TextMeshProUGUI stateText;
    [SerializeField] TextMeshProUGUI documentText;

    private bool isMoving;
    private bool isReadyToInvisible;

    private int document;

    private List<GameObject> destinationList = new List<GameObject>();

    private int index;

    [SerializeField] GameObject greenRoomDestinationNode, blueRoomDestinationNode, pinkRoomDestinationNode, orangeRoomDestinationNode, purpleRoomDestinationNode, grayRoomDestinationNode, yellowRoomDestinationNode;

    private GameObject initialStartNode;

    // Start is called before the first frame update
    void Start()
    {
        isMoving = true;
        isReadyToInvisible = true;
        currentNode = startNode;
        nextNode = currentNode;
        initialStartNode = startNode;

        documentText.text = "Destroy: " + document + " doc(s)"; ;

        transform.position = currentNode.transform.position;

        index = 0;
        if (isRedSpy)
        {
            destinationList.Add(greenRoomDestinationNode);
            destinationList.Add(grayRoomDestinationNode);
            destinationList.Add(blueRoomDestinationNode);
            destinationList.Add(pinkRoomDestinationNode);
            destinationList.Add(orangeRoomDestinationNode);
            destinationList.Add(purpleRoomDestinationNode);
            destinationList.Add(yellowRoomDestinationNode);
        }
        else
        {
            destinationList.Add(yellowRoomDestinationNode);
            destinationList.Add(purpleRoomDestinationNode);
            destinationList.Add(orangeRoomDestinationNode);
            destinationList.Add(pinkRoomDestinationNode);
            destinationList.Add(blueRoomDestinationNode);
            destinationList.Add(greenRoomDestinationNode);
            destinationList.Add(grayRoomDestinationNode);
        }

        destinationNode = destinationList[index];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Guard" && isReadyToInvisible)
        {
            //StartCoroutine(InvisibleCounter());
            //StartCoroutine(ReadyToInvisibleCounter());
        }
        if (other.gameObject.tag == "Guard" && isMoving && Vector3.Distance(transform.position, other.gameObject.transform.position) < 2.0f)
        {
            StartCoroutine(JailCounter());
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Document" && Vector3.Distance(transform.position, other.gameObject.transform.position) < 2.0f)
        {
            document += 1;
            documentText.text = "Destroy: " + document + " doc(s)";
            //Debug.Log(Vector3.Distance(transform.position, other.gameObject.transform.position));
            other.gameObject.SetActive(false);
        }
        if (other.gameObject.tag == "Guard" && isMoving && Vector3.Distance(transform.position, other.gameObject.transform.position) < 2.0f)
        {
            StartCoroutine(JailCounter());
        }
    }

    IEnumerator JailCounter()
    {
        stateText.text = "In jail";
        transform.position = jailPosition.transform.position;
        isMoving = false;
        yield return new WaitForSeconds(8.0f);
        isMoving = true;
        transform.position = initialStartNode.transform.position;
        currentNode = initialStartNode;
        nextNode = currentNode;
    }

    IEnumerator InvisibleCounter()
    {
        stateText.text = "Invisible";
        isMoving = false;
        yield return new WaitForSeconds(4.0f);
        isMoving = true;
    }

    IEnumerator ReadyToInvisibleCounter()
    {
        isReadyToInvisible = false;
        yield return new WaitForSeconds(30.0f);
        isReadyToInvisible = true;
    }

    void Update()
    {
        if (currentNode == destinationNode)
        {
            index += 1;
            destinationNode = destinationList[index];
            startNode = currentNode;
        }
        else
        {
            if (Vector3.Distance(transform.position, nextNode.gameObject.transform.position) < 0.1f)
            {
                previousNode = currentNode;
                currentNode = nextNode;

                float closest = 10000.0f;
                Pathnode pathnode = currentNode.GetComponent<Pathnode>();
                GameObject targetNode = currentNode;

                for (int i = 0; i < pathnode.connections.Count; i++)
                {
                    if (Vector3.Distance(destinationNode.transform.position, pathnode.connections[i].transform.position) < closest && pathnode.connections[i] != previousNode)
                    {
                        closest = Vector3.Distance(destinationNode.transform.position, pathnode.connections[i].transform.position);
                        targetNode = pathnode.connections[i];
                    }
                }

                nextNode = targetNode;
                //nextNode = currentNode.GetComponent<Pathnode>().connections[Random.Range(0, currentNode.GetComponent<Pathnode>().connections.Count)];

            }
            else
            {
                if (isMoving)
                {
                    stateText.text = "Spying";
                    transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
                }
            }
        }
    }
}
