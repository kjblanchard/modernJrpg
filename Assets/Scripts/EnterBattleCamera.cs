using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class EnterBattleCamera : MonoBehaviour
{
    [SerializeField] private BlurOptimized _blurOptimized;
    [SerializeField] private GameObject _blackBarTransitionGameObject;
    [SerializeField] private GameObject _cameraTweenGameObject;
    [SerializeField] private Animator _blackBarAnimation;
    [SerializeField] private const string _blackBarTriggerString = "start";

    /// <summary>
    /// Starts the regular battle transition
    /// </summary>
    public void StartBattleTransition()
    {
        BlurBackground();
        _cameraTweenGameObject.SetActive(true);
        _blackBarTransitionGameObject.SetActive(true);
        _blackBarAnimation.SetTrigger(_blackBarTriggerString);

    }

    private void BlurBackground()
    {
        _blurOptimized.enabled = true;

    }

}
