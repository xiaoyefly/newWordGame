using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_WEBGL
public class WebLogin : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Web3Connect();

    [DllImport("__Internal")]
    private static extern string ConnectAccount();

    [DllImport("__Internal")]
    private static extern void SetConnectAccount(string value);

    private int expirationTime;
    private string account;

    private bool CheckConnect = false;
    public void OnLogin()
    {
        try
        {
            Web3Connect();
        }
        catch (Exception e)
        {
           
        }

        try
        {
            if (!CheckConnect)
            {
                OnConnected();
            }
        }
        catch (Exception e)
        {
         
        }
#if UNITY_EDITOR
        PlayerPrefs.SetString("Account", "0xb2875F6bdaAF5213E64380877871Ad25187678e7");
        // reset login message
        // SetConnectAccount("");
        // load next scene
        SceneManager.LoadScene("Main");
#endif
    }

    async private void OnConnected()
    {
        CheckConnect = true;
        account = ConnectAccount();
       
        while (account == "") {
            await new WaitForSeconds(1f);
            account = ConnectAccount();
        };
        CheckConnect = false;
        // save account for next scene
        PlayerPrefs.SetString("Account", account);
        // reset login message
        SetConnectAccount("");
        // load next scene
        SceneManager.LoadScene("Main");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OnSkip()
    {
        // burner account for skipped sign in screen
        PlayerPrefs.SetString("Account", "");
        // move to next scene
        SceneManager.LoadScene("Main");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
#endif
