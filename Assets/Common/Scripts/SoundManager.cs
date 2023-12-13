using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    //�V���O���g��
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
    [Header("�����ɖ炵�������̐�")]
    [SerializeField, Range(0, 20)]
    public int audioSourceNumber;

    [SerializeField]
    private SoundData[] BGM_Datas;
    [SerializeField]
    private SoundData[] SE_Datas;

    //AudioSource�i�X�s�[�J�[�j�𓯎��ɖ炵�������̐������p��
    private AudioSource[] audioSourceList;

    //�ʖ�(name)���L�[�Ƃ����Ǘ��pDictionary
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
        //auidioSourceList�z��̐�����AudioSource���������g�ɐ������Ĕz��Ɋi�[
        audioSourceList = new AudioSource[audioSourceNumber];
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            audioSourceList[i] = gameObject.AddComponent<AudioSource>();
        }

        //soundDictionary�ɃZ�b�g
        foreach (var soundData in BGM_Datas)
        {
            BGM_Dictionary.Add(soundData.name, soundData);
        }
        foreach (var soundData in SE_Datas)
        {
            SE_Dictionary.Add(soundData.name, soundData);
        }
    }

    //���g�p��AudioSource�̎擾 �S�Ďg�p���̏ꍇ��null��ԋp
    private AudioSource GetUnusedAudioSource()
    {
        for (var i = 0; i < audioSourceList.Length; ++i)
        {
            if (audioSourceList[i].isPlaying == false) return audioSourceList[i];
        }

        return null; //���g�p��AudioSource�͌�����܂���ł���
    }

    //�w�肳�ꂽAudioClip�𖢎g�p��AudioSource�ōĐ�
    public void Play(AudioClip clip, bool loop, float volume, float pitch)
    {
        var audioSource = GetUnusedAudioSource();
        if (audioSource == null) return; //�Đ��ł��܂���ł���
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = MasterVolume * volume;
        audioSource.pitch = pitch;
        audioSource.Play();
        
    }

    //�w�肳�ꂽ�ʖ��œo�^���ꂽAudioClip���Đ�
    public void PlayBGM(string name)
    {
        if (BGM_Dictionary.TryGetValue(name, out var soundData)) //�Ǘ��pDictionary ����A�ʖ��ŒT��
        {
            Play(soundData.audioClip, true, soundData.volume * BGMVolume, soundData.pitch); //����������A�Đ�
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
        }
    }
    public void PlaySE(string name)
    {
        if (SE_Dictionary.TryGetValue(name, out var soundData)) //�Ǘ��pDictionary ����A�ʖ��ŒT��
        {
            Play(soundData.audioClip, false, soundData.volume * SEVolume, soundData.pitch); //����������A�Đ�
        }
        else
        {
            Debug.LogWarning($"���̕ʖ��͓o�^����Ă��܂���:{name}");
        }
    }
}
