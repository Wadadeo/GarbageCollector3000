using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NEngine;

public class HealthUIBehaviour : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField]
    private RectTransform panel;
    [Header("Parameters")]
    [SerializeField]
    private GameObject healthPrefab;
    
    private List<GameObject> currentHealth;

    public void Init(GameEngine engine)
    {
        engine.healthChangedEvent += RefreshHealth;
        currentHealth = new List<GameObject>(10);
    }

    private void RefreshHealth(byte newHealth)
    {
        int delta = newHealth - currentHealth.Count;

        while (delta != 0)
        {
            if (delta > 0)
            {
                currentHealth.Add(Instantiate(healthPrefab, panel));
                --delta;
            }
            else
            {
                Destroy(currentHealth[0]);
                currentHealth.RemoveAt(0);
                ++delta;
            }
        }
    }
}
