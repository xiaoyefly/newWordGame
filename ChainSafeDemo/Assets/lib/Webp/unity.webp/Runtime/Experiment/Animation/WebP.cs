using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace WebP.Experiment.Animation
{
    /// <summary>
    /// Entry of all the loading logic.
    /// </summary>
    public class WebP
    {

#if USE_FAIRYGUI
        /// <summary>
        /// Async loading webp files, and return WebPRender for render.
        /// This function return NTextures which would work with fairygui: https://en.fairygui.com/
        /// </summary>
        /// <param name="url">Remote urls to download or project related absolute path(based on platform)</param>
        /// <returns>WebPRederer to provide NTexture for rendering</returns>
        public static async Task<WebPRendererWrapper<NTexture>> LoadNTexturesAsync(string url)
        {
            var bytes = await WebPLoader.Load(url);
            if (bytes == null || bytes.Length <= 0) return null;

            var sw = new Stopwatch();
            var textures = await WebPDecoderWrapper.Decode(bytes);
            var nTextures = textures.ConvertAll(ret =>
            {
                var (texture, timestamp) = ret;

                return (new NTexture(texture), timestamp);
            });
            var renderer = new WebPRendererWrapper<NTexture>(nTextures);
            Debug.Log($"[WebP] Decode webp into textures in: {sw.ElapsedMilliseconds}");
            sw.Stop();
            return renderer;
        }
#endif

        /// <summary>
        /// Async loading webp files, and return WebPRender for render.
        /// </summary>
        /// <param name="url">Remote urls to download or project related absolute path(based on platform)</param>
        /// <returns>WebPRederer to provide Texture for rendering</returns>
        public static async Task<WebPRendererWrapper<Texture2D>> LoadTexturesAsync(string url,Action<Texture2D> handle=null)
        {
            byte[] bytes = await WebPLoader.Load(url);
            if (bytes == null || bytes.Length <= 0)
            {
                return null;
            }

            List<(Texture2D, int)> textures = await WebPDecoderWrapper.Decode(bytes);
            WebPRendererWrapper<Texture2D> renderer = new WebPRendererWrapper<Texture2D>(textures,handle);
            return renderer;
        }

        public static IEnumerator LoadIEnumerator(string url,Action<Texture2D> handle=null)
        {
            if (string.IsNullOrEmpty(url))
            {
                Debug.LogError("[WebP] Loading path can not be empty");
                yield break;
            }
            Debug.Log($"[WebP] Try loading WebP file: {url}");

            byte[] bytes = null;

            if (url.Contains("//") || url.Contains("///"))
            {
                // var www = new WWW(url);
                // while (!www.isDone)
                // { 
                //     new WaitForSeconds(1f);
                // }
                // var www = LoadWWWIEnumerator(url);
                WWW www = new WWW(url);
                while (!www.isDone)
                {
                    // yield return null;
                    yield return new WaitForSeconds(0.5f);
                }

                // if (www.error != null)
                // {
                //     yield return null;
                // }
                // else
                // {
                //     yield return www;
                // }
                if (www != null &&  www.bytes != null)
                {
                    bytes = www.bytes;
                }

                if (www != null)
                {
                    www.Dispose();
                }
                
            }
            else
            {
                try
                {
                    bytes = File.ReadAllBytes(url);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[WebP] load error: {e.Message}");
                }
            }

            if (bytes == null || bytes.Length <= 0)
            {
                yield break;
            }

            LoadTexturesAsync11(bytes,handle);
            
            // return renderer;
            // yield return bytes;
        }

        public static async Task<WebPRendererWrapper<Texture2D>> LoadTexturesAsync11(byte[] bytes,Action<Texture2D> handle=null)
        {
            List<(Texture2D, int)> textures = await WebPDecoderWrapper.Decode(bytes);
            WebPRendererWrapper<Texture2D> renderer = new WebPRendererWrapper<Texture2D>(textures,handle);
            return renderer;
        }

        public static async Task<WebPRendererWrapper<Texture2D>> LoadTexturesAsync(byte[] bytes)
        {
            Assert.IsNotNull(bytes);

            List<(Texture2D, int)> textures = await WebPDecoderWrapper.Decode(bytes);
            WebPRendererWrapper<Texture2D> renderer = new WebPRendererWrapper<Texture2D>(textures);
            return renderer;
        }
    }
}