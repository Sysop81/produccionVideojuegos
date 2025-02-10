using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    [SerializeField] private GameObject canvasGamePad;
    private PlayerInput _playerInput;
    private int _padIndex = 0;
    private readonly int _maxStates = 3;
    
    /// <summary>
    /// Method Start
    /// Start is called before the first frame update
    /// </summary>
    void Start()
    {
        canvasGamePad.transform.GetChild(_padIndex + 1).gameObject.SetActive(false);
        _playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        
        // Testing ....
        //canvasGamePad.SetActive(Application.platform == RuntimePlatform.Android);
    }
    
    /// <summary>
    /// Method Update 
    /// </summary>
    void Update()
    {
        if (_playerInput.actions["Change"].WasReleasedThisFrame()) ChangeVirtualPad();
    }
    
    /// <summary>
    /// method ChangeVirtualPad
    /// This method render or not the virtual buttons on the screen 
    /// </summary>
    private void ChangeVirtualPad()
    {
        _padIndex = (_padIndex + 1) % _maxStates;


        for (int i = 0; i < canvasGamePad.transform.childCount - 1; i++)
        {
            if (_padIndex == 2)
            {
                canvasGamePad.transform.GetChild(i).gameObject.SetActive(false);
            }else
            {
                canvasGamePad.transform.GetChild(_padIndex == 0 ? _padIndex + 1 : _padIndex - 1).gameObject.SetActive(false);
                canvasGamePad.transform.GetChild(_padIndex).gameObject.SetActive(true);
                if(i > 1) canvasGamePad.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
    }
}
