import { Schema, model, connect, ObjectId, Model, Types, Document } from "mongoose";
import { v4 as uuidv4 } from 'uuid';

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
        custom_1_id : string,

        hasCustom_2 : boolean,
        custom_2_id : string,

        hasCustom_3 : boolean,
        custom_3_id : string,

        hasCustom_4 : boolean,
        custom_4_id : string
    },

    mataData : {
        isProduction : boolean,
        productionUnitIDs : [string]
    }
}

export interface PlayerInfo{
    _id : string,

    ID : string,
    PW : string,

    nickName : string,
    
    win : number,
    lose : number
}

export class MongoSchemas{

    private readonly objectDataSchema = new Schema<ObjectData>({
        _id : {type : String ,default: ()=> uuidv4()},

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
            custom_1_id :{type : String, required : false},

            hasCustom_2 :{type : Boolean, required : false}, 
            custom_2_id :{type : String, required : false},

            hasCustom_3 :{type : Boolean, required : false}, 
            custom_3_id :{type : String, required : false},

            hasCustom_4 :{type : Boolean, required : false}, 
            custom_4_id :{type : String, required : false},
        }, required : false},

        mataData : { type : {
            isProduction :{type : Boolean, required : false}, 
            productionUnitIDs :{type : [String], required : false},
        }, required : false}

    }, {
        versionKey : false
    });

    private readonly playerInfoSchema = new Schema<PlayerInfo>({
        _id : {type : String ,default: ()=> uuidv4()},

        ID :{type : String, required : false},
        PW :{type : String, required : false},

        nickName :{type : String, required : false},

        win :{type : Number, required : false}, 
        lose :{type : Number, required : false}, 
    }, {
        versionKey : false
    });

    public getObjectDataModel() : Model<ObjectData, {}, {}> {
        return model<ObjectData>('ObjectData', this.objectDataSchema);
    }
    
    public getPlayerInfoModel() : Model<PlayerInfo, {}, {}> {
        return model<PlayerInfo>('User', this.playerInfoSchema);
    }
}

export default MongoSchemas;