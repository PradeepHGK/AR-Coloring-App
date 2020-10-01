var app = require('express')();
var routes = require('./routes');
var os = require('os')
var bodyParser=require('body-parser');

var datetime = require('node-datetime');

port = process.env.PORT | 3805;

app.use('/', routes);
app.use(bodyParser.urlencoded({extended:true}));
app.use(bodyParser.json());

app.get('/tests', (req, res)=>{
    res.json({
        date: Date.now()
    })
})

app.listen(port, (data) => {
    console.log("Server listening port no: ", port);
    console.log(os.platform(), os.version(), os.hostname(), os.userInfo());
});

