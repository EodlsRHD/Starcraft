import { BooleanModel } from "aws-sdk/clients/gamelift";
import { NumberList } from "aws-sdk/clients/iot";
import { bool } from "aws-sdk/clients/redshiftdata";
import { Schema, model, connect, ObjectId, Model, Types, Document } from "mongoose";
import CryptoJS from "crypto-js";
import { v4 as uuidv4 } from 'uuid';

//#region Legacy

export interface UserHouse{
    uuid : string,
    structJson : string,
    roomName : string,
    description : string,
    tag : string,
    thumbnailURL : string,
    paintingJson : string,
    roomScreenInfo : {
        state : number,
        imageURL : string,
        youtubeURL : string,
        advertiseInfo : {
            imageURL : string,
            title : string,
            description : string
        }
    },
    wallpaperPreset : number
}
export interface Inventory{
    linkID : string,
    itemCount : number,
    itemList : [{               
        itemID: number,
        itemUUID: number,
        isInUse: boolean,
        createDate: string,
        durability: number,
        isDestroyed: boolean}]
}

export interface PlayerData{
    linkID : number,
    nickName : string, 
    characterType : number,
    customCharacterUrl : string,
    favoriteContents : number,
    profilePicture : {
        url : string,
        key : string
    },
    tag : string
}

export interface Painting{
    uid : number,
    owner : string,
    url : string,
    key : string,
    title : string,
    description : string,
    link : string,
}

export interface GlobalValue{
    hubPicture : number[],
    featuredUsers : string[],
}

export interface OldProject {
    _id:ObjectId,
    owner : string,
    title : string,
    description : string,
    coverImageURL : string,
    isPublic : boolean,
    tag : string[],
    maxUser : number,
    invitedUser : string[],
    authorizedUser : string[],
    projectNotice : [{
        id : number,
        title : string,
        description : string,
        date : number,
        status : Number
    }],
    projectSchedule : [{
        date : number,
        title : string,
        contents : string
    }],

    projectBriefing : [{
        state : number,
        title : string,
        ownerName : string,
        startTime : number,
        endTime : number,
    
        sttList : [{
            name : string,
            content : string,
            time : number,
        }]
    }],
}

export class CachedGlobalValue {
    public paintings : number[] = [];
    public featuredUsers : {
        userUUID : string, 
        galleryThumbnail : string
    }[] = [];
}

export interface MessageBox {
    _id : string,
    owner : string,
    messages : Message[],
    uidRef : number
}

export class Message{

    public id : number;
    public messageType : number;
    public senderUID : string;
    public senderName : string;
    public content : string;
    public createdDate : number;
    public projectID : string;
}

export interface FriendList {
    owner : string,
    friends : string[]
}

export interface OldWorldDescription{
    ownerID : string,
    ownerName : string,
    bundleName : string,
    worldName : string,
    worldDescription : string,
    worldID : number,
    buildType : number,
    createdDate : number,
    uploadedDate : number,
    thumbnailURL : string,
    isVerified : boolean,
}

// export interface Featured {
//     url : string,
//     billboardurl : string,

//     userID : string,
//     type1 : number,
//     type2 : number
// }

export interface UserSuccessLog{
    userid : string,
    date : string,
    action : string,
    json : string
}

export interface UserActionLog{
    userid : string,
    date : string,
    action : string,
    json : string
}

//#endregion

//#region New

export interface RoomDB{
    _id : string,
    title : string,
    description :string,
    ownerID : string,
    thumbnailURL : string,
    createAt : number,
    updateAt : number,
    worldNumber : number,
    tag : string[],
    featuredID : string,
    reportIDs : string[],
    isPublic : boolean,
    isDelete : Boolean,
    deleteAt : number,
    content : MyGallery | Project | WorldDescription | WebBuild;
}

export class MyGallery{
    public structJson : string;
    public paintingJson : string;
    public wallpaperPreset : number;
    public roomScreenInfo : RoomScreenInfo;
    constructor(structJson : string, paintingJson : string,wallpaperPreset: number,
        roomScreenInfo: RoomScreenInfo){
        this.structJson = structJson;
        this.paintingJson = paintingJson;
        this.wallpaperPreset = wallpaperPreset;
        this.roomScreenInfo = roomScreenInfo;
    }
}

