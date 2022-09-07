using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[GenerateAuthoringComponent]
public struct BulletData : IComponentData
{
    public float moveSpeed;
    public float lifeTime;
   
}
