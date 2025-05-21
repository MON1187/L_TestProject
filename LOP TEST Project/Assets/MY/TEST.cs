using UnityEngine;
using Mirror;
using System;
using TMPro;

public class TEST : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField = null; // ����ڰ� �Է��� �Է� �ʵ�
    [SerializeField] private GameObject canvas = null; // ä�� UI ĵ����

    [SerializeField] private static event Action<string> onMessage; // ���ο� �޽����� ���ŵ� �� ȣ��Ǵ� �̺�Ʈ

    public GameObject UI;
    public GameObject gameObject;

    public override void OnStartAuthority()
    {
        canvas.SetActive(true);

        onMessage += HandleNewMessage; // OnMessage �̺�Ʈ�� HandleNewMessage �޼��带 ����
    }

    // Ŭ���̾�Ʈ�� �������� ���� �� ȣ��
    [ClientCallback]
    private void OnDestroy()
    {
        onMessage -= HandleNewMessage; // OnMessage �̺�Ʈ���� HandleNewMessage �޼��� ���� ����
    }

    // ���ο� �޽����� �߰��� ��, ��ũ�� ���� �ؽ�Ʈ�� ������Ʈ�Ͽ� ���ο� �޽����� ����
    private void HandleNewMessage(string message)
    {
        GameObject newChat = Instantiate(new GameObject(), new Vector3(0,0,0), transform.rotation);
        TMP_Text chatText = newChat.GetComponent<TMP_Text>();
        chatText.text += message; // ���ο� �޽����� �߰�
    }

    // Ŭ���̾�Ʈ�� ���� ��ư�� ������ ��, �Է� �ʵ��� �޽����� ����
    [Client]
    public void Send()
    {
        // �Է� �ʵ尡 ��������� �����Ѵ�.
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }

        CmdSendMessage(inputField.text); // ������ �޽����� ����
        inputField.text = string.Empty;
    }

    // �������� ȣ��Ǿ� ��� Ŭ���̾�Ʈ�� �޽����� �����Ѵ�.
    [Command]
    private void CmdSendMessage(string message)
    {
        TESTPLAYER testPlayerName = GetComponent<TESTPLAYER>();  // �÷��̾� �̸��� �����´�. �÷��̾� �̸��� ����ȭ�� �Ǿ�� ����� �����߾���. 

        RpcHandleMessage($"[{testPlayerName.name}]: {message}"); // ��� Ŭ���̾�Ʈ���� �޽����� ����
    }

    [ClientRpc] // ��� Ŭ���̾�Ʈ���� �޽����� ó���Ѵ�.
    private void RpcHandleMessage(string message)
    {
        // 'OnMessage'�� null�� �ƴ� ���� 'Invoke()'�� ȣ��
        // 'Invoke()'�� OnMessage �̺�Ʈ�� ȣ���ϴ� �޼����̸�, 'Invoke()'�� �̺�Ʈ�� ��� �����ڿ��� �޽����� �����Ѵ�.
        onMessage?.Invoke($"{message}\n"); // OnMessage �̺�Ʈ�� ȣ���Ͽ� �޽����� ó���մϴ�.
    }

    void Update()
    {
        // ���� ��ư�� ������ �� �޽��� �߼����� ����
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Return)) // ���� �÷��̾ ���� Ű�� ������ ��
        {
            Send(); // �޽����� ������.
        }
    }
}
