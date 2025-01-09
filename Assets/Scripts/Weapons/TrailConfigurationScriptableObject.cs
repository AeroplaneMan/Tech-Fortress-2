using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trail Config", menuName = "Weapons/Trail Configuration", order = 4)]
public class TrailConfigurationScriptableObject : ScriptableObject
{
    public Material material;
    public AnimationCurve widthCurve;
    public float duration = 0.5f;
    public float minVertexDistance = 0.1f;
    public Gradient color;

    public float MissDistance = 100f;
    public float simulationSpeed = 100f;
}
  