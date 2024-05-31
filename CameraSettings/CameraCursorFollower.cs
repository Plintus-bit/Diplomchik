using System;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace CameraSettings
{
 public class CameraCursorFollower : MonoBehaviour
 {
     private Quaternion _startRotation;
     private float _mousePosX;
     private float _mousePosY;
     private float _screenHeight;
     private float _screenWidth;
     
     private Vector3 _startPosition;
     public Vector2Int rotationMultiplier;
     public Vector2Int positionMultiplier;
 
     void Start()
     {
         _screenWidth = Screen.width;
         _screenHeight = Screen.height;
         _startRotation = Quaternion.LookRotation(Vector3.zero);
         _startPosition = transform.position;
     }
 
     private void FixedUpdate()
     {
         if (Math.Abs(_mousePosX - Input.mousePosition.x) < 0.4
             && Math.Abs(_mousePosY - Input.mousePosition.y) < 0.4) return;
         _mousePosX = Input.mousePosition.x;
         _mousePosY = Input.mousePosition.y;
         SetNewRotation();
         SetNewPosition();
     }
 
     public void SetNewRotation()
     {
         float alphaX = (_mousePosX - _screenWidth / 2)/ Mathf.Pow(rotationMultiplier.x, 2);
         float alphaY = (_mousePosY  - _screenHeight / 2)/ Mathf.Pow(rotationMultiplier.y, 2);
 
         transform.rotation = _startRotation;
         transform.Rotate(Vector3.down, _startRotation.x + alphaX);
         transform.Rotate(Vector3.right, _startRotation.y + alphaY);
     }
 
     public void SetNewPosition()
     {
         float deltaX = (_mousePosX - _screenWidth / 2)/ Mathf.Pow(positionMultiplier.x, 2);
         transform.position = _startPosition;
         transform.Translate(_startPosition.x + deltaX, 0, 0);
     }
 
 }   
}
