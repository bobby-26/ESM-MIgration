var sn;

if (sn === null || sn === undefined || sn === '') {
    sn ={ visual: {} };
}

sn.visual = {
    default: {
        holder: {
            defaultSize: 'small',
            size: {
                'small':    {width : 315, height: 200},
                'medium':   {width : 415, height: 300},
                'large':    {width : 515, height: 400},
            }
        },
        tile: {
            padding: 2,
            margin: 5,
            headerHeight: 20,
            backgroundColor: 'white',
            hostColor: '#0f2758'
        },
        decoration: {
            textSize: 8,
            fontSize: 8,
            fontFamily: 'Segoe UI'
        },
        margin : { left: 70, right: 50, top: 40, bottom: 70 },
        marginFullScreen: {left: 70, right: 80, top: 40, bottom: 70},
        colorScheme: ['pbi-default-bright', 'pbi-default-darker', 'pbi-default-dark', 'pbi-default-normal'],
        supress: {
            onExpandClick: false,
            printVisual: false,
            aboutVisual: false,
            downloadVisual: false,
            plotTotalOnVisual: false
        },
        maxZIndex: 2147483647,
        version: '2.0'
    },
    helper: {
        getUniqueId: function (prefix, length, suffix) {
            length = (length == null || length > 17) ? 17 : (length.length == 0) ? length + 1 : length;
            prefix = (prefix == null) ? '' : prefix;
            suffix = (suffix == null) ? '' : suffix;
            var randomNumber = Math.round(Math.random() * Math.pow(10, length));
            return (prefix + randomNumber + suffix).replace(/ /g, '');
        },
        _get_visual_holder_size: function (param) {
            let height = null,
             width = null,
             holderSize = null;
            holderSize = sn.visual.default.holder.size;
            switch (param.size) {
                case 'small':
                    width = holderSize.small.width;
                    height = holderSize.small.height;
                    break;
                case 'medium':
                    width = holderSize.medium.width;
                    height = holderSize.medium.height;
                    break;
                case 'large':
                    width = holderSize.large.width;
                    height = holderSize.large.height;
                    break;
                case '_full':
                    width = window.innerWidth;
                    height = window.innerHeight - 75;
                    break;
                case '_previous':
                    width = param._prev_width;
                    height = param._prev_height;
                    break;
                case '_about':
                    width = '800';
                    //height = '550';
                    height = window.innerHeight - 75;
                    break;
            }
            return {'width': width,'height': height};
        },
        getTitleAsHtml: function (param) {
            let result = this.getInteractIconAsHtml(param);

            if (_isNotNullOrEmpty(_get.d3.title(param['_id'])) == true) {
                result += `<label class="v-title v-text-overflow-hide">${param.chartTitle}</label>`;
            }

            return result;
        },
        getMargin() {
            return JSON.parse(JSON.stringify(sn.visual.default.margin));
        },
        getInteractIconAsHtml: function (param) {
            return param.interaction !== '' ? `<i class="fa fa-bolt v-interaction-icon"  title="This visual has Interaction with one or more Visuals"></i>&nbsp&nbsp` : '';
        },
        _tile_min_max: function (param, isAbout = false) {

            // preserve selection
            if (param.selection == null) {
                param.selection = {};
            }
            param.selection.data = [];
            _get.d3.visual(param._id).selectAll('.ve-inter-selected').each(function (d) {
                let sel = { _id: _isNotNullOrEmpty(d['_id']) ? d['_id'] : (d.data && _isNotNullOrEmpty(d.data['_id']) ? d.data['_id'] : 0) };
                param.selection.data.push(sel);
            });
            param._isAbout = isAbout;
            //remove the all elements
            d3.selectAll('#' + _get.id.visual(param._id) + ' > *').exit();
            d3.selectAll('#' + _get.id.visual(param._id) + ' > *').remove();
            /* proceeding to maximize */
            if (_get.d3.holder(param._id).classed('vh-full') == false) {

                d3.select('body').style('overflow', 'hidden');
                // param._margin.right = 80;
                param._margin.right = sn.visual.default.marginFullScreen.right;

                param._prev_width =  param.width;
                param._prev_height = param.height;
                // param.width  =   window.innerWidth - window.innerWidth * 0.5 - 0;
                param.width  =   window.innerWidth - window.innerWidth - 55 * 0.4;
                param.height  =  window.innerHeight - window.innerHeight * 0.25;
                sn.visual._core_lib.buttons.hideFullScreenIcons(param._id);
                window.onresize =
                    function () {
                        _get.d3.holder(param._id)
                            .style('width', window.innerWidth + 'px')
                            .style('height', window.innerHeight + 'px')
                    };

                _get.d3.holder(param._id)
                    .style('width', window.innerWidth + 'px')
                    .style('height', window.innerHeight + 'px')
                    .style('top', '-5px')
                    .style('left', '-5px');
            }
            else
                /* proceeding to restore */
            {
                d3.select('body').style('overflow', 'auto');

                sn.visual._core_lib.about.hide(param);
                let visualHolderSize = sn.visual.helper._get_visual_holder_size(param);
                param.width =  param._prev_width;
                param.height = param._prev_height;
                param._margin.right = sn.visual.default.margin.right;
                _get.d3.resizeFull(param._id)
                    .classed('fa-window-maximize', true)
                    .classed('fa-window-restore', false)
                    .attr('title', 'Resize to full')
                    .style('display', 'block') // this is for new style

                _get.d3.holder(param._id)
                    .classed('vh-full', false);

                _get.d3.holder(param._id)
                    .style('width', param.width + 'px')
                    .style('height', param.height + 20 + 'px')
                // _get.d3.title(param._id).style('width', (param.width - 30) + 'px');

                window.onresize = null;
            }
            param.recreateVisual(param);
        },
        baseTitle:  {
                create: function(g, title, margin, height, width) {
                    g.append("g")
                        .attr("transform", "translate(" + (width / 2) + " ," + (height + margin.bottom) + ")")
                        .append("text")
                        .attr('class', 'va-text-baseaxis-title')
                        .text(title);
                },
                update: function (g, title) {
                    g.select('.va-text-baseaxis-title')
                       .text(title);
                }
        },
        plotTitle:  {
                create: function (g, title, margin, height) {
                   let plotTitle =  g.append("g")
                        .attr("transform", 'translate(' + -margin.left + ',' + (height / 2) +  ')')
                        .attr('id', 'va-text-plotaxis-title')
                        .append("text")
                        .attr("transform", "rotate(-90)")
                        .attr("dy", "1em")
                        .text(title);
                        return plotTitle;
                },
                update: function (g, title) {
                    g.text(title);
                }
        },
        setPreSelection: function (param) {

        },
        plotTotalOnVisual: {
                _calculateTotal : function (param, visual) {
                    let result = {total: 0, label: null};
                    if (_get.d3.visual(param._id).selectAll('.ve-inter-selected').size() === 0)
                    { 
                        result.total =  d3.sum(param._data, function (d) {
                            return parseFloat(eval('d["' + param.plotAxisKey + '"]'));
                        });
                        result.total = result.total.toFixed(2);
                    } else {
                        visual.selectAll('.ve-inter-selected').each(function(d) {
                            result.total += sn.visual.helper.getDataFromVisual(d, param)[param.plotAxisKey];
                        });
                    }
                    result.label = param.aggregationTitle == "" ? "Sum : " :  param.aggregationTitle + " : " ;
                    
                    return result;
                },
                create: function (param, visual) {
                    if (this.plotTotalConfigChecker(param)) {
                        return;
                    }
                    let totalLabelElement, totalValueElement;
                    let totalValue = this._calculateTotal(param, visual);

                    let tickFormat = param.plotAxisTickFormat;
                    if (_isNullOrEmpty(tickFormat) == true) {
                        tickFormat = '.0f';
                    }

                    let plotTotal = visual.append('g')
                        .attr('class', 'v-text-noselect v-plot-total')    
                        .append('text')
                        .attr('transform', `translate(${ parseInt(visual.style('width')) - 30 }, 15)`);

                    totalLabelElement = plotTotal.append('tspan')
                        .attr('class', 'v-plot-total-label')
                        .text(totalValue.label)

                    totalValueElement = plotTotal.append('tspan')
                        .attr('class', 'v-plot-total-value')
                        .text(d3.format(tickFormat)(totalValue.total));
                },
                update: function (param, visual) {
                    if (this.plotTotalConfigChecker(param)) {
                        return;
                    }
                    let totalValue = this._calculateTotal(param, visual);
                    let plotTotal = visual.select('.v-plot-total');
                    let totalLabelElement = plotTotal.select('.v-plot-total-label');
                    let totalValueElement = plotTotal.select('.v-plot-total-value');
                    totalLabelElement.text(totalValue.label);
                    totalValueElement.text(totalValue.total);
                },
                plotTotalConfigChecker: function (param) {
                    if (sn.visual.default.supress.plotTotalOnVisual === true) {
                        return true;
                    }
                    return false;
                }
        },
        switchSelection: function (param, currElem) {
                 // ctrl select 
                if (d3.event.ctrlKey == true) {
                    d3.select(currElem).classed('ve-inter-selected') === true ?
                                    d3.select(currElem).classed('ve-inter-selected', false) :
                                    d3.select(currElem).classed('ve-inter-selected', true);

                    if (_get.d3.visual(param._id).selectAll('.ve-inter-unselected').size() === _get.d3.visual(param._id).selectAll('.ve-default').size()
                        && _get.d3.visual(param._id).selectAll('.ve-inter-selected').size() === 0
                    ) {
                        _get.d3.visual(param._id).selectAll('.ve-default').each(function (d, i, node) {
                            d3.select(node[i]).classed('ve-inter-unselected', false);
                            d3.select(node[i]).classed('ve-inter-selected', false)
                        });
                    }
                    else if (_get.d3.visual(param._id).selectAll('.ve-inter-selected').size() === _get.d3.visual(param._id).selectAll('.ve-default').size()) {
                        _get.d3.visual(param._id).selectAll('.ve-default').each(function (d, i, node) {
                            d3.select(node[i]).classed('ve-inter-unselected', false);
                            d3.select(node[i]).classed('ve-inter-selected', false)
                        });
                    }
                    else {
                        _get.d3.visual(param._id).selectAll('.ve-default').each(function (d, i, node) {
                            d3.select(node[i]).classed('ve-inter-unselected', true);
                        });
                    }
                } 
                else {
                    d3.select(currElem).classed('ve-inter-selected') === true ?
                              d3.select(currElem).classed('ve-inter-selected', false) :
                              d3.select(currElem).classed('ve-inter-selected', true);
                    if (_get.d3.visual(param._id).selectAll('.ve-inter-selected').size() > 1) {
                        // exclude the current element and resetes the chart
                        _get.d3.visual(param._id).selectAll('.ve-default').filter(d => { 
                            //sn.visual.helper.getDataFromVisual(d3.select(currElem).data()[0], d[param.baseAxisKey]);
                            let selectedElementData = sn.visual.helper.getDataFromVisual(d3.select(currElem).data()[0], param);
                            let data = sn.visual.helper.getDataFromVisual(d, param);
                            //return d3.select(currElem).data()[0][param.baseAxisKey] !=  d[param.baseAxisKey]; 
                                 return selectedElementData[param.baseAxisKey] !=  data[param.baseAxisKey]; 
                            })
                            .each(function (d, i, node) {
                                d3.select(node[i]).classed('ve-inter-unselected', true);
                                d3.select(node[i]).classed('ve-inter-selected', false)
                            });
                    }
                    else if (_get.d3.visual(param._id).selectAll('.ve-inter-selected').size() === 0 ) {
                        // resets the chart
                            _get.d3.visual(param._id).selectAll('.ve-default').each(function (d, i, node) {
                                d3.select(node[i]).classed('ve-inter-unselected', false);
                                d3.select(node[i]).classed('ve-inter-selected', false);
                            });
                    } else {
                        // unselect the rest 
                        _get.d3.visual(param._id).selectAll('.ve-default').each(function (d, i, node) {
                            d3.select(node[i]).classed('ve-inter-unselected', true);
                        });
                    }
                }
        },
        export : {
                print: function (param) {
                    // set title in svg before print
                    _get.d3.svgTitle(param._id).text(param.chartTitle);

                    // print
                    let printWindow = window.open(_emptyStr, 'PrintMap', `width=${screen.availWidth}, height=${screen.availHeight}`);
                    printWindow.document.writeln(_get.d3.visual(param._id).node().outerHTML);
                    printWindow.document.close();
                    printWindow.print();
                    printWindow.close();

                    // clear tht title in svg after print
                    _get.d3.svgTitle(param._id).text(_emptyStr);
                },
                image: function (param) {
                    function copyStylesInline(destinationNode, sourceNode) {
                        let containerElements = ["svg", "g"];
                        for (let cd = 0; cd < destinationNode.childNodes.length; cd++) {
                            let child = destinationNode.childNodes[cd];
                            if (containerElements.indexOf(child.tagName) != -1) {
                                copyStylesInline(child, sourceNode.childNodes[cd]);
                                continue;
                            }
                            let style = sourceNode.childNodes[cd].currentStyle || window.getComputedStyle(sourceNode.childNodes[cd]);
                            if (style == "undefined" || style == null) continue;
                            for (let st = 0; st < style.length; st++) {
                                child.style.setProperty(style[st], style.getPropertyValue(style[st]));
                            }
                        }
                    }
                    function triggerDownload(imgURI, fileName) {
                        let evt = new MouseEvent("click", {
                            view: window,
                            bubbles: false,
                            cancelable: true
                        });
                        let a = document.createElement("a");
                        a.setAttribute("download", fileName);
                        a.setAttribute("href", imgURI);
                        a.setAttribute("target", '_blank');
                        a.dispatchEvent(evt);
                    }
                    function downloadSvg(svg, fileName) {
                        let copy = svg.cloneNode(true);
                        copyStylesInline(copy, svg);
                        let canvas = document.createElement("canvas");
                        let bcr = svg.getBoundingClientRect();
                        canvas.width = bcr.width + 20;
                        canvas.height = bcr.height + 20;
                        let ctx = canvas.getContext("2d");
                        ctx.clearRect(10, 10, bcr.width, bcr.height);
                        let data = (new XMLSerializer()).serializeToString(copy);
                        let DOMURL = window.URL || window.webkitURL || window;
                        let img = new Image();
                        let svgBlob = new Blob([data], { type: "image/svg+xml;charset=utf-8" });
                        let url = DOMURL.createObjectURL(svgBlob);
                        img.onload = function () {
                            ctx.drawImage(img, 0, 0);
                            DOMURL.revokeObjectURL(url);
                            if (typeof navigator !== "undefined" && navigator.msSaveOrOpenBlob) {
                                let blob = canvas.msToBlob();
                                navigator.msSaveOrOpenBlob(blob, fileName);
                            }
                            else {
                                let imgURI = canvas
                                    .toDataURL("image/png")
                                    .replace("image/png", "image/octet-stream");
                                triggerDownload(imgURI, fileName);
                            }
                            document.removeChild(canvas);
                        };
                        img.src = url;
                    }

                    _get.d3.svgTitle(param._id).text(param.chartTitle);
                    downloadSvg(_get.d3.visual(param._id).node(), `${param.chartTitle}.png`);
                    _get.d3.svgTitle(param._id).text(_emptyStr);
                }
        },
        interactionHelper: {
                findVisualParam: function (param) {
                    return sn.visual._model.filter(mod => param.interaction.includes(mod.name))
                },
                interact: function (param, selectedValues) {
                    const model = this.findVisualParam(param);
                    model.forEach(mod => {
                        //    let selectedValues =  sn.visual._core_lib.eventHandler._getSelectionData(param);
                        mod.param._visual.update(mod.id, selectedValues);
                    });
                }
        },
        getNestedDataConvetor: function (key, data) {
                let result = d3.nest()
                .key((d) => {return d[key]})
                .entries(data);
                return result;
        },
        chartCreator: {
                create: function (param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, chartObj) {
                    try {
                        return chartObj.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick);
                    } catch(error) {
                        console.error(error);
                    }
                },
                update: function (id, data, chartObj) {
                    try {
                        return chartObj.update(id, data)
                    }
                    catch(error) {
                        console.error(error);
                    }
                }
        },
        dataConvetor: {
                getBarGroupMetaData: function (param) {
                    let result = [];
                    let morekey = sn.visual.helper.dataConvetor.getMoreKey(param);
                    let metaData = sn.visual.helper.dataConvetor.getMetaData(param);
                    param._moreKey
                    result = result.concat(morekey);
                    result = result.concat(metaData);
                    return result;
                },
                getMoreKey: function(param) {
                    let result = [];
                    let moreKeyTitle = param.moreKeyTitle.split(',');
                    param._cMoreKeyTitle = moreKeyTitle;
                    param._cMoreKey =  param.moreKey.split(',');
                   
                    param._cMoreKey.forEach((key, index) => {
                        result.push({
                            'name': moreKeyTitle[index],
                            'value': key
                        });
                    });
                    return result;
                },
                getMetaData: function(param) {
                    let result = {'name': param.baseAxisGroupKey, 'metaValue': []};
                    let subGroupKeys = param.subGroupKey.split(',');
                    let subGroupKeyTitles = param.subGroupKeyTitle.split(',');
                    
                    param._cSubGroupKeys = subGroupKeys;
                    param._cSubGroupKeyTitles = subGroupKeyTitles;

                    subGroupKeys.forEach((subgroup, index) => {
                        result.metaValue.push({
                            'name': subGroupKeyTitles[index],
                            'value': subgroup
                        });
                    });
                    return result;
                },
                barGroupDataConvetor: function (param, metaData) {
                    const groupedData = [];
                    param._data.forEach(function (item) {
                        const newItem = {};
                        metaData.forEach(meta => {
                            if (Array.isArray(meta.metaValue) && meta.metaValue.length > 0) {
                                newItem[meta.name] = [];
                                meta.metaValue.forEach(metaValue => {
                                    const obj = {};
                                    obj['key'] = metaValue.name;
                                    obj['value'] = item[metaValue.value];
                                    newItem[meta.name].push(obj);
                                });
                            } else {
                                newItem[meta.name] = item[meta.value];
                            }
                        });
                        groupedData.push(newItem);
                    });
                    return groupedData;
                },
                barGroupWODataConvetor: function(param, data) {
                    const tempData = d3.nest()
                        .key(function (dataRow) {
                            const temp = {};
                            temp[param.mainGroupProperty] = dataRow[param.mainGroupProperty]
                            if (param.mainGroupPropertyExt != null ) {
                                param.mainGroupPropertyExt.split(',').forEach(item => {
                                    temp[item] = dataRow[item];
                                });
                            }
                            return JSON.stringify(temp);
                        })
                        .key(function (dataRow) {
                            const temp = {};
                            temp[param.subGroupProperty] = dataRow[param.subGroupProperty];
                            if (param.subGroupPropertyExt != null ) {
                                param.subGroupPropertyExt.split(',').forEach(item => {
                                    temp[item] = dataRow[item];
                                });
                            }

                            return JSON.stringify(temp);
                        })
                        .rollup(function (subGroup) {
                            return d3.sum(subGroup, function (dataItem) {
                                return dataItem[param.valueProperty];
                            });
                        }).entries(data);

                    const groupedData = [];
                    tempData.forEach(function (sourceParent) {
                        const targetParent = {};
                        const tempDatas = JSON.parse(sourceParent['key']);
                        targetParent[param.mainGroupProperty] = tempDatas[param.mainGroupProperty];
                        if (param.mainGroupPropertyExt != null ) {
                            param.mainGroupPropertyExt.split(',').forEach(item => {
                                targetParent[item] = tempDatas[item];
                            });
                        }
                      
                        targetParent[param.baseAxisGroupKey] = [];

                        sourceParent['values'].forEach(function (sourceData) {
                            let subGroupProperty = param.subGroupProperty;
                            sourceData.key = JSON.parse(sourceData.key);

                            const temp = {};
                            temp['key'] = sourceData.key[subGroupProperty];
                            temp['value'] = sourceData['value'];

                            if (param.subGroupPropertyExt != null ) {
                                param.subGroupPropertyExt.split(',').forEach(item => {
                                    temp[item] = sourceData.key[item];
                                });
                            }
                            targetParent[param.baseAxisGroupKey].push(temp);
                        });
                        groupedData.push(targetParent);
                    });
                    return groupedData;
                },
                getArcData: function(arcdata) {
                    return arcdata.data;
                },
        },
        resetChart: function(param) {
                _get.d3.visual(param._id).selectAll('.ve-default').each(function(d) {
                    d3.select(this).classed('ve-inter-unselected', false)
                    d3.select(this).classed('ve-inter-selected', false)
                })
        },
        getDataFromVisual: function(selectedData, param) {
                switch (param._chartType) {
                    case 'pie':
                        return selectedData.data;
                        break;
                    case 'donut':
                        return selectedData.data;
                        break;
                    default:
                        return selectedData;
                }
        },
        getVisualCursor: function(param) {
            return param.interaction !== '' ? 'v-cursor-pointer' : '';
        },
        isNullOrUndefined: function(value) {
            return value === undefined || value === null ? true : false;
        },
        getPlotAxisLabel: function(max) {
            return Math.abs(Number(max)) >= 1.0e+9

            ? "Billions"
            // Six Zeroes for Millions 
            : Math.abs(Number(max)) >= 1.0e+6
        
            ? "Millions"
            // Three Zeroes for Thousands
            : Math.abs(Number(max)) >= 1.0e+3
        
            ? "Thousands"
            // return nothing 
            : '';
        }
    },
    _core_lib: {
            _get_visual_ground_created: function (param) {

                try {
                    param._id = sn.visual.helper.getUniqueId();
                    let visualHolderSize = sn.visual.helper._get_visual_holder_size(param);
                    var visualHolder = d3.select(param.elementSelector)
                        .append('div')
                        .attr('id', _get.id.holder(param._id))
                        .attr('class', 'vh-default row')
                        .style('width', visualHolderSize.width + 'px')
                        .style('height', (visualHolderSize.height + 20) + 'px')
                        .style('margin', sn.visual.default.tile.margin + 'px')
                        .style('padding', sn.visual.default.tile.padding + 'px')
                        .style('background-color', sn.visual.default.tile.backgroundColor)
                        .style('border-color', sn.visual.default.tile.hostColor);

                    param.width = visualHolderSize.width;
                    param.height = visualHolderSize.height;
                    // sn.visual._core_lib.about.create(param);

                    return visualHolder;
                } catch (err) {
                    console.error(err);
                }
            },
            _set_tool_button_visibility: function (param, visible) {
                //let flashButton = _get.d3.titleContainer(param._id).select('.glyphicon');
                let flashButton = _get.d3.titleContainer(param._id).select('.v-interaction-icon');
                let infoButton = _get.d3.info(param._id);
                let resizeFullButton = _get.d3.resizeFull(param._id);
                let expandButton = _get.d3.expand(param._id);
                let printButton = _get.d3.print(param._id);
                let downloadButton = _get.d3.downloadPng(param._id);
                let refreshButton = _get.d3.refresh(param._id);

                if (param._data.length > 0
                    && _get.d3.holder(param._id).classed('vh-full') == false)
                {
                    let displayValue = visible == true ? 'block' : 'none'

                    if (flashButton.size() > 0) {
                        flashButton.style('display', displayValue);
                    }
                    if (infoButton.size() > 0) {
                        infoButton.style('display', displayValue);
                    }
                    if (resizeFullButton.size() > 0) {
                        resizeFullButton.style('display', displayValue);
                    }
                    if (expandButton.size() > 0) {
                        expandButton.style('display', displayValue);
                    }
                    if (printButton.size() > 0) {
                        printButton.style('display', displayValue);
                    }
                    if (downloadButton.size() > 0) {
                        downloadButton.style('display', displayValue);
                    }
                    if (refreshButton.size() > 0) {
                        refreshButton.style('display', displayValue);
                    }
                }
            },
            svgCreator: {
                visual: null,
                create(param) {
                    let visualSize = sn.visual.helper._get_visual_holder_size(param);
                    var visualHolder = _get.d3.holder(param._id);
                    this.visual = visualHolder.append('svg')
                                    .attr('class', 'v-shadow');

                    this.visual.attr('id', _get.id.visual(param._id))
                     .style('width', visualSize.width + 'px')
                     .style('height', visualSize.height + 'px')
                     .style('float', 'left')
                    return this.visual;
                },
                update: function (param) {
                    let visual =   param._isAbout === true ? this._showAbout(param) : this._showFullScreen(param);
                    return visual;
                },
                _showFullScreen: function (param) {
                    param.size = _get.d3.holder(param._id).classed('vh-full') === true ? '_full' : '_previous';
                    let visualSize = sn.visual.helper._get_visual_holder_size(param);
                    let visual = _get.d3.visual(param._id);
                    visual.style('width', visualSize.width + 'px')
                    .style('height', visualSize.height + 'px')
                    return visual;
                },
                _showAbout: function (param) {
                    param.size = '_about';
                    let visualSize = sn.visual.helper._get_visual_holder_size(param);
                    let visual = _get.d3.visual(param._id);
                    visual.style('width', visualSize.width + "px")
                    .style('height', visualSize.height + "px");
                    return visual;
                }
            },
            header: {
                _header: null,
                create: function(param) {
                    let isFullScreen = _get.d3.holder(param._id).classed('vh-full');

                    let holder = _get.d3.holder(param._id);
                    let header =  holder
                                    .append('div')
                                    .attr('id', _get.id.header(param._id))
                                    .attr('class', 'v-header')
                                    //.style('font-size', 'x-small')
                                    .style('height', `${sn.visual.default.tile.headerHeight}px`)
                                    //.style('width', '100%')

                    let titleContainer = _get.d3.titleContainer(param._id);

                    if (titleContainer.size() == 0 ) {
                        titleContainer = header.append('div')
                            .attr('id', _get.id.titleContainer(param._id))
                            .attr('class', 'v-title')
                            //.style('float', 'left');
                    }

                    _get.d3.info(param._id).style('display', isFullScreen ? 'none' : 'block');

                    if (titleContainer.selectAll('div').size() < 1) {
                        //<td id="back_button_separator" style="border-left: 1px solid gray; border-right: 1px solid gray; padding-right: 0px;"></td>
                        titleContainer.append('div')
                                .html(` <table class="v-text-noselect" style="z-order: 9999999;">
                                <tr>
                                <td id="${_get.id.resizeSmall(param._id)}_2" title="Restore back to normal size" style="font-size: x-small; font-weight: 800; background-color: black; color: white; border-radius: 11px; cursor: pointer; opacity: 0.7;">
                                <span class ="fa fa-arrow-circle-left" style="font-size: small; color: white; cursor: pointer; opacity: 0.8;">&nbsp; Back &nbsp; </span>
                                 </td>
                                <td class ="v-head-text-holder" style="font-size: small; font-weight:800;">
                                <div id="${_get.id.title(param._id)}">${sn.visual.helper.getTitleAsHtml(param)}</div>
                                </td>
                                </tr>
                                </table>
                        `);
                        d3.selectAll('#' + _get.id.resizeSmall(param._id) + '_1').on('click', function () {
                            sn.visual.helper._tile_min_max(param);
                        })
                        d3.selectAll('#' + _get.id.resizeSmall(param._id) + '_2').on('click', function () {
                            sn.visual.helper._tile_min_max(param);
                        })
                    }
                    _get.d3.title(param._id).html(sn.visual.helper.getTitleAsHtml(param));

                    titleContainer.selectAll('#back_button_separator').style('display', isFullScreen ? 'block' : 'none');
                    d3.select(`#${_get.id.resizeSmall(param._id)}_2`).style('display', isFullScreen ? 'block' : 'none');

                    let titleWidth = parseInt(_get.d3.holder(param._id).style('width'));
                    titleWidth = titleWidth - titleWidth / 3;
                    let theTitleLabel = _get.d3.title(param._id).select('label');

                    theTitleLabel.style('width', `${titleWidth - 25}px`);
                    _get.d3.title(param._id).style('width', `${titleWidth}px`);
                    _set.element.text_as_title(theTitleLabel.node());

                    // theTitleLabel.style('padding-top', `${isFullScreen? 0 : 2}px`)

                    header
                        .style('margin', '-' + sn.visual.default.tile.padding + 'px')
                        .style('padding', '2px')
                        .style('border-radius', holder.style('border-radius'))
                        .style('font-size', 'x-small')
                        .style('font-weight', '900')
                        .style('border-bottom-left-radius', '0px')
                        .style('border-bottom-right-radius', '0px');
                    return header;
                },
                update: function (param) {
                    let isFullScreen = _get.d3.holder(param._id).classed('vh-full');
                    let titleContainer = _get.d3.titleContainer(param._id);

                    let titleWidth = parseInt(_get.d3.holder(param._id).style('width'));
                    titleWidth = titleWidth - titleWidth / 3;
                    let theTitleLabel = _get.d3.title(param._id).select('label');

                    theTitleLabel.style('width', `${titleWidth - 25}px`);
                    _get.d3.title(param._id).style('width', `${titleWidth}px`);

                    titleContainer.selectAll('#back_button_separator').style('display', isFullScreen ? 'block' : 'none');
                    d3.select(`#${_get.id.resizeSmall(param._id)}_2`).style('display', isFullScreen ? 'block' : 'none');
                }
            },
            loader: {
                _timerId: null,
                create: function(param) {
                    function degToRad (degrees) {
                        return degrees * Math.PI / 180;
                    }

                    // Returns a tween for a transition�s "d" attribute, transitioning any selected
                    // arcs from their current angle to the specified new angle.
                    function arcTween(newAngle, angle) {
                        return function(d) {
                            var interpolate = d3.interpolate(d[angle], newAngle);
                            return function(t) {
                                d[angle] = interpolate(t);
                                return arc(d);
                            };
                        };
                    }

                    const animationTime = 1200;
                    const loaderRadius = 60;
                    const loaderColor = '#ccc';

                    var arc = d3.arc()
                        .innerRadius(0)
                        .outerRadius(loaderRadius);
                    // create svg
                    var svg = _get.d3.holder(param._id)
                        .append('svg')
                        .attr('class', 'v-loader')
                        .style('display', 'none')
                        .style('width', param.width + 'px')
                        .style('height', param.height + 'px')
                        .attr('id', _get.id.loader(param._id)),
                        width = param.width,
                        height = param.height,
                        g = svg.append("g").attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");
                            
                    var loader = g.append("path")
                        .datum({endAngle: 0, startAngle: 0})
                        .style("fill", loaderColor)
                        .attr("d", arc);

                    var loaderText = g
                    .append('g')
                    .attr('transform', 'translate(-10, 75)')
                    .append('text')
                    .text('Loading...');

                   this._timerId = d3.interval(function() {
                        loader.datum({endAngle: 0, startAngle: 0})
  
                        loader.transition()
                            .duration(animationTime)
                            .attrTween("d", arcTween(degToRad(360), 'endAngle'));
  
                        loader.transition()
                           .delay(animationTime)
                           .duration(animationTime)
                           .attrTween("d", arcTween(degToRad(360), 'startAngle'));
                    }, animationTime * 2);
                    return this;
                },
                show: function(param) {
                    _get.d3.loader(param._id).style('display', 'block');
                    _get.d3.visual(param._id).style('display', 'none');
                },
                hide: function(param) {
                    _get.d3.loader(param._id).style('display', 'none');
                    _get.d3.visual(param._id).style('display', 'block');
                    clearInterval(this._timerId);
                }
            },
            tooltip: {
                create: function() {
                    try {
                        sn.visual._core_lib._tooltip._create();
                    } catch (err) {
                        console.error('sn.visual - tooltip module not imported - ' + error);
                    }
                    return this;
                },
                show: function (data, param) {
                    try {
                        sn.visual._core_lib._tooltip._show(data, param)
                    } catch (error) {
                        console.error('sn.visual - tooltip module not imported - '  + error);
                    }
                },
                hide: function () {
                    try {
                        sn.visual._core_lib._tooltip._hide();
                    } catch (error) {
                        console.error('sn.visual - tooltip module not imported - '+ error);
                    }
                },
                get: function() {
                    return this;
                }
            },
            about: {
                prepare: function (param) {
                    try {
                        sn.visual._core_lib._about._prepare(param);
                    } catch (error) {
                        console.error('sn.visual - about module not imported');
                    }
                },
                create: function (param, data) {
                    try {
                        sn.visual._core_lib._about._create(param, data);
                    } catch (error) {
                        console.error('sn.visual - about module not imported');
                    }
                },
                show: function (param) {
                    try {
                        sn.visual._core_lib._about._show(param);
                    } catch (error) {
                        console.error('sn.visual - about module not imported');
                    }
                },
                hide: function (param) {
                    try {
                        sn.visual._core_lib._about._hide(param);
                    } catch (error) {
                        console.error('sn.visual - about module not imported');
                    }
                },
                aggregate: function (param) {
                    try {
                        sn.visual._core_lib._about._aggregate(param);
                    } catch (error) {
                        console.error('sn.visual - about module not imported');
                    }
                }
            },
            buttons: {
                create: function (container, param) {

                    // need to check the supress option and library add it

                    if (sn.visual.default.supress.onExpandClick) {
                        container.append('span')  // button to expand
                            .attr('id', 'expand' + param._id)
                            .attr('title', 'Expand')
                            .attr('class', 'fa fa-external-link')
                            .attr('style', `float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer; display: block;`)
                            .style('color', container.style('color'))
                            .on('click', function () {
                                param.listener.onExpandClick();
                            });
                    }

                    if (sn.visual.default.supress.onRefreshClick) {
                        container.append('span')  // button to refresh
                            .attr('id', 'refresh' + param._id)
                            .attr('title', 'Refresh')
                            .attr('class', 'fa fa-refresh')
                            .attr('style', 'float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer;')
                            .style('color', container.style('color'))
                            .on('click', function () {
                                param.listener.onRefreshClick();
                            });
                    }

                    if (param.type === 'table') {

                        let editIcon = container.append('span')  // button to filter
                           .attr('id', 'editable' + param._id)
                           .attr('title', 'Edit')
                           .attr('class', 'fa fa-edit')
                           .attr('style', 'float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer;')
                           .style('color', container.style('color'))
                           .on('click', function () {
                               if (_get.id.editable(param._id).classed('vt-editable-icon') === false) {
                                   _get.id.editable(param._id).classed('vt-editable-icon', true);
                                   sn.visual.helper.editColumnColorChange(param);
                               } else {
                                   _get.id.editable(param._id).classed('vt-editable-icon', false);
                                   sn.visual.helper.editColumnColorChange(param);
                               }
                           });
                    }

                    container.append('span') // maximize / restore
                        .attr('id', _get.id.resizeFull(param._id))
                        .attr('title', 'Resize to full')
                        .attr('class', 'fa fa-window-maximize')
                        .attr('style', `float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer; display: ${param._data.length > 0 ? "block" : "none"}`)
                        .style('color', container.style('color'))
                        .on('click', function () {
                            sn.visual.helper._tile_min_max(param);
                        });

                    if (sn.visual.default.supress.printVisual == false) {
                        container.append('span') // print
                            .attr('id', _get.id.print(param._id))
                            .attr('title', 'Print this visual')
                            .attr('class', 'fa fa-print')
                            .attr('style', `float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer; display: ${param._data.length > 0 ? "block" : "none"}`)
                            .style('color', container.style('color'))
                            .on('click', function () {
                                // sn.visual.helper.printVisual(param);
                                sn.visual.helper.export.print(param);
                            });
                    }

                    if (sn.visual.default.supress.downloadVisual == false) {
                        container.append('span')
                            .attr('id', _get.id.downloadPng(param._id))
                            .attr('title', 'Download this visual as png')
                            .attr('class', 'fa fa-download')
                            .attr('style', `float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer; display: ${param._data.length > 0 ? "block" : "none"}`)
                            .style('color', container.style('color'))
                            .on('click', function () {
                                // sn.visual.helper.saveAsPng(param);
                                sn.visual.helper.export.image(param);
                            });
                    }

                    if (sn.visual.default.supress.aboutVisual == false) {
                        let temp = container.append('text')
                            .attr('id', `_temp_about_visual${param._id}`)
                            .style('display', 'none')
                            .html(param.basic && param.basic.about && _isNotNullOrEmpty(param.basic.about.aboutVisual) ? param.basic.about.aboutVisual : _emptyStr);

                        let title = temp.text();
                        title = title.substring(0, 50).trim();
                        let isEmpty = title.length == 0;
                        title = title + (isEmpty == false ? '...' : _emptyStr)

                        container.append('span')
                            .attr('id', 'info_' + param._id)
                            .attr('title', title + (_isNullOrEmpty(title) == false ? (isEmpty == false ? ' | ' : _emptyStr) : _emptyStr) + 'More on this Visual')
                            .attr('class', 'fa fa-info')
                            .attr('style', `float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer; display: ${param._data.length > 0 ? "block" : "none"}`)
                            .style('color', container.style('color'))
                            .on('click', function () {
                                // sn.visual.helper._tile_min_max(param, { 'show-about': true });
                                sn.visual._core_lib.about.show(param);
                                sn.visual.helper._tile_min_max(param, true);
                            });

                        temp.remove();
                    }
                },
                hideFullScreenIcons: function (id) {
                    _get.d3.resizeFull(id).style('display', 'none');

                    _get.d3.info(id).style('display', 'none');

                    _get.d3.holder(id).style('z-index', sn.visual.default.maxZIndex)
                      .classed('vh-full', true);
                }
            },
            color: {
                _colors: [],
                _prepareColorFromTheme: function () {
                    if (this._colors.length === 0) {
                        sn.visual.default.colorScheme.forEach(schemes => {
                            this._colors = this._colors.concat(sn.visual._color_scheme[schemes]);
                        });
                    }
                    return this._colors;
                },
                getSeriesColor: function () {
                    let colors = this._prepareColorFromTheme();
                    return colors;
                },
                getRandomColors: function (colorLength) {
                    let result = this._prepareColorFromTheme();
                    let colors = [];
                    for (let index = 0; index < colorLength; index++) {
                        colors.push(result[Math.round(Math.random() * result.length)]);
                    }
                    return colors;
                },
                getSingleColor: function () {
                    let colors = this.getSeriesColor();
                    return colors.slice(0, 1);
                }
            },
            eventHandler: {
                _getSelectionData: function (param) {
                    let selectedValues = [];
                    let selections = [];
                    if (_get.d3.visual(param._id).selectAll('.ve-inter-selected').size() === 0) {
                        selections = _get.d3.visual(param._id).selectAll('.ve-default');
                    }else {
                        selections = _get.d3.visual(param._id).selectAll('.ve-inter-selected');
                    }
                    selections.each((d) => {
                        param._dataType === 'internal' ?  selectedValues.push(d.data) : selectedValues.push(d);
                    });
                    return selectedValues;
                },
                element_click_handling: function (param, selectedData) {
                    sn.visual._core_lib.eventHandler._element_click_handling(param, selectedData);
                },
                _element_click_handling: function (param, selectedValues) {
                    let getSelectedData = this._getSelectionData(param);
                    typeof(param._visualClick) === 'function' ? param._visualClick(getSelectedData, param) : null;
                },
                baseAxis_click_handling: function (param, selectedValues) {
                    typeof(param._visualClick) === 'function' ? param._baseAxisClick(selectedValues, param) : null;
                },
                onAboutVisualEdited: function (params) {
                
                },
                checkBaseAxisHandler: function (param) {
                    if (param._element) {
                    
                    }
                },
                checkElementClickHandler: function (param) {
               
                }
            },
            configuration: {
                setDefault: function (param) {
                    // prepare the core library
                    let defaultParams = sn.visual.default;
                    // param.supress.supress = ;
                }
            },
            legend: {
                _prepareLegends: function (param) {
                    let legends = param._data.map(d => {
                        return d[param.baseAxisKey];
                    });
                    return legends;
                },
                create: function (param) {
                    if (_get.d3.holder(param._id).classed('vh-full') === false) {
                        return;
                    }
                    let visual = _get.d3.visual(param._id);
                    let legends = this._prepareLegends(param);

                    //legend group
                    let legendGroup = visual
                                        .append('g')
                                        .attr('class', 'v-legend')
                                        // .attr('transform', 'translate( '+ (param._width - 75) + ' , '+ 15 + ')' )
                                        .attr('transform', 'translate( '+ (param._width + param._margin.right) + ' , '+ 15 + ')' )

                    legends.forEach((leg, i) => {
                        let group = legendGroup
                            .append('g')
                            .attr('class', 'v-legend-row')
                            .attr('transform','translate( 0 , '+ (i * 20) + ')');

                        group.append('rect')
                            .attr('width', '10px')
                            .attr('height', '10px')
                            .attr('fill', param._color(i));

                        group.append('text')
                            .attr("x", 15)
                            .attr("y", 10)
                            .text(leg);
                    });
                },
                update: function (param) {
                    let visual = _get.d3.visual(param._id);
                    visual.selectAll('.v-legend-row').remove().exit();
                    this.create(param);
                }
            },
            model: {
                add: function (param, visual) {
                    sn.visual._model.push({
                        'id': param._id,
                        'name': param.name,
                        'param': param,
                        'visual': visual
                    });
                }
            }
        },
        _model: [],
        _color_scheme: {
            'pbi-default-darker': [
                '#666666',
                '#000000',
                '#015C55',
                '#1C2325',
                '#7F312F',
                '#796408',
                '#303637',
                '#456A76',
                '#7F4B33'
            ],
            'pbi-default-dark': [
                '#808080',
                '#1A1A1A',
                '#018A80',
                '#293537',
                '#BE4A47',
                '#B6960B',
                '#475052',
                '#689FB0',
                '#BF714D'
            ],
            'pbi-default-normal': [
                '#B3B3B3',
                '#333333',
                '#34C6BB',
                '#5F6B6D',
                '#FD817E',
                '#F5D33F',
                '#7F898A',
                '#A1DDEF',
                '#FEAB85'
            ],
            'pbi-default-light': [
                '#CCCCCC',
                '#666666',
                '#67D4CC',
                '#879092',
                '#FEA19E',
                '#F7DE6F',
                '#9FA6A7',
                '#B9E5F3',
                '#FEC0A3'
            ],
            'pbi-default-lighter': [
                '#e6e6e6',
                '#999999',
                '#99e3dd',
                '#afb5b6',
                '#fec0bf',
                '#fae99f',
                '#bfc4c5',
                '#d0eef7',
                '#ffd5c2'
            ],
            'pbi-default-bright': [
                '#01b8aa',
                '#374649',
                '#fd625e',
                '#f2c80f',
                '#5f6b6d',
                '#8ad4eb',
                '#fe9666',
                '#000000'
            ]
        },
        getData: function(url, fnSuccess, fnError) {
            d3.json(url, { method: 'POST' }).then(function(data) {
                if (typeof(fnSuccess) !== 'function') {
                    console.error('you must pass the function as parameter');
                }
                fnSuccess(data);
            }).catch(function(error) {
                if (typeof(fnError) !== 'function') {
                    console.error('you must pass the function as parameter');
                }
                fnError(error);
            });
        },
        bar: {
            vertical: {
                create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                    return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual.bar._vertical);
                },
                update: function(id, data) {
                    return sn.visual.helper.chartCreator.update(id, data, sn.visual.bar._vertical);
                }
            },
            horizontal: {
                create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                    return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual.bar._horizontal);
                },
                update:  function (id, data) {
                    return sn.visual.helper.chartCreator.update(id, data, sn.visual.bar._horizontal);
                }
            },
        },
        barGrouped: {
            vertical: {
                create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                    return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual.barGrouped._vertical);
                },
                update:  function (id, data) {
                    return sn.visual.helper.chartCreator.update(id, data, sn.visual.barGrouped._vertical);
                }
            },
            horizontal: {

            }
        
        },
        line: {
            create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual._line);
            },
            update:  function (id, data) {
                return sn.visual.helper.chartCreator.update(id, data, sn.visual._line);
            }
        },
        lineGrouped: {
            vertical: {},
            horizontal: {},
        },
        pie: {
            create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual._pie);
            },
            update:  function (id, data) {
                return sn.visual.helper.chartCreator.update(id, data, sn.visual._pie);
            }
        },
        donut: {
            create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual._donut);
            },
            update:  function (id, data) {
                return sn.visual.helper.chartCreator.update(id, data, sn.visual._donut);
            }
        },
        table: {
            create: function(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick) {
                return sn.visual.helper.chartCreator.create(param, data, fnVisualClick, fnBaseAxisClick, fnAboutVisualEdited, fnRefreshClick, sn.visual._table);
            },
            update:  function (id, data) {
                return sn.visual.helper.chartCreator.update(id, data, sn.visual._table);
            }
        },
    }