using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class HoldOutArms : MonoBehaviour
{
    public GameObject MenuObject;
    public Vector3 Position;
    [SerializeField]
    private Vector3 SmallPosition;
    public Vector3 PositionBack;

    void Start()
    {
        SmallPosition = Position;
    }

    void Update()
    {
        if (Keyboard.current.eKey.isPressed)
        {
            StartCoroutine(ArmsHold());
        }
        else
        {
            StartCoroutine(ArmsBack());
            if (Keyboard.current.qKey.isPressed)
            {
                StartCoroutine(SmallArmsHold());
            }
            else
            {
                StartCoroutine(ArmsBack());
            }
        }
    }

    IEnumerator ArmsHold()
    {
        MenuObject.transform.localPosition = Vector3.Lerp(MenuObject.transform.localPosition, Position, 10 * Time.deltaTime);
        yield return null;
    }

    IEnumerator ArmsBack()
    {
        MenuObject.transform.localPosition = Vector3.Lerp(MenuObject.transform.localPosition, PositionBack, 10 * Time.deltaTime);
        yield return null;
    }

    IEnumerator SmallArmsHold()
    {
        MenuObject.transform.localPosition = Vector3.Lerp(MenuObject.transform.localPosition, SmallPosition, 10 * Time.deltaTime);
        yield return null;
    }
}
