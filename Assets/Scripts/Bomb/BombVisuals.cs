using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class BombVisuals : MonoBehaviour
{
    private Material _material;
    private Color _originalColor;
    private bool _isTransparentMode;

    private void Awake()
    {
        var renderer = GetComponent<Renderer>();

        _material = new Material(renderer.material);
        renderer.material = _material;

        _originalColor = _material.color;
        _isTransparentMode = false;
    }

    public void UpdateFade(float progress)
    {
        if (_isTransparentMode == false && progress > 0)
        {
            SetMaterialTransparent(true);
            _isTransparentMode = true;
        }

        _material.color = new Color(
            _originalColor.r,
            _originalColor.g,
            _originalColor.b,
            1 - progress
        );
    }

    public void ResetVisuals()
    {
        _material.color = _originalColor;

        if (_isTransparentMode)
        {
            SetMaterialTransparent(false);
            _isTransparentMode = false;
        }
    }

    private void SetMaterialTransparent(bool isTransparent)
    {
        if (isTransparent)
        {
            _material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            _material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            _material.EnableKeyword("_ALPHABLEND_ON");
            _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
        }
        else
        {
            _material.DisableKeyword("_ALPHABLEND_ON");
            _material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
        }
    }
}