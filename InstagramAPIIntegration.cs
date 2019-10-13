using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using MiniJSON;
public class InstagramAPIIntegration : MonoBehaviour
{
    public GameObject instagramGameobejctprefab;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FetchInstagramPicture());
    }

    IEnumerator FetchInstagramPicture()
    {
        string url = "https://api.instagram.com/v1/users/self/media/recent/?access_token=1189340012.924d248.5064087a8046a5ab3d8f47cab80293";

        using(UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.Log(webRequest.error);
            }
            else
            {
                string apiResponse = webRequest.downloadHandler.text;
                IDictionary apiParse = (IDictionary)Json.Deserialize(apiResponse);
             
                IList apiInstagramPicturesList = (IList)apiParse["data"];

                foreach (IDictionary instagramPicture in apiInstagramPicturesList)
                {
                    IDictionary images = (IDictionary)instagramPicture["images"];
                    IDictionary standardResolation = (IDictionary)images["standard_resolution"];
                    string mainPicUrl = (string)standardResolation["url"];

                    using (UnityWebRequest www = UnityWebRequestTexture.GetTexture(mainPicUrl))
                    {
                        yield return www.SendWebRequest();
                        if (webRequest.isNetworkError || webRequest.isHttpError)
                        {
                            Debug.Log(webRequest.error);
                        }
                        else
                        {
                            var texture = DownloadHandlerTexture.GetContent(www);
                            GameObject instagramPic = Instantiate(instagramGameobejctprefab);
                            instagramPic.transform.Find("mainPicture").GetComponent<MeshRenderer>().material.mainTexture = texture;
                        }
                    }
                }
            }
        }
    }
}
