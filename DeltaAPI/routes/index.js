const routes = require('express').Router();
var datetime = require('node-datetime');

routes.get('/api/v1', (req, res) => {
    console.log(datetime.create()._now.getDate.toString())
    res.status(200).json(
        {
            message: 'Connected',
            startTime: 'Hello',
        }
    );
})


routes.post('/api/v1/:username/:password', (req, res, err) => {
    res.json({
        userName: req.params.username,
        password: req.params.password,
        created_date: Date.now
    });
})


routes.post('/api/v1/:secretCode', (req, res, err) => {

})


module.exports = routes, datetime;