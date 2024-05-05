using MarksAssets.FullscreenWebGL;
using UnityEngine;
using UnityEngine.UI;

public class UIFullscreenButton : MonoBehaviour
{
    [SerializeField] private Button _button;

    private void Awake() => _button.onClick.AddListener(UpdateFullscreenMode);

    private void Start()
    {
        gameObject.SetActive(FullscreenWebGL.isFullscreenSupported());
    }

    private void UpdateFullscreenMode()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
