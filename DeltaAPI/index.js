var app = require('express')();
var routes = require('./routes');

port = process.env.PORT | 3805;

app.use('/', routes);

app.listen(port, (data) => {
    console.log("Server listening port no: ", port);
});

