using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testperfab : MonoBehaviour
{
    [Header("Weapon Sway")]
    [Tooltip("Toggle weapon sway.")]
    public bool weaponSway;

    public float swayAmount = 0.02f;

    public float maxSwayAmount = 0.06f;

    public float swaySmoothValue = 4.0f;

    Vector3 initialSwayPosition;

    
    // Start is called before the first frame update
    void Start()
    {
        initialSwayPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        if (weaponSway == true)
        {
            float movementX = -Input.GetAxis("Mouse X") * swayAmount;
            float movementY = -Input.GetAxis("Mouse Y") * swayAmount;

            movementX = Mathf.Clamp(movementX, -maxSwayAmount, maxSwayAmount);
            movementY = Mathf.Clamp(movementY, -maxSwayAmount, maxSwayAmount);

            Vector3 finalSwayPosition = new Vector3(movementX, movementY, 0);
            transform.localPosition = Vector3.Lerp
                (transform.localPosition, finalSwayPosition + initialSwayPosition, Time.deltaTime * swaySmoothValue);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
