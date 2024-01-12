import MongoSchemas, { ObjectData, PlayerInfo } from "./MongoSchemas";

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

            console.log("CreatePlayerInfo");

            let result = new model();
            result.ID = id;
            result.PW = pw;
            result.nickName = nickName;
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

            console.log("SignInPlayerInfo");

            let isFound = true;

            let result = await model.findOne({ID : id, PW : pw}).catch((reason) => 
            {
                console.log("SignInPlayerInfo    " +  reason);
                isFound = false;
            });

            console.log(isFound);

            res(isFound);
        });

        return promise;
    }

    public async GetPlayerInfo(id : string, pw : string) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({ID : id, PW : pw});

            console.log("GetPlayerInfo");

            res(result);
        });

        return promise;
    }

    public async GetPlayerInfo_ObjectID(id : string) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({_id : id});

            console.log("GetPlayerInfo_ObjectID");

            res(result);
        });

        return promise;
    }

    public async UpdatePlayerInfo(id : string, newInfo : any) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({_id : id});

            result = newInfo;

            result.ID = newInfo.ID;
            result.PW = newInfo.PW;
            result.nickName = newInfo.nickName;
            result.win = newInfo.win;
            result.lose = newInfo.lose;

            console.log("UpdatePlayerInfo");

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

    public async SetObjectDatas(objectDatas : ObjectData[]) : Promise<void>{
        let promise = new Promise<void>(async(res, rej) => {
            let model = this.schemas.getObjectDataModel();
            model.updateMany(objectDatas);
            
            res();
        });

        return promise;
    }
}