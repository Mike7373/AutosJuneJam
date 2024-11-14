using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

[ExecuteInEditMode]
public class TestRig : MonoBehaviour
{
    public bool shouldBuildRig;

    private void Update()
    {
        if (shouldBuildRig)
        {
            GetComponent<RigBuilder>().Build();

            shouldBuildRig = false;
        }
    }
}