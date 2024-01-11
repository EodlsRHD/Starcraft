import { Schema, Context, type } from "@colyseus/schema";
import { Room, Client } from "colyseus";

export class MyRoomState extends Schema {

  @type("number") worldNumber : number = -1;

  @type("string") roomOwner : string = "";

  @type("string") roomID : string = "";

  public players : Player[] = [];
}

export class Player extends Schema { 
  @type("number") userID : number = -1;
  @type("number") userPosX : number = 0;
  @type("number") userPosY : number = 0;
  @type("number") userPosZ : number = 0;

  @type("boolean") isMaster : boolean = false;

  public sessionID : string = "";

  public loadDone : boolean = false;

  public client : Client = null;

  @type("string") public uuid : string = "";
}
