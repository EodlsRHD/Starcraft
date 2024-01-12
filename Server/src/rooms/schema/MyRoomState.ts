import { Schema, Context, type } from "@colyseus/schema";

export class MyRoomState extends Schema {

  @type("string") SceneNumber: number = -1;

  @type("string") ownerID: string = "";

}
