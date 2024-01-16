import config from "@colyseus/tools";
import { monitor } from "@colyseus/monitor";
import { playground } from "@colyseus/playground";
import { MongooseDriver } from "@colyseus/mongoose-driver";

import userRouter from "./router/userRouter";
import objectDataRouter from "./router/objectDataRouter";
import roomRouter from "./router/roomRouter";

import formData from "express-form-data";

import cors from "cors";
import http from "http";
import express from "express";
import path from "path";

/**
 * Import your Room files
 */
import { MyRoom } from "./rooms/MyRoom";
import mongoose from "mongoose";

mongoose.connect("mongodb+srv://admin:admin@starcraft.lxxbebz.mongodb.net/StarcraftDataBase?retryWrites=true&w=majority").then(() => {
    console.log("Connected to MongoDB");
}).catch(() => {
    console.log("Couldn't connect to MongoDB");
})

export default config({

    getId: () => "Starcraft Server",

    options : {
        driver : new MongooseDriver("mongodb+srv://admin:admin@starcraft.lxxbebz.mongodb.net/StarcraftDataBase?retryWrites=true&w=majority"),
        //driver : new MongooseDriver("mongodb://127.0.0.1:27017/starcraft"),
    },

    initializeGameServer: (gameServer) => {
        /**
         * Define your room handlers:
         */
        gameServer.define('my_room', MyRoom).filterBy(["SceneNumber","ownerID"]);

    },

    initializeExpress: (app) => {
        /**
         * Bind your custom express routes here:
         * Read more: https://expressjs.com/en/starter/basic-routing.html
         */
        app.use(cors());

        app.use(formData.format());
        app.use(formData.union());

        app.use(express.json());

        app.get("/", (req, res) => {
            res.send("It's time to kick ass and chew bubblegum!");
        });

        /**
         * Use @colyseus/playground
         * (It is not recommended to expose this route in a production environment)
         */
        if (process.env.NODE_ENV !== "production") {
            app.use("/", playground);
        }

        app.use("/user", userRouter);
        app.use("/objectData", objectDataRouter);
        app.use("/room", roomRouter);

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
