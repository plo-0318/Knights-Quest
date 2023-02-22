using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineDynamics : MonoBehaviour
{
    private CinemachineVirtualCamera vc;

    private void Awake()
    {
        vc = GetComponent<CinemachineVirtualCamera>();
    }

    public void Follow(Transform trans)
    {
        vc.Follow = trans;
    }
}
