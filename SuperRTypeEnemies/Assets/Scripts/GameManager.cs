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
        //canvasGamePad.SetActive(Application.platform == RuntimePlatform.Android);
    }

    void Update()
    {
        //Debug.Log(_playerInput.actions["Change"].IsPressed());
        if (_playerInput.actions["Change"].WasReleasedThisFrame()) ChangeVirtualPad();
    }

    private void ChangeVirtualPad()
    {
        _padIndex = (_padIndex + 1) % _maxStates;

        switch (_padIndex)
        {
            case 0:
                canvasGamePad.gameObject.SetActive(true);
                canvasGamePad.transform.GetChild(_padIndex + 1).gameObject.SetActive(false);
                canvasGamePad.transform.GetChild(_padIndex).gameObject.SetActive(true);
                break;
            case 1:
                canvasGamePad.transform.GetChild(_padIndex-1).gameObject.SetActive(false);
                canvasGamePad.transform.GetChild(_padIndex).gameObject.SetActive(true);
                break;
            default:
                canvasGamePad.gameObject.SetActive(false);
                break;
        }
    }
}
