using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //シングルトン
    public static SoundManager instance;
    
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip audioClip;
        [SerializeField, Range(0f, 1f)]
        public float volume;
        [SerializeField, Range(-3f, 3f)]
        public float pitch;
    }

    [SerializeField, Range(0f, 1f)]
    public float MasterVolume;
    [SerializeField, Range(0f, 1f)]
    public float BGMVolume;
    [SerializeField, Range(0f, 1f)]
    public float SEVolume;
    [Header("同時に鳴らしたい音の数")]
    [SerializeField, Range(0, 20)]
    public int audioSourceNumber;

    [SerializeField]
    private SoundData[] BGM_Datas;
    [SerializeField]
    private SoundData[] SE_Datas;

    //AudioSource（スピーカー）を同時に鳴らしたい音の数だけ用意
    private AudioSource[] audioSourceList;

    //別名(name)をキーとした管理用Dictionary
    private Dictionary<string, SoundData> BGM_Dictionary = new Dictionary<string, SoundData>();
    private Dictionary<string, SoundData> SE_Dictionary = new Dictionary<string, SoundData>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        //auidioSourceList配列の数だけAudioSourceを自分自身に生成して配列に格納
        audioSourceList = new AudioSource[audioSourceNumber];
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        //soundDictionaryにセット
        foreach (var soundData in BGM_Datas)
        {
            BGM_Dictionary.Add(soundData.name, soundData);
        }
        foreach (var soundData in SE_Datas)
        {
            SE_Dictionary.Add(soundData.name, soundData);
        }
    }

    //未使用のAudioSourceの取得 全て使用中の場合はnullを返却
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //未使用のAudioSourceは見つかりませんでした
    }

    //指定されたAudioClipを未使用のAudioSourceで再生
    public void Play(AudioClip clip, bool loop, float volume, float pitch)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //再生できませんでした
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = MasterVolume * volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        
    }

    //指定された別名で登録されたAudioClipを再生
    public void PlayBGM(string name)
    {
        if (BGM_Dictionary.TryGetValue(name, out var soundData)) //管理用Dictionary から、別名で探索
        {
            Play(soundData.audioClip, true, soundData.volume * BGMVolume, soundData.pitch); //見つかったら、再生
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
    public void PlaySE(string name)
    {
        if (SE_Dictionary.TryGetValue(name, out var soundData)) //管理用Dictionary から、別名で探索
        {
            Play(soundData.audioClip, false, soundData.volume * SEVolume, soundData.pitch); //見つかったら、再生
        }
        else
        {
            Debug.LogWarning($"その別名は登録されていません:{name}");
        }
    }
}
