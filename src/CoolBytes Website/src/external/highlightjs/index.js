var hljs = require('./highlight');

hljs.registerLanguage('cs', require('./languages/cs.js'));

module.exports = hljs;