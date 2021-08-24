if (sn === null || sn === undefined || sn === '') {
    sn = {
        visual: {}
    };
}


sn.visual.pie = {
    _create: function (param, visual) {
        //let margin = sn.visual.default.margin;
        let margin = sn.visual.helper.getMargin();
        let width = parseInt(visual.style('width'));
        let height = parseInt(visual.style('height'));

        let group = visual
            .append('g')
            .classed('ve-inter-group-default', true)
            .attr('transform', 'translate(' + width / 2 + ',' + height / 2 + ')')
        param._height = height;
        param._width = width;
        param._margin = margin;
        param._group = group;
    },
    create: function (param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
        param._data = data;
        param._visual = this;
        param._visualClick = fnVisualClick;
        param._baseAxisClick = fnBaseAxisClick;
        param._aboutVisualEdit = fnAboutVisualEdited;
        param._refreshClick = fnRefreshClick;
        param._chartType = 'pie';

        let holder = sn.visual._core_lib._get_visual_ground_created(param);
        let header = sn.visual._core_lib.header.create(param);
        let about = sn.visual._core_lib.about.create(param);
        let visual = sn.visual._core_lib.svgCreator.create(param);
        let tooltip = sn.visual._core_lib.tooltip.create(param);
        let buttons = sn.visual._core_lib.buttons.create(header, param);

        holder.on('mouseenter', function () {
                sn.visual._core_lib._set_tool_button_visibility(param, true);
            })
            .on('mouseleave', function () {
                sn.visual._core_lib._set_tool_button_visibility(param, false);
            });

      

        sn.visual._core_lib._set_tool_button_visibility(param, false);
        sn.visual._core_lib.model.add(param, this);
        sn.visual.helper.plotTotalOnVisual.create(param, visual);
        // loader starts
        param._loader = sn.visual._core_lib.loader.create(param);

        param._tooltip = tooltip;
        this._create(param, visual);
        this.update(param._id, data);
        param.recreateVisual = this.reCreateVisual;
        sn.visual._core_lib.legend.create(param);
        return param._id;
    },
    update: function (id, data) {
        let model = sn.visual._model.filter(vis => vis.id === id)[0];
        let param = model.param;
        let visual = _get.d3.visual(param._id);
        let mainGroup = visual.select('.ve-inter-group-default');
        let t = d3.transition().duration(1500);
        let tooltip = param._tooltip;
        param._data = data;
        radius = Math.min(param._width, param._height) / 2;

        param._dataType = 'internal';
        
        var color = d3.scaleOrdinal().range(sn.visual._core_lib.color.getSeriesColor());
        // .domain(data, function (d) {
        //     return d[param.plotAxisKey];
        // });
        param._color = color;
        const pie = d3.pie()
            .value(d => d[param.plotAxisKey])
            .sort(null);

        const arc = d3.arc()
            .innerRadius(0)
            .outerRadius(radius - 30);

        function arcTween(a) {
            const i = d3.interpolate(this._current, a);
            this._current = i(1);
            return (t) => arc(i(t));
        }

        mainGroup.selectAll('text').remove().exit();
       
        let path = mainGroup.selectAll("path")
        .data(pie(data));
    
        path.transition().duration(200) .attr("fill", (d, i) => color(i)).attrTween("d", arcTween);

        // Enter new arcs
        path.enter()
            .append("path")
            .attr('class', 've-default ' + sn.visual.helper.getVisualCursor(param))
            .on("mousemove", function (d) {
                tooltip.show(d.data, param);
            })
            .on("mouseover", function () {
                tooltip.hide();
            })
            .on("mouseout", function () {
                tooltip.hide();
            })
            .attr("fill", (d, i) => color(i))
            // .attr("fill", (d, i) => {
            //    return  color(i);
            // })
            .attr("d", arc)
            .attr("stroke", "white")
            .attr("stroke-width", "6px")
            .each(function(d) { this._current = d; })
            .attr('data-visual-id', _get.id.visual(param._id))
            .on('click', function (d) {
               sn.visual.helper.switchSelection(param, this);
               sn.visual._core_lib.eventHandler.element_click_handling(param);
               sn.visual.helper.plotTotalOnVisual.update(param, visual);
            });

            path.exit().remove();

            // appending text fails
           let text = mainGroup.selectAll('text').data(pie(data));

           text.exit().remove();

           text.enter().append('text')
               .attr('class', 've-text-default v-text-noselect v-cursor-default v-font-weight-normal ')
               .style('stroke-width', function () {
                   return  1 + 'px';
               })
               .each(function (d) {
               let center = arc.centroid(d);
               d3.select(this).attr('x', center[0]).attr('y', center[1])
               .text(d.data[param.plotAxisKey]);
           })
           .on("mousemove", function (d) {
               tooltip.show(d.data, param);
           })
           .on("mouseover", function () {
               tooltip.hide();
           })
           .on("mouseout", function () {
               tooltip.hide();
           })
           .on('click', function (d) {
               sn.visual.helper.switchSelection(param, this);
               //sn.visual._core_lib.eventHandler.element_click_handling(param);
               if (d3.select(this).classed('ve-inter-selected') === true) {
                   sn.visual._core_lib.eventHandler.element_click_handling(param, d3.select(this).data());
               }
            });

            sn.visual._core_lib.legend.update(param);
            sn.visual.helper.resetChart(param);
            sn.visual.helper.plotTotalOnVisual.update(param, visual);
        
    },
    reCreateVisual: function (param) {
        let header = sn.visual._core_lib.header.update(param);
        let visual = sn.visual._core_lib.svgCreator.update(param);
        sn.visual.helper.plotTotalOnVisual.create(param, visual);
        let visualObj = param._visual;
        visualObj._create(param, visual);
        visualObj.update(param._id, param._data);
    }
};