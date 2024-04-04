using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Square : MonoBehaviour
{
    [SerializeField] private GameObject controlObject;
    [SerializeField] private int index;
    private Controller controller;

    private void Start()
    {
        controller = controlObject.GetComponent<Controller>();
    }

    private void OnMouseDown()
    {
        controller.PlayerSelect(index);
    }
}
