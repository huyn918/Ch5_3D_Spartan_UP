using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
   // 캐릭터매니저 캡슐화
    private static CharacterManager _instance;
    public static CharacterManager Instance
        { get
            {
            if (_instance == null)
                _instance = new GameObject("CharacterManager").AddComponent<CharacterManager>();
            return _instance;
            }
        }

    // 플레이어 캡슐화
    private Player _player;
    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
    
    // 캐릭터매니저 싱글톤화
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else
        {
            if (_instance != null) Destroy(gameObject);
        }
    }

}
