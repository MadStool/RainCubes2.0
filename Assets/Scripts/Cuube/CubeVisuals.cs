using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class CubeVisuals : MonoBehaviour
{
    private Renderer _renderer;
    private Material _material;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
    }

    public void SetColor(Color color)
    {
        _material.color = color;
    }
}