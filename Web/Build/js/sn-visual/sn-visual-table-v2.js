if (sn === null || sn === undefined || sn === '') {
    sn = {
        visual: {}
    };
}
sn.visual._table = {
    _create: function (param, visual) {

    },
    create: function (param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
        param._data = data;
        param._visual = this;
        param._visualClick = fnVisualClick;
        param._baseAxisClick = fnBaseAxisClick;
        param._aboutVisualEdit = fnAboutVisualEdited;
        param._chartType = 'table';

        param._margin = sn.visual.default.margin;
        param._visual = this;
        let holder = sn.visual._core_lib._get_visual_ground_created(param);
        let header = sn.visual._core_lib.header.create(param);
        let buttons = sn.visual._core_lib.buttons.create(header, param);

        // loader starts
        param._loader = sn.visual._core_lib.loader.create(param);

        sn.visual._core_lib.model.add(param, this);

        holder.on('mouseenter', function () {
            sn.visual._core_lib._set_tool_button_visibility(param, true);
        })
        .on('mouseleave', function () {
            sn.visual._core_lib._set_tool_button_visibility(param, false);
        });

        sn.visual._core_lib._set_tool_button_visibility(param, false);

        sn.visual._core_lib.model.add(param, this);
        this.update(param._id, data);
        param.recreateVisual = this.reCreateVisual;
        return param._id;
    },
    update: function (id, data) {
        let model = sn.visual._model.filter(vis => vis.id === id)[0];
        let param = model.param;
        let holder = _get.d3.holder(param._id);
        let thresholdHeight = 50;
        let height = parseInt(holder.style('height')) - thresholdHeight;
        param._data = data;

        _get.d3.visual(param._id).remove();

        let table = holder.append('table')
            .attr('class', 'vt-default v-text-noselect')
            .attr('id', _get.id.visual(param._id))
            // .style('display', 'grid')
            .style('border-spacing', '0px')
            .style('font-size', 10 + 'px')
            .style('width', '100%')
            .style('height',  height + 'px');

        let thead = table.append('thead')
                .attr('class', 'vt-head');
        let tbody = table.append('tbody')
            .attr('class', 've-inter-group-default vt-body')
            .style('height', 80 + '%');
        let tfoot = table.append('tfoot')
            .attr('class', 'vt-foot');
        
        let columns = [];
        columns.push({
            'name': param.baseAxisKey,
            'title': param.baseAxisTitle,
        });

        sn.visual._table.getMoreKey(param, columns);
      
        columns.push({
            'name': param.plotAxisKey,
            'title': param.plotAxisTitle,
        });

        const _scroll_fill = '_scroll_fill';
        let col_head =  thead.append('tr').style('border-top', `1px solid none`)
        .selectAll('th')
        .data(columns).enter()
        .append('th')
        //.style('cursor', 'pointer')
        .attr('class', function (d) {
            return (d.name == param.baseAxisKey ?
                'vt-c-text'  + sn.visual.helper.getVisualCursor(param) :
                (param._data.length > 0 && _isNotNullOrEmpty(d.name) && typeof (param._data[0][d.name]) == 'number' ? 'vt-c-number' : 'vt-c-text'));
        })
        .style('border-right', function (d) {
            if (d.name == _scroll_fill) {
                return '0px'
            } else {
                return d3.select(this).attr('border-right');
            }
        })
        .style('width', function (d) {
            if (d.name == _scroll_fill) {
                return '14px'
            } else {
                return d3.select(this).attr('width');
            }
        })
        .style('font-size', _get.d3.holder(param._id).classed('vh-full') == true ? '14px' : '11px')
        .attr('title', function (d) {
            return d.title;
        })
        .text(function (d) {
            return d.title;
        });

        // integration the data with table

        let rows =  table.selectAll('tbody').selectAll('tr')
        .data(data)
        .enter()
        .append('tr')
        .style('display', 'table')
        .style('width', '100%')
        .style('table-layout', 'fixed')
        .attr('data-visual-id', _get.id.visual(param['_id']))
        .attr('class', function (d, i) {
            let result = 'table-row  ve-inter-table-default vt-r-data ' + (i % 2 == 1 ? 'vt-r-data-even' : '');
            return result;
        })
        .on('click', function (d) {
            sn.visual.helper.switchSelection(param, this);
            sn.visual._core_lib.eventHandler.element_click_handling(param, d3.select(this).data());
            //if (d3.select(this).classed('ve-inter-selected') === true) {

            //}
        })

        let cells = rows.selectAll('td')
        .data(function (row) {
            return columns.filter(c => c.name != _scroll_fill).map(function (column) {
                return {
                    key: column,
                    value: row[column.name],
                    _id: row['_id']
                };
            });
        })
        .enter()
        .append('td')
        .style('border-right', function (d) {
            return `2px solid rgb(221, 221, 221);`;
        })
        .attr('nowrap', '1')
        .attr('class', function (d, i) {
            return (d.key.name == param.baseAxisKey ? 'vt-c-text' :
                (typeof (d.value) == 'number' ? 'vt-c-number' : 'vt-c-text')
            );
        })
        .attr('title', function (d) {
            return d.value;
        })
        .html(function (d, i) {
            result = d.value;
            return result;
        });

        // do the summary method
        tfoot.append('tr')
       .attr('data-visual-id', 'visual' + param['_id'])
       .selectAll('th')
       .data(columns)
       .enter()
       .append('th')
       .attr('class', 'vt-c-number vt-c-summary')
       .style('width','14px')
       .text(function (d) {
          return sn.visual._table.getSummaryReduceMethod(param, d);
        });
    },
    reCreateVisual: function (param) {
        let header = sn.visual._core_lib.header.update(param);
        let barObj = param._visual;
        barObj.update(param._id, param._data);
    },
    getSummaryReduceMethod: function (param, data) {
        if (data.name === param.baseAxisKey) {
            return param.summaryTitle;
        }
        else if (data.name === param.summaryProperty) {
            return sn.visual._table.getSummary(param, param.summaryMethod);
        }
    },
    getSummary: function (param, operations) {
        let summaryReducedValue = '';
        // change this to get the selected data from the property
        let summarySource = param._data;
        switch (operations.toLowerCase()) {
            case 'sum':
                summaryReducedValue = d3.sum(summarySource, function (d) {  return d[param.summaryProperty] });
                break;
            case 'avg':
                summaryReducedValue = (summarySource.map((d) => { return d[param.summaryProperty] }).reduce((acc, red) => acc + red)
                    / summarySource.length).toFixed(0);
                break;
            case 'count':
                summaryReducedValue = summarySource.length;
                break;
            case 'min':
                summaryReducedValue = d3.min(summarySource, function (d) { return d[param.summaryProperty] })
                break;
            case 'max':
                summaryReducedValue = d3.max(summarySource, function (d) { return d[param.summaryProperty] })
                break;
        }
        return summaryReducedValue;
    },
    getMoreKey: function (param, columns) {
        if (param.moreKey === null) {
            return;
        }
        let moreKey = param.moreKey.split(',');
        let moreKeyTitles = param.moreKeyTitle.split(',');

        moreKey.forEach((key, index) => {
            columns.push({
                'name': key,
                'title': moreKeyTitles[index]
            })
        });
    }
}