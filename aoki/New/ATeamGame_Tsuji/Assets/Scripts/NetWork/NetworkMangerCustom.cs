using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

public class NetworkMangerCustom : NetworkManager
{
    public static void NetworkGame()
    {
        //singleton.StartCoroutine((singleton as NetworkMangerCustom).DiscoveryNetwork());
        singleton.StartMatchMaker();                                                                                                    //Unetの使用対戦開始
        singleton.matchMaker.ListMatches(0,20," ",false,0,0,singleton.OnMatchList);
        // 引数１：startpagenumber:何ページのリスト表示
        // 引数２：one pageがいくつかある
        // 引数３：必要のルームの名前
        // 引数４：パスワード付き部屋を戻るか
        // 引数５：？？？
        // 引数６：地域値でルームを戻る
        // 引数７：戻り関数
    }

    public override void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        if (!success) return;

        if(matchList!=null)
        {
            List<MatchInfoSnapshot> availableMatches = new List<MatchInfoSnapshot>();
            foreach(MatchInfoSnapshot match in matchList)
            {
                if(match.currentSize<match.maxSize)
                {
                    availableMatches.Add(match);                                                            //プレイヤー＜4人時、プレイヤー追加
                }
            }

            //List＝０か、もし＝０、サーバ作る、もし！＝０、サーバに入る
            if(availableMatches.Count==0)
            {
                //サーバをクリエイト
                CreateMatch();
            }
            else
            {
                //サーバに入る
                matchMaker.JoinMatch(availableMatches[Random.Range(0,availableMatches.Count-1)].networkId," "," "," ",0,0,OnMatchJoined);
            }
        }
    }

    /// <summary>
    /// サーバをクリエイト（Unetにサーバ一つ申し込み）
    /// </summary>
    void CreateMatch()
    {
        matchMaker.CreateMatch(" ",matchSize,true," "," "," ",0,0,OnMatchCreate);
        //引数１：ルームの名前
        //引数２：最大プレイヤー数
        //引数３：
        //引数４：命令
        //引数５：公的のクライアントIPアドレス
        //引数６：私的のクライアントIPアドレス
    }

    public override void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (!success) return;
        StartHost(matchInfo);                                                        //Unetでサーバを作る
    }

    public override void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
    {
        if (!success)
        {
            int currentScenes = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentScenes);
            return;
        }
        StartClient(matchInfo);                                                     //Unetでクライアント起動
    }

    IEnumerator DiscoveryNetwork()
    {
        //Discovery COMを取得
        NetworkDiscoverCustom discovery = GetComponent<NetworkDiscoverCustom>();
        discovery.Initialize();                                                                                                                    //ネームワーク初期化
        discovery.StartAsClient();                                                                                                           //ネームワークに入る
        yield return new WaitForSeconds(2F);

        //もしネットワーク探してない場合は、サーバを立てる
        if(discovery.running)
        {
            discovery.StopBroadcast();
            yield return new WaitForSeconds(0.5F);

            discovery.StartAsServer();                                                                                                      //自分でサーバになる 
            StartHost();                                                                                                                                //サーバとクライアント同時に
            //StartClient();                                                                                                                              //クライアントだけとして起動
            //StartServer();                                                                                                                             //サーバだけとして起動     
        }
    }


    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        GameObject player = Instantiate(playerPrefab);
        player.gameObject.GetComponent<Ctrl.LocalPlayerManeger>().playerNum = GameManger.GetInstance().GetPlayerFill();

        GameManger.GetInstance().size[player.gameObject.GetComponent<Ctrl.LocalPlayerManeger>().playerNum]++;
        Debug.Log(player.gameObject.GetComponent<Ctrl.LocalPlayerManeger>().playerNum);
        GameManger.GetInstance().UI.OnPlayerSizeChanged(SyncListInt.Operation.OP_DIRTY, player.gameObject.GetComponent<Ctrl.LocalPlayerManeger>().playerNum);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        
        
    }

    private void OnApplicationQuit()
    {
        StopHost();
        StopClient();   
    }
}
