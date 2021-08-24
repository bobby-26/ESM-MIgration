if (sn === null || sn === undefined || sn === '') {
    sn = {
        visual: {}
    };
}

sn.visual._line = {
    _create: function (param, visual) {
        //let margin = sn.visual.default.margin;
        let margin = sn.visual.helper.getMargin();
        let width = parseInt(visual.style('width')) - margin.left - margin.right;
        let height = parseInt(visual.style('height')) - margin.top - margin.bottom;
        

        let group = visual
            .style('width', width + margin.left + margin.right + 'px')
            .style('height', height + margin.top + margin.bottom + 'px')
            .append('g')
            .classed('ve-inter-group-default', true)
            .attr('transform', 'translate(' + margin.left + ',' + margin.top + ')')


        let x = d3.scaleBand()
            .range([0, width])
            .padding(0.9)
            
        let y = d3.scaleLinear()
            .range([height, 0])

        let xAxisGroup = group.append('g');
        let yAxisGroup = group.append('g');
        param._xAxisGroup = xAxisGroup;
        param._yAxisGroup = yAxisGroup;
        param._xScale = x;
        param._yScale = y;
        param._height = height;
        param._width = width;
        param._margin = margin;
        param._group = group;
    },
    create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
        param._data = data;
        param._visual = this;
        param._visualClick = fnVisualClick;
        param._baseAxisClick = fnBaseAxisClick;
        param._aboutVisualEdit = fnAboutVisualEdited;
        param._chartType = 'line';
        
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
        //checking listner present
        param._tooltip = tooltip;
        this._create(param, visual);
        this.update(param._id, data);
        param.recreateVisual = this.reCreateVisual;

        sn.visual.helper.baseTitle.create(param._group, param.baseAxisTitle, param._margin, param._height, param._width);
        sn.visual.helper.plotTitle.create(param._group, param.plotAxisTitle, param._margin, param._height);
        sn.visual.helper.plotTotalOnVisual.create(param, visual);
        // sn.visual.helper.setPreSelected(param);
        // sn.visual.helper.plotTotalOnVisual(param, visual);
        sn.visual._core_lib.legend.create(param);
        return param._id;
    },
    update: function (id, data) {
        // find the id and get the param
        let model = sn.visual._model.filter(vis => vis.id === id)[0];
        let param = model.param;
        param._data = data;
        let xScale = param._xScale;
        let yScale = param._yScale;
        let xAxisGroup = param._xAxisGroup;
        let yAxisGroup = param._yAxisGroup;
        
        let height = param._height;
        let width = param._width;
        let margin = param._margin;
        let tooltip = param._tooltip;
     
        let t = d3.transition().duration(1500);
        let colorScale = d3.scaleOrdinal(sn.visual._core_lib.color.getSingleColor(data.length));
        param._color = colorScale;
        let visual = _get.d3.visual(param._id);

        xScale.domain(data.map(function (d) {
            return d[param.baseAxisKey];
        }));

        yScale.domain([0, d3.max(data, function (d) {
            return d[param.plotAxisKey];
        })]);

        let format = d3.format('.2s');

        let xAxis = d3.axisBottom(xScale);
        let yAxis = d3.axisLeft(yScale).tickFormat(function (d) {
                return format(d);
        })
        .ticks(5);

        let line = d3.line()
            .x(function (d, i) {
                return xScale(d[param.baseAxisKey]);
            })
            .y(function (d) {
                return yScale(d[param.plotAxisKey]);
            })
            .curve(d3.curveMonotoneX) // apply smoothing to the line

        xAxisGroup
            .attr('transform', 'translate(0,' + height + ')')
            .transition(t)
            .call(xAxis)
            .selectAll('text')
            .attr('transform', 'rotate(-40)')
            .attr('x', '-5')
            .attr('y', '10')
            .attr('class', 'v-text-default')


        yAxisGroup
            .transition(t)
            .call(yAxis)
            .selectAll('text')
            .attr('class', 'v-text-default');

        // data join
        let group = _get.d3.visual(param._id)
            .select('.ve-inter-group-default');

            function transition(path) {
                path.transition(t)
                    .attrTween("stroke-dasharray", tweenDash);
            }

            function tweenDash() {
                var l = this.getTotalLength(),
                    i = d3.interpolateString("0," + l, l + "," + l);
                return function (t) { return i(t); };
            }

            exitLine();
            exitBubble();
            updateLine(data);

            function updateLine(data) {
                data.forEach(function (d) {
                    group
                      .append('path')
                      .datum(data)
                      .attr('d', line)
                      .attr('fill', 'none')  
                      .attr('class', 've-connector-default vl-line-bubble' + sn.visual.helper.getVisualCursor(param))
                      .style("stroke-width", "2px")
                      .style('stroke', function (d, i) { return colorScale(i); })
                      .call(transition);
 
                 });
                 createBubble(data);
            }

            function exitLine() {
                group.selectAll('.vl-line-bubble').remove().exit();
            }

            function createBubble(data) {
                group.selectAll('circle')
                .data(data)
                .enter().append('circle')
                .attr('class', 've-bubble v-cursor-pointer')
                .attr('cx', function(d) {
                  return xScale(d[param.baseAxisKey]);
                })
                .attr('cy', function(d) {
                  return yScale(d[param.plotAxisKey]);
                })
                .attr('r', 3)
                .attr('data-visual-id', _get.id.visual(param._id))
                .on('click', function (d) {
                   sn.visual.helper.switchSelection(param, this);
                   sn.visual._core_lib.eventHandler.element_click_handling(param);
                   sn.visual.helper.plotTotalOnVisual.update(param, visual);
                })
                .on("mousemove", function (d) {
                    tooltip.show(d, param);
                })
                .on("mouseover", function () {
                    tooltip.hide();
                })
                .on("mouseout", function () {
                    tooltip.hide();
                })
                .style('fill', colorScale(0))
            }

            function exitBubble() {
                group.selectAll('.ve-bubble').remove().exit();
            }
           
            sn.visual.helper.baseTitle.update(group, param.baseAxisTitle);
            sn.visual.helper.plotTitle.update(group, param.plotAxisTitle);
            sn.visual.helper.plotTotalOnVisual.update(param, visual);
            // sn.visual.helper.setPreSelected(param);
            sn.visual._core_lib.legend.update(param);
            sn.visual.helper.resetChart(param);
    },
    reCreateVisual: function (param) {
        let header = sn.visual._core_lib.header.update(param);
        let visual = sn.visual._core_lib.svgCreator.update(param);
        sn.visual.helper.plotTotalOnVisual.create(param, visual);
        let line = param._visual;
        line._create(param, visual);
        line.update(param._id, param._data);
    }
};
