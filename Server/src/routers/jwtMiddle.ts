import { JwtPayload, sign, verify } from "jsonwebtoken";
import { Request, Response } from "express";
import { eResultCode } from "../ResultCode";

declare module 'express'{
    interface Request{
        jwtParsed? : any
    }
}

export const SECRET_KEY = "bnfcrvs09!@";
export const TOKEN_LIFETIME = "15m";
export const REFRESH_TOKEN_LIFETIME = "14d";

const bypassTokenVerify : boolean = false;

export async function VerifyToken (req : Request, res : Response, next : any) {
    if(bypassTokenVerify == true){
        next();
        return;
    }
    
    if(req.headers["token"] == null || req.headers["refreshtoken"] == null){
        console.error("token or refreshToken is null");
        return res.status(401).json( {resultCode : eResultCode.FAIL, message : "invalide request" });
    }
    let parseToken : JwtPayload  = null;
    let parseRefreshToken : JwtPayload = null;
    //need fix parse try catch
    // let parseToken : JwtPayload = tryParseToken(req.body.token);
    // let parseRefreshToken : JwtPayload = tryParseToken(req.body.refreshToken);

    try {
        parseToken = tryParseToken(req.headers["token"]);
        parseRefreshToken = tryParseToken(req.headers["refreshtoken"]);

    } catch (e) {
        console.log(e);
    }

    let error = false;

    if(null == parseToken){
        //check refresh token
        if(null == parseRefreshToken){
            //token and refreshToken expired
            return res.status(401).json({ resultCode : eResultCode.FAIL, message : "relogin Required" });
        }
                
        let user : any = null;
        await mainServerGetUser(parseRefreshToken["uid"]).then((result) => {
            user = result;
        }).catch((reason) => {
            console.error(reason);
            error = true;
        });

        if(error != false){
            return res.status(401).json({ resultCode : eResultCode.FAIL, message : "relogin required"});
        }

        // await sqlProxy.Instance().queryAsync("select * from account where refreshToken = \"" + req.headers["refreshtoken"] + "\"", null, (err, result) => {
        //     if (null != err){
        //         console.error(err);
        //         error = true;
        //         return;
        //     }

        //     if(result.length < 1){  
        //         error = true;
        //         return;
        //     }

        //     user = result[0];
        // });

        // if(error != false){
        //     //refresh token not expired but can't find on database
        //     return res.status(401).json({ resultCode : eResultCode.FAIL, message : "relogin Required" });
        // }

        //create new token
        let newToken = sign({
            uid : user._id,
            iat : Date.now()
        }, SECRET_KEY, {
            expiresIn : TOKEN_LIFETIME,
            issuer : "creverseLogin"
        });

        //check is new on requests
        //req.body.newToken = newToken;
        res.setHeader("new-token", newToken);

        req.body.parsedUID = user._id;
        next();
        return;
    } 

    if(null == parseRefreshToken){

        let user : any = null;
        await mainServerGetUser(parseToken["uid"]).then((result) => {
            user = result;
        }).catch((reason) => {
            console.error(reason);
            error = true;
        });

        if (false != error) {
            return res.status(401).json({resultCode : eResultCode.FAIL, message: "relogin Required" });
        }

        // token is fine but Refresh token is expired
        let newRefreshToken = sign({ uid : user._id }, SECRET_KEY, {
            expiresIn : REFRESH_TOKEN_LIFETIME,
            issuer : "creverseRefreshToken"
        });

        //let error = false;

        // await sqlProxy.Instance().queryAsync("update account set refreshToken = \"" + newRefreshToken + "\" where uid = " + parseToken["uid"], null, (err, result)=>{
        //     if(null != err){
        //         console.error(err);
        //         error = true;
        //         return;
        //     }

        //     console.log(result);
        // });

        //req.body.newRefreshToken = newRefreshToken;
        res.setHeader("new-refresh-token", newRefreshToken);
        
        req.body.parsedUID = parseToken["uid"];
        next();
        return;
    }

    req.body.parsedUID = parseToken["uid"];
    next();
}

export function tryParseToken(token : any) : JwtPayload {
    let result : JwtPayload = null;

    try{
        result = verify(token, SECRET_KEY) as JwtPayload;
    }catch(e){
        //console.error(e);
    }

    return result;
}

export class RestoreTokenInfo {
    public loginToken : string = "";

    public refreshToken : string = "";
}
export function RestoreToken(user_id : string) : RestoreTokenInfo{

    let tokenInfo = new RestoreTokenInfo();

    tokenInfo.loginToken =  sign({
        uid :user_id,
        iat : Date.now()
    }, SECRET_KEY, {
        expiresIn : TOKEN_LIFETIME,
        issuer : "creverseLogin"
    });

    tokenInfo.refreshToken =  sign({ uid : user_id }, SECRET_KEY, {
        expiresIn : REFRESH_TOKEN_LIFETIME,
        issuer : "creverseRefreshToken"
    });

    return tokenInfo;
}