const routes = require('express').Router();
const e = require('express');
var datetime = require('node-datetime');
const dbConnection = require('../db.config');
const db = require('../db.config');


//Signup - Adding new user accounts 
routes.post('/api/v1/login/:username/:password', (req, res, err) => {
    dbConnection.query("INSERT INTO users (username, password) Values(?,?)",
        [
            req.params.username,
            req.params.password
        ],
        (err, rows, fields) => {
            if (err)
                return res.status(500).json(err)
            else {
                if (rows !== 'undefined' && rows.length == 0) {
                    console.log("undefined");
                    return res.json({
                        "message": "User details are not available",
                        "status": "userid not found"
                    });
                }
                else {
                    return res.status(200).json(
                        {
                            rows,
                            "message": "New User successfully",
                        });
                }
            }
        });
})


//Login - get user details 
routes.get('/api/v1/login/:username/:password', (req, res, err) => {

    // dbConnection.query("SELECT * FROM users", (err, results, fields) => {
    //     console.log("All users", results);
    //     if(err){
    //         console.log("Error: ", err);
    //     }
    //     else{
    //         res.json(results);
    //     }
    // });
    // username = ? AND password = ?
    dbConnection.query("SELECT * FROM users WHERE username =? AND password=?",
        [
            req.params.username,
            req.params.password
        ],
        (err, rows, fields) => {
            if (err)
                return res.status(500).json(err)
            else {
                // console.log(req.params.username, req.params.password);
                console.log(req.params.username);
                if (rows !== 'undefined' && rows.length == 0) {
                    console.log("undefined");
                    return res.json({
                        "message": "User details are not available",
                        "status": "userid not found"
                    });
                }
                else {
                    return res.status(200).json({
                        rows,
                        "message": "User details loaded successfully",
                        // "details": rows
                    });
                }
            }
        });
});


//Secret key - Validate key details
routes.get('/api/v1/verifyBook/:secretCode', (req, res, err) => {

    console.log("Secret Key", req.params.secretCode);
    dbConnection.query("SELECT secretKey FROM book1 where secretKey = ?", [req.params.secretCode], (err, rows) => {
        if (err) {
            return res.status(500);
        }
        else {
            return res.json({
                "status": 200,  
                "verfication": rows,
                "message": "Book Verfied"
            });
        }
    });
})


module.exports = routes, datetime;