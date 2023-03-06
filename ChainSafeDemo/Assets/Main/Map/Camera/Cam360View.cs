
using System;
using System.Collections.Generic;
using UnityEngine;

public class Cam360View : MonoBehaviour
{
  public Transform target;
  public float xSpeed = 200;
  public float ySpeed = 200;
  public float mSpeed = 10;
  public float yMinLimit = -50;
  public float yMaxLimit = 50;
  public float distance = 10;
  public float minDistance = 2;
  public float maxDistance = 30;

  public bool needDamping = true;
  private float damping = 5.0f;
  public float x = 0.0f;
  public float y = 0.0f;

  void Start()
  {
    Vector3 angles = transform.eulerAngles;
    x = angles.y;
    y = angles.x;
  }

  private void LateUpdate()
  {
    if (target)
    {
      // if (Input.GetMouseButton(1))
        if (Input.GetKey(KeyCode.Q))
      {
        x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
        y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
        y = ClampAngle(y, yMinLimit, yMaxLimit);
        

      }

      distance -= Input.GetAxis("Mouse ScrollWheel") * mSpeed;
      distance = Mathf.Clamp(distance, minDistance, maxDistance);
      Quaternion rotation=Quaternion.Euler(y,x,0.0f);
      Vector3 disVector = new Vector3(0f, 0f, -distance);
      Vector3 position = rotation * disVector + target.position;
      if (needDamping)
      {
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * damping);
        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * damping);
        
      }
      else
      {
        transform.rotation = rotation;
        transform.position = position;
      }
    }

    static float ClampAngle(float angle, float min, float max)
    {
      if (angle < -360)
      {
        angle += 360;
      }

      if (angle > 360)
      {
        angle -= 360;
      }

      return Mathf.Clamp(angle, min, max);
    }
  }
}
