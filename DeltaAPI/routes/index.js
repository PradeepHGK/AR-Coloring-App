const routes = require('express').Router();

routes.get('/api/v1', (req, res) => {
    res.status(200).json({ message: 'Connected!' });
})


routes.post('/api/v1/:username/:password([0-9](8))', (req, res, err) => {
    res.json({
        userName: req.params.username,
        password: req.params.password,
        created_date: Date.now.toString()
    });
})

module.exports = routes;