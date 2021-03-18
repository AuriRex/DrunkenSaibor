using DrunkenSaibor.Data;
using HMUI;
using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace DrunkenSaibor.Util
{
    class Utils
    {

        /// <summary>
        /// Loads an embedded resource from the calling assembly
        /// </summary>
        /// <param name="resourcePath">Path to resource</param>
        public static byte[] LoadFromResource(string resourcePath) => GetResource(Assembly.GetCallingAssembly(), resourcePath);

        /// <summary>
        /// Loads an embedded resource from an assembly
        /// </summary>
        /// <param name="assembly">Assembly to load from</param>
        /// <param name="resourcePath">Path to resource</param>
        public static byte[] GetResource(Assembly assembly, string resourcePath)
        {
            Stream stream = assembly.GetManifestResourceStream(resourcePath);
            byte[] data = new byte[stream.Length];
            stream.Read(data, 0, (int) stream.Length);
            return data;
        }

        public static DrunkEffectData LoadSFX(string resourcePath) => LoadSFX(LoadFromResource(resourcePath));

        private static DrunkEffectData LoadSFX(byte[] bytes)
        {
            AssetBundle bundle = AssetBundle.LoadFromMemory(bytes);
            var shaderEffectMetadataGOPrefab = bundle.LoadAsset<GameObject>("Assets/ShaderEffectMetadata.prefab"); 
            GameObject shaderEffectMetadataGO = UnityEngine.Object.Instantiate(shaderEffectMetadataGOPrefab);
            DrunkEffect shaderEffect = shaderEffectMetadataGO.GetComponent<DrunkEffect>();
            DrunkEffectData data = new DrunkEffectData(shaderEffect);
            GameObject.Destroy(shaderEffectMetadataGO);
            bundle.Unload(false);
            return data;
        }

        public static Type[] GetTypesInNamespace(string nameSpace) => GetTypesInNamespace(Assembly.GetExecutingAssembly(), nameSpace);

        public static Type[] GetTypesInNamespace(Assembly assembly, string nameSpace)
        {
            return
              assembly.GetTypes()
                      .Where(t => String.Equals(t.Namespace, nameSpace, StringComparison.Ordinal))
                      .ToArray();
        }


        public static IEnumerator DoAfter(float time, Action action)
        {
            float start = Time.fixedTime;
            while (start + time > Time.fixedTime)
                yield return null;
            action?.Invoke();
            yield break;
        }

        /// <summary>
        /// Animates a Scroll Indicator
        /// </summary>
        /// <param name="startValue"></param>
        /// <param name="endValue"></param>
        /// <param name="verticalScrollIndicator"></param>
        /// <param name="lerpDuration"></param>
        /// <param name="onDone">Method to execute after it's done animating.</param>
        /// <returns></returns>
        public static IEnumerator ScrollIndicatorAnimator(float startValue, float endValue, VerticalScrollIndicator verticalScrollIndicator, float lerpDuration = 0.3f, Action onDone = null)
        {
            float timeElapsed = 0f;
            while (timeElapsed < lerpDuration)
            {
                verticalScrollIndicator.progress = Mathf.Lerp(startValue, endValue, Easing.OutCubic(timeElapsed / lerpDuration));
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            verticalScrollIndicator.progress = endValue;
            onDone?.Invoke();
        }

        /// <summary>
        /// Scroll a ScrollIndicator
        /// </summary>
        /// <param name="up"></param>
        /// <param name="tableView"></param>
        /// <param name="verticalScrollIndicator"></param>
        /// <param name="coroutine"></param>
        public static void ScrollTheScrollIndicator(bool up, TableView tableView, VerticalScrollIndicator verticalScrollIndicator, Coroutine coroutine)
        {
            Tuple<int, int> range = tableView.GetVisibleCellsIdRange();

            float rangeUpper;
            float pageSize = range.Item2 - range.Item1;
            float numOfCells = tableView.numberOfCells;

            if (up)
            {
                rangeUpper = Mathf.Max(0, range.Item2 - pageSize);
            }
            else
            {
                rangeUpper = Mathf.Min(numOfCells, range.Item2 + pageSize);
            }

            float progress = (rangeUpper - pageSize) / (numOfCells - pageSize);

            if (coroutine != null)
            {
                tableView.StopCoroutine(coroutine);
            }

            coroutine = tableView.StartCoroutine(ScrollIndicatorAnimator(verticalScrollIndicator.progress, progress, verticalScrollIndicator, 0.3f, () => {
                tableView.StopCoroutine(coroutine);
                coroutine = null;
            }));
        }

        /// <summary>
        /// Update the Scroll Indicators Graphics
        /// </summary>
        /// <param name="tableView"></param>
        /// <param name="verticalScrollIndicator"></param>
        /// <param name="waitTime">time in ms it should wait</param>
        public static async void UpdateScrollIndicator(TableView tableView, VerticalScrollIndicator verticalScrollIndicator, int waitTime = 0)
        {

            if (waitTime > 0)
                await SiraUtil.Utilities.AwaitSleep(waitTime);

            Tuple<int, int> range = tableView.GetVisibleCellsIdRange();

            float pageSize = range.Item2 - range.Item1;
            float numOfCells = tableView.numberOfCells;

            verticalScrollIndicator.normalizedPageHeight = pageSize / numOfCells;
            verticalScrollIndicator.progress = (range.Item2 - pageSize) / (numOfCells - pageSize);
        }
    }
}
