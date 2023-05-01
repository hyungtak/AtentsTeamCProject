using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class TestSceneChange : MonoBehaviour
{
    PlayerInputActions inputSystem;

    private void Awake()
    {
        inputSystem = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputSystem.Test.Enable();
        inputSystem.Test.Test_1.performed += Test_1_Scene;
        inputSystem.Test.Test_2.performed += Test_2_Scene;
        inputSystem.Test.Test_3.performed += Test_3_Scene;
        inputSystem.Test.Test_4.performed += Test_4_Scene;
        inputSystem.Test.Test_5.performed += Test_5_Scene;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        inputSystem.Test.Test_5.performed -= Test_5_Scene;
        inputSystem.Test.Test_4.performed -= Test_4_Scene;
        inputSystem.Test.Test_3.performed -= Test_3_Scene;
        inputSystem.Test.Test_2.performed -= Test_2_Scene;
        inputSystem.Test.Test_1.performed -= Test_1_Scene;
        inputSystem.Test.Disable();
    }

    private void Test_5_Scene(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void Test_4_Scene(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void Test_3_Scene(InputAction.CallbackContext obj)
    {
        throw new NotImplementedException();
    }

    private void Test_2_Scene(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(2);
    }

    private void Test_1_Scene(InputAction.CallbackContext obj)
    {
        SceneManager.LoadScene(1);
    }
}
