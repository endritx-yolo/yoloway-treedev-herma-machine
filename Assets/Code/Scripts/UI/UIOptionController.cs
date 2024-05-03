using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class UIOptionController : MonoBehaviour
{
    [SerializeField] private Button _qualityButton;
    [SerializeField] private Button _shadowsButton;
    [SerializeField] private Button _antiAliasingButton;
    [SerializeField] private Button _modelButton;

    [SerializeField] private Color[] _cameraBackgroundColorArray;
    [SerializeField] private PostProcessLayer _postProcessLayer;
    [SerializeField] private PostProcessVolume _postProcessVolume;
    [SerializeField] private PostProcessProfile[] _profilesArray;

    [SerializeField] private GameObject[] _modelArray;

    private Camera _mainCamera;
    private TextMeshProUGUI _qualityText;
    private TextMeshProUGUI _shadowsText;
    private TextMeshProUGUI _antiAliasingText;
    private TextMeshProUGUI _modelText;

    private int _currPostProcessingVolumeIndex;
    private int _currShadowSettingIndex;
    private int _antiAliasingIndex;
    private int _modelIndex;

    private void Awake()
    {
        if (_postProcessVolume == null || _profilesArray.Length == 0)
            _qualityButton.interactable = false;

        if (_modelArray.Length <= 1)
            _modelButton.interactable = false;

        _qualityButton.onClick.AddListener(UpdateQuality);
        _shadowsButton.onClick.AddListener(UpdateShadows);
        _antiAliasingButton.onClick.AddListener(UpdateAntiAliasing);
        _modelButton.onClick.AddListener(UpdateModel);

        _qualityText = _qualityButton.GetComponent<TextMeshProUGUI>();
        _shadowsText = _shadowsButton.GetComponent<TextMeshProUGUI>();
        _antiAliasingText = _antiAliasingButton.GetComponent<TextMeshProUGUI>();
        _modelText = _modelButton.GetComponent<TextMeshProUGUI>();

        _mainCamera = Camera.main;
    }

    private void Start()
    {
        Application.targetFrameRate = -1;

        _mainCamera.backgroundColor = _cameraBackgroundColorArray[0];
        _postProcessVolume.profile = _profilesArray[0];
        _qualityText.text = $"Quality: {_postProcessVolume.profile.name}";

        QualitySettings.shadowResolution = ShadowResolution.Low;
        QualitySettings.shadows = ShadowQuality.Disable;
        _shadowsText.text = $"Shadows: None";

        _postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
        _antiAliasingText.text = $"Anti-Aliasing: None";

        _modelText.text = $"Model: Herma";
    }

    private void UpdateQuality()
    {
        _currPostProcessingVolumeIndex++;
        if (_currPostProcessingVolumeIndex >= _profilesArray.Length) _currPostProcessingVolumeIndex = 0;
        _postProcessVolume.profile = _profilesArray[_currPostProcessingVolumeIndex];
        _mainCamera.backgroundColor = _cameraBackgroundColorArray[_currPostProcessingVolumeIndex == 0 ? 0 : 1];
        _qualityText.text = $"Quality: {_postProcessVolume.profile.name}";
    }

    private void UpdateShadows()
    {
        _currShadowSettingIndex++;
        if (_currShadowSettingIndex >= 4) _currShadowSettingIndex = 0;
        switch (_currShadowSettingIndex)
        {
            case 0:
                QualitySettings.shadowResolution = ShadowResolution.Low;
                QualitySettings.shadows = ShadowQuality.Disable;
                _shadowsText.text = $"Shadows: None";
                break;
            case 1:
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                _shadowsText.text = $"Shadows: Medium";
                break;
            case 2:
                QualitySettings.shadowResolution = ShadowResolution.High;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                _shadowsText.text = $"Shadows: High";
                break;
            case 3:
                QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
                QualitySettings.shadows = ShadowQuality.HardOnly;
                _shadowsText.text = $"Shadows: Very High";
                break;
        }
    }

    private void UpdateAntiAliasing()
    {
        _antiAliasingIndex++;
        if (_antiAliasingIndex >= 4) _antiAliasingIndex = 0;
        switch (_antiAliasingIndex)
        {
            case 0:
                _postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.None;
                _antiAliasingText.text = $"Anti-Aliasing: None";
                break;
            case 1:
                _postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.FastApproximateAntialiasing;
                _postProcessLayer.fastApproximateAntialiasing.fastMode = true;
                _antiAliasingText.text = $"Anti-Aliasing: FXAA";
                break;
            case 2:
                _postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.SubpixelMorphologicalAntialiasing;
                _postProcessLayer.subpixelMorphologicalAntialiasing.quality =
                    SubpixelMorphologicalAntialiasing.Quality.Low;
                _antiAliasingText.text = $"Anti-Aliasing: SMAA";
                break;
            case 3:
                _postProcessLayer.antialiasingMode = PostProcessLayer.Antialiasing.TemporalAntialiasing;
                _antiAliasingText.text = $"Anti-Aliasing: TAA";
                break;
        }
    }

    private void UpdateModel()
    {
        _modelIndex++;
        if (_modelIndex >= _modelArray.Length) _modelIndex = 0;
        for (int i = 0; i < _modelArray.Length; i++)
            _modelArray[i].SetActive(false);
        
        _modelArray[_modelIndex].SetActive(true);
        _modelText.text = $"Model: {_modelArray[_modelIndex].name}";
    }
}