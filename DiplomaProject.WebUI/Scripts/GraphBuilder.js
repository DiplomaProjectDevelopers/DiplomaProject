function buildGraph(model) {
    var g = new Viva.Graph.graph();
    model.forEach(item => {
        g.AddNode(item.id, item.Name);
    });
    let renderer = Viva.Graph.View.renderer(g);
    renderer.Run();

}