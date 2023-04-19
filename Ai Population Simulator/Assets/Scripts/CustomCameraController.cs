using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCameraController : MonoBehaviour {
    [SerializeField] private float _velocity = 0;
    [SerializeField] private float _maxVelocity = 2;
    [SerializeField] private float _velocityIncrease = 0.1f;
    [SerializeField] private float _XYOffset = 10;

    [SerializeField] private float _defaultSpeed = 1f;
    [SerializeField] private float _shiftSpeed = 2f;


    void Update() {
        //Get our bounds
        int height = Init.instance.GlobalData.SpawningBounds.y;
        int width = Init.instance.GlobalData.SpawningBounds.x;

        float speedMulti = _defaultSpeed;

        //handle shift speed
        if (Input.GetKey(KeyCode.LeftShift)) {
            speedMulti = _shiftSpeed;
        }

        //Handle speed multi
        float speed = (Time.deltaTime + _velocity) * speedMulti;

        //get axis
        float vertical = Input.GetAxis("Vertical") * speed;
        float horizontal = Input.GetAxis("Horizontal") * speed;

        //translate
        transform.Translate(new Vector3(horizontal, vertical, Input.mouseScrollDelta.y));

        //clamping 
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, -_XYOffset, width + _XYOffset),
            Mathf.Clamp(transform.position.y, 4, 40),
            Mathf.Clamp(transform.position.z, -_XYOffset, height + _XYOffset));

        //Handle velocity
        if (vertical != 0 | horizontal != 0) {
            _velocity += Time.deltaTime * _velocityIncrease;
        } else {
            _velocity -= Time.deltaTime;
        }

        //Clamp it
        _velocity = Mathf.Clamp(_velocity, 0, _maxVelocity);
    }
}
