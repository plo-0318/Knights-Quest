using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
[RequireComponent(typeof(CinemachineConfiner))]
public class CinemachineDynamics : MonoBehaviour
{
    private CinemachineVirtualCamera vc;
    private CinemachineConfiner confiner;

    private void Awake()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner>();
    }

    public void Follow(Transform trans)
    {
        vc.Follow = trans;
    }

    public void Confine(Collider2D col)
    {
        confiner.m_BoundingShape2D = col;
    }
}
