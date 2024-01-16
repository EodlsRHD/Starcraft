import { Schema, Context, type } from "@colyseus/schema";

export class MyRoomState extends Schema {

  @type("number") mapID: number = -1;

  @type("string") ownerID: string = "";

}
