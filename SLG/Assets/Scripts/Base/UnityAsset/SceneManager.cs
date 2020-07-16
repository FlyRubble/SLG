using UnityEngine;
using System.Collections.Generic;

namespace SLG
{
    using Event;
    using Singleton;
    namespace UnityAsset
    {
        /// <summary>
        /// 场景管理
        /// </summary>
        public class SceneManager : Singleton<SceneManager>
        {
            #region Variable
            /// <summary>
            /// 场景加载队列
            /// </summary>
            Dictionary<AsyncOperation, Action> m_queue;
            #endregion

            #region Function
            /// <summary>
            /// 构造函数
            /// </summary>
            public SceneManager()
                : base()
            {
                m_queue = new Dictionary<AsyncOperation, Action>(1 << 2);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            public void LoadScene(int sceneBuildIndex)
            {
                LoadScene(sceneBuildIndex, null);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="complete">Complete.</param>
            public void LoadScene(int sceneBuildIndex, Action complete)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex);
                complete?.Invoke();
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            public void LoadScene(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
            {
                LoadScene(sceneBuildIndex, loadSceneMode, null);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            /// <param name="complete">Complete.</param>
            public void LoadScene(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, Action complete)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneBuildIndex, loadSceneMode);
                complete?.Invoke();
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneName">Scene name.</param>
            public void LoadScene(string sceneName)
            {
                LoadScene(sceneName, null);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="complete">Complete.</param>
            public void LoadScene(string sceneName, Action complete)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                complete?.Invoke();
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            public void LoadScene(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
            {
                LoadScene(sceneName, loadSceneMode, null);
            }

            /// <summary>
            /// 加载场景
            /// </summary>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            /// <param name="complete">Complete.</param>
            public void LoadScene(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, Action complete)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName, loadSceneMode);
                complete?.Invoke();
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            public AsyncOperation LoadSceneAsync(int sceneBuildIndex)
            {
                return LoadSceneAsync(sceneBuildIndex, null);
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="complete">Complete.</param>
            public AsyncOperation LoadSceneAsync(int sceneBuildIndex, Action complete)
            {
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex);
                m_queue.Add(async, complete);
                return async;
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            public AsyncOperation LoadSceneAsync(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
            {
                return LoadSceneAsync(sceneBuildIndex, loadSceneMode, null);
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneBuildIndex">Scene build index.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            /// <param name="complete">Complete.</param>
            public AsyncOperation LoadSceneAsync(int sceneBuildIndex, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, Action complete)
            {
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneBuildIndex, loadSceneMode);
                m_queue.Add(async, complete);
                return async;
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneName">Scene name.</param>
            public AsyncOperation LoadSceneAsync(string sceneName)
            {
                return LoadSceneAsync(sceneName, null);
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="complete">Complete.</param>
            public AsyncOperation LoadSceneAsync(string sceneName, Action complete)
            {
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
                m_queue.Add(async, complete);
                return async;
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            public AsyncOperation LoadSceneAsync(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode)
            {
                return LoadSceneAsync(sceneName, loadSceneMode, null);
            }

            /// <summary>
            /// 异步加载资源
            /// </summary>
            /// <returns>The scene async.</returns>
            /// <param name="sceneName">Scene name.</param>
            /// <param name="loadSceneMode">Load scene mode.</param>
            /// <param name="complete">Complete.</param>
            public AsyncOperation LoadSceneAsync(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, Action complete)
            {
                AsyncOperation async = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
                m_queue.Add(async, complete);
                return async;
            }

            /// <summary>
            /// 更新
            /// </summary>
            public void Update()
            {
                if (m_queue.Count > 0)
                {
                    foreach (var kvp in m_queue)
                    {
                        if (kvp.Key.isDone)
                        {
                            kvp.Value?.Invoke();
                            m_queue.Remove(kvp.Key);
                            break;
                        }
                    }
                }
            }
            #endregion
        }
    }
}