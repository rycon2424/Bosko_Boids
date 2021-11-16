using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public Vector3 accelleration;

    float defaultSpeed;
    float boidRadius;
    float maxX;
    float maxY;
    float maxZ;

    public void SetupBoid(float _maxX, float _maxY, float _maxZ, float _boidRadius, float _defaultSpeed, Vector3 _startDirection)
    {
        accelleration = _startDirection;
        defaultSpeed = _defaultSpeed;
        maxX = _maxX;
        maxY = _maxY;
        maxZ = _maxZ;
        boidRadius = _boidRadius;
    }

    public void MoveBoid()
    {
        transform.Translate((accelleration * defaultSpeed) * Time.deltaTime);
        CheckOffScreen();
    }

    public void Seperation(Boid[] boids, float seperationStrength)
    {
        Vector3 target = Vector3.zero;
        int total = 0;
        foreach (var other in boids)
        {
            float distance = Vector3.Distance(other.transform.position, transform.position);
            if (other != this && distance < boidRadius)
            {
                Vector3 diff = transform.position - other.transform.position;
                diff.Normalize();
                diff /= distance;
                target += diff;
                total++;
            }
        }
        if (total == 0)
            return;
        target /= total;
        Vector3 force = (target - accelleration) * seperationStrength / 10;
        accelleration += force;
        accelleration.Normalize();
    }

    public void Cohesion(Boid[] boids, float cohesionStrength)
    {
        Vector3 center = Vector3.zero;
        int total = 0;
        foreach (var other in boids)
        {
            float distance = Vector3.Distance(other.transform.position, transform.position);
            if (other != this && distance < boidRadius)
            {
                center += other.transform.position;
                total++;
            }
        }
        if (total == 0)
            return;
        center /= total;
        Vector3 target = center - transform.position;
        Vector3 force = (target - accelleration) * cohesionStrength / 10;
        accelleration += force;
        accelleration.Normalize();
    }

    public void Alignment(Boid[] boids, float alignmentStrength)
    {
        Vector3 target = Vector3.zero;
        int total = 0;
        foreach (var other in boids)
        {
            float distance = Vector3.Distance(other.transform.position, transform.position);
            if (other != this && distance < boidRadius)
            {
                target += other.transform.position;
                total++;
            }
        }
        if (total == 0)
            return;
        target /= total;
        Vector3 force = (target - accelleration) * alignmentStrength / 10;
        accelleration += force;
        accelleration.Normalize();
    }

    void CheckOffScreen()
    {
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(0, transform.position.y, transform.position.z);
        }
        if (transform.position.x < 0)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        if (transform.position.y > maxY)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        }
        if (transform.position.y < 0)
        {
            transform.position = new Vector3(transform.position.x, maxY, transform.position.z);
        }
        if (transform.position.z > maxZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxZ);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + accelleration.normalized);
    }
}
