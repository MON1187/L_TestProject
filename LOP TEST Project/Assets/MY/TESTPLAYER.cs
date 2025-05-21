using Mirror;
using System;
using UnityEngine;

public class TESTPLAYER : NetworkBehaviour
{
    int myInt = 0;
    string myStr = String.Empty;
    int i;

    public string name = "";

    public override void OnStartServer()
    {
        base.OnStartServer();

        // 서버에서 MyNetworkMessage를 수신 받을 경우 ReceiveMyNetworkMessageInServer를 호출
        // RegisterHandler: 기존 핸들러가 없을 때만 작동하며, 중복 등록을 시도하면 오류가 발생
        NetworkServer.RegisterHandler<MyNetworkMessage>(ReceiveInServer);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // 클라이언트에서 MyNetworkMessage를 수신 받을 경우 ReceiveMyNetworkMessageInClient를 호출
        // ReplaceHandler: 기존 핸들러가 있든 없든 상관없이 무조건 새로운 핸들러로 설정
        NetworkClient.ReplaceHandler<MyNetworkMessage>(ReceiveInClient);
    }

    private void ReceiveInServer(NetworkConnection conn, MyNetworkMessage message)
    {
        // 메시지를 처리할 코드
        i = message.i;
    }

    private void ReceiveInClient(MyNetworkMessage message)
    {
        // 메시지를 처리할 코드
        myStr = message.str;
    }

    public struct MyNetworkMessage : NetworkMessage
    {
        public int i;
        public string str;
    }
}