export class RoomScreenInfo{
    public state : number;
    public imageURL : string;
    public youtubeURL : string;
    constructor(state : number, imageURL : string,youtubeURL: string){
        this.state = state;
        this.imageURL = imageURL;
        this.youtubeURL = youtubeURL;
    }
}

export class Project{
    public maxUser : number;
    public invitedUser : string[] =[];
    public authorizedUser : string[] =[];
    public notice : ProjectNotice[];
    public schedule : ProjectSchedule[];
    public briefingBoardID : string;
    constructor(maxUser : number, invitedUser : string[],authorizedUser: string[],
        notice: ProjectNotice[],schedule :ProjectSchedule[],briefingBoardID:string){
        this.maxUser = maxUser;
        this.invitedUser = invitedUser;
        this.authorizedUser = authorizedUser;
        this.notice = notice;
        this.schedule = schedule;
        this.briefingBoardID = briefingBoardID;
    }
}

export class ProjectNotice{
    public index : number;
    public title : string;
    public description : string;
    public createAt : number;
    public updateAt : number;
    public writerID : string;
    public isDelete : bool;
    public deleteAt : number;
    public status : number;
    constructor(index : number, title : string, description: string,createAt: number, 
        updateAt :number,writerID:string,isDelete: bool,deleteAt:number,status:number){
        this.index = index;
        this.title = title;
        this.description = description;
        this.createAt = createAt;
        this.updateAt = updateAt;
        this.writerID = writerID;
        this.isDelete = isDelete;
        this.deleteAt = deleteAt;
        this.status = status;
    }
}

export class ProjectSchedule{
    public title : string;
    public description : string;
    public createAt : number;
    public updateAt : number;
    public isDelete : bool;
    public deleteAt : number;
    public writerID : string;    
    constructor(title : string, description: string,createAt: number, updateAt :number,
        writerID:string,isDelete: bool,deleteAt:number){
        this.title = title;
        this.description = description;
        this.createAt = createAt;
        this.updateAt = updateAt;
        this.isDelete = isDelete;
        this.deleteAt = deleteAt;
        this.writerID = writerID;
    }
}

export interface ProjectStorage{
    linkID : string,
    files : [{
        name : string,
        description : string,
        url : string,
        key : string,
        fileType : number,
        uploadedDate : number,
        uploaderID : string,
        fileName : string,
    }]
}

export class WorldDescription{
    public bundleURL : string;
    public bundleType : number;
    public seed: number;
    public isVerified : bool;
    constructor(bundleURL : string, bundleType : number,seed: number,
        isVerified: bool){
        this.bundleURL = bundleURL;
        this.bundleType = bundleType;
        this.seed = seed;
        this.isVerified = isVerified;
    }
}

export class WebBuild{
    public bundleURL : string;
    public seed: number;
    public isBlock : bool;
    public portals : Portal[];
    constructor(bundleURL : string,seed: number, isBlock: bool,portals : Portal[]){
        this.bundleURL = bundleURL;
        this.seed = seed;
        this.isBlock = isBlock;
        this.portals = portals;
    }
}

export class Portal{
    public x : number;
    public y : number;
    public z : number;
    public name : string;
    public to : string;
    constructor(x : number,y: number, z: number,name : string,to:string){
        this.x = x;
        this.y = y;
        this.z = z;
        this.name = name;
        this.to = to;
    }
}

export interface Featured{
    thumbnailURL : string,
    externalURL : string,
    screenType : number,
    worldNumber : number,
    ownerID : string,
    roomID : string,
    createAt : number,
    updateAt : number
}

export interface Report{
    reportType : number,
    reporterID : string,
    suspectID : string,
    reason : string,
    createAt : string,
    updateAt : string,
    rejectionReasons : string,
    status : number
}

export interface BriefingBoard{
    _id : string,
    state : number,
    title : string,
    ownerName : string,
    startTime : number,
    endTime : number,
    dataJsonPath : string,
    lastModifiedID : string,
    createAt : number,
    updateAt : number,
    isDelete : bool,
    deleteAt : number,
    ProjectID : string
}

export class BriefingPageData{
    public fieldname : string;
    public originalname : string;
    public encoding : string;
    public mimetype : string;
    public buffer : buffer;
}

export class buffer{
    public _bsontype : string;
    public sub_type : number;
    public position : number;
    public buffer : Buffer;
}

