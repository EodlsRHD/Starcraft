import express, { request, Request, Response } from "express";
import { MongoManager } from "../db/mongoManager";
import { matchMaker, Room, RoomListingData } from "colyseus";

const router = express.Router();

export enum eResultCode {
    FAIL = -1,
    SUCCESSE = 0,
}

router.post("/join", async(req : Request, res : Response) => {
    if(req.body == null){
        res.status(500).json({resultCode : eResultCode.FAIL});
        return;
    }

    console.log(req.body);

    let seatReservation = await matchMaker.joinOrCreate("my_room", { 
        mapID : req.body.mapID,
        isPrivate : req.body.isPrivate,
        name : req.body.name, 
        roomOwner : req.body.roomOwner 
    }).catch((reason) => 
    {
        console.log(reason);
    });

    console.log("GetReservation : " + seatReservation);

    res.status(200).json({
        resultCode : eResultCode.SUCCESSE,
        seatReservation : seatReservation
    });
});

router.post("/getMapDataInfos", async(req : Request, res : Response) => {
    let isError = false;

    let mapDataInfos = await MongoManager.Instance().GetMapDataInfos().catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getMapDataInfos Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : "", mapDataInfos : mapDataInfos});
});

router.post("/enterMapDataInfo", async(req : Request, res : Response) => {
    let id = req.body._id;
    let newInfo = req.body.newInfo;

    let isError = false;

    let playerInfo = await MongoManager.Instance().UpdatePlayerInfo(id, newInfo).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        //res.send({ resultCode : eResultCode.FAIL, message : "/updatePlayerInfo Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : ""});
});

router.post("/addMapDataInfo", async(req : Request, res : Response) => {
    let info = req.body.info;

    let isError = false;

    await MongoManager.Instance().AddMapDataInfos(info).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/addMapDataInfo Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, message : ""});
});

export default router;