using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Hunter : MonoBehaviour
{
    float movementSpeed, initialSpeed = 5.0f, boostSpeed = 8.0f;
    float forwardDist = 1.0f, sideDist = 3.0f;

    Rigidbody rb;

    bool isLeft, isRight;

    RaycastHit hit;

    float initialRange = 10.0f;

    float runnerRange = 10.0f;
    float speedBoostRange = 10.0f;
    //8;
    private bool isRotating = false;

    Vector3 direction;

    float forwardDist = 2.0f


    //[SerializeField] List<GameObject> runners;
    //GameObject runner;

    [SerializeField] List<GameObject> speedBoosts;
    GameObject speedBoost;

    bool isPlayerInRange = false;

    void Start()
    {
        direction = Vector3.forward;
        rb = GetComponent<Rigidbody>();
        movementSpeed = initialSpeed;
    }

    void Update()
    {
        //CheckTargets();
        //FollowRunner();
        if (!isPlayerInRange)
        {
            FollowSpeedBoost();
        }

        AvoidWalls();
        //FollowRunner();
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    bool CheckInRange(GameObject runner)
    {
        Debug.Log("CheckInRange");
        RaycastHit outHit;
        MoverScript runnerObj = runner.gameObject.transform.gameObject.GetComponent<MoverScript>();
        if (runnerObj != null)
        {
            if (Physics.Raycast(transform.position, runner.transform.position - transform.position, out outHit, 10.0f) && !runnerObj.isInvisible)
            {
                return true;
            }
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Runner" && CheckInRange(other.gameObject))
        {
            isPlayerInRange = true;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, other.gameObject.transform.position - transform.position, 1, 1));
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) < 2.0f)
            {
                other.gameObject.SetActive(false);
            }
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        isPlayerInRange = false;
        if (other.gameObject.tag == "Runner" && CheckInRange(other.gameObject))
        {
            isPlayerInRange = true;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, other.gameObject.transform.position - transform.position, 1, 1));
            if (Vector3.Distance(other.gameObject.transform.position, transform.position) < 2.0f)
            {
                other.gameObject.SetActive(false);
            }
        }
        else
        {
            isPlayerInRange = false;
        }
    }

    IEnumerator ChangeSpeed()
    {
        movementSpeed = boostSpeed;
        yield return new WaitForSeconds(5);
        movementSpeed = initialSpeed;
    }


    void AvoidWalls()
    {
        //isRotating = false;
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDist))
        {
            if (hit.transform.gameObject.tag == "Guard")
            {
                Debug.Log("hit");
            }
        }
        //else
        //{
        //    if (!isRotating) 
        //    {
        //        if (!FollowRunner())
        //        {
        //            FollowSpeedBoost();
        //        }
        //    }
        //}
    }

    void FollowSpeedBoost()
    {
        speedBoostRange = initialRange;
        for (int i = 0; i < speedBoosts.Count; i++)
        {
            if (Vector3.Distance(transform.position, speedBoosts[i].transform.position) < speedBoostRange)
            {
                speedBoostRange = Vector3.Distance(transform.position, speedBoosts[i].transform.position);
                speedBoost = speedBoosts[i];
            }
        }
        if (speedBoost != null)
        {
            if (speedBoost.gameObject.activeSelf)
            {
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), speedBoost.transform.position - transform.position, out hit, Quaternion.identity))
                {
                    if (hit.transform.tag != "Wall")
                    {
                        if (hit.transform.tag == "SpeedBoost")
                        {
                            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, speedBoost.transform.position - transform.position, 1, 1));
                        }
                        if (Vector3.Distance(speedBoost.transform.position, transform.position) < 2.0f && hit.transform.tag == "SpeedBoost")
                        {
                            StartCoroutine(ChangeSpeed());
                            speedBoost.SetActive(false);
                            speedBoost = null;
                            for (int i = 0; i < speedBoosts.Count; i++)
                            {
                                if (speedBoost == speedBoosts[i])
                                {
                                    speedBoosts.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                speedBoost = null;
            }
        }
    }

    //bool FollowRunner()
    //{
    //    bool isPlayerInRange = false;
    //    runnerRange = initialRange;
    //    for (int i = 0; i < runners.Count; i++)
    //    {
    //        if (Vector3.Distance(transform.position, runners[i].transform.position) < runnerRange)
    //        {
    //            runnerRange = Vector3.Distance(transform.position, runners[i].transform.position);
    //            runner = runners[i];
    //        }
    //    }
    //    if (runner != null)
    //    {
    //        if (runner.gameObject.activeSelf)
    //        {
    //            if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), runner.transform.position - transform.position, out hit, Quaternion.identity))
    //            {
    //                MoverScript runnerObj = runner.transform.gameObject.GetComponent<MoverScript>();
    //                if (hit.transform.tag != "Wall") 
    //                {
    //                    if (hit.transform.tag == "Runner")
    //                    {
    //                        //MoverScript runnerObj = runner.transform.gameObject.GetComponent<MoverScript>();
    //                        if (runnerObj != null)
    //                        {
    //                            if (!runnerObj.isInvisible)
    //                            {
    //                                isPlayerInRange = true;
    //                                transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, runner.transform.position - transform.position, 1, 1));
    //                            }
    //                        }
    //                    }
    //                    if (Vector3.Distance(runner.transform.position, transform.position) < 2.0f && hit.transform.tag == "Runner")
    //                    {
    //                        if (runnerObj != null)
    //                        {
    //                            if (!runnerObj.isInvisible)
    //                            {
    //                                runner.SetActive(false);
    //                                runner = null;
    //                                for (int i = 0; i < runners.Count; i++)
    //                                {
    //                                    if (runner == runners[i])
    //                                    {
    //                                        runners.RemoveAt(i);
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            runner = null;
    //        }
    //    }
    //    return isPlayerInRange;
    //        //if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit1, range))
    //        //{
    //        //    GameObject hitObj = hit1.collider.gameObject;
    //        //    if (hitObj.tag == "Runner")
    //        //    {
    //        //        MoverScript runner = hitObj.GetComponent<MoverScript>();
    //        //        if(runner != null)
    //        //        {
    //        //            if (!runner.isInvisible)
    //        //            {
    //        //                Debug.DrawRay(transform.position, transform.forward * hit1.distance, Color.blue);
    //        //                transform.LookAt(hitObj.transform);
    //        //                isFollowingRunner = true;
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        if (hitObj.tag == "SpeedBoost" && !isFollowingRunner)
    //        //        {
    //        //            transform.LookAt(hitObj.transform);
    //        //        }
    //        //    }
    //        //}
    //        //if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, range))
    //        //{
    //        //    GameObject hitObj = hit2.collider.gameObject;
    //        //    if (hitObj.tag == "Runner")
    //        //    {
    //        //        MoverScript runner = hitObj.GetComponent<MoverScript>();
    //        //        if (runner != null)
    //        //        {
    //        //            if (!runner.isInvisible)
    //        //            {
    //        //                Debug.DrawRay(transform.position, -transform.forward * hit2.distance, Color.blue);
    //        //                transform.LookAt(hitObj.transform);
    //        //                isFollowingRunner = true;
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        if (hitObj.tag == "SpeedBoost" && !isFollowingRunner)
    //        //        {
    //        //            transform.LookAt(hitObj.transform);
    //        //        }
    //        //    }
    //        //}
    //        //if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit3, range))
    //        //{
    //        //    GameObject hitObj = hit3.collider.gameObject;
    //        //    if (hitObj.tag == "Runner")
    //        //    {
    //        //        MoverScript runner = hitObj.GetComponent<MoverScript>();
    //        //        if (runner != null)
    //        //        {
    //        //            if (!runner.isInvisible)
    //        //            {
    //        //                Debug.DrawRay(transform.position, transform.right * hit3.distance, Color.blue);
    //        //                transform.LookAt(hitObj.transform);
    //        //                isFollowingRunner = true;
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        if (hitObj.tag == "SpeedBoost" && !isFollowingRunner)
    //        //        {
    //        //            transform.LookAt(hitObj.transform);
    //        //        }
    //        //    }
    //        //}
    //        //if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit4, range))
    //        //{
    //        //    GameObject hitObj = hit4.collider.gameObject;
    //        //    if (hitObj.tag == "Runner")
    //        //    {
    //        //        MoverScript runner = hitObj.GetComponent<MoverScript>();
    //        //        if (runner != null)
    //        //        {
    //        //            if (!runner.isInvisible)
    //        //            {
    //        //                Debug.DrawRay(transform.position, -transform.right * hit4.distance, Color.blue);
    //        //                transform.LookAt(hitObj.transform);
    //        //                isFollowingRunner = true;
    //        //            }
    //        //        }
    //        //    }
    //        //    else
    //        //    {
    //        //        if(hitObj.tag == "SpeedBoost" && !isFollowingRunner)
    //        //        {
    //        //            transform.LookAt(hitObj.transform);
    //        //        }
    //        //    }
    //        //}
    //    }
}