export interface FileStorage{
    _id : string,
    filename : string,
    url : string,
    fileextension : string,
    uploaderID : string,
    filesize : number,
    createAt : number,
    updateAt : number,
    where :  string[]
}

//#endregion

//#region Legacy

export class MongoSchemas {
    private readonly userHouseSchema = new Schema<UserHouse>({
        _id : {type : String ,default: ()=> uuidv4()},
        uuid :{type : String, required : true}, 
        structJson : { type : String, required : false},
        roomName : { type : String, required : true},
        description : { type : String, required : false},
        tag : { type : String, required : false},
        thumbnailURL: { type : String, required : false},
        paintingJson : { type : String, required : false },
        roomScreenInfo : { type : {
            state : Number,
            imageURL : String,
            youtubeURL : String,
            advertiseInfo : {
                imageURL : String,
                title : String,
                description : String
            }
        }, required : false},
        wallpaperPreset : { type : Number, required : false}
    }, {
        versionKey : false
    });

    private readonly inventorySchema = new Schema<Inventory>({
        _id : {type : String ,default: ()=> uuidv4()},
        linkID : { type : String, required : true},
        itemCount : { type : Number, required : true},
        itemList : { type : [{                
            itemID: Number,
            itemUUID: Number,
            isInUse: Boolean,
            createDate: String,
            durability: Number,
            isDestroyed: Boolean}], required : true }
    }, { 
        versionKey : false
    });

    private readonly playerDataSchema = new Schema<PlayerData>({
        _id : {type : String ,default: ()=> uuidv4()},
        linkID: { type : Number, required : true },
        nickName: { type : String, required : true },
        characterType: { type : Number, required : true },
        customCharacterUrl : { type : String, required : false},
        favoriteContents: { type : Number, required : true },
        profilePicture : { type : {
            url : String,
            key : String
        }, required : false},
        tag : { type : String, required : false}
    }, {
        versionKey : false
    });

    private readonly frameDataSchema = new Schema<Painting>({
        _id : {type : String ,default: ()=> uuidv4()},
        uid: { type: Number, required: true },
        owner: { type: String, require: true },
        url: { type: String, required: false },
        key: { type: String, required: false },
        title : { type : String, required : false },
        description : { type : String, required : false },
        link : { type : String, required : false }
    }, {
        versionKey: false
    });

    private readonly globalValueSchema = new Schema<GlobalValue>({
        _id : {type : String ,default: ()=> uuidv4()},
        hubPicture : { type : [ Number ], required : true},
        featuredUsers : { type : [ String ], required : true}
    }, {
        versionKey : false
    });
    
    private readonly projectSchema = new Schema<OldProject>({
        owner : { type : String, required : true },
        title : { type : String, required : false},
        description : { type : String, required : false },
        coverImageURL : { type : String, required : false },
        isPublic :  { type : Boolean, required : true },
        tag : { type : [ String ], required : false },
        maxUser : { type : Number, required : false },
        invitedUser : { type : [ String ], required : false },
        authorizedUser : { type : [ String ], required : false},
        projectNotice : {type : [{
            id : Number,
            title : String,
            description : String,
            date : Number,
            status : Number
        }], required : true},
        projectSchedule : { type : [{
            date : Number,
            title : String,
            contents : String
        }], required : true},
        projectBriefing : {type : [{
            state : Number,
            title : String,
            ownerName : String,
            startTime : Number,
            endTime : Number,
        
            sttList : [{
                name : String,
                content : String,
                time : Number,
            }]
        }], required : true}
    }, {
        versionKey : false
    });

    private readonly projectStorageSchema = new Schema<ProjectStorage>({
        _id : {type : String ,default: ()=> uuidv4()},
        linkID : { type : String, require : true },
        files: {
            type: [{
                name : String,
                description : String,
                url : String,
                key : String,
                fileType : Number,
                uploadedDate : Number,
                uploaderID : String,
                fileName : String,
            }], required : true
        }

    }, {
        versionKey : false
    });

    private readonly messageBoxSchema = new Schema<MessageBox>({
        _id : {type : String ,default: ()=> uuidv4()},
        owner : { type : String, require : true},
        messages : { type : [{
            id : Number,
            messageType : Number ,
            senderUID : String,
            senderName : String,
            content : String,
            createdDate : Number,
            projectID : String,
        }]},
        uidRef : {type : Number}
    },{
        versionKey : false
    });

