using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BreakoutProject.Utility;
using BreakoutProject.Controller;

namespace BreakoutProject
{
    namespace Manager
    {
        /// <Summary>
        /// ゲームコアマネージャー(ブロック崩し部分)
        /// </Summary>
        public class GameCoreManager : MonoBehaviour
        {
            public enum GameState
            {
                NONE = 0,
                LOADING,
                GAME_READY,
                GAME_LOOP,
                GAME_LEVELCLEAR,
                GAME_END,
                UNDEFINED
            };

            [SerializeField] private List<string> _levelList = new List<string>();
            [SerializeField] private List<GameObject> _blcokPrefabList = new List<GameObject>();
            [SerializeField] private GameObject _playerPrefab;

            // 調整パラメータ
            [SerializeField] private int _startLevel = 0;
            [SerializeField] private Vector3 _blockBasePos;
            [SerializeField] private Vector3 _playerDefaultPos;

            // Rootゲームオブジェク
            [SerializeField] private GameObject _blockRoot;
            [SerializeField] private GameObject _playerRoot;

            [SerializeField, ReadOnlyAttribute] private GameState _state = GameState.NONE;
            [SerializeField, ReadOnlyAttribute] private int _currentLevel = 0;
            [SerializeField, ReadOnlyAttribute] private int _targetCount = 0;

            private MasterManager _masterManager;
            private PlayerController _playerController;
            private CSVReader _csvreader;

            // Start is called before the first frame update
            private void Awake()
            {
                GameObject masterManagerObj = GameObject.FindWithTag("Manager");
                _masterManager = masterManagerObj.GetComponent<MasterManager>();
                if (_masterManager == null)
                {
                    Debug.LogError("MasterManager does not exist!");
                    return;
                }

                _csvreader = new CSVReader();

                // 初期化
                InitPlayer();
                InitializeScene(_startLevel);
            }

            private void Update()
            {

            }

            // ステージ初期化
            private void InitializeScene(int level)
            {
                int levleListCount = _levelList.Count;
                if (levleListCount > 0 && level < levleListCount)
                {
                    if (_levelList[level] != null)
                    {
                        string csvFilename = "CSV/" + _levelList[level];
                        StartCoroutine(coInitLevel(csvFilename));
                        _currentLevel = level;
                        _state = GameState.LOADING;
                    }
                }
                else
                {
                    Debug.LogError("Level .CSV file does not exist!");
                    _state = GameState.UNDEFINED;
                }
            }

            // プレイヤー初期化
            private void InitPlayer()
            {
                if (_playerPrefab != null && _playerRoot != null)
                {
                    GameObject playerpaddle = Instantiate(_playerPrefab, _playerDefaultPos, Quaternion.identity, _playerRoot.transform);
                    if (playerpaddle != null)
                    {
                        playerpaddle.tag = _playerRoot.tag;
                        _playerController = playerpaddle.GetComponent<PlayerController>();
                        if(_playerController != null)
                        {
                            _playerController.InitPlayerController(this);
                        }
                    }
                }
            }

            // プレイヤーリセット
            private void ResetPlayer()
            {
                if (_playerController != null)
                {
                    _playerController.transform.position = _playerDefaultPos;
                }
            }

            // フレーム倍率取得
            public float GetSceneDeltaTimeScale()
            {
                if (_masterManager != null)
                {
                    float sceneTimeScale = 1.0f;
                    return _masterManager.GetDeltaTimeScale() * sceneTimeScale;
                }
                return 0.0f;
            }

            public GameState GetGameState()
            {
                return _state;
            }


            IEnumerator coInitLevel(string csvfileStr)
            {
                List<string> levelStr = new List<string>();

                levelStr = _csvreader.LoadCSVFile(csvfileStr);

                // CSV解析、ブロック生成
                if (levelStr.Count > 0)
                {
                    List<int> stageBricks = new List<int>();
                    int row = 0;
                    foreach (string line in levelStr)
                    {
                        string[] info = line.Split(',');

                        for (int i = 0; i < info.Length; i++)
                        {
                            int blockIndex = int.Parse(info[i]);
                            // ブロック初期化
                            if (blockIndex > 0 && blockIndex <= _blcokPrefabList.Count)
                            {
                                GameObject block = null;
                                int prefabIndex = blockIndex - 1;
                                Vector3 brickPos = new Vector3(_blockBasePos.x + i * 2.0f, _blockBasePos.y - (row * 0.5f), _blockBasePos.z);
                                if (_blockRoot != null)
                                {
                                    block = Instantiate(_blcokPrefabList[prefabIndex], brickPos, Quaternion.identity, _blockRoot.transform);
                                }
                                else
                                {
                                    block = Instantiate(_blcokPrefabList[prefabIndex], brickPos, Quaternion.identity);
                                }

                                if (block != null) 
                                {
                                    block.tag = _blockRoot.tag;
                                }

                                _targetCount++;
                            }
                        }
                        row++;
                        yield return null;
                    }

                    // 解析完了
                    Debug.Log(csvfileStr + " load succeeded!");
                    _state = GameState.GAME_READY;
                    yield return null;
                }
            }
        }
    }
}