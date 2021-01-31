using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Element2DController : MonoBehaviour
{
    private Transform cameraTransform;
    [SerializeField] private Transform graphicsTransform;
    private MeshRenderer graphicsMeshRenderer;
    private Material defaultMat;
    

    protected class AnimationWithMaterial
    {
        public List<Material> frames;
        public float timeBetweenFrame;
    }

    private AnimationWithMaterial currentAnim;

    protected virtual void Start()
    {
        cameraTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        graphicsMeshRenderer = graphicsTransform.GetComponent<MeshRenderer>();
        defaultMat = graphicsMeshRenderer.material;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(graphicsTransform != null)
        {
            graphicsTransform.LookAt(cameraTransform);
            graphicsTransform.rotation = Quaternion.Euler(graphicsTransform.eulerAngles.x + 90, graphicsTransform.eulerAngles.y, graphicsTransform.eulerAngles.z);
        }
    }

    protected void LaunchAnimation(AnimationWithMaterial anim)
    {
        if (currentAnim != anim)
        {
            StopAllCoroutines();
            currentAnim = anim;
            StartCoroutine(AnimateWithMaterial(anim));
        }
    }

    private IEnumerator AnimateWithMaterial(AnimationWithMaterial anim)
    {
        foreach (Material mat in anim.frames)
        {
            Debug.LogWarning("Switching frame " + gameObject.name);
            graphicsMeshRenderer.material = mat;
            yield return new WaitForSecondsRealtime(anim.timeBetweenFrame);
        }
        //graphicsMeshRenderer.material = defaultMat;
        currentAnim = null;
    }


}
