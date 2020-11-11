
const express = require('express');

const app = express();
app.use(express.static('./dist/BanglaSketch'));
app.get('/*', function(req, res) {
  res.sendFile('index.html', {root: 'dist/BanglaSketch'}
);
});
app.listen(process.env.PORT || 3000, function(){
  console.log("Express server listening on port %d in %s mode", this.address().port, app.settings.env);
});
