using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public enum InputType {
    Thrust,
    Rotate_L,
    Rotate_R,
    Shoot,
    Hyperspace
}
public class InputManager : MonoBehaviour
{

    public Dictionary<InputType, bool> ActiveInputs = new Dictionary<InputType, bool>();
    
    public static InputManager Instance { get; private set;}

    void Awake() {
        if(Instance != null) {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    void Start() {
        foreach(InputType type in Enum.GetValues(typeof(InputType))) {
            ActiveInputs[type] = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Thrust
        if(Input.GetKey(KeyCode.UpArrow) || ActiveInputs[InputType.Thrust]) GameManager.Instance.PlayerThrust(1f);
        //Rotation
        if(Input.GetKey(KeyCode.LeftArrow) || ActiveInputs[InputType.Rotate_L]) GameManager.Instance.PlayerRotate(-1f);
        else if(Input.GetKey(KeyCode.RightArrow) || ActiveInputs[InputType.Rotate_R]) GameManager.Instance.PlayerRotate(1f);
        else GameManager.Instance.PlayerRotate(0f);
        //Shoot
        if(Input.GetKey(KeyCode.Z) || ActiveInputs[InputType.Shoot]) GameManager.Instance.PlayerShoot();
        //Hyperspace
        if(Input.GetKeyUp(KeyCode.X) || ActiveInputs[InputType.Hyperspace]) GameManager.Instance.PlayerHyperspace();
        
    }
}
