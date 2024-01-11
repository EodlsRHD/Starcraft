import express, { request, Request, Response } from "express";
import { eResultCode } from "../ResultCode";
import { JwtPayload, sign } from "jsonwebtoken";
import { VerifyToken, SECRET_KEY, REFRESH_TOKEN_LIFETIME, TOKEN_LIFETIME, RestoreToken} from "./jwtMiddle";
import { MongoManager } from "../DB/MongoManager";
import { FilterQuery } from "mongoose";
import { PlayerInfo } from "../DB/MongoSchemas";
import { Document } from "mongoose";
import { matchMaker, Room, RoomListingData } from "colyseus";

const router = express.Router();

router.post("/signUp", async (req : Request, res : Response) => 
{
    let ID = req.body.ID;
    let Pw = req.body.PW;
    let nickName = req.body.nickName;

    let isError : boolean = false;

    let result = await MongoManager.Instance().createPlayerInfo(ID, Pw, nickName).catch((reason) => {
        console.error(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, playerInfo : result});
});

router.post("/getPlayerInfo", async (req : Request, res : Response) => 
{
    let ID = req.body.ID;
    let Pw = req.body.PW;

    let isError : boolean = false;

    let result = await MongoManager.Instance().getPlayerInfo(ID, Pw).catch((reason) => {
        console.error(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, playerInfo : result});
});

router.post("/getPlayerInfo_ObjectID", async (req : Request, res : Response) => 
{
    let _id = req.body._id;

    let isError : boolean = false;

    let result = await MongoManager.Instance().getPlayerInfo_ObjectID(_id).catch((reason) => {
        console.error(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, playerInfo : result});
});

router.post("/updatePlayerInfo", async (req : Request, res : Response) => 
{
    let _id = req.body._id;
    let newInfo = req.body.newInfo;

    let isError : boolean = false;

    let result = await MongoManager.Instance().updatePlayerInfo(_id, newInfo).catch((reason) => {
        console.error(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, playerInfo : result});
});

router.post("/getObjectDatas", async (req : Request, res : Response) => 
{
    let isError : boolean = false;

    let result = await MongoManager.Instance().getObjectDatas().catch((reason) => {
        console.error(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, objectDatas : result});
});


export default router;
