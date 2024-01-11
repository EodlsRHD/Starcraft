import express, { request, Request, Response } from "express";
import { eResultCode } from "../ResultCode";
import { JwtPayload, sign } from "jsonwebtoken";
import { VerifyToken, SECRET_KEY, REFRESH_TOKEN_LIFETIME, TOKEN_LIFETIME, RestoreToken} from "./jwtMiddle";
import { MongoManager } from "../DB/MongoManager";
import { FilterQuery } from "mongoose";
import { MyGallery, PlayerData, Project, ProjectNotice, ProjectSchedule, RoomScreenInfo, UserHouse } from "../DB/MongoSchemas";
import { Document } from "mongoose";
import { matchMaker, Room, RoomListingData } from "colyseus";

const router = express.Router();

// router.post(" ")...

export default router;
