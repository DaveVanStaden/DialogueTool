using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float movementSpeed = 5f;      
    public float smoothTime = 0.2f;       
    private Vector3 _velocity = Vector3.zero;
    [SerializeField] private Camera _cam;
    [SerializeField] private float _zoom;

    [SerializeField] private DialogueManager dialogueManager;
    private void Update()
    {
        if (!dialogueManager.inputFieldActive)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");
        
            Vector3 targetPosition = transform.position + new Vector3(horizontalInput * movementSpeed * Time.deltaTime, verticalInput * movementSpeed * Time.deltaTime, 0f);
        
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref _velocity, smoothTime);
            if (_cam.orthographic)
            {
                _cam.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * _zoom;
            }
            else
            {
                _cam.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * _zoom;
            }

            if (_cam.orthographicSize <= 0.5f)
                _cam.orthographicSize = 0.5f;
        }
    }
}
