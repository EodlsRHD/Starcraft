import express, { request, Request, Response } from "express";
import { MongoManager } from "../db/mongoManager";

const router = express.Router();

export enum eResultCode {
    FAIL = -1,
    SUCCESSE = 0,
}

router.post("/getObjectDatas", async(req : Request, res : Response) => {
    let isError = false;

    let result = await MongoManager.Instance().GetObjectDatas().catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getObjectDatas Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, objectDatas : result});
});

router.post("/setObjectDatas", async(req : Request, res : Response) => {
    let objectDatas = req.body.objectDatas;

    let isError = false;

    let result = await MongoManager.Instance().SetObjectDatas(objectDatas).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getObjectDatas Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, objectDatas : result});
});

router.post("/getObjectDataInfos", async(req : Request, res : Response) => {
    let isError = false;

    let result = await MongoManager.Instance().GetObectDataInfos().catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/getCustomDatas Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, objectDatas : result});
});

router.post("/setObjectDataInfos", async(req : Request, res : Response) => {
    let customDatas = req.body.customDatas;

    let isError = false;

    let result = await MongoManager.Instance().SetObjectDataInfos(customDatas).catch((reason) => {
        console.log(reason);
        isError = true;
    });

    if(isError != false)
    {
        res.send({ resultCode : eResultCode.FAIL, message : "/SetCustomDatas Error"});
        return;
    }

    res.send({ resultCode : eResultCode.SUCCESSE, objectDatas : result});
});
export default router;