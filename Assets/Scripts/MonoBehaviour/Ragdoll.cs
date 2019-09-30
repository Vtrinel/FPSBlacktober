using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class Ragdoll : MonoBehaviour
{
    [SerializeField,ReadOnly] List<Rigidbody> rigidbodies;
    [SerializeField,ReadOnly] List<Transform> rigTransforms;

    [Button]
    void GetRagdoll()
    {
        rigidbodies = new List<Rigidbody>();
        rigTransforms = new List<Transform>();
        GetChild(transform);

        for (int i = 0; i < rigTransforms.Count; i++)
        {
            GetRigidbody(rigTransforms[i]);
        }

        SetKinematic();
    }

    void GetChild(Transform currentTransform)
    {
        foreach (Transform child in currentTransform)
        {
            rigTransforms.Add(child);
            if (child.childCount != 0)
            {
                GetChild(child);
            }
        }
    }

    void GetRigidbody(Transform p_tranform)
    {
        Rigidbody rigidbody = p_tranform.GetComponent<Rigidbody>();
        if (rigidbody)
        {
            rigidbodies.Add(rigidbody);
        }
    }

    public void SetKinematic(bool p_isKinematic = true)
    {
        for (int i = 0; i < rigidbodies.Count; i++)
        {
            rigidbodies[i].isKinematic = p_isKinematic;
        }
    }
}
