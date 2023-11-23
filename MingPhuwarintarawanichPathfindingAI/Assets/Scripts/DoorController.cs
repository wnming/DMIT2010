using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool isDoorLocked;
    [SerializeField] bool isOpenLeft = false;
    [SerializeField] bool isSide = false;

    [SerializeField] TextMeshProUGUI text;

    private bool checkOpenDoor;

    private Vector3 initialPosiiton;

    void Start()
    {
        isDoorLocked = true;
        checkOpenDoor = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDoorLocked)
        {
            text.text = "Locked";
        }
        else
        {
            text.text = "Open";
        }
        if (checkOpenDoor)
        {
            StartCoroutine(OpenTheDoor());
        }
    }

    IEnumerator OpenTheDoor()
    {
        checkOpenDoor = false;
        yield return new WaitForSeconds(Random.Range(5.0f, 20.0f));
        isDoorLocked = false;
        initialPosiiton = transform.localPosition;
        if (isSide)
        {
            if (isOpenLeft)
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z + 3.0f);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 3.0f);
            }
        }
        else
        {
            if (isOpenLeft)
            {
                transform.localPosition = new Vector3(transform.localPosition.x - 3.0f, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                transform.localPosition = new Vector3(transform.localPosition.x + 3.0f, transform.localPosition.y, transform.localPosition.z);
            }
        }
        yield return new WaitForSeconds(Random.Range(8.0f, 20.0f));
        transform.localPosition = initialPosiiton;
        isDoorLocked = true;
        checkOpenDoor = true;
    }
}
