using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[ExecuteInEditMode]
public class LaunchBall : MonoBehaviour
{
    /// <summary>This is the highest point of the curve</summary>
    [FormerlySerializedAs("m_h")] [FormerlySerializedAs("h")] public float m_height = 25;
    
    /// <summary>This is the distance the player is moving to</summary>
    [FormerlySerializedAs("m_distX")] [FormerlySerializedAs("distX")] public float m_maxDistX = 40;
    public float g = -18;
    
    /// <summary>This is the rigidbody of the player</summary>
    [FormerlySerializedAs("sphere")] public GameObject m_sphere;

    private Rigidbody m_sphereRigid;

    // Start is called before the first frame update
    private void Start()
    {
        m_sphereRigid = m_sphere.GetComponent<Rigidbody>() != null ? m_sphere.GetComponent<Rigidbody>() : m_sphere.AddComponent<Rigidbody>();
        m_sphereRigid.useGravity = false;
    }

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Space)) return;
        Launch();
    }
    
    /// <summary> This method handle the launch of the player</summary>
    private void Launch()
    {
        m_sphereRigid.useGravity = true;
        Physics.gravity = Vector3.up * g;
        m_sphereRigid.velocity = CalculateCurve();
    }
    
    /// <summary> This method calculate the curve the player should take </summary>
    private Vector3 CalculateCurve()
    {
        //The displacement in Y.
        //If we have a target it's : targetPosY - OurPosY (where our targetPosY shouldn't be higher than our height).
        //If no target, then the displacement is the actual height we want to jump to.
        float displacementY =  m_height;
        
        //The displacement in X and Y
        //If we have a target, it should be TargetPosX - OurPosX and TargetPosZ - OurPosZ
        //If no target, then the displacement is the direction we want to go to. Right now, Z is equal to 0, so we move to the right.
        Vector3 displacementXZ = new Vector3(m_maxDistX -m_sphereRigid.position.x, 0, 0);
        
        
        //We calculate the kinematic equation for the player, based on the direction we want to go to horizontally and vertically
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2*g*m_height);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * m_height / g) + Mathf.Sqrt(2 *(displacementY-m_height)) / g);
        
        //We return the result of the kinematic equation as a Vector3
        return velocityXZ + velocityY;
    }

}
