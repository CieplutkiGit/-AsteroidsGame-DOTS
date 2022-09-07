using Unity.Entities;
using Unity.Transforms;
using UnityEngine;


[GenerateAuthoringComponent]
public struct AsteroidData : IComponentData
{
    public float moveSpeed;
    public float rotateSpeed;
    public float timeToActivate;


    
}
