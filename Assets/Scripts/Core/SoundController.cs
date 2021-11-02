using System.Collections.Generic;
using FMOD.Studio;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    [SerializeField]
    private SerializableDictionaryBase<Bgm, string> _bgmMusicDictionary;
    [SerializeField]
    private SerializableDictionaryBase<Sfx, string> _sfxMusicDictionary;

    private readonly Dictionary<Bgm, EventInstance> _loadedBattleMusic = new Dictionary<Bgm, EventInstance>();
    private EventInstance _currentPlayingEventInstance;


    private const string _bgmBus = "bus:/bgm";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void StopAllBgm()
    {
        FMODUnity.RuntimeManager.GetBus(_bgmBus).stopAllEvents(STOP_MODE.ALLOWFADEOUT);
        _currentPlayingEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
    }

    public void LoadBgm(Bgm bgmToPlay)
    {
        if (_loadedBattleMusic.ContainsKey(bgmToPlay))
            return;

        if (_bgmMusicDictionary.TryGetValue(bgmToPlay, out var bgmToPlayString))
        {
            var loadedInstance = FMODUnity.RuntimeManager.CreateInstance(bgmToPlayString);
            _loadedBattleMusic.Add(bgmToPlay, loadedInstance);
            return;
        }
        DebugLogger.SendDebugMessage("Could not load bgm, sending default instance.");
    }

    public void PlayBgm(Bgm bgmToPlay)
    {
        if (!_loadedBattleMusic.TryGetValue(bgmToPlay, out var bgmEventInstance)) return;
        bgmEventInstance.start();
        //bgmEventInstance.release();
        _currentPlayingEventInstance = bgmEventInstance;
    }

    public void StopBgm(Bgm bgmToPlay, bool destroyAfter = true)
    {
        if (!_loadedBattleMusic.TryGetValue(bgmToPlay, out var bgmEventInstance)) return;
        bgmEventInstance.stop(STOP_MODE.ALLOWFADEOUT);
        if (destroyAfter)
            _loadedBattleMusic.Remove(bgmToPlay);
    }

    public void PlaySfxOneShot(Sfx sfxToPlay)
    {
        if (_sfxMusicDictionary.TryGetValue(sfxToPlay, out var soundToPlay))
            FMODUnity.RuntimeManager.PlayOneShot(soundToPlay);
    }

    public enum Bgm
    {
        DebugRoom,
        Battle1,
        BattleWin,
        BattleRewardGaining,

    }

    public enum Sfx
    {
        Default,
        BattleEnemyDeath,
        LevelUp


    }

}
