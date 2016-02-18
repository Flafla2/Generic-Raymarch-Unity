using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
[AddComponentMenu("Effects/Raymarch (Generic)")]
public class RaymarchGeneric : MonoBehaviour
{
    public Transform SunLight;

    [SerializeField]
    private Shader _EffectShader;

    public Material EffectMaterial
    {
        get
        {
            if (!_EffectMaterial && _EffectShader)
            {
                _EffectMaterial = new Material(_EffectShader);
                _EffectMaterial.hideFlags = HideFlags.HideAndDontSave;
            }

            return _EffectMaterial;
        }
    }
    private Material _EffectMaterial;

    public Camera CurrentCamera
    {
        get
        {
            if (!_CurrentCamera)
                _CurrentCamera = GetComponent<Camera>();
            return _CurrentCamera;
        }
    }
    private Camera _CurrentCamera;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Matrix4x4 corners = GetFrustumCorners(CurrentCamera);
        Vector3 pos = CurrentCamera.transform.position;

        for(int x=0;x<4;x++)
            Gizmos.DrawLine(pos, pos + (Vector3)corners.GetRow(x));
    }

    [ImageEffectOpaque]
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (!EffectMaterial)
        {
            Graphics.Blit(source, destination);
            return;
        }

        // Set any custom shader variables here.  For example, you could do:
        // EffectMaterial.SetFloat("_MyVariable", 13.37f);
        // This would set the shader uniform _MyVariable to value 13.37

        EffectMaterial.SetVector("_LightDir", SunLight ? SunLight.forward : Vector3.down);

        EffectMaterial.SetMatrix("_FrustumCornersWS", GetFrustumCorners(CurrentCamera));
        EffectMaterial.SetVector("_CameraWS", CurrentCamera.transform.position);
        EffectMaterial.SetMatrix("_CameraClipToWorld", (CurrentCamera.projectionMatrix * CurrentCamera.worldToCameraMatrix).inverse);
        EffectMaterial.SetMatrix("_CameraModelView", CurrentCamera.worldToCameraMatrix * transform.localToWorldMatrix);

        CustomGraphicsBlit(source, destination, EffectMaterial, 0);
    }

    /// \brief Stores the normalized rays representing the camera frustum in a 4x4 matrix.  Each row is a vector.
    /// 
    /// The following rays are stored in each row (in worldspace, not cameraspace):
    /// Top Left corner:     row=0
    /// Top Right corner:    row=1
    /// Bottom Right corner: row=2
    /// Bottom Left corner:  row=3
    private Matrix4x4 GetFrustumCorners(Camera cam)
    {
        Transform camtr = cam.transform;
        float camNear = cam.nearClipPlane;
        float camFar = cam.farClipPlane;
        float camFov = cam.fieldOfView;
        float camAspect = cam.aspect;

        Matrix4x4 frustumCorners = Matrix4x4.identity;

        float fovWHalf = camFov * 0.5f;

        float tan_fov = Mathf.Tan(fovWHalf * Mathf.Deg2Rad);
        Vector3 toRight = camtr.right * camNear * tan_fov * camAspect;
        Vector3 toTop = camtr.up * camNear * tan_fov;

        Vector3 topLeft = (camtr.forward * camNear - toRight + toTop);
        topLeft.Normalize();

        Vector3 topRight = (camtr.forward * camNear + toRight + toTop);
        topRight.Normalize();

        Vector3 bottomRight = (camtr.forward * camNear + toRight - toTop);
        bottomRight.Normalize();

        Vector3 bottomLeft = (camtr.forward * camNear - toRight - toTop);
        bottomLeft.Normalize();

        frustumCorners.SetRow(0, topLeft);
        frustumCorners.SetRow(1, topRight);
        frustumCorners.SetRow(2, bottomRight);
        frustumCorners.SetRow(3, bottomLeft);

        return frustumCorners;
    }

    /// \brief Custom version of Graphics.Blit that encodes frustum corner indices into the input vertices.
    /// 
    /// In a shader you can expect the following frustum cornder index information to get passed to the z coordinate:
    /// Top Left vertex:     z=0, u=0, v=0
    /// Top Right vertex:    z=1, u=1, v=0
    /// Bottom Right vertex: z=2, u=1, v=1
    /// Bottom Left vertex:  z=3, u=1, v=0
    /// 
    /// \warning You may need to account for flipped UVs on DirectX machines due to differing UV semantics
    ///          between OpenGL and DirectX.  Use the shader define UNITY_UV_STARTS_AT_TOP to account for this.
    static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
    {
        RenderTexture.active = dest;

        fxMaterial.SetTexture("_MainTex", source);

        GL.PushMatrix();
        GL.LoadOrtho(); // Note: z value of vertices don't make a difference!

        fxMaterial.SetPass(passNr);

        GL.Begin(GL.QUADS);

        GL.MultiTexCoord2(0, 0.0f, 0.0f);
        GL.Vertex3(0.0f, 0.0f, 3.0f); // BL

        GL.MultiTexCoord2(0, 1.0f, 0.0f);
        GL.Vertex3(1.0f, 0.0f, 2.0f); // BR

        GL.MultiTexCoord2(0, 1.0f, 1.0f);
        GL.Vertex3(1.0f, 1.0f, 1.0f); // TR

        GL.MultiTexCoord2(0, 0.0f, 1.0f);
        GL.Vertex3(0.0f, 1.0f, 0.0f); // TL

        GL.End();
        GL.PopMatrix();
    }

}
