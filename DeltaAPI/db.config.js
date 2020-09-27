const db = require('mysql');
var dbConnection = db.createConnection({
    host     : 'localhost',
    user     : 'root',
    password : '1405',
    database : 'Delta'
})

dbConnection.connect((err) => {
    console.log('DB Connection Established', err)
})

module.exports = dbConnection;