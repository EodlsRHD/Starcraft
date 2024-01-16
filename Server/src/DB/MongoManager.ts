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
            let result = new model();
            
            for (let i = 0; i < datas.length; i++) {
                result.key = datas[i].key;

                result.objType = datas[i].objType;
                result.raceType = datas[i].raceType;
                result.unitType = datas[i].unitType;
                result.unitSizeType = datas[i].unitSizeType;
                result.unitAttackType = datas[i].unitAttackType;
                result.farAndNeer = datas[i].farAndNeer;

                result.name = datas[i].name;
                result.productionCode = datas[i].productionCode;

                result.isAir = datas[i].isAir;
                result.maxHp = datas[i].maxHp;

                result.hasShild = datas[i].hasShild;
                result.maxShild = datas[i].maxShild;

                result.hasEnergy = datas[i].hasEnergy;
                result.maxEnergy = datas[i].maxEnergy;

                result.hasAttack = datas[i].hasAttack;
                result.hasAirAttack = datas[i].hasAirAttack;
                result.attack = datas[i].attack;
                result.attackRate = datas[i].attackRate;
                result.attackRange = datas[i].attackRange;

                result.defence = datas[i].defence;
                result.moveSpeed = datas[i].moveSpeed;

                result.custom = datas[i].custom;
                result.metaData = datas[i].metaData;

                await result.save();
            }

            console.log("SetObjectDatas    " + datas.length);

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