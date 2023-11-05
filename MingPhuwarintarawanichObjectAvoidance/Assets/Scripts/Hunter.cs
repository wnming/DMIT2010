using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Hunter : MonoBehaviour
{
    float movementSpeed, initialSpeed = 5.0f, boostSpeed = 8.0f;
    float forwardDist = 1.0f, sideDist = 3.0f;

    Rigidbody rb;

    bool isLeft, isRight;

    RaycastHit hit;

    float range = 8;
    private bool inDistance = false;

    Vector3 direction;

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
        AvoidWalls();
        //FollowRunner();
        transform.Translate(direction * movementSpeed * Time.deltaTime);
        FollowRunner();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SpeedBoost")
        {
            other.gameObject.SetActive(false);
            StartCoroutine(ChangeSpeed());
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

    void FollowRunner()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit1, range))
        {
            GameObject hitObj = hit1.collider.gameObject;
            if (hitObj.tag == "Runner")
            {
                MoverScript runner = hitObj.GetComponent<MoverScript>();
                if(runner != null)
                {
                    if (!runner.isInvisible)
                    {
                        Debug.DrawRay(transform.position, transform.forward * hit1.distance, Color.blue);
                        transform.LookAt(hitObj.transform);
                    }
                }
            }
        }
        if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, range))
        {
            GameObject hitObj = hit2.collider.gameObject;
            if (hitObj.tag == "Runner")
            {
                MoverScript runner = hitObj.GetComponent<MoverScript>();
                if (runner != null)
                {
                    if (!runner.isInvisible)
                    {
                        Debug.DrawRay(transform.position, -transform.forward * hit2.distance, Color.blue);
                        transform.LookAt(hitObj.transform);
                    }
                }
            }
        }
        if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit3, range))
        {
            GameObject hitObj = hit3.collider.gameObject;
            if (hitObj.tag == "Runner")
            {
                MoverScript runner = hitObj.GetComponent<MoverScript>();
                if (runner != null)
                {
                    if (!runner.isInvisible)
                    {
                        Debug.DrawRay(transform.position, transform.right * hit3.distance, Color.blue);
                        transform.LookAt(hitObj.transform);
                    }
                }
            }
        }
        if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit4, range))
        {
            GameObject hitObj = hit4.collider.gameObject;
            if (hitObj.tag == "Runner")
            {
                MoverScript runner = hitObj.GetComponent<MoverScript>();
                if (runner != null)
                {
                    if (!runner.isInvisible)
                    {
                        Debug.DrawRay(transform.position, -transform.right * hit4.distance, Color.blue);
                        transform.LookAt(hitObj.transform);
                    }
                }
            }
        }
    }
}
