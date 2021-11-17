using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoidsSetup : MonoBehaviour
{
    [Header("Main Settings / Works Runtime")]
    public bool seperate = true;
    public bool coherate = true;
    public bool align = true;
    [Space]
    [Range(0, 1)] public float seperationWeight = 0.2f;
    [Range(0, 1)] public float alignmentWeight = 0.5f;
    [Range(0, 1)] public float cohesionWeight = 0.5f;

    [Header("Cam/Simulation Settings")]
    public bool only2D; // Shows boids in 2d
    [Space]
    public GameObject camera2D;
    public GameObject camera3D;

    [Header("Spawn Settings")]
    public int boidsAmount = 20;
    public GameObject boidPrefab;
    [Header("Cube/SpawnRange Settings")]
    public float spawnRangeX;
    public float spawnRangeY;
    public float spawnRangeZ;

    [Header("Boid Settings")]
    public float boidRadius = 1;
    [Space]
    [Range(0, 10)] public float minSpeed = 1;
    [Range(0, 10)] public float maxSpeed = 5;

    Boid[] boids;

    private void Awake()
    {
        if (only2D)
            camera2D?.SetActive(true);
        else
            camera3D?.SetActive(true);
    }

    void Start()
    {
        boids = new Boid[boidsAmount];
        for (int i = 0; i < boids.Length; i++)
        {
            Vector3 spawnPosition = new Vector3
            (
               Random.Range(0, spawnRangeX - 1),
               Random.Range(0, spawnRangeY - 1),
               Random.Range(0, spawnRangeZ - 1)
            );
            Vector3 randomStartSpeed = new Vector3
            (
               Random.Range(-1f, 1f),
               Random.Range(-1f, 1f),
               Random.Range(-1f, 1f)
            );

            Boid tempBoid = Instantiate(boidPrefab, spawnPosition, Quaternion.identity).GetComponent<Boid>();
            float defaultSpeed = Random.Range(minSpeed, maxSpeed);
            if (only2D)
            {
                randomStartSpeed.z = 0;
                tempBoid.SetupBoid(spawnRangeX, spawnRangeY, 0, boidRadius, defaultSpeed, randomStartSpeed);
            }
            else
            {
                tempBoid.SetupBoid(spawnRangeX, spawnRangeY, spawnRangeZ, boidRadius, defaultSpeed, randomStartSpeed);
            }
            boids[i] = tempBoid;
        }
    }
    
    void Update()
    {
        foreach (var b in boids)
        {
            b.MoveBoid();
            if (seperate)
            {
                b.Seperation(boids, seperationWeight);
            }
            if (coherate)
            {
                b.Cohesion(boids, cohesionWeight);
            }
            if (align)
            {
                b.Alignment(boids, alignmentWeight);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(new Vector3((spawnRangeX) / 2, (spawnRangeY) / 2, (spawnRangeZ) / 2), new Vector3(spawnRangeX, spawnRangeY, spawnRangeZ));
    }

}
