import express, { request, Request, Response } from "express";
import { MongoManager } from "../db/mongoManager";

const router = express.Router();

export enum eResultCode {
    FAIL = -1,
    SUCCESSE = 0,
}

router.post("/signUp", async(req : Request, res : Response) => {
    let id = req.body.userID;
    let pw = req.body.userPW;
    let nickName = req.body.nickName;

    let isError = false;

    let playerInfo = await MongoManager.Instance().CreatePlayerInfo(id, pw, nickName).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/signUp Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE,  message : "", playerInfo : playerInfo});
});

router.post("/signIn", async(req : Request, res : Response) => {
    let id = req.body.userID;
    let pw = req.body.userPW;

    let isError = false;

    let isFound = await MongoManager.Instance().SignInPlayerInfo(id, pw).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/signIn Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE,  message : "", isFound : isFound});
});

router.post("/getPlayerInfo", async(req : Request, res : Response) => {
    let id = req.body.userID;
    let pw = req.body.userPW;

    let isError = false;

    let playerInfo = await MongoManager.Instance().GetPlayerInfo(id, pw).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getPlayerInfo Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : "", playerInfo : playerInfo});
});

router.post("/getPlayerInfo_ObjectID", async(req : Request, res : Response) => {
    let id = req.body._id;

    let isError = false;

    let playerInfo = await MongoManager.Instance().GetPlayerInfo_ObjectID(id).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getPlayerInfo_ObjectID Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : "", playerInfo : playerInfo});
});

router.post("/updatePlayerInfo", async(req : Request, res : Response) => {
    let id = req.body._id;
    let newInfo = req.body.newInfo;

    let isError = false;

    let playerInfo = await MongoManager.Instance().UpdatePlayerInfo(id, newInfo).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/updatePlayerInfo Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : "", playerInfo : playerInfo});
});

export default router;