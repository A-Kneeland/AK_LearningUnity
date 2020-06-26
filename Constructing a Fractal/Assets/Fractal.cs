using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fractal : MonoBehaviour
{
    public Mesh[] meshes;
    public Material material;
    public int maxDepth;
    private int depth;
    public float childScale;
    private Material[] materials;
    public float spawnProbability;
    public float maxRotationSpeed;
    private float rotationSpeed;
    public float maxTwist;


    private void InitalizeMaterials()
    {
        materials = new Material[maxDepth + 1];
        for (int i = 0; i < materials.Length; i++)
        {
            materials[i] = new Material(material);
            materials[i].color = Color.Lerp(Color.gray, Color.red, (float)i / maxDepth);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rotationSpeed = Random.Range(-maxRotationSpeed, maxRotationSpeed);
        transform.Rotate(Random.Range(-maxTwist, maxTwist), 0f, 0f);

        if (materials == null)
        {
            InitalizeMaterials();
        }

        gameObject.AddComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        gameObject.AddComponent<MeshRenderer>().material = materials[depth];
        if(depth < maxDepth)
        {
            StartCoroutine(CreateChildren());
            
        }
    }

    private static Vector3[] childDirections =
    {
        Vector3.up,
        Vector3.right,
        Vector3.left,
        Vector3.forward,
        Vector3.back
    };

    private static Quaternion[] childOrientations =
    {
        Quaternion.identity,
        Quaternion.Euler(0, 0, -90),
        Quaternion.Euler(0, 0, 90),
        Quaternion.Euler(90, 0, 0),
        Quaternion.Euler(-90, 0, 0)
    };

    private IEnumerator CreateChildren()
    {
        for(int i = 0; i < childDirections.Length; i++)
        {
            if(Random.value < spawnProbability)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
                new GameObject("Fractal Child").AddComponent<Fractal>().Initialize(this, i);
            }
            
        }
    }

    private void Initialize(Fractal parent, int childIndex)
    {
        meshes = parent.meshes;
        materials = parent.materials;
        maxDepth = parent.maxDepth;
        depth = parent.depth + 1;
        spawnProbability = parent.spawnProbability;
        childScale = parent.childScale;
        transform.parent = parent.transform;
        transform.localScale = Vector3.one * childScale;
        transform.localPosition = childDirections[childIndex] * (0.5f + 0.5f * childScale);
        transform.localRotation = childOrientations[childIndex];
        maxRotationSpeed = parent.maxRotationSpeed;
        maxTwist = parent.maxTwist;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
