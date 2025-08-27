using UnityEngine;
using TMPro;
// public class WobblyText : MonoBehaviour
// {
//     public TMP_Text textComponent;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {
        
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         textComponent.ForceMeshUpdate();
//         var textInfo = textComponent.textInfo;
//         for (int i = 0; i < textInfo.characterCount; ++i)
//         {
//             var charInfo = textInfo.characterInfo[i];

//             if (!charInfo.isVisible)
//             {
//                 continue;
//             }
//             var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;
//             for (int j = 0; j < 4; ++j)
//             {
//                 var orig = verts[charInfo.vertexIndex + j];
//                 verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * 2f + orig.x * 0.01f) * 10f, 0);
//             }
//         }
//         for (int i = 0; i < textInfo.meshInfo.Length; ++i)
//         {
//             var meshInfo = textInfo.meshInfo[i];
//             meshInfo.mesh.vertices = meshInfo.vertices;
//             textComponent.UpdateGeometry(meshInfo.mesh, i);
//         }
//     }
// }
public class WobblyText : MonoBehaviour
{
    public TMP_Text textComponent;

    [Header("Wobble Settings")]
    public float amplitude = 0.5f;  // height of wobble
    public float frequency = 1.2f;   // speed of wobble
    public float offsetFactor = 0.44f; // horizontal phase difference
    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
    }
    void Update()
    {
        if (textComponent == null) return;

        // Force TMP to update its mesh data
        textComponent.ForceMeshUpdate();
        var textInfo = textComponent.textInfo;

        for (int i = 0; i < textInfo.characterCount; ++i)
        {
            var charInfo = textInfo.characterInfo[i];
            if (!charInfo.isVisible) continue;

            // Get the vertices for this character
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; ++j)
            {
                // Always use the *original* unmodified vertex positions from cachedMeshInfo
                Vector3 orig = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices[charInfo.vertexIndex + j];

                // Offset with sine wave
                verts[charInfo.vertexIndex + j] =
                    orig + new Vector3(0, Mathf.Sin(Time.time * frequency + orig.x * offsetFactor) * amplitude, 0);
            }
        }

        // Apply modified vertices back
        for (int i = 0; i < textInfo.meshInfo.Length; ++i)
        {
            var meshInfo = textInfo.meshInfo[i];
            meshInfo.mesh.vertices = meshInfo.vertices;

            if (textComponent is TextMeshProUGUI)
            {
                // For Canvas (UI)
                textComponent.UpdateGeometry(meshInfo.mesh, i);
            }
            else if (textComponent is TextMeshPro)
            {
                // For World-space TMP
                textComponent.mesh.vertices = meshInfo.vertices;
                textComponent.mesh.RecalculateBounds();
            }
        }
    }
}