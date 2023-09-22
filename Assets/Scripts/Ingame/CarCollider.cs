using System.Collections;
using System;
using UnityEngine;

public class CarCollider : MonoBehaviour
{
    public Action<PowerUp> OnCollideWithMoneyPwerUp;

    public static Action<Gate> OnCollideWithGate;
    private void OnTriggerEnter(Collider other) {
        PowerUp powerUp = other.GetComponent<PowerUp>();
        if (powerUp is MoneyPowerUp) {
            powerUp.GetPowerUp();
            return;
        }
        if (powerUp is HeartPowerUp) {
            powerUp.GetPowerUp();
            return;
        }
        Gate gate = other.GetComponent<Gate>();
        if (gate != null) {
            gate.ProcessDisplay();
            OnCollideWithGate?.Invoke(gate);
        }
    }
}