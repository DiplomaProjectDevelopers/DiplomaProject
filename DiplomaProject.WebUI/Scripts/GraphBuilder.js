//var Dracula = require('../wwwroot/lib/graphdracula/index');
import Dracula from '~/lib/graphdracula/index'
function buildGraph(array) {
    if (Dracula) {
        const Graph = Dracula.Graph;
        console.log(Graph);
    }
}