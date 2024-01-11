import mongoose ,{ Schema, model, connect, ObjectId, FilterQuery, Document } from "mongoose";
import { MongoSchemas, ObjectData, PlayerInfo } from "./MongoSchemas";

export class MongoManager {

    private static instance : MongoManager = null;

    private connected : boolean = false;

    private readonly schemas : MongoSchemas = new MongoSchemas();
    
    public static Instance() : MongoManager {

        if(null == this.instance){
            console.log("Initialize MongoManager");
            this.instance = new MongoManager();
        }

        return this.instance;
    }

    public async createPlayerInfo(ID : string, PW : string, nickName : string): Promise<PlayerInfo>{
        let promise : Promise<PlayerInfo> = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfo();

            let newInfo = new model();
            newInfo.ID = ID;
            newInfo.PW = PW;
            newInfo.nickName = nickName;

            await newInfo.save();

            res(newInfo);
        });

        return promise;
    }

    public async getPlayerInfo(ID : string, PW : string) : Promise<PlayerInfo>{

        let promise : Promise<PlayerInfo> = new Promise<PlayerInfo>(async(res, rej) => {
            
            let model = this.schemas.getPlayerInfo();
            let result = await model.findOne({ ID : ID, PW : PW});

            res(result);
        });

        return promise;
    }

    public async getPlayerInfo_ObjectID(_id : string) : Promise<PlayerInfo>{

        let promise : Promise<PlayerInfo> = new Promise<PlayerInfo>(async(res, rej) => {
            
            let model = this.schemas.getPlayerInfo();
            let result = await model.findOne({ _id : _id});

            res(result);
        });

        return promise;
    }

    public async updatePlayerInfo(_id : string, newInfo : any) : Promise<PlayerInfo>{

        let promise : Promise<PlayerInfo> = new Promise<PlayerInfo>(async(res, rej) => {
            
            let model = this.schemas.getPlayerInfo();
            let result = await model.findOne({ _id : _id});

            result = newInfo;

            result.save();

            res(result);
        });

        return promise;
    }
    
    public async getObjectDatas() : Promise<ObjectData[]>{

        let promise : Promise<ObjectData[]> = new Promise<ObjectData[]>(async(res, rej) => {
            
            let model = this.schemas.getObjectData();
            let result = await model.find();

            res(result);
        });

        return promise;
    }
}