if (sn === null || sn === undefined || sn === '') {
    sn = {
        visual: {}
    };
}

sn.visual.barGrouped._vertical = {
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

        let x0 = d3.scaleBand()
            .rangeRound([0, width])
            .paddingInner(0.1);


        let x1 = d3.scaleBand()
            .paddingInner(0.04)
            .paddingOuter(0.2);


        let y = d3.scaleLinear()
            .rangeRound([height, 0]);

        let xAxisGroup = group.append('g');
        let yAxisGroup = group.append('g');
        param._xAxisGroup = xAxisGroup;
        param._yAxisGroup = yAxisGroup;
        param._xGroupScale = x0;
        param._xScale = x1;
        param._yScale = y;
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
        param._chartType = 'bar-grouped-vertical';

        let holder = sn.visual._core_lib._get_visual_ground_created(param);
        let about = sn.visual._core_lib.about.create(param);
        let header = sn.visual._core_lib.header.create(param);
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
        // loader starts
        param._loader = sn.visual._core_lib.loader.create(param);

        //checking listner present
        param._tooltip = tooltip;
        this._create(param, visual);
        this.update(param._id, data);
        param.recreateVisual = this.reCreateVisual;

        sn.visual.helper.baseTitle.create(param._group, param.baseAxisTitle, param._margin, param._height, param._width);
        sn.visual.helper.plotTitle.create(param._group, param.plotAxisTitle, param._margin, param._height);
        // sn.visual.helper.setPreSelected(param);
        //sn.visual.helper.plotTotalOnVisual.create(param, visual);
        sn.visual._core_lib.legend.create(param);
        return param._id;
    },
    update: function (id, data) {
        // find the id and get the param
        let model = sn.visual._model.filter(vis => vis.id === id)[0];
        let param = model.param;
        param._data = data;
        let xScaleGroup = param._xGroupScale;
        let xScale = param._xScale;
        let yScale = param._yScale;
        let xAxisGroup = param._xAxisGroup;
        let yAxisGroup = param._yAxisGroup;

        let height = param._height;
        let width = param._width;
        let margin = param._margin;
        let tooltip = param._tooltip;


        let t = d3.transition().duration(1500);
        let colorScale = d3.scaleOrdinal(sn.visual._core_lib.color.getSeriesColor(data.length));
        param._color = colorScale;

        // get all the group key
        let keys = [];
        let groupKeys = [];

        function getUniqueKeys(property, data, result) {
            data.map((d) => {
                if (result.indexOf(d[property]) === -1) {
                    result.push(d[property]);
                }
            });
        }

        let cData = null;

        if (param.woPivot === "1") {
             cData = sn.visual.helper.dataConvetor.barGroupWODataConvetor(param, data);
            param._cData = cData;

        } else {
            let metaData = sn.visual.helper.dataConvetor.getBarGroupMetaData(param);
            cData = sn.visual.helper.dataConvetor.barGroupDataConvetor(param, metaData);
            param._cData = cData;
        }

    
        getUniqueKeys(param.baseAxisKey, cData, groupKeys);
        getUniqueKeys('key', cData[0][param.baseAxisGroupKey], keys);

        let visual = _get.d3.visual(param._id);
        let group = visual.select('.ve-inter-group-default');
        let format = d3.format('.2s');

        xScaleGroup.domain(groupKeys);
        xScale.domain(keys).rangeRound([0, xScaleGroup.bandwidth()]);
        yScale.domain([0, d3.max(cData, function (d) {
            return d3.max(d[param.baseAxisGroupKey], function (val) {
                return val[param.plotAxisKey];
            });
        })]).nice();

        let xAxis = d3.axisBottom(xScaleGroup);
        let yAxis = d3.axisLeft(yScale).tickFormat(function (d) {
            return format(d);
        })
        .ticks(5);

        xAxisGroup
            .attr('transform', 'translate(0,' + height + ')')
            .attr('class', 'v-base-axis')
            .transition(t)
            .call(xAxis)
            .selectAll('text')
            .attr('transform', 'rotate(-40)')
            .attr('x', '-5')
            .attr('y', '10')
            .attr('class', 'v-text-default');

        xAxisGroup
            .selectAll('text')
            .append('title')
            .text(function (d) { return d; });

        
        yAxisGroup
            .attr('class', 'v-plot-axis')
            .transition(t)
            .call(yAxis)
            .selectAll('text')
            .attr('class', 'v-text-default');

        //manually removing the sub group
        group.selectAll('.ve-bar-sub-group').remove();

        let subGroup = group
            .selectAll(".ve-bar-sub-group")
            .data(cData)

        subGroup.exit().remove();

        let rect = null;
        if (param.woPivot === "1") {
            rect = subGroup
            .enter()
            .append("g")
            .attr('class', 've-bar-sub-group')
            .attr("transform", function (d) {
                return "translate(" + xScaleGroup(d[param.baseAxisKey]) + ",0)";
            })
            .merge(subGroup)
            .selectAll("rect")
            .data(function (d) {
                return d[param.baseAxisGroupKey].map(function (value) {
                    return value;
                })
            });

        } else {
            rect = subGroup
            .enter()
            .append("g")
            .attr('class', 've-bar-sub-group')
            .attr("transform", function (d) {
                return "translate(" + xScaleGroup(d[param.baseAxisKey]) + ",0)";
            })
            .merge(subGroup)
            .selectAll("rect")
            .data(function (d) {
                return d[param.baseAxisGroupKey].map(function (value) {
                    const result = {};
                    result['key'] = value['key'];
                    result['value'] = value['value'];
                    param._cMoreKeyTitle.forEach(key => {
                        result[key] = d[key];
                    });
                    return result;
                })
            });
        }

        //exit
        rect.exit().transition(t).style('opacity', 0).remove();

        // update 
        rect.transition(t)
            .attr("x", function (d) {
                return xScale(d['key']);
            })
            .attr("width", xScale.bandwidth())
            .attr("y", function (d) {
                return yScale(d['value']);
            })
            .attr("height", function (d) {
                return height - yScale(d['value']);
            })
            .attr('data-visual-id', _get.id.visual(param._id));

        //bind new data
        rect.enter()
            .append("rect")
            .attr("x", function (d) {
                return xScale(d['key']);
            })
            .attr('y', function (d) {
                return yScale(0);
            })
            .attr("width", xScale.bandwidth())
            .attr('height', function (d) {
                return 0;
            })
            .attr("fill", function (d) {
                return colorScale(d['key']);
            })
            .attr('class', 've-inter-default ve-default v-bar ' + sn.visual.helper.getVisualCursor(param))
            .attr('data-visual-id', _get.id.visual(param._id))
            .on('click', function (d) {
                sn.visual.helper.switchSelection(param, this);
                sn.visual._core_lib.eventHandler.element_click_handling(param, d3.select(this).data());
            })
            .on("mousemove", function (d) {
                sn.visual.barGrouped._vertical.toolTipHelper(param);
                tooltip.show(d, param);
            })
            .on("mouseover", function () {
                tooltip.hide();
            })
            .on("mouseout", function () {
                tooltip.hide();
            })
            .merge(subGroup)
            .transition(t)
            .attr("y", function (d) {
                return yScale(d['value']);
            })
            .attr("height", function (d) {
                return height - yScale(d['value']);;
            });
        sn.visual.helper.resetChart(param);
        return param._id;
    },
    reCreateVisual: function (param) {
        let header = sn.visual._core_lib.header.update(param);
        let visual = sn.visual._core_lib.svgCreator.update(param);
        let obj = param._visual;
        obj._create(param, visual);
        obj.update(param._id, param._data);
    },
    toolTipHelper: function (param) {
        if (param.toolTipProperty === undefined) {
            param.toolTipTitleProperty = 'key';
            param.toolTipProperty = 'value';
        }
    },
    barGroupWOPivoted: function(rect, param) {
        rect.data(function (d) {
            return d[param.baseAxisGroupKey].map(function (value) {
                return result;
            })
        });
    },
    barGroupWithPivot: function (rect, param) {
        rect.data(function (d) {
            return d[param.baseAxisGroupKey].map(function (value) {
                const result = {};
                result['key'] = value['key'];
                result['value'] = value['value'];
                param._cMoreKeyTitle.forEach(key => {
                    result[key] = d[key];
                });
                return result;
            })
        });
    }
}
