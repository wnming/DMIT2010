using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TomatoController : MonoBehaviour
{
    public List<TomatoScript> tomatoList;

    private float targetTime;

    void Start()
    {
        targetTime = 11;
    }

    void Update()
    {
        targetTime -= Time.deltaTime;
        if (targetTime <= 0) 
        {
            if (tomatoList.Where(x => x.gameObject.activeSelf == true).Count() <= 0)
            {
                Debug.Log("rannn");
                tomatoList[Random.Range(0, 4)].gameObject.SetActive(true);
            }
            targetTime = Random.Range(10, 25);
        }
    }

    public TomatoScript getThisTomato()
    {
        return tomatoList[tomatoList.FindIndex(x => x.gameObject.activeSelf == true)];
    }

    public bool IsTomatoShows()
    {
        return tomatoList.Where(x => x.gameObject.activeSelf == true).Count() <= 0 ? false : true;
    }
}
