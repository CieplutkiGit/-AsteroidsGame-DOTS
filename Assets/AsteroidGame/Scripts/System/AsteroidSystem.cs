using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;
using System;
using Unity.Physics.Systems;

public partial class AsteroidSystem : SystemBase
{
    private StepPhysicsWorld stepPhysicsWorld;
    private EndSimulationEntityCommandBufferSystem commandBufferSystem;

    protected override void OnCreate()
    {
        stepPhysicsWorld = World.GetOrCreateSystem<StepPhysicsWorld>();
        commandBufferSystem = World.GetOrCreateSystem<EndSimulationEntityCommandBufferSystem>();
    }

    protected override void OnUpdate()
    {
        float deltaTime = Time.DeltaTime;

        Entities.ForEach((ref PhysicsVelocity vel,ref Translation translation, ref Rotation rot, ref AsteroidData asteroidData) =>
        {
            asteroidData.timeToActivate -= deltaTime;
            if (asteroidData.timeToActivate <= 0)
            {
                float3 newVel = new float3(noise.cnoise(translation.Value * 0.1f), noise.cnoise(translation.Value * 0.1f), 0f) * asteroidData.moveSpeed/2;





                vel.Linear = newVel;

                vel.Linear.z = 0;
            }

            vel.Angular = math.forward() * asteroidData.rotateSpeed * noise.cnoise(translation.Value * 0.1f)/2;


        }).ScheduleParallel();

        var job = new DetectCollision
        {
            asteroids = GetComponentDataFromEntity<AsteroidData>(true),
            bullets = GetComponentDataFromEntity<BulletData>(true),
            ship = GetComponentDataFromEntity<SpaceShipData>(true),
            entityCommandBuffer = commandBufferSystem.CreateCommandBuffer()
        };
        Dependency = job.Schedule(stepPhysicsWorld.Simulation, Dependency);
        commandBufferSystem.AddJobHandleForProducer(Dependency);

    }





    [BurstCompile]
    struct DetectCollision : ITriggerEventsJob
    {
        [ReadOnly] public ComponentDataFromEntity<AsteroidData> asteroids;
        [ReadOnly] public ComponentDataFromEntity<BulletData> bullets;
        [ReadOnly] public ComponentDataFromEntity<SpaceShipData> ship;
        public EntityCommandBuffer entityCommandBuffer;

        public void Execute(TriggerEvent triggerEvent)
        {
            Entity entityA = triggerEvent.EntityA;
            Entity entityB = triggerEvent.EntityB;
            Entity shipToDestroy = triggerEvent.EntityA;

            if (bullets.HasComponent(entityB) && asteroids.HasComponent(entityA))
            {
                // AsteroidsSpawn.instance.SpawnAsteroid();
                //ScoreData.score += 1;
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);

            }
            if (asteroids.HasComponent(entityB))
            {
                //AsteroidsSpawn.instance.SpawnAsteroid();
                Debug.Log("zderzenie");
                entityCommandBuffer.DestroyEntity(entityA);
                entityCommandBuffer.DestroyEntity(entityB);

            }

            if (asteroids.HasComponent(shipToDestroy) && ship.HasComponent(entityB))
            {
                Debug.Log("aaaa");
                entityCommandBuffer.DestroyEntity(shipToDestroy);
                entityCommandBuffer.DestroyEntity(entityB);
            }




            //entityCommandBuffer.DestroyEntity(ship);


        }
    }
}
