using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RouletteController : MonoBehaviour
{
    float rotSpeed = 0;
    bool shouldSlowDown = false;
    int win = 0;
    int loss = 0;
    float initialSpeed = 12.6f;
    float EstRotation;

    void Start()
    {
    }

    void Update()
    {
        float slowDownFactor = 0.965f - (0.002f * win);
        slowDownFactor = Mathf.Max(0.9f, slowDownFactor);

        if (rotSpeed != 0)
        {
            transform.Rotate(0, 0, this.rotSpeed);

            if (shouldSlowDown)
            {
                this.rotSpeed *= slowDownFactor;

                if (this.rotSpeed < 0.05f)
                {
                    this.rotSpeed = 0;
                    shouldSlowDown = false;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            float currentAngle = transform.eulerAngles.z;
            bool isWinningArea = (0 <= currentAngle && currentAngle <= 60) || (120 <= currentAngle && currentAngle <= 180) || (240 <= currentAngle && currentAngle <= 300);
            bool willWinArea = (0 <= EstRotation && EstRotation <= 60) || (120 <= EstRotation && EstRotation <= 180) || (240 <= EstRotation && EstRotation <= 300);

            if (!IsRotating())
            {
                if (isWinningArea)
                {
                    win++;
                    loss = 0;
                }
                else
                {
                    win = 0;
                    loss++;
                }
                StartSpin();
            }
            else if (!shouldSlowDown)
            {
                EstRotation = (currentAngle + (slowDownFactor * 9730) - 29.45f) % 360;

                if (willWinArea)
                {
                    slowDownFactor -= 0.001f * win;
                }
                else
                {
                    slowDownFactor -= 0.001f * loss;
                }

                InitiateSlowDown();
            }
        }
    }

    public void StartSpin()
    {
        this.rotSpeed = initialSpeed;
        this.shouldSlowDown = false;
    }

    public void InitiateSlowDown()
    {
        this.shouldSlowDown = true;
    }

    bool IsRotating()
    {
        return rotSpeed != 0;
    }
}
