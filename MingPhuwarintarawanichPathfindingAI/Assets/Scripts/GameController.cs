using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameController : MonoBehaviour
{
    [SerializeField] SpyPath redSpy;
    [SerializeField] SpyPath blueSpy;

    [SerializeField] GameObject finishGamePanel;
    [SerializeField] TextMeshProUGUI winner;
    [SerializeField] TextMeshProUGUI red;
    [SerializeField] TextMeshProUGUI blue;

    [SerializeField] List<GameObject> documents;

    void Start()
    {
        finishGamePanel.SetActive(false);
    }

    void QuitGame()
    {
        var keyboard = Keyboard.current;
        if (keyboard != null)
        {
            if (keyboard.escapeKey.wasPressedThisFrame)
            {
                Application.Quit();
            }
        }
    }

    void Update()
    {
        QuitGame();
        if (documents.Where(x => !x.activeSelf).Count() == documents.Count)
        {
            Time.timeScale = 0;
            
            finishGamePanel.SetActive(true);
            winner.text = redSpy.document > blueSpy.document ? "Red spy wins!" : "Blue spy wins!";
            red.text = "Red Spy => Destroy: " + redSpy.document + " doc(s)";
            blue.text = "Blue Spy => Hold: " + blueSpy.document + " doc(s)"; ;
        }
    }
}
