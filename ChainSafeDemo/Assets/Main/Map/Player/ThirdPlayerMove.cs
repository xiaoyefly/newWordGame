
using System;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPlayerMove : MonoBehaviour
{
  private float h;
  private float v;
  public float speed = 6;
  public float turnSpeed = 15;
  public Transform camTransform;
  private Vector3 movement;
  private Vector3 camForward;

  void Update()
  {
    Move();
  }

  void Move()
  {
    h = Input.GetAxis("Horizontal");
    v = Input.GetAxis("Vertical");
    transform.Translate(camTransform.right*h*speed*Time.deltaTime+camForward*v*speed*Time.deltaTime,Space.World);
    if (h != 0 || v != 0)
    {
      Rotation(h, v);
    }
  }

  void Rotation(float hh,float vv)
  {
    camForward = Vector3.Cross(camTransform.right, Vector3.up);
    Vector3 targetDir = camTransform.right * hh + camForward * vv;
    Quaternion targetRotation = Quaternion.LookRotation(targetDir, Vector3.up);
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
  }
}
