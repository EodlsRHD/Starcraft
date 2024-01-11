import { Room, Client } from "@colyseus/core";

import { MongoManager } from "../DB/MongoManager";
import { MyRoomState } from "./schema/MyRoomState";

import { IncomingMessage } from "http";

export class MyRoom extends Room<MyRoomState> {
  maxClients = 6;

  onCreate (options: any) {
    this.setState(new MyRoomState());

    this.onMessage("type", (client, message) => {
      //
      // handle "type" message
      //
    });
  }

  onAuth(client: Client, options: any, requset?: IncomingMessage){
    
  }

  onJoin (client: Client, options: any) {
    console.log(client.sessionId, "joined!");
  }

  onLeave (client: Client, consented: boolean) {
    console.log(client.sessionId, "left!");
  }

  onDispose() {
    console.log("room", this.roomId, "disposing...");
  }

}
