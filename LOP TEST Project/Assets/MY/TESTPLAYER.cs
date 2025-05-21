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

        // �������� MyNetworkMessage�� ���� ���� ��� ReceiveMyNetworkMessageInServer�� ȣ��
        // RegisterHandler: ���� �ڵ鷯�� ���� ���� �۵��ϸ�, �ߺ� ����� �õ��ϸ� ������ �߻�
        NetworkServer.RegisterHandler<MyNetworkMessage>(ReceiveInServer);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        // Ŭ���̾�Ʈ���� MyNetworkMessage�� ���� ���� ��� ReceiveMyNetworkMessageInClient�� ȣ��
        // ReplaceHandler: ���� �ڵ鷯�� �ֵ� ���� ������� ������ ���ο� �ڵ鷯�� ����
        NetworkClient.ReplaceHandler<MyNetworkMessage>(ReceiveInClient);
    }

    private void ReceiveInServer(NetworkConnection conn, MyNetworkMessage message)
    {
        // �޽����� ó���� �ڵ�
        i = message.i;
    }

    private void ReceiveInClient(MyNetworkMessage message)
    {
        // �޽����� ó���� �ڵ�
        myStr = message.str;
    }

    public struct MyNetworkMessage : NetworkMessage
    {
        public int i;
        public string str;
    }
}
