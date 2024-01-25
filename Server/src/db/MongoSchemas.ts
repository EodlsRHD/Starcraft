import { Schema, model, connect, ObjectId, Model, Types, Document } from "mongoose";
import { v4 as uuidv4 } from 'uuid';

export interface MapdataInfo{
    _id : string,

    name : string,
    description : string,
    version : string,
    maker : string,
    maxPlayer : number,

    classification : number,
    teamCount : number,

    roomHostUuid : string,
    members : [string],

    thumbnailPath : string,

    mapSizeX : number,
    mapSizeY : number,

    fileDownloadUrl : string
}

export interface ObjectData{
    _id : string,
    key: number,

    objType : number,
    raceType : number,
    unitType : number,
    unitSizeType : number,
    unitAttackType : number,
    farAndNeer : number,

    name : string,
    productionCode : number,

    isAir : boolean,
    maxHp : number,

    hasShild : boolean,
    maxShild : number,

    hasEnergy : boolean,
    maxEnergy : number,

    hasAttack : boolean,
    hasAirAttack : boolean,
    attack : number,
    attackRate : number,
    attackRange : number,

    defence : number,
    moveSpeed : number,

    custom : {
        hasCustom_1 : boolean,
        custom_1_key : number,
        custom_1_id : string,

        hasCustom_2 : boolean,
        custom_2_key : number,
        custom_2_id : string,

        hasCustom_3 : boolean,
        custom_3_key : number,
        custom_3_id : string,

        hasCustom_4 : boolean,
        custom_4_key : number,
        custom_4_id : string
    },

    metaData : {
        killCount : number,

        HpKey : number,
        attackKey : number,
        defenceKey : number,
        shildKey : number,

        currentHp : number,
        currentShild : number,
        currentEnergy : number,

        upgradeAttack : number,
        upgradedDefence : number,
        upgradeShild : number,

        isProduction : boolean,
        productionUnitIDs : [string]
    }
}

export interface PlayerInfo{
    _id : string,

    userID : string,
    userPW : string,

    nickName : string,
    brood : number,

    team : number,
    color : number,

    x : number,
    z : number,
    
    win : number,
    lose : number
}

export interface objectDataInfo{
    _id : string,
    orderKey : number,

    name : string,
    description : string,
}

export class MongoSchemas{

    private readonly mapDataInfoSchema = new Schema<MapdataInfo>({
        _id : {type : String, default : ()=> uuidv4()},
        
        name :{type : String, required : false}, 
        description :{type : String, required : false}, 
        version :{type : String, required : false}, 
        maker :{type : String, required : false}, 
        maxPlayer :{type : Number, required : false}, 

        classification :{type : Number, required : false}, 
        teamCount :{type : Number, required : false}, 

        roomHostUuid :{type : String, required : false}, 
        members :{type : [String], required : false}, 

        thumbnailPath :{type : String, required : false}, 

        mapSizeX :{type : Number, required : false}, 
        mapSizeY :{type : Number, required : false}, 

        fileDownloadUrl :{type : String, required : false}
    });

    private readonly objectDataSchema = new Schema<ObjectData>({
        _id : {type : String, default : ()=> uuidv4()},
        key :{type : Number, required : true}, 

        objType :{type : Number, required : true}, 
        raceType :{type : Number, required : true}, 
        unitType :{type : Number, required : true}, 
        unitSizeType :{type : Number, required : true}, 
        unitAttackType :{type : Number, required : true}, 
        farAndNeer :{type : Number, required : true}, 

        name :{type : String, required : false}, 
        productionCode :{type : Number, required : false}, 

        isAir :{type : Boolean, required : false}, 
        maxHp :{type : Number, required : false}, 

        hasShild :{type : Boolean, required : false}, 
        maxShild :{type : Number, required : false}, 

        hasEnergy :{type : Boolean, required : false}, 
        maxEnergy :{type : Number, required : false}, 

        hasAttack :{type : Boolean, required : false}, 
        hasAirAttack :{type : Boolean, required : false}, 
        attack :{type : Number, required : false}, 
        attackRate :{type : Number, required : false}, 
        attackRange :{type : Number, required : false}, 

        defence :{type : Number, required : false}, 
        moveSpeed :{type : Number, required : false}, 

        custom : { type : {
            hasCustom_1 :{type : Boolean, required : false}, 
            custom_1_key :{type : Number, required : false}, 
            custom_1_id :{type : String, required : false},

            hasCustom_2 :{type : Boolean, required : false}, 
            custom_2_key :{type : Number, required : false}, 
            custom_2_id :{type : String, required : false},

            hasCustom_3 :{type : Boolean, required : false}, 
            custom_3_key :{type : Number, required : false}, 
            custom_3_id :{type : String, required : false},

            hasCustom_4 :{type : Boolean, required : false}, 
            custom_4_key :{type : Number, required : false}, 
            custom_4_id :{type : String, required : false},
        }, required : false},

        metaData : { type : {
            killCount :{type : Number, required : false}, 

            HpKey :{type : Number, required : false}, 
            attackKey :{type : Number, required : false}, 
            defenceKey :{type : Number, required : false}, 
            shildKey :{type : Number, required : false}, 

            currentHp :{type : Number, required : false}, 
            currentShild :{type : Number, required : false}, 
            currentEnergy :{type : Number, required : false}, 

            upgradeAttack :{type : Number, required : false}, 
            upgradedDefence :{type : Number, required : false}, 
            upgradeShild :{type : Number, required : false}, 

            isProduction :{type : Boolean, required : false}, 
            productionUnitIDs :{type : [String], required : false},
        }, required : false}

    }, {
        versionKey : false
    });

    private readonly playerInfoSchema = new Schema<PlayerInfo>({
        _id : {type : String, default : ()=> uuidv4()},

        userID :{type : String, required : true},
        userPW :{type : String, required : true},

        nickName :{type : String, required : true},
        brood :{type : Number, required : false}, 

        team :{type : Number, required : false}, 
        color :{type : Number, required : false}, 

        x :{type : Number, required : false}, 
        z :{type : Number, required : false}, 

        win :{type : Number, required : true}, 
        lose :{type : Number, required : true}, 
    }, {
        versionKey : false
    });

    private readonly objectDataInfoSchema = new Schema<objectDataInfo>({
        _id :{type : String, default : () => uuidv4()},
        orderKey :{type : Number, required : false}, 

        name :{type : String, required : true},
        description :{type : String, required : true},
    });

    public getMapDataInfoModel() : Model<MapdataInfo, {}, {}> {
        return model<MapdataInfo>('mapDataInfo', this.mapDataInfoSchema);
    }

    public getObjectDataModel() : Model<ObjectData, {}, {}> {
        return model<ObjectData>('ObjectData', this.objectDataSchema);
    }
    
    public getPlayerInfoModel() : Model<PlayerInfo, {}, {}> {
        return model<PlayerInfo>('User', this.playerInfoSchema);
    }

    public getObjectDataInfoModel() : Model<objectDataInfo, {}, {}> {
        return model<objectDataInfo>('ObjectDataInfo', this.objectDataInfoSchema);
    }
}

export default MongoSchemas;