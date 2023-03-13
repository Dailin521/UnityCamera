
using System;
using UnityEngine;

public class CameraInitInform : MonoBehaviour
{
    [SerializeField, Range(15, 60)] public float Y_Rotate = 45;
    [SerializeField, Range(-360, 360)] public float X_Rotate = 0;
    [SerializeField, Range(5, 50)] public float Scroll = 10;
}
