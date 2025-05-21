using UnityEngine;
using Mirror;
using System;
using TMPro;

public class TEST : NetworkBehaviour
{
    [SerializeField] private TMP_InputField inputField = null; // 사용자가 입력할 입력 필드
    [SerializeField] private GameObject canvas = null; // 채팅 UI 캔버스

    [SerializeField] private static event Action<string> onMessage; // 새로운 메시지가 수신될 때 호출되는 이벤트

    public GameObject UI;
    public GameObject gameObject;

    public override void OnStartAuthority()
    {
        canvas.SetActive(true);

        onMessage += HandleNewMessage; // OnMessage 이벤트에 HandleNewMessage 메서드를 구독
    }

    // 클라이언트가 서버에서 나갈 때 호출
    [ClientCallback]
    private void OnDestroy()
    {
        onMessage -= HandleNewMessage; // OnMessage 이벤트에서 HandleNewMessage 메서드 구독 해제
    }

    // 새로운 메시지가 추가될 때, 스크롤 뷰의 텍스트를 업데이트하여 새로운 메시지를 포함
    private void HandleNewMessage(string message)
    {
        GameObject newChat = Instantiate(new GameObject(), new Vector3(0,0,0), transform.rotation);
        TMP_Text chatText = newChat.GetComponent<TMP_Text>();
        chatText.text += message; // 새로운 메시지를 추가
    }

    // 클라이언트가 엔터 버튼을 눌렀을 때, 입력 필드의 메시지를 전송
    [Client]
    public void Send()
    {
        // 입력 필드가 비어있으면 리턴한다.
        if (string.IsNullOrWhiteSpace(inputField.text)) { return; }

        CmdSendMessage(inputField.text); // 서버로 메시지를 전송
        inputField.text = string.Empty;
    }

    // 서버에서 호출되어 모든 클라이언트에 메시지를 전달한다.
    [Command]
    private void CmdSendMessage(string message)
    {
        TESTPLAYER testPlayerName = GetComponent<TESTPLAYER>();  // 플레이어 이름을 가져온다. 플레이어 이름이 동기화가 되었어서 사용이 가능했었다. 

        RpcHandleMessage($"[{testPlayerName.name}]: {message}"); // 모든 클라이언트에게 메시지를 전달
    }

    [ClientRpc] // 모든 클라이언트에서 메시지를 처리한다.
    private void RpcHandleMessage(string message)
    {
        // 'OnMessage'가 null이 아닐 때만 'Invoke()'를 호출
        // 'Invoke()'는 OnMessage 이벤트를 호출하는 메서드이며, 'Invoke()'는 이벤트의 모든 구독자에게 메시지를 전달한다.
        onMessage?.Invoke($"{message}\n"); // OnMessage 이벤트를 호출하여 메시지를 처리합니다.
    }

    void Update()
    {
        // 전송 버튼이 눌렸을 때 메시지 발송으로 변경
        if (isLocalPlayer && Input.GetKeyDown(KeyCode.Return)) // 로컬 플레이어가 엔터 키를 눌렀을 때
        {
            Send(); // 메시지를 보낸다.
        }
    }
}