    private readonly friendListSchema = new Schema<FriendList>({
        _id : {type : String ,default: ()=> uuidv4()},
        owner : { type : String, require : true },
        friends : { type : [String] }
    },{
        versionKey : false
    });

    private readonly worldDescriptionSchema = new Schema<OldWorldDescription>({
        ownerID : { type : String, require : true },
        bundleName : { type : String },
        worldName : { type : String },
        worldDescription : { type : String },
        worldID : { type : Number },
        buildType : { type : Number },
        ownerName : {type : String },
        createdDate : { type: Number},
        uploadedDate : {type : Number },
        thumbnailURL : {type : String },
        isVerified : { type : Boolean },
    },{
        versionKey : false
    });

    // private readonly FeaturedSchema = new Schema<Featured>({
    //     url : { type : String },
    //     billboardurl : {type : String},
    //     userID : { type : String , require : true },
    //     type1 : { type : Number },
    //     type2 : { type : Number }
    // },{
    //     versionKey : false
    // });

    private readonly UserSuccessLogSchema = new Schema<UserSuccessLog>({
        _id : {type : String ,default: ()=> uuidv4()},
        userid : {type: String , require : true},
        date : {type : String},
        action : {type : String},
        json : {type : String}
    }, {
        versionKey: false 
    });

    private readonly UserActionLogSchema = new Schema<UserActionLog>({
        _id : {type : String ,default: ()=> uuidv4()},
        userid : {type: String , require : true},
        date : {type : String},
        action : {type : String},
        json : {type : String}
    }, {
        versionKey: false 
    }); 

    //#endregion

//#region  New
    private readonly RoomSchema = new Schema<RoomDB>({
        _id : {type : String ,default: ()=> uuidv4()},
        title : { type : String },
        description : { type : String },
        ownerID : { type : String, required : true },
        thumbnailURL : { type : String },
        createAt : { type : Number },
        updateAt: { type : Number },
        worldNumber : {type:Number, required : true},
        tag :  { type : [String] },
        featuredID : { type : String },
        reportIDs : { type : [String] },
        isPublic : { type : Boolean },
        isDelete : { type : Boolean },
        deleteAt : { type : Number },
        content : { 
        }
    }, {
        versionKey: false
    });
      
    // private readonly GallerySchema = new Schema<Gallery>({
    //     structJson : { type : String },
    //     paintingJson : { type : String },
    //     wallpaperPreset : { type : Number },
    //     roomScreenInfo : { type : {
    //         state : String,
    //         imageURL : String,
    //         yoytubeURL : String
    //     }}
    // }, {
    //     versionKey: false 
    // });

    // private readonly ProjectSchema = new Schema<Project>({
    //     maxUSer : { type : Number },
    //     invitedUSer : [{ type : String }],
    //     authorizedUser : [{ type : String }],
    //     notice : [{ type : {
    //         index : Number,
    //         title : String,
    //         description : String,
    //         createAt : String,
    //         updateAt : String
    //     } }],
    //     schedule : [{ type : {
    //         title : String,
    //         description : String,
    //         createAt : String,
    //         updateAt : String
    //     } }],
    //     BriefingBoardData : { type : String }
    // }, {
    //     versionKey: false 
    // });

    // private readonly WorldDescriptionSchema = new Schema<WorldDescription>({
    //     bundleURL : { type : String },
    //     bundleType : { type : Number }
    // })

    private readonly FeaturedSchema = new Schema<Featured>({
        _id : {type : String ,default: ()=> uuidv4()},
        thumbnailURL : { type : String },
        externalURL : { type : String },
        screenType : {type : Number},
        worldNumber : { type : Number },
        ownerID : { type : String },
        roomID : { type : String },
        createAt : { type : Number },
        updateAt : { type : Number }
    }, {
        versionKey: false 
    });

    private readonly ReportSchema = new Schema<Report>({
        _id : {type : String ,default: ()=> uuidv4()},
        reportType : { type : Number },
        reporterID : { type : String },
        suspectID : { type : String },
        reason : { type : String },
        createAt : { type : Number },
        updateAt : { type : Number },
        rejectionReasons : { type: String},
        status : {type: Number}
    }, {
        versionKey: false 
    });

