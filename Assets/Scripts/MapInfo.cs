using UnityEngine;

public class MapInfo : MonoBehaviour
{
    [SerializeField] private SoundController.Bgm bgmToPlay;

    [SerializeField] private SoundController.Bgm[] bgmToLoad;
    // Start is called before the first frame update

    void Awake()
    {
        foreach (var _bgm in bgmToLoad)
        {
            SoundController.Instance.LoadBgm(_bgm);
        }
    }
    void Start()
    {
        SoundController.Instance.PlayBgm(bgmToPlay);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
