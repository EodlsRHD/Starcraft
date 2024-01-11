import { Schema, model, connect, ObjectId, Model, Types, Document } from "mongoose";
import { Interface } from "readline";
import { v4 as uuidv4 } from 'uuid';

export interface ObjectData{
    _id : string,
    key : number,

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
        custom_1_id : number,

        hasCustom_2 : boolean,
        custom_2_key : number,
        custom_2_id : number,

        hasCustom_3 : boolean,
        custom_3_key : number,
        custom_3_id : number,

        hasCustom_4 : boolean,
        custom_4_key : number,
        custom_4_id : number,
    }

    metaData : {
        killCount : number,

        HpKey :number, 
        attackKey : number, 
        defenceKey : number, 
        shildKey : number, 
    
        currentHp : number, 
        currentShild : number, 
        currentEnergy : number, 
    
        upgradeAttack : number, 
        upgradedDefence : number, 
        upgradeShild : number, 
    
        isProduction :boolean, 
        productionUnitIDs : []
    }
}

export interface PlayerInfo{
    _id : string,

    ID : string,
    PW : string,

    nickName : string,
    brood : number,

    team : number,
    color : number,

    x : number,
    z : number,

    win : number,
    lose : number
}

export class MongoSchemas {
    private readonly ObjectDataSchema = new Schema<ObjectData>({
        _id : { type : String ,default: ()=> uuidv4() },
        key : { type : Number },

        objType : { type : Number },
        raceType : { type : Number },
        unitType : { type : Number },
        unitSizeType : { type : Number },
        unitAttackType : { type : Number },
        farAndNeer : { type : Number },

        name : { type : String },
        productionCode : { type : Number },

        isAir : { type : Boolean },
        maxHp : { type : Number },

        hasShild : { type : Boolean },
        maxShild : { type : Number },

        hasEnergy : { type : Boolean },
        maxEnergy : { type : Number },

        hasAttack : { type : Boolean },
        hasAirAttack : { type : Boolean },
        attack : { type : Number },
        attackRate : { type : Number },
        attackRange : { type : Number },

        defence : { type : Number },
        moveSpeed : { type : Number },

        custom : {
            hasCustom_1 : { type : Boolean },
            custom_1_key : { type : Number },
            custom_1_id : { type : String },

            hasCustom_2 : { type : Boolean },
            custom_2_key : { type : Number },
            custom_2_id : { type : String },

            hasCustom_3 : { type : Boolean },
            custom_3_key : { type : Number },
            custom_3_id : { type : String },

            hasCustom_4 : { type : Boolean },
            custom_4_key : { type : Number },
            custom_4_id : { type : String },
        },

        metaData : {
            custom_1_key : { type : Number },

            HpKey : { type : Number },
            attackKey : { type : Number },
            defenceKey : { type : Number },
            shildKey : { type : Number },

            currentHp : { type : Number },
            currentShild : { type : Number },
            currentEnergy : { type : Number },

            upgradeAttack : { type : Number },
            upgradedDefence : { type : Number },
            upgradeShild : { type : Number },

            isProduction : { type : Boolean },
            productionUnitIDs : { type : [String] },
        }
    }, {
        versionKey: false 
    });

    private readonly PlayerInfoSchma = new Schema<PlayerInfo>({
        _id : { type : String ,default: ()=> uuidv4() },

        ID : { type : String },
        PW : { type : String },

        nickName : { type : String },
        brood : { type : Number },

        team : { type : Number },
        color : { type : Number },

        x : { type : Number },
        z : { type : Number },

        win : { type : Number },
        lose : { type : Number },
    }, {
        versionKey: false 
    });

    public getObjectData() : Model<ObjectData, {}, {}> {
        return model<ObjectData>('ObjectData', this.ObjectDataSchema);
    }

    public getPlayerInfo() : Model<PlayerInfo, {}, {}> {
        return model<PlayerInfo>('PlayerInfo', this.PlayerInfoSchma);
    }
}

export default MongoSchemas;