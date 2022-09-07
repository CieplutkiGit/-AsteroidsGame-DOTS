using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
[GenerateAuthoringComponent]
public struct SpaceShipData : IComponentData
{
    public float moveSpeed;
    public float rotateSpeed;
    public float fireRate;
    public Prefab bulletPrefab;
    public Entity entity;
}
