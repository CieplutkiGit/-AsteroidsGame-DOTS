using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;


public partial class BulletSystem : SystemBase
{
    EndSimulationEntityCommandBufferSystem ecbSystem;
    protected override void OnCreate()
    {
        ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }
    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;
        var ecb = ecbSystem.CreateCommandBuffer();

        Entities.ForEach((Entity e,ref PhysicsVelocity vel, ref Rotation rotation, ref BulletData bullet) =>
        {
            
            float3 newVel = math.mul(rotation.Value, new float3(0f, 1f, 0f)) * bullet.moveSpeed;


            newVel +=  1* deltaTime;


            vel.Linear = newVel;

            vel.Linear.z = 0;


            bullet.lifeTime -= deltaTime;
           
            if (bullet.lifeTime <= 0)
            {
               
                ecb.DestroyEntity(e);
            }


        }).Run();


    }
}


