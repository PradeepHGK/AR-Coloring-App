const { v4: uuidv4 } = require('uuid');
const { options } = require('./routes');

var os = require("os");
const fs = require('fs')



var GenerateKeys = function (value = Number, fileName = String) {
    for (let index = 0; index < value; index++) {
        var uuid1 = uuidv4() 
        var range = uuid1.substring(0, 8);
        console.log(uuid1, typeof (uuid1), "---", uuid1.substring(0, 8));

        //index + " " +
        var content = range + os.EOL;
        fs.appendFile('./' + fileName , content , (err) => {
            if (err) {
                console.log("Error: ", err);
            }
        }); 
    }
}



GenerateKeys(100, "Volume1.txt");