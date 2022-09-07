using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;
using System;


public partial class SpaceShipSystem : SystemBase
{
    public static EndSimulationEntityCommandBufferSystem ecbSystem;
    float timeToShoot = 0;

    protected override void OnCreate()
    {
        ecbSystem = World.GetExistingSystem<EndSimulationEntityCommandBufferSystem>();
    }



    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        float move = Input.GetAxis("Vertical");
        float rotate = Input.GetAxis("Horizontal");
     


        Entities.ForEach((ref PhysicsVelocity vel,ref Rotation rotation, in SpaceShipData spaceShipData) =>
        {

            float3 newVel = math.mul(rotation.Value, new float3(0f, 1f, 0f)) * move * spaceShipData.moveSpeed;


            newVel +=  move  * deltaTime;


            vel.Linear = newVel;

            vel.Linear.z = 0;

            rotation.Value = math.mul(rotation.Value, quaternion.RotateZ(-math.radians(rotate *spaceShipData.rotateSpeed * deltaTime)));



        }).Run();

        var ecb = ecbSystem.CreateCommandBuffer();




        
        timeToShoot += deltaTime;

        if (timeToShoot >= 0.5f)
        {

            Entities.ForEach((ref BulletPrefab bulletPrefab,ref Rotation rotation, in SpaceShipData data, in LocalToWorld ltw) =>
            {

                Entity bulletEntity = ecb.Instantiate(bulletPrefab.bulletPrfab);

                ecb.SetComponent(bulletEntity, new Rotation() { Value = ltw.Rotation });
                ecb.SetComponent(bulletEntity, new Translation() { Value = ltw.Position });

            }).Schedule();


            ecbSystem.AddJobHandleForProducer(Dependency);

            timeToShoot = 0;
        }



    }




}





