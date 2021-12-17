using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    private int _health = 100;

    public int GetHealth()
    {
        return _health;
    }

    public void SehHealth(int health)
    {
        _health = health;
    }
}
