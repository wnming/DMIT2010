using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MoverScript : MonoBehaviour
{
    float movementSpeed, initialSpeed = 5.0f, boostSpeed = 8.0f;
    float forwardDist = 1.0f, sideDist = 3.0f;

    bool isLeft, isRight;

    float range = 8;

    RaycastHit hit;

    private bool isBeingFollowed = false;

    Vector3 direction;

    Rigidbody rb;

    public bool isInvisible = false;

    //[SerializeField] List<GameObject> collectables;
    //[SerializeField] GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        direction = Vector3.forward;
        rb = GetComponent<Rigidbody>();
        movementSpeed = initialSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //CheckTargets();

        AvoidWalls();
        //GetAwayFromHunter();
        transform.Translate(direction * movementSpeed * Time.deltaTime);
        GetAwayFromHunter();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Runner")
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
        if (other.gameObject.tag == "Hunter" && !isInvisible)
        {
            gameObject.SetActive(false);
        }
        if(other.gameObject.tag == "SpeedBoost")
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ChangeSpeed());
        }
        if (other.gameObject.tag == "DisguiseItem")
        {
            other.gameObject.SetActive(false);
            isInvisible = true;
            StartCoroutine(Invisible());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Runner")
        {
            gameObject.GetComponent<BoxCollider>().enabled = true;
        }
    }

    IEnumerator Invisible()
    {
        transform.Find("Rabbit").GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(7);
        transform.Find("Rabbit").GetComponent<Renderer>().material.color = Color.white;
        isInvisible = false;
    }

    IEnumerator ChangeSpeed()
    {
        movementSpeed = boostSpeed;
        yield return new WaitForSeconds(5);
        movementSpeed = initialSpeed;
    }

    void AvoidWalls()
    {
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDist))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {

                // Rotate based on what is to the sides
                isLeft = Physics.Raycast(transform.position, -transform.right, sideDist);
                isRight = Physics.Raycast(transform.position, transform.right, sideDist);

                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, -hit.normal, 1, 1));

                if (isLeft && isRight)
                {
                    transform.Rotate(Vector3.up, 180);
                }
                else if (isLeft && !isRight)
                {
                    transform.Rotate(Vector3.up, 90);
                }
                else if (!isLeft && isRight)
                {
                    transform.Rotate(Vector3.up, -90);
                }
                else
                {
                    if (Random.Range(1, 3) == 1)
                    {
                        transform.Rotate(Vector3.up, 90);
                    }
                    else
                    {
                        transform.Rotate(Vector3.up, -90);
                    }
                }
            }
        }
    }

    void GetAwayFromHunter()
    {
        if (!isInvisible) 
        {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
            {
                isBeingFollowed = false;
                GameObject hitObj = hit.collider.gameObject;
                if (hitObj.tag == "Hunter")
                {
                    Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
                    transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
                    isBeingFollowed = true;
                }
                else
                {
                    if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
                    {
                        transform.LookAt(hitObj.transform.position);
                    }
                    else
                    {
                        if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
                        {
                            transform.LookAt(hitObj.transform.position);
                        }
                    }
                }
            }
            if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, range))
            {
                GameObject hitObj = hit2.collider.gameObject;
                if (hitObj.tag == "Hunter")
                {
                    Debug.DrawRay(transform.position, -transform.forward * hit.distance, Color.yellow);
                    transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
                    isBeingFollowed = true;
                    //transform.rotation = Quaternion.LookRotation(transform.position - hitObj.transform.position);
                }
                else
                {
                    if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
                    {
                        transform.LookAt(hitObj.transform.position);
                    }
                    else
                    {
                        if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
                        {
                            transform.LookAt(hitObj.transform.position);
                        }
                    }
                }
            }
            if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit3, range))
            {
                GameObject hitObj = hit3.collider.gameObject;
                if (hitObj.tag == "Hunter")
                {
                    Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
                    transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
                    isBeingFollowed = true;
                    //transform.rotation = Quaternion.LookRotation(transform.position - hitObj.transform.position);
                }
                else
                {
                    if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
                    {
                        transform.LookAt(hitObj.transform.position);
                    }
                    else
                    {
                        if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
                        {
                            transform.LookAt(hitObj.transform.position);
                        }
                    }
                }
            }
            if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit4, range))
            {
                GameObject hitObj = hit4.collider.gameObject;
                if (hitObj.tag == "Hunter")
                {
                    Debug.DrawRay(transform.position, -transform.right * hit.distance, Color.yellow);
                    transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
                    isBeingFollowed = true;
                }
                else
                {
                    if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
                    {
                        transform.LookAt(hitObj.transform.position);
                    }
                    else
                    {
                        if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
                        {
                            transform.LookAt(hitObj.transform.position);
                        }
                    }
                }
            }
        }
    }
    void CheckTargets()
    {
        //float distance = 10000.0f;

        //for (int i = 0; i < collectables.Count; i++)
        //{
        //    if (Vector3.Distance(transform.position, collectables[i].transform.position) < distance)
        //    {
        //        distance = Vector3.Distance(transform.position, collectables[i].transform.position);
        //        target = collectables[i];
        //    }
        //}

        //if (target != null)
        //{
        //    if (target.gameObject.activeSelf)
        //    {
        //        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), target.transform.position - transform.position, out hit, Quaternion.identity))
        //        {
        //            if (hit.transform.tag != "Wall")
        //            {
        //                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, 1, 1));
        //            }

        //            if (Vector3.Distance(target.transform.position, transform.position) < 2.0f)
        //            {
        //                target.SetActive(false);
        //                target = null;
        //                for (int i = 0; i < collectables.Count; i++)
        //                {
        //                    if (target == collectables[i])
        //                    {
        //                        collectables.RemoveAt(i);
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        target = null;
        //    }
        //}
    }
}
