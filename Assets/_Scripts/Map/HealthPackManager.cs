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
        HealthPack firstAvailable = null;
        float closestDistance = float.MaxValue;
        foreach (var healthPack in healthPacks)
        {
            if (healthPack.isAvailable)
            {
                firstAvailable = healthPack;
            }

            float distance = Vector3.Distance(position, healthPack.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = healthPack;
            }
        }

        if (!closest.isAvailable)
        {
            return firstAvailable;
        }
        return closest;
    }
}
