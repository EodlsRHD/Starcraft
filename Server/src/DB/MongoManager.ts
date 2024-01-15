import MongoSchemas, { ObjectData, PlayerInfo, customData } from "./MongoSchemas";

export class MongoManager{
    private static instance : MongoManager = null;
    private readonly schemas : MongoSchemas = new MongoSchemas();

    private connected : boolean = false;

    public static Instance() : MongoManager{
        if(this.instance == null)
        {
            this.instance = new MongoManager();
        }

        return this.instance;
    }

    public async CreatePlayerInfo(id : string, pw : string, nickName : string) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();

            let result = new model();
            result.userID = id;
            result.userPW = pw;
            result.nickName = nickName;
            result.brood = 0;
            result.team = 0;
            result.color = 0;
            result.x = 0;
            result.z = 0;
            result.win = 0;
            result.lose = 0;

            await result.save();
 
            res(result);
        });

        return promise;
    }

    public async SignInPlayerInfo(id : string, pw : string) : Promise<boolean>{
        let promise = new Promise<boolean>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();

            let isFound = true;

            let result = await model.findOne({userID : id, userPW : pw});

            console.log("Sign In     " + result);

            if(result == null)
            {
                isFound = false;
                res(isFound);
            }
            
            res(isFound);
        });

        return promise;
    }

    public async GetPlayerInfo(id : string, pw : string) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({userID : id, userPW : pw});
            
            res(result);
        });

        return promise;
    }

    public async GetPlayerInfo_ObjectID(id : string) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({_id : id});

            res(result);
        });

        return promise;
    }

    public async UpdatePlayerInfo(id : string, newInfo : any) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({_id : id});

            result = newInfo;

            result.userID = newInfo.ID;
            result.userPW = newInfo.PW;
            result.nickName = newInfo.nickName;
            result.win = newInfo.win;
            result.lose = newInfo.lose;

            await result.save();

            res(result);
        });

        return promise;
    }

    public async GetObjectDatas() : Promise<ObjectData[]>{
        let promise = new Promise<ObjectData[]>(async(res, rej) => {
            let model = this.schemas.getObjectDataModel();
            let result = await model.find();

            res(result);
        });

        return promise;
    }

    public async SetObjectDatas(datas : ObjectData[]) : Promise<void>{
        let promise = new Promise<void>(async(res, rej) => {
            let model = this.schemas.getObjectDataModel();
            model.updateMany(datas);
            
            console.log(datas.length);

            res();
        });

        return promise;
    }

    public async getCustomDatas() : Promise<customData[]>{
        let promise = new Promise<customData[]>(async(res, rej) => {
            let model = this.schemas.getCustomData();
            let result = await model.find();

            res(result);
        });

        return promise;
    }

    public async SetCustomDatas(datas : customData[]) : Promise<void>{
        let promise = new Promise<void>(async(res, rej) => {
            let model = this.schemas.getCustomData();
            model.updateMany(datas);
            
            res();
        });

        return promise;
    }
}