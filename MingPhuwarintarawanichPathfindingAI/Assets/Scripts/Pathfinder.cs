using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject currentNode, nextNode, startNode, destinationNode, previousNode;

    [SerializeField] float movementSpeed;

    [SerializeField] GameObject secondDestination;

    [SerializeField] TextMeshProUGUI stateText; 

    //[SerializeField] Rigidbody rb;

    private GameObject initialDestinationNode;
    private GameObject initialStartNode;

    public bool isMoving;

    // Start is called before the first frame update
    void Start()
    {
        currentNode = startNode;
        nextNode = currentNode;
        initialStartNode = startNode;
        initialDestinationNode = destinationNode;

        transform.position = currentNode.transform.position;

        isMoving = true;
    }

    public IEnumerator BlindFoldOn()
    {
        isMoving = false;
        stateText.text = "Blind fold";
        yield return new WaitForSeconds(4.0f);
        isMoving = true;
    }

    public IEnumerator DeplayAfterCapture()
    {
        isMoving = false;
        stateText.text = "Capturing";
        yield return new WaitForSeconds(2.0f);
        stateText.text = "Not moving";
        yield return new WaitForSeconds(2.0f);
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            stateText.text = "Patrolling";
        }
        if (currentNode == destinationNode)
        {
            if(destinationNode == initialStartNode) 
            {
                destinationNode = initialDestinationNode;
            }
            else
            {
                if (destinationNode != secondDestination)
                {
                    destinationNode = secondDestination;
                }
                else
                {
                    destinationNode = initialStartNode;
                }
            }
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
                    Pathnode currentPathNode = currentNode.GetComponent<Pathnode>();
                    Pathnode nextPathNode = nextNode.GetComponent<Pathnode>();
                    if (currentPathNode.door != null && nextPathNode.door != null)
                    {
                        if (!currentPathNode.door.isDoorLocked)
                        {
                            //transform.LookAt((nextNode.transform.position - transform.position).normalized);

                            transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
                        }
                    }
                    else
                    {
                        //transform.LookAt((nextNode.transform.position - transform.position).normalized);

                        transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);

                    }
                }
            }
        }
    }
}
