using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    [SerializeField] GameObject currentNode, nextNode, startNode, destinationNode, previousNode;

    [SerializeField] float movementSpeed;

    [SerializeField] GameObject secondDestination;

    private GameObject initialDestinationNode;
    private GameObject initialStartNode;

    // Start is called before the first frame update
    void Start()
    {
        currentNode = startNode;
        nextNode = currentNode;
        initialStartNode = startNode;
        initialDestinationNode = destinationNode;

        transform.position = currentNode.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
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
                Pathnode currentPathNode = currentNode.GetComponent<Pathnode>();
                Pathnode nextPathNode = nextNode.GetComponent<Pathnode>();
                if (currentPathNode.door != null && nextPathNode.door != null)
                {
                    if (!currentPathNode.door.isDoorLocked)
                    {
                        transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
                    }
                }
                else
                {
                    transform.Translate((nextNode.transform.position - transform.position).normalized * movementSpeed * Time.deltaTime);
                }
            }
        }
    }
}
