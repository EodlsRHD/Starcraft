import { matchMaker, Room, RoomListingData } from "colyseus";
import express, { Request, Response } from "express";
import { MongoManager } from "../DB/MongoManager";
import { eResultCode } from "../ResultCode";
import { MyGallery , RoomDB,Project , WorldDescription , WebBuild } from "../DB/MongoSchemas";
import { Document, Mongoose } from "mongoose";

const router = express.Router();

// private...


export default router;