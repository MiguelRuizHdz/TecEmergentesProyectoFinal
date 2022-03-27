using System.Collections;
using UnityEngine;

public class XRController : MonoBehaviour
{
    public static XRController Instance;

    // almacenar Camaras
    public GameObject cameraRA;
    public GameObject cameraRV;
    [Space]
    public Material fadeMaterial;
    public Canvas canvasRV;
    
    private GameObject _contenidoActual; // VR

    private bool _isEnterRV;
    private bool _isFading;
    private float _fadeValue;
    private Color _color;

     private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        _color = fadeMaterial.color;

        _fadeValue = 0;
        UpdateValue();

    }

    public void XREnter(GameObject contenido )
    {
        if (!_isFading)
        {
            _isFading = true;
            _isEnterRV = true;
            _contenidoActual = contenido;

            StartCoroutine(Fade());
        }
    }

    public void XRExit()
    {
        if (!_isFading)
        {
            _isFading = true;
            _isEnterRV = false;

            StartCoroutine(Fade());
        }
    }

    // corutina
    private IEnumerator Fade()
    {
        while (_fadeValue < 1) //cero es transparente a negro
        {
            _fadeValue += 0.05f; // transición lenta
            UpdateValue();
            yield return new WaitForSeconds(0.05f); // esperar
        }

        cameraRV.SetActive(_isEnterRV);
        cameraRA.SetActive(!_isEnterRV);
        _contenidoActual.SetActive(_isEnterRV);
        canvasRV.enabled = _isEnterRV;

        while (_fadeValue > 0) // 1 es negro a transparente
        {
            _fadeValue -= 0.05f; // transición lenta
            UpdateValue();
            yield return new WaitForSeconds(0.05f); // esperar
        }

        _isFading = false;
    }

    private void UpdateValue() 
    {
        _color.a = _fadeValue; // alfa
        fadeMaterial.color = _color; // asignar color al material
    }

}
