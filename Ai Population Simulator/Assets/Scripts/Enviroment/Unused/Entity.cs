using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public static bool isPaused = false;
    protected Material material; 

    
    /*public virtual void Init(Vector3 InitPosition) {
        if (gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>() != null) {
            material = gameObject.transform.GetComponentInChildren<SkinnedMeshRenderer>().material;
        }

        if (material == null) {
            material = new Material(Shader.Find("Specular"));
        }

        transform.position = InitPosition;
        transform.rotation = Quaternion.Euler(0, Random.Range(0, 360), 0);
        BoxCollider bc = gameObject.AddComponent<BoxCollider>();
        bc.center = new Vector3(0, 0.5f, 0);
        gameObject.AddComponent<Rigidbody>().useGravity = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    } */
     
}