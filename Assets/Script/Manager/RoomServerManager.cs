using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class RoomServerManager : MonoBehaviour
{
    private static RoomServerManager _instance = null;

    public static RoomServerManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public Colyseus.ColyseusRoom<MyRoomState> _room = null;

    private void Start()
    {
        _instance = this;
    }

    public async void Initialize(Colyseus.ColyseusRoom<MyRoomState> room)
    {
        if (_room != null)
        {
            await Disconnect();
        }

        this._room = room;

        _room.OnLeave += (code) =>
        {
            switch (code)
            {
                case 1006:
                case 4000:
                    //invalidated player data send to Login Scene
                    InterfaceManager.instance.OpenOneButton("Connection Error", "The connection to the server has been lost. Exit the application.", () =>
                    {
                        InterfaceManager.instance.ClosePopUp();
                    });
                    break;
                case 4001:
                    //kicked by project owner
                    InterfaceManager.instance.OpenOneButton("KICKED CREATOR", "You have been expelled from the current project.", () =>
                    {
                        InterfaceManager.instance.ClosePopUp();
                    });
                    break;
            }
        };

        //_room.OnMessage<RES_USER_JOIN>("USER_JOIN", OnUserJoin);
        //_room.OnMessage<RES_ROOM_INFO>("ROOM_INFO", OnRoomInfo);
        //_room.OnMessage<RES_LEAVE_USER>("LEAVE_USER", OnLeaveUser);
        //_room.OnMessage<RES_USER_MOVE>("USER_MOVE", OnUserMove);
        //_room.OnMessage<RES_CREATE_PORTAL>("CREATE_PORTAL", OnCreatePortal);
        //_room.OnMessage<RES_REMOVE_PORTAL>("REMOVE_PORTAL", OnRemovePortal);
        //_room.OnMessage<RES_LOAD_DONE>("LOAD_DONE", OnLoadDone);
        //_room.OnMessage<RES_INTERACT_OBJECT_SYNC>("INTERACT_OBJECT_SYNC", OnInteractObjectSync);
        //_room.OnMessage<RES_ANIM_SYNC>("ANIM_SYNC", OnAnimSync);
        //_room.OnMessage<REQ_SYNC_BUILD>("BUILD_SYNC", OnBuildSync);
        //_room.OnMessage<REQ_SYNC_SCREEN_INFO>("SCREEN_INFO_SYNC", OnscreenInfo);
        //_room.OnMessage<RES_CHAT_SYNC>("CHAT_SYNC", OnChatSync);
        //_room.OnMessage<RES_INVENTORY_SYNC>("INVENTORY_SYNC", OnInventorySync);
        //_room.OnMessage<REQ_SYNC_PAINTING>("PAINTING_SYNC", OnPaintingSync);
        //_room.OnMessage<RES_WHISPER_SYNC>("WHISPER_SYNC", OnWhisperSync);
        //_room.OnMessage<RES_CHAT_ROOM_SYNC>("CHAT_ROOM_SYNC", OnChatRoomSync);
        //_room.OnMessage<RES_CATTING_SYNC>("CHATTING_SYNC", OnChattingSync);

        //_room.OnMessage<RES_PROJECT_BRIEFINGBOARD_ACTIVATION>("PROJECT_BRIEFINGBOARD_ACTIVATION_REQUEST", OnBriefingBoardActivion);
        //_room.OnMessage<RES_PROJECT_BRIEFINGBOARD_UPDATE>("PROJECT_BRIEFINGBOARD_UPDATE_REQUEST", OnBriefingBoardUpdate);
        //_room.OnMessage<RES_PROJECT_BRIEFINGBOARD_LAYERUPDATE>("PROJECT_BRIEFINGBOARD_DRAWLAYERUPDATE_REQUEST", OnBriefingBoardDrawLayerUpdate);
        //_room.OnMessage<RES_PROJECT_BRIEFINGBOARD_PAGE>("PROJECT_BRIEFINGBOARD_PAGE_REQUEST", OnBriefingBoardPage);
    }

    public async Task Disconnect()
    {
        await _room.Leave();
        _room = null;
    }
}
