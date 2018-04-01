using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class AndroidTest : NetworkBehaviour {

//    private Transform canvasTras;//UGUI的Canvas  
//    private Text text;//用来显示 界面的文本  
//    private Button btn;//前端的按钮  

//    private AndroidJavaObject jo = null;

//    private string str;
//    private int int_ = 0;
//    private string test_str = ">";

//    private void Awake()
//    {
//#if UNITY_ANDROID
//        str = "这里是安卓设备^_^";
//#endif

//#if UNITY_IPHONE
//                str = "这里是苹果设备>_<";  
//#endif

//#if UNITY_STANDALONE_WIN
//                str = "我是从Windows的电脑上运行的T_T";  
//#endif
//    }

//    // Use this for initialization  
//    void Start()
//    {
//        //固定写法  
//        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
//        jo = jc.GetStatic<AndroidJavaObject>("currentActivity");//获取当前Activity对象，即Android中的MainActivity  

//        canvasTras = GameObject.Find("Canvas").transform;
//        text = canvasTras.Find("Text").GetComponent<Text>();
//        btn = canvasTras.Find("Button").GetComponent<Button>();

//        text.text = str;
//        test_str = ">";
//        btn.onClick.AddListener(Click);//按钮点击事件监听下面的Click()方法  
//    }

//    //这个方法是在界面点击按钮的时候调用，使用的是UGUI的BUTTON组件  
//    public void Click()
//    {
//        text.text = "";//点击的时候先清空  

//        //***  

//        test_str = test_str + "<";
//        string Mensaje = test_str;
//        CmdEnviar(Mensaje);

//        //int res = jo.Call<int>("add", 56, 100);//调用Android中的方法，"add"为方法名字，56，100，分别是想加的参数  
//        //text.text = "56 + 100的结果是:" + res.ToString();//显示前端  
//    }


//    //***  
//    [Command]
//    void CmdEnviar(string mensaje)
//    {
//        RpcRecivir(mensaje);

//    }

//    [ClientRpc]
//    public void RpcRecivir(string mensaje)
//    {
//        text = canvasTras.Find("Text").GetComponent<Text>();
//        text.text = mensaje;
//        //TxtTexto.text += ">>" + mensaje + "\n";  
//    }
}
