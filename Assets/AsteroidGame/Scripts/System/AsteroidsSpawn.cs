using Unity.Entities;
using Unity.Physics;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Burst;
using Unity;
using System.Collections;
using System.Collections.Generic;

public partial class AsteroidsSpawn : MonoBehaviour
{
    Grid grid;

    public int gridX;
    public int gridY;

    public Vector3 nextPoint;

    public GameObject asteroid;

    private Entity entityAsteroid;
    private EntityManager entityManager;
    private BlobAssetStore blobAssetStore;

    EndSimulationEntityCommandBufferSystem ecbSystem;

    float asterCount;



    public static AsteroidsSpawn instance;

    Vector3Int startPoint;

    private void Awake()
    {
        ecbSystem = SpaceShipSystem.ecbSystem;

         entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        blobAssetStore = new BlobAssetStore();

        GameObjectConversionSettings settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, blobAssetStore);

        entityAsteroid = GameObjectConversionUtility.ConvertGameObjectHierarchy(asteroid, settings);

        instance = this;
        
    }

    private void Start()
    {
        

        grid = GetComponent<Grid>();





        StartCoroutine(Spawner());



    }

    IEnumerator Spawner()
    {

        startPoint = grid.WorldToCell(new Vector3(-gridX * 2, -gridY * 2));
        float2 x = new float2(-10,10);
        
        

        while (true)
        {
           
            
            for (int i = 0; i < gridY; i++)
            {
                yield return new ();
                for (int j = 0; j < gridX; j++)
                {
                    
                    var ecb = ecbSystem.CreateCommandBuffer();
                    nextPoint = grid.GetCellCenterWorld(startPoint) + new Vector3(grid.cellSize.x * j, grid.cellSize.y * i);
                    Entity newAsteroid = entityManager.Instantiate(entityAsteroid);
                    ecb.SetComponent(newAsteroid, new Translation() { Value = nextPoint });
                    //ecb.SetComponent(newAsteroid, new AsteroidData() { rotateSpeed = 10,moveSpeed = 5 }) ;
                    asterCount += 1;
                    Debug.Log(asterCount + "/" + gridX*gridY);

                }
            }

            if (asterCount >= gridY * gridX)
            {
                break;
            }
        }
        
    }


    public void SpawnAsteroid()
    {
        Debug.Log("spawnuje");
    }

    
    private void OnDestroy()
    {
        blobAssetStore.Dispose();
    }


}
