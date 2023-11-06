using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class MoverScript : MonoBehaviour
{
    float movementSpeed, initialSpeed = 5.0f, boostSpeed = 8.0f;
    float forwardDist = 1.0f, sideDist = 3.0f;

    bool isLeft, isRight;

    float initialRange = 10.0f;

    float hunterRange = 10.0f;
    float speedBoostRange = 10.0f;
    float disguiseItemRange = 10.0f;
    //8;

    RaycastHit hit;

    private bool isBeingFollowed = false;

    Vector3 direction;

    Rigidbody rb;

    public bool isInvisible = false;

    private bool isRotating = false;

    bool isHunterInRange = false;

    //[SerializeField] List<GameObject> hunters;
    //[SerializeField] GameObject hunter;

    [SerializeField] List<GameObject> speedBoosts;
    GameObject speedBoost;

    [SerializeField] List<GameObject> disguiseItems;
    GameObject disguiseItem;

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
        AvoidWalls();

        if (!isHunterInRange)
        {
            FollowDisguiseItem();
            FollowSpeedBoost();
        }

        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    bool CheckInRange(GameObject hunter)
    {
        RaycastHit outHit;
        if (Physics.Raycast(transform.position, hunter.transform.position - transform.position, out outHit, 10.0f))
        {
            return true;
            ////&& !isRotating
            //if (hit.transform.tag != "Wall")
            //{
            //    if (hit.transform.tag == "Hunter")
            //    {
            //        isHunterInRange = true;
            //        Vector3 direction = transform.position - hit.transform.position;
            //        direction.y = 0;
            //        transform.rotation = Quaternion.LookRotation(direction.normalized);
            //        //transform.rotation = hunter.transform.rotation;
            //    }
            //}
        }
        return false;
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    isHunterInRange = false;
    //    if (other.gameObject.tag == "Hunter" && CheckInRange(other.gameObject))
    //    {
    //        isHunterInRange = true;
    //        Vector3 direction = transform.position - other.gameObject.transform.position;
    //        direction.y = 0;
    //        transform.rotation = Quaternion.LookRotation(direction.normalized);

    //        if (Vector3.Distance(other.gameObject.transform.position, transform.position) < 2.0f && !isInvisible)
    //        {
    //            gameObject.SetActive(false);
    //        }
    //        //gameObject.SetActive(false);
    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {
        isHunterInRange = false;
        if (other.gameObject.tag == "Hunter" && CheckInRange(other.gameObject))
        {
            isHunterInRange = true;
            Vector3 direction = transform.position - other.gameObject.transform.position;
            direction.y = 0;
            transform.rotation = Quaternion.LookRotation(direction.normalized);

            if(Vector3.Distance(other.gameObject.transform.position, transform.position) < 2.0f && !isInvisible)
            {
                gameObject.SetActive(false);
            }
            //gameObject.SetActive(false);
        }
        //if (other.gameObject.tag == "SpeedBoost")
        //{
        //    other.gameObject.SetActive(false);
        //    StartCoroutine(ChangeSpeed());
        //}
        //if (other.gameObject.tag == "DisguiseItem")
        //{
        //    other.gameObject.SetActive(false);
        //    isInvisible = true;
        //    StartCoroutine(Invisible());
        //}
    }

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.tag == "Wall")
    //    {
    //        //isRotating = false;
    //    }
    //}

    IEnumerator Invisible()
    {
        isInvisible = true;
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
        //isRotating = false;
        if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), transform.forward, out hit, Quaternion.identity, forwardDist))
        {
            if (hit.transform.gameObject.tag == "Wall")
            {
                //isRotating = true;
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
                //}
                //else
                //{
                //    isRotating = false;
                //}
            }
        }
    }

    void FollowDisguiseItem()
    {
        disguiseItemRange = initialRange;
        for (int i = 0; i < disguiseItems.Count; i++)
        {
            if (Vector3.Distance(transform.position, disguiseItems[i].transform.position) < disguiseItemRange)
            {
                disguiseItemRange = Vector3.Distance(transform.position, disguiseItems[i].transform.position);
                disguiseItem = disguiseItems[i];
            }
        }
        if (disguiseItem != null)
        {
            if (disguiseItem.gameObject.activeSelf)
            {
                if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), disguiseItem.transform.position - transform.position, out hit, Quaternion.identity))
                {
                    if (hit.transform.tag != "Wall") 
                    {
                        if (hit.transform.tag == "DisguiseItem")
                        {
                            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, hit.transform.position - transform.position, 1, 1));
                        }
                        if (Vector3.Distance(hit.transform.position, transform.position) < 2.0f && hit.transform.tag == "DisguiseItem")
                        {
                            StartCoroutine(Invisible());
                            disguiseItem.SetActive(false);
                            disguiseItem = null;
                            for (int i = 0; i < disguiseItems.Count; i++)
                            {
                                if (disguiseItem == speedBoosts[i])
                                {
                                    disguiseItems.RemoveAt(i);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                disguiseItem = null;
            }
        }
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

    //bool GetAwayFromHunter()
    //{
    //    hunterRange = initialRange;
    //    bool isHunterInRange = false;
    //    for (int i = 0; i < hunters.Count; i++)
    //    {
    //        if (Vector3.Distance(transform.position, hunters[i].transform.position) < hunterRange)
    //        {
    //            hunterRange = Vector3.Distance(transform.position, hunters[i].transform.position);
    //            hunter = hunters[i];
    //        }
    //    }
    //    if (hunter != null)
    //    {
    //        if (hunter.gameObject.activeSelf)
    //        {
    //            if (Physics.BoxCast(transform.position, new Vector3(0.5f, 0.5f, 0.5f), hunter.transform.position - transform.position, out hit, Quaternion.identity))
    //            {
    //                //&& !isRotating
    //                if (hit.transform.tag != "Wall") 
    //                {
    //                    if (hit.transform.tag == "Hunter")
    //                    {
    //                        isHunterInRange = true;
    //                        Vector3 direction = transform.position - hit.transform.position;
    //                        direction.y = 0;
    //                        transform.rotation = Quaternion.LookRotation(direction.normalized);
    //                        //transform.rotation = hunter.transform.rotation;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            hunter = null;
    //        }
    //    }
    //    //Debug.Log(hunterRange);
    //    return isHunterInRange;
    //}
    void CheckTargets()
    {
        //if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, range))
        //{
        //    GameObject hitObj = hit.collider.gameObject;
        //    if (hitObj.tag == "Hunter")
        //    {
        //        Debug.DrawRay(transform.position, transform.forward * hit.distance, Color.yellow);
        //        transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
        //        isBeingFollowed = true;
        //    }
        //    else
        //    {
        //        if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
        //        {
        //            transform.LookAt(hitObj.transform.position);
        //        }
        //        else
        //        {
        //            if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
        //            {
        //                transform.LookAt(hitObj.transform.position);
        //            }
        //        }
        //    }
        //}
        //if (Physics.Raycast(transform.position, -transform.forward, out RaycastHit hit2, range))
        //{
        //    GameObject hitObj = hit2.collider.gameObject;
        //    if (hitObj.tag == "Hunter")
        //    {
        //        Debug.DrawRay(transform.position, -transform.forward * hit.distance, Color.yellow);
        //        transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
        //        isBeingFollowed = true;
        //        //transform.rotation = Quaternion.LookRotation(transform.position - hitObj.transform.position);
        //    }
        //    else
        //    {
        //        if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
        //        {
        //            transform.LookAt(hitObj.transform.position);
        //        }
        //        else
        //        {
        //            if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
        //            {
        //                transform.LookAt(hitObj.transform.position);
        //            }
        //        }
        //    }
        //}
        //if (Physics.Raycast(transform.position, transform.right, out RaycastHit hit3, range))
        //{
        //    GameObject hitObj = hit3.collider.gameObject;
        //    if (hitObj.tag == "Hunter")
        //    {
        //        Debug.DrawRay(transform.position, transform.right * hit.distance, Color.yellow);
        //        transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
        //        isBeingFollowed = true;
        //        //transform.rotation = Quaternion.LookRotation(transform.position - hitObj.transform.position);
        //    }
        //    else
        //    {
        //        if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
        //        {
        //            transform.LookAt(hitObj.transform.position);
        //        }
        //        else
        //        {
        //            if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
        //            {
        //                transform.LookAt(hitObj.transform.position);
        //            }
        //        }
        //    }
        //}
        //if (Physics.Raycast(transform.position, -transform.right, out RaycastHit hit4, range))
        //{
        //    GameObject hitObj = hit4.collider.gameObject;
        //    if (hitObj.tag == "Hunter")
        //    {
        //        Debug.DrawRay(transform.position, -transform.right * hit.distance, Color.yellow);
        //        transform.LookAt(transform.position + (transform.position - hitObj.transform.position));
        //        isBeingFollowed = true;
        //    }
        //    else
        //    {
        //        if (hitObj.tag == "DisguiseItem" && !isBeingFollowed)
        //        {
        //            transform.LookAt(hitObj.transform.position);
        //        }
        //        else
        //        {
        //            if (hitObj.tag == "SpeedBoost" && !isBeingFollowed)
        //            {
        //                transform.LookAt(hitObj.transform.position);
        //            }
        //        }
        //    }
        //}
    }
}
