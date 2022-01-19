using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    public bool InPause;
    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.Escape))
       {
           if (InPause)
           {
               UnPause();
               
           }
           else
           {
               Pause();
           }
           
       } 
    }
    public void UnPause()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        InPause = false; 
        canvas.SetActive(false);
    }
    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        InPause = true;
        canvas.SetActive(true);
    }
    public void LeaveGame() => Application.Quit();
}
