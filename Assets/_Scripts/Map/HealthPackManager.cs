using System.Collections.Generic;
using UnityEngine;

public class HealthPackManager : Singleton<HealthPackManager>
{
    List<HealthPack> healthPacks = new List<HealthPack>();

    public void RegisterHealthPack(HealthPack healthPack)
    {
        if (!healthPacks.Contains(healthPack))
        {
            healthPacks.Add(healthPack);
        }
    }

    public void UnregisterHealthPack(HealthPack healthPack)
    {
        if (healthPacks.Contains(healthPack))
        {
            healthPacks.Remove(healthPack);
        }
    }

    public HealthPack GetClosestHealthPack(Vector3 position)
    {
        HealthPack closest = null;
        float closestDistance = float.MaxValue;
        foreach (var healthPack in healthPacks)
        {
            float distance = Vector3.Distance(position, healthPack.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = healthPack;
            }
        }
        return closest;
    }
}
