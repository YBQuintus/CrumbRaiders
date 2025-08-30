using UnityEngine;

public class CutOut : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask wallMask;
    [SerializeField] private Vector2 cutOutOffset;

    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector2 cutOutPos = mainCamera.WorldToViewportPoint(target.position);
        cutOutPos += cutOutOffset;
        cutOutPos.y /= (Screen.width / Screen.height);

        Vector3 offset = target.position - transform.position;

        RaycastHit[] hits = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].transform.TryGetComponent<Renderer>(out var renderer)) continue;
            Material[] materials  = renderer.materials;
            for (int m = 0; m < materials.Length; m++)
            {
                materials[m].SetVector("_CutoutPosition", new Vector4(cutOutPos.x, cutOutPos.y, 0, 0));
                materials[m].SetFloat("_CutoutSize", 0.05f);
                materials[m].SetFloat("_FalloffSize", 0);
            }
        }

    }
}
