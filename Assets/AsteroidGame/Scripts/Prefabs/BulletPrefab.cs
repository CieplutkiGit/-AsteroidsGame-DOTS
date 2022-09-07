using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletPrefab : IComponentData
{
    public Entity bulletPrfab;
}
