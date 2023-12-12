using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateInfoController : MonoBehaviour
{
    [SerializeField] GameObject st1;
    [SerializeField] GameObject st2;

    void Start()
    {
        st1.SetActive(false);
        st2.SetActive(false);
    }

    public void OpenSt1Info()
    {
        if (st2.activeSelf)
        {
            st2.SetActive(false);
        }
        Time.timeScale = 0f;
        st1.SetActive(true);
    }

    public void CloseSt1Info()
    {
        Time.timeScale = 1f;
        st1.SetActive(false);
    }

    public void OpenSt2Info()
    {
        if (st1.activeSelf)
        {
            st1.SetActive(false);
        }
        Time.timeScale = 0f;
        st2.SetActive(true);
    }

    public void CloseSt2Info()
    {
        Time.timeScale = 1f;
        st2.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
