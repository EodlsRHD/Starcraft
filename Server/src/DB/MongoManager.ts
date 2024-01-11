import mongoose ,{ Schema, model, connect, ObjectId, FilterQuery, Document } from "mongoose";
import { CachedGlobalValue, MongoSchemas, } from "./MongoSchemas";

export class MongoManager {

    private static instance : MongoManager = null;

    private connected : boolean = false;

    private readonly schemas : MongoSchemas = new MongoSchemas();

    private globalValue : CachedGlobalValue = null;
    
    public static Instance() : MongoManager {

        if(null == this.instance){
            console.log("Initialize MongoManager");
            this.instance = new MongoManager();
        }

        return this.instance;
    }
    
    public async GetGlobalValue(getNew : boolean = false) : Promise<CachedGlobalValue> {
        let promise = new Promise<CachedGlobalValue>(async(res, rej) => {

            if(getNew == false && this.globalValue != null){
                
                res(this.globalValue);
                return;
            }

            let globalValueModel = this.schemas.getGlobalValue();

            let info = await globalValueModel.findOne({});

            if(info == null){
                let value = new globalValueModel({
                    hubPicture : [],
                    featuredUsers : [],
                });

                await value.save();
                info = value;
            }

            console.log(info);

            this.globalValue = new CachedGlobalValue();
            this.globalValue.paintings = info.hubPicture;

            // info.featuredUsers?.forEach(async(e) => {

            //     let roomInfo : UserHouse & Document<any, any, UserHouse> = null;
            //     let isError = false;
                
            //     await this.GetPlayerRoomProfile(e).then(roomProfile => {
            //         roomInfo = roomProfile;
            //     }).catch((reason) => {
            //         console.error("getGlobalValue " + reason);
            //         isError = true;
            //     });

            //     if(isError != false || roomInfo == null){
            //         return;
            //     }

            //     this.globalValue.featuredUsers.push({ galleryThumbnail : roomInfo.thumbnailURL, userUUID : roomInfo.uuid });
            // });

            res(this.globalValue);
        });

        return promise;
    }
}