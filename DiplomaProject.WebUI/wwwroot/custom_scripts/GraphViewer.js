function renderGraph(nodes, edges) {
    const graph = {
        edges: [],
        nodes: []
    };
    $('#graph-container').empty();
    nodes.forEach((node, index) => {
        graph.nodes.push({
            id: node.Id,
            label: node.InitialSubject ? `${node.Name}(${node.InitialSubject})` : node.Name,
            x: Math.random(),
            y: Math.random(),
            size: Math.random(),
            type: 'cycle',
            color: node.IsNew ? '#4ddbff' : '#99ff66'
        });
    });
    edges.forEach((edge, index) => {
        graph.edges.push({
            id: edge.Id,
            source: edge.FromNode,
            target: edge.ToNode,
            size: Math.random(),
            type: 'arrow',
            color: '#999966'
        });
    });
    s = new sigma({
        graph,
        container: 'graph-container',
        settings: {
            defaultLabelColor: '#fff',
            autoResize: true,
            autoRescale: true,
            defaultLabelSize: 14,
            maxNodeSize: 8,
            minNodeSize: 8,
            maxEdgeSize: 2,
            minEdgeSize: 2,
            minArrowSize: 5,
            doubleClickEnabled: false,
            mouseEnabled: true,
            mouseWheelEnabled: false,
            borderColor: '#eee'
        }
    });
    s.refresh();
    var dragListener = sigma.plugins.dragNodes(s, s.renderers[0]);
}