    private readonly BriefingBoardDataSchema = new Schema<BriefingBoard>({
        _id : {type : String ,default: ()=> uuidv4()},
        state : { type : Number },
        title : { type : String },
        ownerName  : { type : String },
        startTime  : { type : Number },
        endTime  : { type : Number },
        dataJsonPath : { type : String},
        lastModifiedID : { type : String },
        createAt : { type : Number },
        updateAt : { type : Number },
        isDelete : { type : Boolean },
        deleteAt : { type : Number },
        ProjectID : { type : String }
    }, {
        versionKey: false 
    });

    private readonly FileStorageSchema = new Schema<FileStorage>({
        _id : {type : String ,default: ()=> uuidv4()},
        filename : { type : String },
        url : { type : String },
        fileextension : { type : String },
        uploaderID : { type : String },
        filesize : { type : Number },
        createAt : { type : Number },
        updateAt : { type : Number },
        where : { type : [String]}
    }, {
        versionKey: false 
    });

    //#endregion

    public getGlobalValue() : Model<GlobalValue, {}, {}> {
        let globalValueModel = model<GlobalValue>('globalValue', this.globalValueSchema);
        
        return globalValueModel;
    }

    public getInventoryModel() : Model<Inventory, {}, {}>{
        let  inventoryModel = model<Inventory>('inventory', this.inventorySchema);

        return inventoryModel;
    }

    public getUserHouseModel() : Model<UserHouse,{},{}>{

        let roomCachesModel = model<UserHouse>('roomdata',  this.userHouseSchema);

        return roomCachesModel;
    }

    public getPlayerDataModel() : Model<PlayerData, {}, {}>{
        let  playerDataModel = model<PlayerData>('playerdata', this.playerDataSchema);

        return playerDataModel;
    }

    public getPaintingDataModel() : Model<Painting, {}, {}> {
        let paintingDataModel = model<Painting>('painting', this.frameDataSchema);
        
        return paintingDataModel;
    }

    public getOldProjectDataModel() : Model<OldProject, {}, {}> {
        let projectDataModel = model<OldProject>('project', this.projectSchema);

        return projectDataModel;
    }

    public getProjectStorageDataModel() : Model<ProjectStorage, {}, {}>{

        let projectStorageModel = model<ProjectStorage>('project_storage', this.projectStorageSchema);
        
        return projectStorageModel;
    }

    public getMessageBoxDataModel() : Model<MessageBox, {}, {}>{

        let messageBoxModel = model<MessageBox>('messagebox', this.messageBoxSchema);

        return messageBoxModel;
    }

    public getFriendListDataModel() : Model<FriendList, {}, {}>{
        let friendListModel = model<FriendList>('friend_list', this.friendListSchema);

        return friendListModel;
    }

    public getOldWorldDescription() : Model<OldWorldDescription, {}, {}>{
        let worldDescriptionModel = model<OldWorldDescription>('worldDescription', this.worldDescriptionSchema);

        return worldDescriptionModel;
    }

    public getFeaturedDataModel() : Model<Featured, {}, {}>{
        let FeaturedModel = model<Featured>('Featured', this.FeaturedSchema);

        return FeaturedModel;
    }
    public getUserSuccessLogDataModel() : Model<UserSuccessLog, {}, {}>{
        let UserLogModel = model<UserSuccessLog>('UserSuccessLog', this.UserSuccessLogSchema);

        return UserLogModel;
    }
    public getUserActionLogDataModel() : Model<UserActionLog, {}, {}>{
        let UserLogModel = model<UserActionLog>('UserActionLog', this.UserActionLogSchema);

        return UserLogModel;
    }

    public getRoomDataModel() : Model<RoomDB, {}, {}>{
        let RoomModel = model<RoomDB>('Room', this.RoomSchema);

        return RoomModel;
    }

    public getReportDataModel() : Model<Report, {}, {}>{
        let ReportModel = model<Report>('Report', this.ReportSchema);

        return ReportModel;
    }

    public getBriefingBoardDataModel() : Model<BriefingBoard, {}, {}>{
        let BriefingBoardModel = model<BriefingBoard>('BriefingBoard', this.BriefingBoardDataSchema);

        return BriefingBoardModel;
    }

    public getFileStorageSchemaModel() : Model<FileStorage, {}, {}>{
        let FileStorageModel = model<FileStorage>('FileStorage', this.FileStorageSchema);

        return FileStorageModel;
    }
}

export default MongoSchemas;