import Arena from "@colyseus/arena";
import config from "@colyseus/tools";
import { monitor } from "@colyseus/monitor";
import { playground } from "@colyseus/playground";

import { Server, LocalDriver, matchMaker, MongooseDriver } from "colyseus";

import formData from "express-form-data";
import cors from "cors";
import express from "express";
import path from "path";

//mongodb+srv://eodls0810:shjin5405@starcraft.lxxbebz.mongodb.net/?retryWrites=true&w=majority

/**
 * Import your Room files
 */
import { MyRoom } from "./rooms/MyRoom";

import userRouter from "./routers/userRouter";
import roomRouter from "./routers/roomRouter";
import { MongoManager } from "./DB/MongoManager";

export default Arena({

    options : {
        driver : new MongooseDriver("mongodb+srv://eodls0810:shjin5405@starcraft.lxxbebz.mongodb.net/?retryWrites=true&w=majority")
    },
    
    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
        gameServer.define('my_room', MyRoom).filterBy(["worldNumber", "roomOwner"]);
    },

    initializeExpress: async(app) => {
        /**
         * Bind your custom express routes here:
         * Read more: https://expressjs.com/en/starter/basic-routing.html
         */

        app.use(cors());
        app.use(formData.format());
        app.use(formData.union());


        app.get("/hello", (req, res) => {
            res.send("It's time to kick ass and chew bubblegum!");
        });

        app.use("/user", userRouter);
        app.use("/room", roomRouter);

        /**
         * Use @colyseus/playground
         * (It is not recommended to expose this route in a production environment)
         */
        if (process.env.NODE_ENV !== "production") {
            app.use("/", playground);
        }

        /**
         * Use @colyseus/monitor
         * It is recommended to protect this route with a password
         * Read more: https://docs.colyseus.io/tools/monitor/#restrict-access-to-the-panel-using-a-password
         */
        app.use("/colyseus", monitor());
        app.use("/", express.static(path.join(__dirname, "static")));
    },


    beforeListen: () => {
        /**
         * Before before gameServer.listen() is called.
         */
    }
});
