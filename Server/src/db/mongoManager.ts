import MongoSchemas, { MapdataInfo, ObjectData, PlayerInfo, objectDataInfo } from "./MongoSchemas";

export class MongoManager{
    private static instance : MongoManager = null;
    private readonly schemas : MongoSchemas = new MongoSchemas();

    private connected : boolean = false;

    public static Instance() : MongoManager{
        if(this.instance == null)
        {
            this.instance = new MongoManager();
        }
        let a;
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

    public async UpdatePlayerInfo(id : string, newInfo : PlayerInfo) : Promise<PlayerInfo>{
        let promise = new Promise<PlayerInfo>(async(res, rej) => {
            let model = this.schemas.getPlayerInfoModel();
            let result = await model.findOne({_id : id});

            if(newInfo == null)
            {
                return;
            }

            if(newInfo.userID) result.userID = newInfo.userID;
            if(newInfo.userPW) result.userPW = newInfo.userPW;
            if(newInfo.nickName) result.nickName = newInfo.nickName;
            if(newInfo.win) result.win = newInfo.win;
            if(newInfo.lose) result.lose = newInfo.lose;

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

            for (let i = 0; i < datas.length; i++) {
                let result = new model();

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

                console.log("Upload name    " + result.name);

                await result.save();
            }

            console.log("SetObjectDatas    " + datas.length + "--------------------------------------------");

            res();
        });

        return promise;
    }

    public async GetObectDataInfos() : Promise<objectDataInfo[]>{
        let promise = new Promise<objectDataInfo[]>(async(res, rej) => {
            let model = this.schemas.getObjectDataInfoModel();
            let result = await model.find();

            res(result);
        });

        return promise;
    }

    public async SetObjectDataInfos(datas : objectDataInfo[]) : Promise<void>{
        let promise = new Promise<void>(async(res, rej) => {
            let model = this.schemas.getObjectDataInfoModel();
            
            for (let i = 0; i < datas.length; i++) {
                let result = new model();

                result.objectDataID = datas[i].objectDataID;

                result.name = datas[i].name;
                result.description = datas[i].description;
                result.useCondition = datas[i].useCondition;

                result.mineral = datas[i].mineral;
                result.gas = datas[i].gas;
                result.productionTime = datas[i].productionTime;
                result.energy = datas[i].energy;
                result.population = datas[i].population;

                console.log("Upload name    " + result.name);

                await result.save();
            }
            
            console.log("SetObjectDataInfos    " + datas.length + "===========================================");

            res();
        });

        return promise;
    }

    public async GetMapDataInfos() : Promise<MapdataInfo[]>{
        let promise = new Promise<MapdataInfo[]>(async(res, rej) => {
            let model = this.schemas.getMapDataInfoModel();
            let result = await model.find();

            res(result);
        });

        return promise;
    }

    public async AddMapDataInfos(newInfo : MapdataInfo) : Promise<void>{
        let promise = new Promise<void>(async(res, rej) => {
            let model = this.schemas.getMapDataInfoModel();

            let info = new model();
            info.name = newInfo.name;
            info.description = newInfo.description;
            info.version = newInfo.version;
            info.maker = newInfo.maker;
            info.maxPlayer = newInfo.maxPlayer;

            info.classification = newInfo.classification;
            info.teamCount = newInfo.teamCount;

            info.roomHostUuid = newInfo.roomHostUuid;
            info.members = newInfo.members;

            info.thumbnailPath = newInfo.thumbnailPath;

            info.mapSizeX = newInfo.mapSizeX;
            info.mapSizeY = newInfo.mapSizeY;

            info.fileDownloadUrl = newInfo.fileDownloadUrl;

            await info.save();

            res();
        });

        return promise;
    }
}