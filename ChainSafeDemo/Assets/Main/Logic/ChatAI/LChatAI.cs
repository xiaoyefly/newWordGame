using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Main.Logic.ChatAI
{
    public class LChatAI : Main.Utility.Singleton<LChatAI>
    {
        private const string m_Api_Url = "https://api.openai.com/v1/completions";
        private const string m_OpenAI_Key = "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";

        public void TestChat()
        {
            // "model": "text-davinci-003",
            // "prompt": "",
            // "temperature": 0,
            // "max_tokens": 100,
            // "top_p": 1,
            // "frequency_penalty": 0.0,
            // "presence_penalty": 0.0,
            // "stop": ["\n"]
            ChatAIPostData _postData = new ChatAIPostData
            {
                model = "text-davinci-003",
                prompt = "你叫什么名字",
                max_tokens = 100,
                temperature = 0,
                top_p = 1,
                frequency_penalty = 0f,
                presence_penalty = 0f,
                stop = @"['\n']"
            };
            GetPostData(_postData,"", (answer) =>
            {
                string result = answer;
            });
        }
        public IEnumerator GetPostData(ChatAIPostData chatAIPostData,string _postWord, System.Action<string> _callback)
        {
            var request = new UnityWebRequest(m_Api_Url, "POST");
            // ChatAIPostData _postData = new ChatAIPostData
            // {
            //     model = m_PostDataSetting.model,
            //     prompt = _postWord,
            //     max_tokens = m_PostDataSetting.max_tokens,
            //     temperature = m_PostDataSetting.temperature,
            //     top_p = m_PostDataSetting.top_p,
            //     frequency_penalty = m_PostDataSetting.frequency_penalty,
            //     presence_penalty = m_PostDataSetting.presence_penalty,
            //     stop = m_PostDataSetting.stop
            // };

            string _jsonText = JsonUtility.ToJson(chatAIPostData);
            byte[] data = System.Text.Encoding.UTF8.GetBytes(_jsonText);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(data);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", string.Format("Bearer {0}", m_OpenAI_Key));

            yield return request.SendWebRequest();

            if (request.responseCode == 200)
            {
                string _msg = request.downloadHandler.text;
                TextCallback _textback = JsonUtility.FromJson<TextCallback>(_msg);
                if (_textback != null && _textback.choices.Count > 0)
                {
                    _callback(_textback.choices[0].text);
                }

            }
        }

        private class TextCallback
        {
            public string id;
            public string created;
            public string model;
            public List<TextSample> choices;

            [System.Serializable]
            public class TextSample
            {
                public string text;
                public string index;
                public string finish_reason;
            }
        }
    }
    
    
    public class ChatAIPostData
    {
        public string model;
        public string prompt;
        public int max_tokens;
        public float temperature;
        public int top_p;
        public float frequency_penalty;
        public float presence_penalty;
        public string stop;
    }
}