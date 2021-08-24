/* ================================================================
 * SN.Visual: sn.visual.js
 * ================================================================
 * © Copyright 2018 South Nests Software Solutions Pvt. Ltd.
 * 
 * A visual library which renders JSON data into different kinds of 
 * graphical forms like table, charts, etc. which are interactive.
 * 
 * ---- Version History ----
 * Version, Released
 * -------------------------
 * 1.6.5,  16th May     2018
 * 1.6.4,  14th May     2018
 * 1.6.3,  11th May     2018
 * 1.6.2,  11th May     2018
 * 1.6.1,   9th May     2018
 * 1.6,     8th May     2018
 * 1.5.12,  3rd May     2018
 * 1.5.11, 27th April   2018
 * 1.5.10, 17th April   2018
 * 1.5.9, 12th April    2018
 * 1.5.8, 10th April    2018
 * 1.5.7,  9th April    2018
 * 1.5.6,  4th April    2018
 * 1.5.5,  4th April    2018
 * 1.5.4, 28th March    2018
 * 1.5.3, 27th March    2018
 * 1.5.2, 27th March    2018
 * 1.5.1, 21st March    2018
 * 1.5.0, 16th March    2018
 * 1.4.2,  9th March    2018
 * 1.4.0,  6th March    2018
 * 1.4.0,  6th March    2018
 * 1.3.0,  7th February 2018
 * 1.2.0, 24th January  2018
 * 1.1.0, 12th January  2018
 * 1.0.0,  3rd January  2018
 * commenced on 1st week of Dec 2017

 * ================= V 2.0 Objectives ==================================================================
 * do-2: dynamically frame the _data from the datasource, instead of persisting right from the begining. preseve the parent element's select context in child and so on to filter data properly.
 * do-2: have ground ready for all scaling adjustments.
 * do-2: should be able to add all types in one visual
 * do-2: multiple plot axis should be possible
 * do-2: plot area should be scrollable / zoomable (scroll box)
 * do-2: in-place drill down for all types
 * do-2: resize visual tile, so that visual plot area adjusts accordingly (scroll box)
 * do-2: decimals in plot axis should be optional
 * do-2: x,y tracker with floating tooltip
 * do-2: 3D object facilitation.
 * =====================================================================================================
 
 * do-0: avoid using anchor elements in visuals.
 * do-0: implement tickStyle, tickIncrement related parameters for all axis type visuals including 'bar-grouped' 
 * do-0: in auto interaction mode, when there are case differences in base axis key value, the summary to the child visuals are missing some entries to aggregate. (this will be rectified in 2.0)
 * ---------------------------------
 * 24th April 2018 - findings by Hari
 * ---------------------------------
 * do-0: all visual type - on maximize the selection is not retained - need to fix the issue
 * do-0: suppress legend of 'line' type visual produces error and the visual is not rendered
 * do-0: allow suppress axis text. enable on mouse over tooltip in the axis
 * -----------------------------------
 * do-0: allow to set the standard size, instead of manual scaling.
 * do-0: add a description for the visual. provide this facility through a button.
 * do-0: introduce vertical scroll for the table if records are more
 * do-0: show percentage along with the element value in tool tip.

 * do-0: all text - display only to cover the measured width and add ellipse to tooltip.
 * do-0: implement legend and text on element options together for all visual types. 1) pattern.legend.position = ['on-element', 'ne'], 2) option.suppress.textOnElement = bool 
 
 * do-0: visual scalability 1) visual's tile should adjust in the container. 2) width and height should be auto. 3) apply resize to the tile

 * do-0: ...more keys - 1) display in table after the base key column - done, 2) use for bar drill down, 3) use for doughnut drill down

 * do-0: draw labels outside the pie with a line pointer to the segment
 * do-0: pie/donut - make value on the element, and legend at the ouside and connect with line
 
 * do-1: create drawing method of bar-group 'horizontal'
 * do-1: introduce scroll for the svg visual as option, if the data elements are more than a few the svg content should scroll.
 * do-1: implement tile size change (Smallest-50x50, Smaller-75x75, Small-100x100, Normal-125x125, Big-150x150, Bigger-175x175, Biggest-200x200) part of 'More Options'
 * do-1: ensure the hand point cursor does not appear for leaf level visuals and disable element click and 'ctrl' click.
 * do-1: in case of 'bar-grouped' validate the presence of 'key' in the groupKey data list.
 * do-1: validate 'groupElementKey' array and 'valueKey' array in case of bar groupded
 * do-1: receive id as parameter from user and associate to the existing visual and its children. validation is required. create sn.visual.update(param) for this, assign only the non-null members of param to the existing param and proceed with redraw.
 * do-1: include 'checkbox' functionality in table visual type
 
 * do-1: the axis text - when the visual getting maximized the position of the text should be straight as possible.
 * do-1: zoom out - on hover - all over the text

 * do-9: create same level visuals, and they should interact with the style of highlighting(blurring the other element parts), filtering (rendering only the elements only relevant to the selection)
 
 * do-1: implement 'self count' in transformation
 * do-1: create a detail list to display multiple record details.
 
 * do-1: include the tooltype like pbi, the black background with white texts. introduce a tooltip type for this.
 * do-1: context menu for visual: 1) Save Data as ...(csv,tsv), 2) Save Visual as ...(png, svg), 3) View Data (as table along with visual - maximize during this - orientation option horizontal / vertical), 4) Highlight
 
 * do-3: work on shadow for line - vertical
 * do-3: facility to suppress x-axis, y-axis titles
 * do-3: decoration.textSize - implement for all visuals other than table type
 * do-3: facilitate bar text direction, and font

 * do-9: table type - the vertical overflow should be made scrollable
 * do-9: bubble chart - bubble shape should be in proportionate size(done) - bubble shape should be min (proportion to lowest value), normal (averate of min, max) ,max(proportaion to highest value) to be configured.
 * do-9: disable header bar
 
 * do-1: provide class to all text elements
 
 * do-9: interactive - parent-to-child and child-to-parent do for all other than table, all bar types, pie an donut
 * do-0: drilldown - interaction in the same holder using 'back' button
 * do-0: have ids in json instead of the long names. bind the visual with id and then have a look up of name by id as and when required to render. this will ensure the preparation of visuals at the first time is faster.

 * do-1: introduce validation for - groupKey, groupElementKey, groupValueKey, reduceMethod (sum, avg, max, min, count, etc.)

 * do-1: transition speed should be included in param - duration - slower(2500), slow(2000), normal (1500), fast (1000), faster(500)
 * do-3: axis title alignment - TextAlign: start, center, end
 * do-4: allocate 'width' for visual, axis title and recalculate the scale accordingly. better maintain margin(top, right, bottom, left) with width and height for easy calculation
 * do-9: sort the dimention first, and then plot in visual, as option
 * do-9: (is this approach need a redesign ? THINK...) select and un-select behaviour should be restricted only to the visuals sharing the same data. create data specific group id for this in the visual level.
 * do-9: perform 'is null or empty' check every where using the global routine
 * do-0: include missing validations in 'validate param' function
    
 * do-: provide ruler - horizontal and vertical bar visuals
 * do-3: legend in all 8 directions, still need to be make perfect
 * do-1: axis spacing according to the presence of legend
 * do-0: ...tooltip on visual element 1) with the color or the element (done), 2) black back ground with details, 3) simple - achieve all using tooltip template from implementor. (may be have these three templates as option)

 * do-0: provide dual axis with different scale, this is for plot axis

 * do-1: suspend multi-select of visual elements on maximize 
 * do-1: legend click highlight / filter behaviour
 * do-4: create bar - stacked 100 % facility
 * do-4: create slicer facility
 * do-4: create check list box facility
 

 * do-3: facility to print, export visual
 * do-9: include shadow effects for all visual elements
 * do-9: include 3d effects for all visual types    
 * d0-9: provide map based latitude / longtitude marking visual pattern, say type = 'map-plot'
 * do-9: [ *** DEFERRED *** ] interactive - parent-to-child and child-to-parent do for table, all bar types, pie an donut. it seems this is not required and not that easy (as well as will be confusing) to implement in the current hierarchical model.

 * ---------------------- done items ---------------------------------------------------------

 * done: axis tracker on mouse move on x and y axis
 * done: level marker with different colors parallel to plot axis

 * done: include cosmetic param properties in update (ans. basic title etc. are handled. need to include one-by-one as and when required)
 * done: provide 'baseAxis.displayKey' and 'plotAxis.displayKey' - use this to render the text in visual. 'key' is for internal purpose as usual. - (Ans. use the title attribute for this).
 

 * done: introduce a call back on visual's element click with necessary parameter (baseAxis.key, plotAxis.key) - implemented on visual element click and passing data of the selected elements
 * done: grouped-bar - show 0 value in the absence of bar.
 * done: show pie / doughnut with greyed area for 0 value.
 * done: make pie as a self-contained in terms of interactivity
 * done: issue. refreshing from 1st to 3rd level increases the count. need to fix.
 * done: [...] color theme using d3 scheme categories, set theme to fetch random colors, randomization should be an option
 * done: get raw data and convert to nested json data required for the visual
 * done: add event listener for additional custom capabilities. element click.
 * done: maximize - restore buttons not behaving properly while restoring back from maximize.
 * done: provide the facility to (1) create visual in the existing holder - this will keep adding the visuals. (2) only update the data for an existing visual - update data is working
 * done: transition type - bar (grow), pie/donut (circle, elastic, bounce, back)
 * done: introduce 0 value plotting in bar. (done for bar vertical, horizontal and bar-grouped)
 * done: make visual interaction in place - done for the visuals - bar - h/v/grouped, doughnut/pie, table, line, bubble
 * done: header-bar on zoom-out does not align properly
 * done: is_visual_interactable() - make this generic based on the conditions enabling the interaction. and use this for multi-selection
 * done: do tool tip on element - todo: line, bubble, grouped-bar, done: bar-hor, bar-vert, pie.
 * done: table-caption tooltip is not done. completed.
*/

var sn = (typeof (sn) == null || typeof (sn) != 'object') ? {} : sn;

const _emptyStr = String.fromCharCode();

/** @summary Checks the value is null or empty.
 * @description Checks the value is undefined, null or has zero length then returns true otherwise false.
 * @param {var} value The value to check
 * @return {boolean}  
 */
this._isNullOrEmpty = function (value) {
    return (value === undefined || value == null || value.length <= 0) ? true : false;
}

this._isNotNullOrEmpty = function (value) {
    return _isNullOrEmpty(value) == false;
}

this._get = {
    d3: {
        header: function (id) { return d3.select('#header' + id); },
        title: function (id) { return d3.select('#title' + id); },
        titleContainer: function (id) { return d3.select('#title_container' + id); },
        holder: function (id) { return d3.select('#holder' + id); },
        resizeFull: function (id) { return d3.select('#resize_full' + id); },
        resizeSmall: function (id) { return d3.select('#resize_small' + id); },
        tooltip: function (id) { return d3.select('#tooltip' + id); },
        visual: function (id) { return d3.select('#visual' + id); },
        tcaption: function (id) { return d3.select('#tcaption' + id); }
    },
    id: {
        header: function (id) { return 'header' + id; },
        title: function (id) { return 'title' + id },
        titleContainer: function (id) { return 'title_container' + id },
        holder: function (id) { return 'holder' + id; },
        resizeFull: function (id) { return 'resize_full' + id; },
        resizeSmall: function (id) { return 'resize_small' + id; },
        tooltip: function (id) { return 'tooltip' + id; },
        visual: function (id) { return 'visual' + id; },
        tcaption: function (id) { return d3.select('tcaption' + id); }
    },
    is_visual_interactable: function (param) {
        let result = false;

        if (param) {
            let interactionMode = param.option.behaviour.interactionMode;
            let listener = param.listener;

            result = ((interactionMode == 'auto' && param.children && param.children.length > 0)
                    || (interactionMode == 'manual' && listener && listener.onVisualElementClick && typeof (listener.onVisualElementClick) == 'function')
                    || (interactionMode == 'semi-auto' && listener && listener.beforeRefresh && typeof (listener.beforeRefresh) == 'function')
                    );
        }

        return result;
    },
    json_cloned: function (data) {
        return JSON.parse(JSON.stringify(data));
    }
}

this._set = {
    element: {
        text_as_title: function (id) {
            let element = document.getElementById(id);
            if (element.offsetWidth < element.scrollWidth) {
                element.setAttribute('title', element.innerText);
            }
        }
    }
}

sn.visual = {
    about: {
        get copyright() {
            return this._local.copyrightText;
        },
        get released() {
            return this._local.releasedDate;
        },
        get version() {
            return this._local.majorVersion + '.' + this._local.minorVersion + '.' + this._local.revisionNumber;
        },
        get all() {
            return 'SN.Visual - version <font style="color:white;font-weight:bold;">' + this.version + '</font> ' + this.copyright + ' <br>released <font style="color:white;font-weight:bold;">' + this.released + '</font>';
        },
        _local: {
            majorVersion: 1,
            minorVersion: 6,
            revisionNumber: 4,
            releasedDate: '14-05-2018',
            copyrightText: '© 2018 South Nests Software Solutions Pvt. Ltd.'
        }
    },
    /**
     * @property {object}  default - The default 
     */
    default: {
        /**
         * @property {object} param - The default param
         */
        param: {
            /**
             * @property {string} holder - The default visual holder
             */

            holder: {
                elementSelector: 'body'
            },
            basic: {
                title: ''
            },
            pattern: {
                type: 'bar',
                orientation: 'vertical',
                dataKey: {
                    baseAxis: {
                        textDirection: 'w-to-e',
                    },
                    plotAxis: {
                        textDirection: 'w-to-e',
                        tickStyle: 'auto',
                        tickIncrement: 1
                    }
                },
                legend: {
                    position: 'ne',
                    orientation: 'vertical',
                    shape: 'rect'
                }
            },
            dataSource: {
                format: 'json'
            },
            decoration: {
                textSize: 8
            },
            option: {
                elementShadow: false, // backward compatibility v: 1.0
                behaviour: {
                    interactionMode: 'auto' // set this default to 'semi-auto' once done
                },
                suppress: {
                    blankTicks: true,
                    elementShadow: true,
                    legend: false,
                    randomColors: true,
                    textOnElement: true,
                    tileBorder: true,
                    tileHeader: true,
                    toolBar: true,
                    transition: false,
                    visualBorder: true,
                    checkBox: true,
                },
                colorScheme: ['pbi-default-bright', 'pbi-default-darker', 'pbi-default-dark', 'pbi-default-normal'],
            },
            scale: {
                tile: {
                    padding: 2,
                    margin: 5,
                    headerBarHeight: 20,
                    backgroundColor: 'white',
                    hostColor: '#0f2758' // 'darkslategray'
                },
                legend: {
                    width: 100
                },
                get width() {
                    var l = this._local;
                    return _isNullOrEmpty(l.width) ? l.xRatio * l.factor : l.width;
                },
                get maxwidth() {
                    var l = this._local;
                    return (l.maxwidth == null ? l.xRatio * l.maxfactor : l.maxwidth);
                },
                set width(value) {
                    this._local.width = value;
                },
                get height() {
                    var l = this._local;
                    return (l.height == null ? l.yRatio * l.factor : l.height);
                },
                get maxheight() {
                    var l = this._local;
                    return (l.maxheight == null ? l.yRatio * l.maxfactor : l.maxheight);
                },
                set height(value) {
                    this._local.height = value;
                },
                get margin() {
                    return this._local.margin;
                },
                set margin(value) {
                    this._local.margin = (value > l.margin ? l.margin : value);
                    return this._local.margin;
                },
                _local: {
                    width: null,
                    height: null,
                    maxwidth: 1000,
                    maxheight: 390,
                    margin: 40,
                    xRatio: 3,
                    yRatio: 2,
                    factor: 100,
                    maxfactor: 225
                },
            }
        },
        zeroPlotValue: {
            elementSize: 1,  //pixels
            textColor: '#555555',
            elementColor: '#FFFFFF'
        },
        maxZIndex: 2147483647 // max int value;
    },
    helper: {
        /** 
        * @summary Provides a unique id to use with a html element.<br/>
        * @description Generates a 17 digit unique number, if the length is not specified, combined with the prefix and suffix.<br/>
        * @description Mention the length if you want a shorter unique number less than 17.<br/>
        * @description Note: Return value will be starting with a number in the absense of non-numeric prefix. Hence do not use straight away as Javascript identifier.
        * @param {string}[prefix] The prefix to be added before the unique number. Pass null if you don't need a prefix
        * @param {number}[length] The length of unique number to be generated. Pass null if you don't have any preference.
        * @param {string}[suffix] The suffix to be added after the unique number.
        * @return {string}
        */
        getUniqueId: function (prefix, length, suffix) {
            length = (length == null || length > 17) ? 17 : (length.length == 0) ? length + 1 : length;
            prefix = (prefix == null) ? '' : prefix;
            suffix = (suffix == null) ? '' : suffix;
            var randomNumber = Math.round(Math.random() * Math.pow(10, length));
            // reg. exp: slash space slash g (represents a single space)
            return (prefix + randomNumber + suffix).replace(/ /g, '');
        },
        getVisualTypeNames: function () {
            return sn.visual._core_lib._pattern_types.getVisualTypeNames();
        },
        getLegendValues: function (param) {
            var legendValues = [];
            param._data.forEach(function (item) {
                if (legendValues.indexOf(item[param.pattern.dataKey.baseAxis.key]) < 0) {
                    legendValues.push(item[param.pattern.dataKey.baseAxis.key]);
                }
            });

            return legendValues;
        },
        getMouseAtContext: function (visualId) {
            var eRect = document.getElementById(visualId).getBoundingClientRect();
            eRect.midX = eRect.x + eRect.width / 2;
            eRect.midY = eRect.y + eRect.height / 2;

            return {
                'visual-left': eRect.left,
                'visual-top': eRect.top,
                'mid-x': eRect.midX,
                'mid-y': eRect.midY,
                'at-right-top': (d3.event.pageX >= eRect.midX) && (d3.event.pageY < eRect.midY),
                'at-right-bottom': (d3.event.pageX >= eRect.midX) && (d3.event.pageY >= eRect.midY),
                'at-left-bottom': (d3.event.pageX < eRect.midX) && (d3.event.pageY >= eRect.midY),
                'at-left-top': (d3.event.pageX < eRect.midX) && (d3.event.pageY < eRect.midY),
                'page-x': d3.event.pageX,
                'page-y': d3.event.pageY
            }
        },
        /** 
        * @summary Enables data with behavioural properties.<br/>
        * @description Data is treated with properties to enable the interactive, drilldown and other behaviour capabilities.<br/>
        * @description Caution: Only for internal use. Do not invoke this function straight away.<br/>
        * @param {Object}[data] Data used by the visual.
        * @return {void}
        */
        setUniqueIdToData: function (param) {
            return param._data.forEach(function (dataElement) {
                if (dataElement._id == null) {
                    //dataElement._id = '_ui_';
                    var result = '_ui_';
                    Object.keys(dataElement).filter(item => typeof (dataElement[item]) != 'object').forEach(function (element) {
                        result += dataElement[element];
                    });

                    result = result.replace(/ /g, 'sp').replace(/-/g, 'ms');

                    //do-9: refine the special character replacement logic for unique id to data.

                    //  .replace(/!/g, 'em')
                    //  .replace(/"/g, 'dq')
                    //  .replace(/#/g, 'nr')
                    //  .replace(/$/g, 'dr')
                    //  .replace(/%/g, 'pt')
                    //  .replace(/&/g, 'ad')
                    //  .replace(/'/g, 'sq')
                    //  .replace(/\(/g, 'lp')
                    //  .replace(/\)/g, 'rp')
                    //  // replace * character
                    //  .replace(/\+/g, 'ps')
                    //  .replace(/,/g, 'ca')

                    //  .replace(/-/g, 'ms')
                    //  .replace(/./g, 'pd')
                    //  .replace(/\//g, 'sh')
                    //  .replace(/:/g, 'cn')
                    //  .replace(/;/g, 'sn')
                    //  .replace(/</g, 'ln')
                    //  .replace(/=/g, 'el')
                    //  .replace(/>/g, 'gn')
                    //  .replace(/\?/g, 'qn')
                    //  .replace(/\@/g, 'at')
                    //  .replace(/\[/g, 'lsb')
                    //  .replace(/\\/g, 'bs')
                    //  .replace(/]/g, 'rsb')
                    //  //.replace(/^/g, 'ct')
                    //  //.replace('_', 'us') -- not required to replace, part of valid identifier char
                    //  .replace(/`/g, 'gv')
                    //  .replace(/{/g, 'lcb')
                    ////  .replace(/|/g, 'vb')
                    //  .replace(/}/g, 'rcb')

                    //  .replace(/~/g, 'td');

                    dataElement._id = result;
                }
            });
        },
        reduceParamPlotValue: function (param, dataOfLevel) {
            // do-1: if there are children then this routine 'reduce param plot vlaue' should be invoked recursively
            if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.reduceMethod)) {
                var reducedValue;
                var key = param.pattern.dataKey.plotAxis.key;
                var groupValueKey = param.pattern.dataKey.plotAxis.groupValueKey;
                var reduceMethod = param.pattern.dataKey.plotAxis.reduceMethod;

                dataOfLevel.forEach(function (currentLevelData) {
                    if (param.children != null) {
                        param.children.forEach(function (child) {
                            sn.visual.helper.reduceParamPlotValue(child, currentLevelData[child.pattern.dataKey.baseAxis.dataSourceKey]);
                        });
                    }

                    reducedValue = 0;
                    if (reduceMethod == 'count') {
                        reducedValue = currentLevelData[param.pattern.dataKey.baseAxis.groupKey].length;
                    }
                    else {
                        //do-3: generalize the aggregation part so that can be invoked from any part of the code.
                        var childplotValues = [];
                        currentLevelData[param.pattern.dataKey.baseAxis.groupKey].forEach(function (child) {
                            childplotValues.push(child[groupValueKey]);
                        });

                        if (reduceMethod == 'sum' || reduceMethod == 'avg') {
                            reducedValue = childplotValues.reduce(function (a, b) { return a + b; });
                        }
                        else if (reduceMethod == 'min') {
                            reducedValue = d3.entries(childplotValues)
                                .sort(function (a, b) { return d3.ascending(a.value, b.value); })
                                [0].value;
                        }
                        else if (reduceMethod == 'max') {
                            reducedValue = d3.entries(childplotValues)
                                .sort(function (a, b) { return d3.descending(a.value, b.value); })
                                [0].value;
                        }

                        if (reduceMethod == 'avg') {
                            reducedValue = reducedValue / childplotValues.length;
                        }
                    }

                    currentLevelData[key] = reducedValue;
                });
            }
        },
        reduceToSummaryValue: function (d, param) {
            var plotAxis = param.pattern.dataKey.plotAxis;

            if (d == param.pattern.dataKey.baseAxis.key) {
                return (_isNullOrEmpty(plotAxis.summaryTitle) == false) ? plotAxis.summaryTitle :
                            (plotAxis.summaryReduceMethod == 'sum') ? 'Total' :
                                (plotAxis.summaryReduceMethod == 'avg') ? 'Average' :
                                    (plotAxis.summaryReduceMethod == 'min') ? 'Minimum' :
                                        (plotAxis.summaryReduceMethod == 'max') ? 'Maximum' :
                                            (plotAxis.summaryReduceMethod == 'count') ? 'Count' : _emptyStr;
            }
            else if (d == param.pattern.dataKey.plotAxis.key) {
                let summaryReducedValue = 0;
                if (plotAxis.summaryReduceMethod == 'count') {
                    summaryReducedValue = param._data.length;
                }
                else if (plotAxis.summaryReduceMethod == 'sum' || plotAxis.summaryReduceMethod == 'avg') {
                    summaryReducedValue = d3.sum(param._data, function (d) {
                        return d[param.pattern.dataKey.plotAxis.key]
                    });

                    if (plotAxis.summaryReduceMethod == 'avg') {
                        summaryReducedValue = summaryReducedValue / param._data.length;
                    }
                }
                else if (plotAxis.summaryReduceMethod == 'min') {
                    summaryReducedValue = d3.entries(plotValues)
                        .sort(function (a, b) { return d3.ascending(a.value, b.value); })[0]
                        .value;
                }
                else if (plotAxis.summaryReduceMethod == 'max') {
                    summaryReducedValue = d3.entries(plotValues)
                        .sort(function (a, b) { return d3.descending(a.value, b.value); })[0]
                    .value;
                }

                return summaryReducedValue;
            }
            else {
                return _emptyStr;
            }
        },
        reduceSelectedAll: function (param, selectedData) {
            const result = [];
            if (_isNotNullOrEmpty(param) == true && selectedData.length > 0) {
                //do-1: the multi-level parent-child interaction is done only for the first child. this needs to be extended for all children.
                const groupKey = param.pattern.dataKey.baseAxis.groupKey;
                const baseKey = param.pattern.dataKey.baseAxis.key;
                const valueKey = param.pattern.dataKey.plotAxis.key;

                // aggregate
                const otherMembers = Object.keys(selectedData[0]);
                delete otherMembers.splice(otherMembers.indexOf(baseKey), 1);
                delete otherMembers.splice(otherMembers.indexOf(valueKey), 1);

                // ------------------------------------------------------------------------------------------------------------------------------------------------------------
                // *** IMPORTANT NOTE *** : must clone the selected data, to avoid data references carrying over.
                // violating this will COLLAPSE the interactivity of the visuals, no end user can interpret (will become literally M-A-D) what happens on visual element clicks
                selectedData = _get.json_cloned(selectedData);
                // ------------------------------------------------------------------------------------------------------------------------------------------------------------

                // add other members to the result from data to draw.
                var item;
                for (var selectedDataIndex = 0; selectedDataIndex < selectedData.length; selectedDataIndex++) {
                    item = selectedData[selectedDataIndex];

                    var updateIndex = result.findIndex(f => f[baseKey] == item[baseKey]);
                    if (updateIndex < 0) {
                        var newItem = {};
                        newItem[baseKey] = item[baseKey];
                        newItem[valueKey] = item[valueKey];
                        result.push(newItem);
                        updateIndex = result.length - 1;
                    }
                    else {
                        result[updateIndex][valueKey] += item[valueKey];
                    }

                    //do-9: is this true? Check. !!! dynamic reduction on selected data - here it handles only one child in the immediate next level, this should be extended to all the children of this immediate next level.
                    if (param.children != null && param.children.length > 0) {
                        const childParam = param.children[0]; 
                        const baseKeyOfChild = childParam.pattern.dataKey.baseAxis.key;
                        const valueKeyOfChild = childParam.pattern.dataKey.plotAxis.key;

                        var member;
                        for (var memberIndex = 0; memberIndex < otherMembers.length; memberIndex++) {
                            member = otherMembers[memberIndex];

                            if (result[updateIndex][member] == null) {
                                result[updateIndex][member] = item[member];
                            }
                            else {
                                if (Array.isArray(item[member]) == true) {
                                    var itemMember = 0;
                                    for (var itemMemberIndex = 0; itemMemberIndex < item[member].length; itemMemberIndex++) {
                                        itemMember = item[member][itemMemberIndex];
                                        var indexOfChild = result[updateIndex][member].findIndex(child => child[baseKeyOfChild] == itemMember[baseKeyOfChild]);
                                        if (indexOfChild < 0) {
                                            result[updateIndex][member].push(itemMember);
                                        }
                                        else {
                                            result[updateIndex][member][indexOfChild][valueKeyOfChild] += itemMember[valueKeyOfChild];
                                        }
                                    };
                                }
                                else if (typeof (item[member]) == 'object') {
                                    // do-9: handle here the json key/value pair addition
                                }
                            }
                        }
                    }
                }
            }

            return result;
        },

        validateParam: function (param) {
            this.patternMinimumValid = false;

            try {
                if (param == null) {
                    throw { error: 'param is not specified', key: 'param', value: param };
                }

                if (param.holder == null) {
                    throw { error: 'param.holder is not specified', key: 'param.holder', value: param.holder };
                }

                if (_isNullOrEmpty(param.holder.elementSelector)) {
                    throw { error: 'holder.elementSelector is not specified', key: 'holder.elementSelector', value: param.holder.elementSelector };
                }

                if (d3.select(param.holder.elementSelector).size() == 0) {
                    throw { error: 'No element found seleced using the mentioned element selector', key: 'holder.elementSelector', value: param.holder.elementSelector };
                }

                if (param.pattern == null) {
                    throw { error: 'param.pattern is not specified', key: 'param', value: param };
                }

                if (param.pattern.type == null) {
                    throw { error: 'pattern.type is not specified', key: 'pattern', value: param.pattern };
                }

                var _pattern_types = sn.visual._core_lib._pattern_types;
                param.pattern.type = param.pattern.type == 'donut' ? 'doughnut' : param.pattern.type;
                if (_pattern_types.getAllTypes().indexOf(param.pattern.type) < 0) {
                    var typeIndex = sn.visual.helper.getVisualTypeNames().indexOf(param.pattern.type);

                    if (typeIndex < 0) {
                        throw { error: 'pattern.type should have any one of these values', validList: _pattern_types.getAllTypes(), key: 'pattern', value: param.pattern };
                    }
                    else {
                        param.pattern.type = _pattern_types.getAllTypes()[typeIndex];
                    }
                }

                // perform axis related validations on specific pattern.type values
                var isAxisBasedVisual = sn.visual._core_lib._pattern_types._isAxisBasedVisual(param.pattern.type);


                if (param.pattern.dataKey == null) {
                    throw { error: 'pattern.dataKey is not specified', key: 'pattern', value: param.pattern };
                }

                if (param.pattern.dataKey.baseAxis == null) {
                    throw { error: 'dataKey.baseAxis is not specified', key: 'dataKey', value: param.pattern.dataKey };
                }

                if (param.pattern.dataKey.baseAxis.key == null) {
                    throw { error: 'baseAxis.key is not specified', key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                }

                if (_isNotNullOrEmpty(param.pattern.dataKey.baseAxis.textDirection)
                    && sn.visual._core_lib._get_text_direction().indexOf(param.pattern.dataKey.baseAxis.textDirection) < 0) {
                    throw {
                        error: 'baseAxis.textDirection should have any one of these values',
                        validList: sn.visual._core_lib._get_text_direction(),
                        key: 'baseAxis',
                        value: param.pattern.dataKey.baseAxis
                    };
                }

                if (param.pattern.dataKey.baseAxis.moreKey != null) {
                    let moreKey = param.pattern.dataKey.baseAxis.moreKey;
                    if ((Array.isArray(moreKey) == false) && (typeof (moreKey) != 'object')) {
                        throw { error: 'baseAxis.moreKey does not seem to be an array', key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                    }
                }

                if (param.pattern.dataKey.plotAxis == null) {
                    throw { error: 'dataKey.plotAxis is not specified', key: 'dataKey', value: param.pattern.dataKey };
                }

                if (param.pattern.dataKey.plotAxis.key == null) {
                    throw { error: 'plotAxis.key is not specified', key: 'plotAxis', value: param.pattern.dataKey.plotAxis };
                }

                if (sn.visual._core_lib._pattern_types._isAxisBasedVisual(param.pattern.type) == true) {
                    if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.textDirection)
                        && sn.visual._core_lib._get_text_direction().indexOf(param.pattern.dataKey.plotAxis.textDirection) < 0) {
                        throw {
                            error: 'plotAxis.textDirection should have any one of these values',
                            validList: sn.visual._core_lib._get_text_direction(),
                            key: 'plotAxis',
                            value: param.pattern.dataKey.plotAxis
                        };
                    }

                    if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.tickStyle)
                        && sn.visual._core_lib._get_tick_style().indexOf(param.pattern.dataKey.plotAxis.tickStyle) < 0) {
                        throw {
                            error: 'plotAxis.tickStyle should have any one of these values',
                            validList: sn.visual._core_lib._get_tick_style(),
                            key: 'plotAxis',
                            value: param.pattern.dataKey.plotAxis
                        };
                    }
                }

                if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.reduceMethod)) {
                    if (sn.visual._core_lib._get_reduce_methods().indexOf(param.pattern.dataKey.plotAxis.reduceMethod) < 0) {
                        throw { error: 'plotAxis.reduceMethod should have any one of these values', validList: sn.visual._core_lib._get_reduce_methods(), key: 'plotAxis', value: param.pattern.dataKey.plotAxis };
                    }
                }

                if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.summaryReduceMethod)) {
                    if (sn.visual._core_lib._get_reduce_methods().indexOf(param.pattern.dataKey.plotAxis.summaryReduceMethod) < 0) {
                        throw { error: 'plotAxis.summaryReduceMethod should have any one of these values', validList: sn.visual._core_lib._get_reduce_methods(), key: 'plotAxis', value: param.pattern.dataKey.plotAxis };
                    }
                }

                // decoration validation
                if (_isNotNullOrEmpty(param.decoration)) {
                    if (_isNotNullOrEmpty(param.decoration.textSize)
                        && parseInt(param.decoration.textSize) == NaN) {
                        throw { error: 'decoration.textSize is not a number', key: 'decoration', value: param.decoration };
                    }
                    if (param.decoration.textSize < 8 || param.decoration.textSize > 40) {
                        throw { error: 'decoration.textSize should be in the range of 8 and 40', key: 'decoration', value: param.decoration };
                    }
                }

                // option validation
                if (_isNotNullOrEmpty(param.option) == true) {
                    if (param.option.suppress && Array.isArray(param.option.suppress)) {
                        let suppressOptions = [];
                        sn.visual._core_lib._get_valid_suppress_options().forEach(function (option) {
                            suppressOptions.push(option.toLowerCase());
                        });

                        param.option.suppress.forEach(function (option) {
                            if (suppressOptions.indexOf(option.toLowerCase()) < 0) {
                                throw { error: 'option.suppress should have any one of these values', validList: sn.visual._core_lib._get_valid_suppress_options(), key: 'option.suppress', value: param.option.suppress };
                            }
                        });
                    }

                    if (_isNotNullOrEmpty(param.option.suppress) && param.option.suppress.legend != true) {
                        if (_isNotNullOrEmpty(param.pattern.legend)) {
                            if (_isNotNullOrEmpty(param.pattern.legend.position) && sn.visual._core_lib._get_position_list().indexOf(param.pattern.legend.position) < 0) {
                                throw { error: 'legend.position should have any one of these values', validList: sn.visual._core_lib._get_position_list(), key: 'legend', value: param.pattern.legend };
                            }
                            if (_isNotNullOrEmpty(param.pattern.legend.orientation) && sn.visual._core_lib._get_orientation_list().indexOf(param.pattern.legend.orientation) < 0) {
                                throw { error: 'legend.orientation should have any one of these values', validList: sn.visual._core_lib._get_orientation_list(), key: 'legend', value: param.pattern.legend };
                            }
                        }
                    }

                    if (_isNotNullOrEmpty(param.option.colorScheme)) {
                        let colorSchemeList = Object.keys(sn.visual._color_scheme);
                        if (typeof (param.option.colorScheme) == 'string') {
                            if (colorSchemeList.filter(name => name == param.option.colorScheme).length < 1) {
                                throw { error: 'option.colorScheme should have any of these values', validList: colorSchemeList, key: 'option', value: param.option };
                            }
                        }
                        else if (Array.isArray(param.option.colorScheme)) {
                            param.option.colorScheme.forEach(function (scheme) {
                                if (colorSchemeList.filter(name => name == scheme).length < 1) {
                                    throw { error: 'option.colorScheme should have any of these values', validList: colorSchemeList, key: 'option', value: param.option };
                                }
                            });
                        }
                    }

                    if (_isNotNullOrEmpty(param.option.behaviour.interactionMode)
                        && sn.visual._core_lib._get_interaction_mode_list().filter(name => name == param.option.behaviour.interactionMode).length < 1) {
                        throw { error: 'option.behaviour.interactionMode should have any of these values', validList: sn.visual._core_lib._get_interaction_mode_list(), key: 'option', value: param.option };
                    }
                }

                // transformation from raw data(data source) to data iff data is not available
                if (param.data == null) {
                    if (param.dataSource == null) {
                        throw { error: 'param.dataSource is expected in the absence of param.data', key: 'param', value: param };
                    }

                    if (_isNotNullOrEmpty(param.dataSource.format) && sn.visual._core_lib._data_formats.getAllFormats().indexOf(param.dataSource.format) < 0) {
                        throw { error: 'dataSource.format should have any one of these value(s)', validList: sn.visual._core_lib._data_formats.getAllFormats(), key: 'param.dataSource', value: param.dataSource };
                    }

                    if (param.dataSource.data) {
                        param.dataSource.format =
                            param.dataSource.format ? param.dataSource.format : sn.visual.default.param.dataSource.format;

                        if (param.dataSource.format == 'json') {
                            //done: json validation is to be done on the dataSource                            
                            if (Array.isArray(param.dataSource.data) == true) {
                                if (param.dataSource.data.length == 0) {
                                    throw { error: 'dataSource.data does not contain data', key: 'dataSource', value: param.dataSource }
                                }

                                //if (typeof (param.dataSource.data[0]) != 'object') {
                                if (param.dataSource.data.filter(f => Array.isArray(f) != true && typeof (f) == 'object').length < param.dataSource.data.length) {
                                    throw { error: 'dataSource.data does not have one or more elements in json format (or) contains one or more nested json elements', key: 'dataSource', value: param.dataSource }
                                }
                            }
                        }
                    }

                    //if (param.dataSource.format == 'csv' || param.dataSource.format == 'tsv' || param.dataSource.format == 'psv') {
                    //    throw { error: 'Transformation of data source of this type is not yet implemented.', key: 'param.dataSource.format', value: param.dataSource.format };
                    //}

                    if (param.option.behaviour.interactionMode == 'auto' && sn.visual._core_lib._transform_rawdata_to_data(param) == false) {
                        throw { error: 'Unable to transform raw data from dataSource as per the keys specified.', key: 'param', value: param }
                    }
                }

                // data validattion
                if (param.data == null) {
                    throw { error: 'param.data is not specified', key: 'param', value: param };
                }
                if (Array.isArray(param.data) == false) {
                    // done: (parentGroupKey is deprecated, and dataSourceKey is introduced )- handle here to assign the data source key name to parent group key
                    throw { error: 'param.data is not an array', key: 'param', value: param };
                }

                if (param.data.length == 0) {
                    throw { error: 'Array param.data does not have element inside', key: 'param', value: param };
                }

                if (param.data.length > 0) {
                    let firstDataRowKeyListString = Object.keys(param.data[0]).toString();
                    let keyListString;

                    for (var index = 0; index < param.data.length; index++) {
                        if (typeof (param.data[index]) != 'object') {
                            throw { error: `Element in param.data at index '${index}' does not seem to be a JSON`, key: 'param.data[' + index + ']', value: param.data[index] };
                        }

                        keyListString = Object.keys(param.data[index]);
                        if (typeof (param.data[index]) == 'object' && keyListString.length == 0) {
                            throw { error: `JSON element in param.data at index '${index}' does not have any key`, key: 'param.data[' + index + ']', value: param.data[index] };
                        }

                        if (keyListString.toString() != firstDataRowKeyListString) {
                            throw { error: `Key(s) of JSON element in param.data at index '${index}' differs in name or alpha-case from it\'s previous element(s)`, key: 'param.data[' + index + ']', value: param.data[index] };
                        }
                    }

                    if (isAxisBasedVisual == true && param.pattern.type != 'bar-grouped' && param.option.behaviour.interactionMode == 'auto') {
                        if (Object.keys(param.data[0]).indexOf(param.pattern.dataKey.baseAxis.key) < 0) {
                            throw { error: `Unable to find baseAxis.key '${param.pattern.dataKey.baseAxis.key}' in data`, key: 'param.data[0]', value: param.data[0] };
                        }
                    }

                    this.paramHasChildren =
                            (param.children
                             && param.children.length > 0
                             && param.children.filter(function (child) { return child.pattern.dataKey.plotAxis.valueKey == param.pattern.dataKey.plotAxis.valueKey }).length > 0);

                    this.validateGroupKey =
                           (param.pattern.type == 'bar-grouped')
                        || (param.children != null && this.paramHasChildren)
                        || (param.pattern.dataKey.plotAxis.reduceMethod);

                    // done: *** HIGH *** important to check the current level groupKey is one of it's child's dataSourceKey, so that validate group key else skip.
                    this.validateBaseAxisMoreKey = (param.pattern.dataKey.baseAxis.moreKey != null)
                    this.validateGroupElementKey = (param.pattern.type == 'bar-grouped' && param.option.behaviour.interactionMode == 'auto');
                    this.validateGroupValueKey = _isNotNullOrEmpty(param.pattern.dataKey.plotAxis.groupValueKey);
                    this.validatePlotAxiskey = (param.pattern.type != 'bar-grouped');

                    // d0-0: handle here 1. the validation of dataSourceKey in all levels, 2. ensure validate the key's presence in the param.data 
                    // ---------------------------------------------------------------------------------------------------------------------------

                    if (this.validateBaseAxisMoreKey) {
                        let firstDataRowKeyList = Object.keys(param.data[0]);
                        let moreKey = param.pattern.dataKey.baseAxis.moreKey;
                        moreKey = (Array.isArray(moreKey) ? moreKey : [moreKey]);

                        moreKey.forEach(function (item) {
                            if (firstDataRowKeyList.indexOf(item.key) < 0) {
                                throw { error: "One of the key mentioned in baseAxis.moreKey does not present in data", key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                            }

                            if (_isNotNullOrEmpty(item.title) && typeof (item.title) != 'string') {
                                throw { error: "One of the title mentioned in baseAxis.moreKey does not seems to be valid", key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                            }
                        });
                        param.pattern.dataKey.baseAxis.moreKey = moreKey; // assigning the treated one - as array of json 
                    }

                    if (this.validateGroupKey) {
                        // baseAxis.groupKey validation
                        if (param.pattern.dataKey.plotAxis.reduceMethod && _isNullOrEmpty(param.pattern.dataKey.baseAxis.groupKey)) {
                            throw { error: `plotAxis.reduceMethod is mentioned as '${param.pattern.dataKey.plotAxis.reduceMethod}', so baseAxis.groupKey is required to calculate the plot value from it's children`, key: 'dataKey', value: param.pattern.dataKey };
                        }

                        if (_isNullOrEmpty(param.pattern.dataKey.baseAxis.groupKey)) {
                            throw { error: "baseAxis.groupKey is not specified", key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                        }

                        if (typeof (param.data[0][param.pattern.dataKey.baseAxis.groupKey]) != 'object') {
                            throw { error: `Unable to find baseAxis.groupKey '${param.pattern.dataKey.baseAxis.groupKey}' as object in one or more element(s) data`, key: 'param.data[0]', value: param.data[0] };
                        }

                        if (this.paramHasChildren) {
                            if (sn.visual._core_lib._is_group_key_one_of_data_source_key(param) == false) {
                                throw { error: "One or more 'baseAxis.groupKey' is/are not matching with the 'dataSourceKey' of one of it's child", key: 'param', value: param };
                            }
                        }
                    }
                    if (this.validateGroupElementKey) {
                        // baseAxis.groupElementKey validation
                        if (_isNullOrEmpty(param.pattern.dataKey.baseAxis.groupElementKey)) {
                            throw { error: "baseAxis.groupElementKey is not specified", key: 'baseAxis', value: param.pattern.dataKey.baseAxis };
                        }

                        //if (Object.keys(param.data[0][param.pattern.dataKey.baseAxis.groupKey][0]).indexOf(param.pattern.dataKey.baseAxis.groupElementKey) < 0) {
                        //    throw { error: `Unable to find baseAxis.groupElementKey '${param.pattern.dataKey.baseAxis.groupElementKey}' in data`, key: 'param.data[0]', value: param.data[0] };
                        //}

                        // revisit the below validation
                        //if (Object.keys(param.data[0][param.pattern.dataKey.baseAxis.groupKey][0]).indexOf(param.pattern.dataKey.plotAxis.key) < 0) {
                        //    throw { error: `Unable to find plotAxis.key '${param.pattern.dataKey.plotAxis.key}' in data`, key: 'param.data[0]', value: param.data[0] };
                        //}
                    }
                    if (this.validateGroupValueKey) {
                        // plotAxis.groupValueKey
                        if (Object.keys(param.data[0][param.pattern.dataKey.baseAxis.groupKey][0]).indexOf(param.pattern.dataKey.plotAxis.groupValueKey) < 0) {
                            throw { error: `Unable to find the mentioned plotAxis.groupValueKey '${param.pattern.dataKey.plotAxis.groupValueKey}' under baseAxis.groupKey '${param.pattern.dataKey.baseAxis.groupKey}' of data`, key: 'param', value: param };
                        }
                    }
                    if (this.validatePlotAxiskey) {
                        if (Object.keys(param.data[0]).indexOf(param.pattern.dataKey.plotAxis.key) < 0) {
                            throw { error: `Unable to find plotAxis.key '${param.pattern.dataKey.plotAxis.key}' in data`, key: 'param.data[0]', value: param.data[0] };
                        }
                    }
                }

                this.patternMinimumValid = true;
            }
            catch (e) {
                sn.visual.helper.handleException(e);
            }
            finally {
                return this.patternMinimumValid;
            }
        },
        handleParam: function (param) {
            if (param != null && typeof (param) == 'object') {
                var defaultParam = _get.json_cloned(sn.visual.default.param);

                if (param.holder != null) {
                    var ph = param.holder;
                    var dh = defaultParam.holder;
                    ph.elementSelector = _isNullOrEmpty(ph.elementSelector) ? dh.elementSelector : ph.elementSelector;
                }
                else {
                    param.holder = defaultParam.holder;
                }

                if (param.basic != null) {
                    var pb = param.basic;
                    var db = defaultParam.basic;
                    pb.title = _isNullOrEmpty(pb.title) ? db.title : pb.title;
                }
                else {
                    param.basic = defaultParam.basic;
                }

                if (param.pattern != null) {
                    var pp = param.pattern;
                    var dp = defaultParam.pattern;

                    pp.dataKey.baseAxis.textDirection =
                        (pp.dataKey != null) && (pp.dataKey.baseAxis != null) && (pp.dataKey.baseAxis.textDirection == null) ?
                            dp.dataKey.baseAxis.textDirection : pp.dataKey.baseAxis.textDirection;
                    pp.dataKey.plotAxis.textDirection =
                        (pp.dataKey != null) && (pp.dataKey.plotAxis != null) && (pp.dataKey.plotAxis.textDirection == null) ?
                            dp.dataKey.plotAxis.textDirection : pp.dataKey.plotAxis.textDirection;

                    pp.dataKey.baseAxis.title =
                        (pp.dataKey != null) && (pp.dataKey.baseAxis != null) && (pp.dataKey.baseAxis.title == null) ?
                            pp.dataKey.baseAxis.key : pp.dataKey.baseAxis.title;
                    pp.dataKey.plotAxis.title =
                        (pp.dataKey != null) && (pp.dataKey.plotAxis != null) && (pp.dataKey.plotAxis.title == null) ?
                            pp.dataKey.plotAxis.key : pp.dataKey.plotAxis.title;

                    if (sn.visual._core_lib._pattern_types._isAxisBasedVisual(param.pattern.type) == true) {
                        pp.dataKey.plotAxis.tickStyle =
                            (pp.dataKey != null) && (pp.dataKey.plotAxis != null) && (pp.dataKey.plotAxis.tickStyle == null) ?
                                dp.dataKey.plotAxis.tickStyle : pp.dataKey.plotAxis.tickStyle;
                        pp.dataKey.plotAxis.tickIncrement =
                            (pp.dataKey != null) && (pp.dataKey.plotAxis != null) && (pp.dataKey.plotAxis.tickIncrement == null) ?
                                dp.dataKey.plotAxis.tickIncrement : pp.dataKey.plotAxis.tickIncrement;
                    }
                }
                else {
                    // note: pattern is something the implementor must assign, as of now no default on pattern
                }

                if (param.dataSource && param.dataSource.data) {
                    param.dataSource.format =
                        param.dataSource.format ? param.dataSource.format : defaultParam.dataSource.format;
                }

                if (param.scale != null) {
                    var ps = param.scale;
                    var ds = defaultParam.scale;
                    ps.margin = parseInt((ps.margin == null || ps.margin < ds.margin) ? ds.margin : ps.margin);
                    ps.width = parseInt(ps.width == null ? ds.width : ps.width);
                    ps.height = parseInt(ps.height == null ? ds.height : ps.height);

                    if (ps.tile != null) {
                        ps.tile.margin = ps.tile.margin == null ? ds.tile.margin : ps.tile.margin;
                        ps.tile.padding = ps.tile.padding == null ? ds.tile.padding : ps.tile.padding;
                        ps.tile.headerBarHeight = ps.tile.headerBarHeight == null ? ds.tile.headerBarHeight : ps.tile.headerBarHeight;
                        ps.tile.backgroundColor = ps.tile.backgroundColor == null ? ds.tile.backgroundColor : ps.tile.backgroundColor;
                    }
                    else {
                        ps.tile = ds.tile;
                    }

                    if (ps.legend != null) {
                        ps.legend.width = ps.legend.width == null ? ds.legend.width : ps.legend.width;
                    }
                    else {
                        ps.legend = ds.legend;
                    }

                    if (ps.title != null) {
                        ps.title.height = ps.title.height == null ? ds.title.height : ps.title.height;
                    }
                    else {
                        ps.title = ds.title;
                    }
                }
                else {
                    param.scale = defaultParam.scale;
                }

                if (param.decoration != null) {
                    var pd = param.decoration;
                    var dpd = defaultParam.decoration;
                    pd.textSize = (pd.textSize == null) ? dpd.textSize : pd.textSize;
                }
                else {
                    param.decoration = defaultParam.decoration;
                }

                if (param.option != null) {
                    var po = param.option;
                    var dop = defaultParam.option;
                    po.elementShadow = po.elementShadow == null ? dop.elementShadow : po.elementShadow;
                    po.colorScheme = po.colorScheme == null ? dop.colorScheme : po.colorScheme;

                    if (po.behaviour != null) {
                        po.behaviour.interactionMode = po.behaviour.interactionMode == null ? dop.behaviour.interactionMode : po.behaviour.interactionMode;
                    }
                    else {
                        po.behaviour = dop.behaviour;
                    }

                    if (po.suppress != null) {
                        // transforming suppress options to json, if mentioned in array
                        if (Array.isArray(po.suppress) == true) {
                            let suppressOptions = po.suppress;
                            po.suppress = dop.suppress;
                            suppressOptions.forEach(function (option) {
                                po.suppress[option] = true;
                            });
                        }

                        // starts------ backward compatibility for svgBorder
                        if (po.suppress.svgBorder != null) {
                            po.suppress.visualBorder = po.suppress.svgBorder;
                            delete po.suppress.svgBorder;
                        }
                        // ends------ backward compatibility for svgBorder

                        po.suppress.elementShadow = po.suppress.elementShadow == null ?
                                typeof (po.elementShadow) == 'boolean' ? (po.elementShadow == false) /* backward compatibility */ : dop.suppress.elementShadow
                            : po.suppress.elementShadow;
                        po.suppress.legend = typeof(po.suppress.legend) != 'boolean' ? dop.suppress.legend : po.suppress.legend;

                        //do-1: improve the random color generation logic - when there is more data the logic takes indefinite time
                        //po.suppress.randomColors = typeof (po.suppress.randomColors) != 'boolean' ? dop.suppress.randomColors : po.suppress.randomColors;

                        // do-9: **** currently enforcing the random color option to 'switch off'. this shall be removed once the random color generation logic is improved for better response time.
                        po.suppress.randomColors = true;

                        po.suppress.textOnElement = typeof(po.suppress.textOnElement) != 'boolean'? dop.suppress.textOnElement: po.suppress.textOnElement;
                        po.suppress.tileBorder = typeof(po.suppress.tileBorder) != 'boolean'? dop.suppress.tileBorder : po.suppress.tileBorder;
                        po.suppress.tileHeader = typeof(po.suppress.tileHeader) != 'boolean'? dop.suppress.tileHeader : po.suppress.tileHeader;
                        po.suppress.toolBar = typeof(po.suppress.toolBar) != 'boolean'? dop.suppress.toolBar : po.suppress.toolBar;
                        po.suppress.transition = typeof(po.suppress.transition) != 'boolean'? dop.suppress.transition : po.suppress.transition;
                        po.suppress.visualBorder = typeof (po.suppress.visualBorder) != 'boolean' ? dop.suppress.visualBorder : po.suppress.visualBorder;
                        po.suppress.checkBox = typeof (po.suppress.checkBox) != 'boolean' ? dop.suppress.checkBox : po.suppress.checkBox;
                        if (sn.visual._core_lib._pattern_types._isAxisBasedVisual(param.pattern.type) == true) {
                            po.suppress.blankTicks = typeof(po.suppress.blankTicks) != 'boolean'? po.suppress.blankTicks : dop.suppress.blankTicks;
                        }
                    }
                    else {
                        po.suppress = dop.suppress;
                    }
                }
                else {
                    param.option = defaultParam.option;
                }

                // multi param sections - handling

                // ... if colors are not specified
                sn.visual._core_lib._set_colors_from_scheme(param);

                // .... legend input values handling
                if (param.option.suppress.legend != true) {
                    var pp = param.pattern;
                    var dp = defaultParam.pattern;

                    if (_isNotNullOrEmpty(pp.legend)) {
                        pp.legend.position = (pp.legend.position == null ? dp.legend.position : pp.legend.position);
                        pp.legend.orientation = (pp.legend.orientation == null ? dp.legend.orientation : pp.legend.orientation);
                    }
                    else {
                        pp.legend = dp.legend;
                    }

                    //do-0: fix - consider user value first, then assign the default. review this for all param handling code lines.
                    pp.legend.shape = (param.pattern.type == 'pie' || param.pattern.type == 'doughnut' || param.pattern.type == 'bubble' || param.pattern.type == 'line' || param.pattern.type == 'curve-line') ? 'circle' : 'rect';
                    pp.legend.shape = pp.legend.shape != null ? pp.legend.shape : dp.legend.shape;

                    // treatment of un-favourable orientation values of legend position and enforcing with a favourable one
                    if (['n', 's'].indexOf(pp.legend.position) >= 0) {
                        pp.legend.orientation = 'horizontal'
                    }
                    else if (['e', 'w'].indexOf(pp.legend.position) >= 0) {
                        pp.legend.orientation = 'vertical'
                    }
                }
                else {
                    param.holder = defaultParam.holder;
                    param.scale = defaultParam.scale;
                    param.option = defaultParam.option;
                }

                if (param.option.suppress.tileHeader == true) {
                    param.scale.tile.headerBarHeight = 0;
                }

                return param;
            }
        },
        handleException: function (e) {
            if (e.error != null) {
                if (e.validList != null) {
                    console.error(sn.visual._message_template.get('error-validlist-key-value'), e.error, e.validList, e.key, e.value);
                }
                else if (e.key != null && e.value != null) {
                    console.error(sn.visual._message_template.get('error-key-value'), e.error, e.key, e.value);
                }
                else {
                    console.error(sn.visual._message_template.get('error'), e.error);
                }
            }
            else if (e.warning != null) {
                if (e.validList != null) {
                    console.warning(sn.visual._message_template.get('warning-validlist-key-value'), e.error, e.validList, e.key, e.value);
                }
                else if (e.key != null && e.value != null) {
                    console.warning(sn.visual._message_template.get('warning-key-value'), e.error, e.key, e.value);
                }
                else {
                    console.error(sn.visual._message_template.get('warning'), e.warning);
            }
            }
            else {
                console.error('%s. %o', e.message, e);
            }
        },
        saveAsSvg: function (svgEl, name) {
            // svgEl.setAttribute("xmlns", "http://www.w3.org/2000/svg");
            svgEl.attr("xmlns", "http://www.w3.org/2000/svg");
            var svgData = svgEl.outerHTML;
            var preface = '<?xml version="1.0" standalone="no"?>\r\n';
            var svgBlob = new Blob([preface, svgData], { type: "image/svg+xml;charset=utf-8" });
            var svgUrl = URL.createObjectURL(svgBlob);
            var downloadLink = document.createElement("a");
            downloadLink.href = svgUrl;
            downloadLink.download = name;
            document.body.appendChild(downloadLink);
            downloadLink.click();
            document.body.removeChild(downloadLink);
        }
    },
    _core_lib: {
        // common constant
        _pattern_types: {
            types: [
                { 'type': 'bar', 'name': 'Bar chart', 'axis-type': true, 'chart-type': true },
                { 'type': 'bar-grouped', 'name': 'Grouped Bar chart', 'axis-type': true, 'chart-type': true },
                { 'type': 'bubble', 'name': 'Bubble chart', 'axis-type': true, 'chart-type': true },
                { 'type': 'line', 'name': 'Line chart', 'axis-type': true, 'chart-type': true },
                { 'type': 'curve-line', 'name': 'Curved-Line chart', 'axis-type': true, 'chart-type': true },
                { 'type': 'pie', 'name': 'Pie chart', 'axis-type': false, 'chart-type': true },
                { 'type': 'doughnut', 'name': 'Doughnut chart', 'axis-type': false, 'chart-type': true },
                { 'type': 'table', 'name': 'Table', 'axis-type': null, 'chart-type': false },
                { 'type': 'check-list', 'name': 'Check List', 'axis-type': null, 'chart-type': false }
            ],
            getTypesFiltered: function (filterKey, filterValue, valueKey) {
                var result = [];
                if (valueKey == null) { valueKey = 'type' }
                Object.values(this.types).filter(item => item[filterKey] == filterValue).forEach(function (element) {
                    result.push(element[valueKey]);
                });
                return result;
            },
            getAllTypes: function () {
                return this.getTypesFiltered();
            },
            getAxisTypes: function () {
                return this.getTypesFiltered('axis-type', true);
            },
            getNonAxisTypes: function () {
                return this.getTypesFiltered('axis-type', false);
            },
            getVisualTypeNames: function () {
                return this.getTypesFiltered('chart-type', true, 'name');
            },
            _isAxisBasedVisual: function (visualType) {
                var _pattern_types = sn.visual._core_lib._pattern_types;
                return (_pattern_types.getAxisTypes().indexOf(visualType) >= 0);
            },
            _isVisualChartType: function (type) {
                var result = false;

                var _pattern_types = sn.visual._core_lib._pattern_types;
                for(var key of Object.keys(_pattern_types.types)) {
                    if (_pattern_types.types[key][type] != null) {
                        result = (_pattern_types.types[key][type]['chart-type'] == true);
                        break;
                    }
                };

                return result;
            }
        },
        _set_interaction_mode: function (param, value) {
            if (param == null) {
                console.error('sn.visual: Unable to set interaction mode.')
            }

            if (param.option == null) {
                param.option = {};
            }
            if (param.option.behaviour == null) {
                param.option.behaviour = {
                    interactionMode: value
                }
            }
            if (param.option.behaviour.interactionMode == null) {
                param.option.behaviour.interactionMode = value;
            }
        },
        _casacade_down_the_interaction_mode: function(param, interactionMode) {
            if (param.children && param.children.length > 0) {
                let child;
                for (var index = 0; index < param.children.length; index++) {
                    child = param.children[index];

                    this._set_interaction_mode(param, interactionMode);
                    if (child.children && child.children.length > 0) {
                        this._casacade_down_the_interaction_mode(child, interactionMode);
                    }
                }
            }
        },

        _data_formats: {
            formats: [
                { format: 'json', name: 'JsON', description: 'Javascript object Notation' },
                //{ format: 'csv', name: 'CSV', description: 'Comma Separated Values' },
                //{ format: 'tsv', name: 'TSV', description: 'Tab Separated Values' },
                //{ format: 'psv', name: 'PSV', description: 'Pipeline Separated Values' },
            ],
            getFormatsFiltered: function (filterKey, filterValue, valueKey) {
                var result = [];
                if (valueKey == null) { valueKey = 'format' }
                Object.values(this.formats).filter(item => item[filterKey] == filterValue).forEach(function (element) {
                    result.push(element[valueKey]);
                });
                return result;
            },
            getAllFormats: function () {
                return this.getFormatsFiltered();
            }
        },
        _get_valid_suppress_options: function () {
            return [
                'blankTicks',
                'elementShadow',
                'legend',
                'randomColors',
                'tileBorder',
                'tileHeader',
                'toolBar',
                'transition',
                'visualBorder'
            ];
        },
        _get_reduce_methods: function () {
            return ['sum', 'avg', 'min', 'max', 'count'];
        },
        _get_text_direction: function () {
            return ['sw-to-ne', 'w-to-e', 'nw-to-se', 'n-to-s', 'ne-to-sw', 'e-to-w', 'se-to-nw', 's-to-n'];
        },
        _get_tick_style: function () {
            return ['auto', 'blank', 'no-decimal'];
        },
        _get_orientation_list: function () {
            return ['horizontal', 'vertical'];
        },
        _get_position_list: function (nature) {
            var result;
            if (_isNotNullOrEmpty(nature) == true) {
                if (nature.scaleAffected == 'width') {
                    result = ['ne', 'e', 'se', 'sw', 'w', 'nw'];
                }
                else if (nature.scaleAffected == 'height') {
                    result = ['ne', 'n', 'nw', 'sw', 's', 'se'];
                }
            }

            return (result ? result : ['n', 'ne', 'e', 'se', 's', 'sw', 'w', 'nw']);
        },
        _get_interaction_mode_list: function () {
            return ['auto', 'semi-auto', 'manual'];
        },
        _get_colors_of_scheme: function (fromColorScheme) {
            let result = [];

            if (typeof (fromColorScheme) == 'string') {
                result = sn.visual._color_scheme[fromColorScheme];
            }
            else if (Array.isArray(fromColorScheme)) {
                fromColorScheme.forEach(function (scheme) {
                    result = result.concat(sn.visual._color_scheme[scheme]);
                });
            }

            return result;
        },
        _get_rbg_string_to_hex_string: function (rgbString) {
            this.result = '#000000';

            if (rgbString && rgbString.toLowerCase().indexOf('rgb') >= 0) {
                var colorValues = rgbString
                    .split("(")[1]
                    .split(")")[0]
                    .split(",")
                    .map(function (x) {
                        x = parseInt(x).toString(16);
                        return (x.length == 1) ? "0" + x : x;
                    });
                this.result = "#" + colorValues.join("");
            }

            return (this.result);
        },
        _get_color_readable: function (color) {
            var reverseColor;
            var midColorValue = (Math.pow(256, 3) - 1) / 2;
            hexColor = sn.visual['_color_list'][color];
            color = hexColor != null ? hexColor : this._get_rbg_string_to_hex_string(color);
            try {
                colorValue = parseInt(color.substring(1), 16);
                if (Number.isNaN(colorValue)) {
                    reverseColor = color;
                }
                else {
                    reverseColor = '#' + (colorValue < midColorValue ? 'FFFFFF' /* white */ : '000000' /* black */);
                }
            }
            catch (ex) {
                reverseColor = color;
            }

            return reverseColor;
        },
        _get_colors_random: function (fromColorScheme, requiredColorCount) {
            var result = [];
            var newColorIndex;
            var colorList = [];

            if (typeof (fromColorScheme) == 'string') {
                colorList = sn.visual._color_scheme[fromColorScheme];
            }
            else if (Array.isArray(fromColorScheme)) {
                fromColorScheme.forEach(function(scheme) {
                    colorList = colorList.concat(sn.visual._color_scheme[scheme])
                });
            }

            if ((requiredColorCount > 0) && _isNotNullOrEmpty(colorList)) {
                var availableColorCount = colorList.length;
                if (availableColorCount > 0) {
                    while (result.length < requiredColorCount) {
                        newColorIndex = Math.floor(Math.random() * availableColorCount);
                        if (result.length >= availableColorCount || result.indexOf(colorList[newColorIndex]) < 0) {
                            result.push(colorList[newColorIndex]);
                        }
                    }
                }
            }
            return result;
        },

        // scale helper
        _get_visual_holder_size: function (param) {
            return {
                width: param.scale.width
                        + ((param.pattern.type != 'table' && param.pattern.type != 'check-list') ? this._getLegendWidth(param) : 0)
                        + (param.scale.margin * 2)
                        + (param.scale.tile.padding * 2),
                height: param.scale.height
                        + (param.scale.margin * 2) + (param.scale.tile.padding * 2)
                        + (param.option.suppress.tileHeader == false ? param.scale.tile.headerBarHeight : 0)
            };
        },
        _get_visual_size: function (param) {
            return {
                width: param.scale.width
                        + ((param.pattern.type != 'table' && param.pattern.type != 'check-list') ? this._getLegendWidth(param) : 0)
                        + (param.scale.margin * 2)
                        + (param.scale.tile.padding * 2)
                        + (param.pattern.type == 'table'? -5: 0),
                height: param.scale.height
                        + (param.scale.margin * 2)
                        + (param.scale.tile.padding * 2)
                        + (param.option.suppress.tileHeader == false ? -1 * param.scale.tile.headerBarHeight / 2 : 0)
                
            };
        },
        _getLegendWidth: function (param) {
            if (this._get_position_list({ scaleAffected: 'width' }).indexOf(param.pattern.legend.position) >= 0) {
                return param.scale.legend.width;
            } else {
                return 0;
            }
        },
        _getLegendHeight: function (param) { // do-9: is it required to customize the legend height ? analyse and decide.
            if (this._get_position_list({ scaleAffected: 'height' }).indexOf(param.pattern.legend.position) >= 0) {
                return param.scale.legend.height;
            } else {
                return 0;
            }
        },

        _set_colors_from_scheme: function (param, enforeColorSchemeFetch) {
            if (param.pattern.dataKey.plotAxis.colors == null || enforeColorSchemeFetch == true) {
                param.pattern.dataKey.plotAxis.colors = param.option.suppress.randomColors == true ?
                        sn.visual._core_lib._get_colors_of_scheme(param.option.colorScheme || defaultParam.option.colorScheme)
                    : sn.visual._core_lib._get_colors_random(param.option.colorScheme || defaultParam.option.colorScheme, param.data.length);
            }
        },

        // visual style helper
        _set_axis_text_direction: function (axis, axisPosition, textDirection) {
            if (axisPosition == 'bottom') {
                if (textDirection == 'w-to-e') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(0)')
                        .attr('x', '0')
                        .attr('y', '10')
                        .style('text-anchor', 'middle');
                }
                else if (textDirection == 'nw-to-se') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(45)')
                        .attr('x', '0')
                        .attr('y', '10')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 'n-to-s') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(90)')
                        .attr('x', '10')
                        .attr('y', '-5')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 'ne-to-sw') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(135)')
                        .attr('x', '10')
                        .attr('y', '-5')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 'e-to-w') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(180)')
                        .attr('x', '0')
                        .attr('y', '-20')
                        .style('text-anchor', 'middle');
                }
                else if (textDirection == 'se-to-nw') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(225)')
                        .attr('x', '-7')
                        .attr('y', '-10')
                        .style('text-anchor', 'end');
                }
                else if (textDirection == 's-to-n') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(270)')
                        .attr('x', '-15')
                        .attr('y', '-3')
                        .style('text-anchor', 'end');
                }
                else if (textDirection == 'sw-to-ne') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(315)')
                        .attr('x', '-7')
                        .attr('y', '0')
                        .style('text-anchor', 'end');
                }
            }
            else if (axisPosition == 'left') {
                if (textDirection == 'w-to-e') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(0)')
                        .attr('x', '-10')
                        .attr('y', '0')
                        .style('text-anchor', 'end');
                }
                else if (textDirection == 'nw-to-se') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(45)')
                        .attr('x', '-7')
                        .attr('y', '7')
                        .style('text-anchor', 'end');
                }
                else if (textDirection == 'n-to-s') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(90)')
                        .attr('x', '0')
                        .attr('y', '15')
                        .style('text-anchor', 'middle');
                }
                else if (textDirection == 'ne-to-sw') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(135)')
                        .attr('x', '7')
                        .attr('y', '5')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 'e-to-w') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(180)')
                        .attr('x', '7')
                        .attr('y', '0')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 'se-to-nw') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(225)')
                        .attr('x', '7')
                        .attr('y', '-7')
                        .style('text-anchor', 'start');
                }
                else if (textDirection == 's-to-n') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(270)')
                        .attr('x', '0')
                        .attr('y', '-17')
                        .style('text-anchor', 'middle');
                }
                else if (textDirection == 'sw-to-ne') {
                    axis.selectAll('text')
                        .attr('transform', 'rotate(315)')
                        .attr('x', '-7')
                        .attr('y', '-7')
                        .style('text-anchor', 'end');
                }
            }
        },
        _set_legend: function (legend_params) {
            var param = legend_params.param,
                legendHolder = legend_params.legendHolder,
                keys = legend_params.keys,
                colorScale = legend_params.colorScale;

            var legendMargin = 7;

            var legendSize = (param.scale.width + param.scale.height) / 100 * 2 + legendMargin;
            var legendWidth = param.scale.legend.width;

            var legend = legendHolder.append("g")
                .attr('class', 'v-legend v-text-noselect')
                .style("font-size", 7 + legendSize / 9)
                .style("text-anchor", "end")
                .attr("transform", function (d, i) {
                    let result = 'translate(0, 0)';

                    if (param.pattern.legend.position == 'ne') {
                        if (param.pattern.legend.orientation == 'vertical') {
                            if (param.pattern.type == 'pie' || param.pattern.type == 'doughnut') {
                                let coord = legendHolder.attr('transform').replace('translate(', '').replace(')', '').split(',');
                                result = "translate("
                                            + -1 * (parseInt(coord[0]) - 40) + ","
                                            + -1 * (parseInt(coord[1]) + 10) + ")";
                            }
                            /*  // **** this needs to be enabled only when the scale adjustment method is implemented
                            else //do-0: need to reconsider re-translating legends for the visual types one-by-one while adjusting the scale.
                            if (param.pattern.type == 'bar-grouped') {
                                result = "translate(" + (-1 * legendWidth / ((param.scale.width / 100) + 1)) + ",0)";
                            }
                            */
                        }
                    }

                    return result;
                })
                .selectAll("g")
                .data(function () {
                    var reverseTheKeys = sn.visual._core_lib._pattern_types._isAxisBasedVisual(param.pattern.type) && (
                           (param.pattern.legend.position == 'ne') && (param.pattern.legend.orientation == 'horizontal')
                        || (param.pattern.legend.position == 'e') && (param.pattern.legend.orientation == 'vertical')
                        || (param.pattern.legend.position == 'se') && (param.pattern.legend.orientation == 'horizontal')
                        || (param.pattern.legend.position == 'se') && (param.pattern.legend.orientation == 'vertical')
                        );

                    return reverseTheKeys == true ? keys.reverse() : keys;
                })
                .enter().append("g")
                .attr("transform", function (d, i) {
                    if (param.pattern.legend.position == 'nw') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + (-1 * (param.scale.width) + i * 100) + "," + (legendSize - legendMargin) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 'n') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + (-1 * (param.scale.width - keys.length * 100) + i * 100) + "," + (legendSize) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 'ne') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + (-i * 100) + "," + (legendSize - legendMargin) + ")";
                        } else if (param.pattern.legend.orientation == 'vertical') {
                            if (param.pattern.type == 'pie' || param.pattern.type == 'doughnut') {
                                return "translate(0, " + (i * 11) + ")"
                            }
                            else if (param.pattern.type == 'bubble') {
                                return `translate(0, ${i * 11})`;
                            }
                            else if (param.pattern.type == 'line') {
                                return `translate(0, ${i * 11})`;
                            }
                            else {
                                let coord = legendHolder.attr('transform').replace('translate(', '').replace(')', '').split(',');
                                return "translate("
                                            + -1 * (parseInt(coord[0]) - param.scale.margin) + ","
                                            + -1 * (i - 1) * (parseInt(coord[1]) - 11) + ")";
                            }
                        }
                    }
                    else if (param.pattern.legend.position == 'e') { // legend position 'e' needs to be well calculated
                        if (param.pattern.legend.orientation == 'vertical') {
                            return "translate(0," + (param.scale.height / 2 - (i + 1) * legendSize) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 'se') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + (-i * 100) + "," + (param.scale.height + 70) + ")";
                        }
                        else if (param.pattern.legend.orientation == 'vertical') {
                            return "translate(0," + (param.scale.height - (i + 1) * legendSize) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 's') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + (-1 * (param.scale.width - keys.length * 100) + i * 100) + "," + (param.scale.height + 70) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 'sw') {
                        if (param.pattern.legend.orientation == 'horizontal') {
                            return "translate(" + ((-1 * param.scale.width) + i * 100) + "," + (param.scale.height + 70) + ")";
                        }
                    }
                    else if (param.pattern.legend.position == 'w') { //do-3: legend position 'w' needs to be well calculated
                        if (param.pattern.legend.orientation == 'vertical') {
                            return "translate(" + (-1 * param.scale.width - param.scale.margin) + "," + ((param.scale.height - keys.length * legendSize) / 2 + (i + 1) * legendSize) + ")";
                        }
                    }

                    // not implemented handling
                    console.warn('sn.visual: Legend position ' + param.pattern.legend.position
                            + ' orientation ' + param.pattern.legend.orientation + ' is not yet implemented.');
                    return "translate(-" + param.scale.height + ", -" + param.scale.width + ")"; // purposely making the legend invisible to handle the case not implemented
                });

            if (param.pattern.legend.shape == 'rect') {
                legend.append('rect')
                    .attr("class", "ve-default")
                    .attr("x", param.scale.width + param.scale.margin + this._getLegendWidth(param) - (legendSize + legendMargin / 10))
                    .attr("y", (legendSize + legendMargin) / 2 + legendMargin - 5)
                    .attr("width", 9)
                    .attr("height", 9)
                    .style('fill', function (d, i) {
                        return colorScale(i);
                    });
            }
            else if (param.pattern.legend.shape == 'circle') {
                legend.append('circle')
                    .attr("class", "ve-default")
                    .attr("cx", function(d, i) { 
                        return param.scale.width + param.scale.margin + sn.visual._core_lib._getLegendWidth(param) - (legendSize + legendMargin / 10);
                    })
                    .attr("cy", function(d, i) {
                        return (legendSize + legendMargin) / 2 + legendMargin;
                    })
                    .attr("r", 5)
                    .style('fill', function (d, i) {
                        return colorScale(i);
                    });
            }

            legend.append("text")
                .attr("x", param.scale.width + param.scale.margin + this._getLegendWidth(param) - (legendSize + legendMargin))
                .attr("y", (legendSize + legendMargin) / 2 + legendMargin / 0.75)
                .attr("text-anchor", "end")
                .text(function (d) {
                    return d;
                });
        },

        // do-9: write _define_visual_line_element_shadow
        // do-9: write _define_visual_bar_grouped_element_shadow

        _define_visual_bar_element_shadow: function (draw_param) {
            var visual = draw_param.visual;
            var defs = visual.append("defs");
            var filter = defs
                        .append("filter")
                        .attr("id", "drop-shadow");

            filter.append("feGaussianBlur")
                .attr("in", "SourceAlpha")
                .attr("stdDeviation", 2)
                .attr("result", "blur");

            filter.append("feOffset")
                .attr("in", "blur")
                .attr("dx", 1)
                .attr("dy", -1)
                .attr("result", "offsetBlur");

            var feMerge = filter.append("feMerge");
            feMerge.append("feMergeNode")
                .attr("in", "offsetBlur")
            feMerge.append("feMergeNode")
                .attr("in", "SourceGraphic");
        },

        // visual user interaction - highlight helper
        /*
        _set_ui_mouse_behavior: function (_param) {
            return;
            
            //do-3: temporarily the ui mouse behavior routine is suspended, this needs to be generatlized

            d3.selectAll(_param.currentElementSelector)
                .on('mousemove', function (d) {
                    //d3.select('#' + d3.select(this).attr('_sn_tooltip_id'))
                    //    .style("left", d3.event.pageX - 50 + "px")
                    //    .style("top", d3.event.pageY - 70 + "px")
                    //    .style("display", "inline-block")
                    //    .html("Visual element tool tip");
                })
                .on("mouseout", function (d) {
                    //d3.select('#' + d3.select(this).attr('_sn_tooltip_id'))
                    //    .style("display", "none");
                })
                .on('mouseenter', function () {
                    d3.select(this).classed('ve-mouse-on', true);
                })
                .on('mouseleave', function () {
                    d3.select(this).classed('ve-mouse-on', false);
                })
                .on('click', function () {
                    sn.visual._core_lib._reset_current_selection_state();
                    sn.visual._core_lib._set_new_selection_state(this.id);
                });
        
        },
        */

        // visual switch helpers
        _reset_current_selection_state: function () {
            d3.selectAll('.ve-active').classed('ve-active', false);
            d3.selectAll('.ve-inactive').classed('ve-inactive', false);
        },

        _set_new_selection_state: function (id) {
            var selected = id;
            d3.selectAll('.ve-default').classed('ve-inactive', true);
            d3.selectAll('#' + selected).classed('ve-active', true);
        },

        _get_draw_visual_routine: function (type, orientation) {
            var result = null;

            if (type == 'bar') {
                if (orientation == 'vertical') {
                    result = this._draw_visual_bar_vertical;
                }
                else
                    if (orientation == 'horizontal') {
                        result = this._draw_visual_bar_horizontal;
                    }
            }
            else if (type == 'bar-grouped') {
                if (orientation == 'vertical') {
                    result = this._draw_visual_bar_grouped_vertical;
                }
            }
            else if (type == 'line' || type == 'curve-line') {
                if (orientation == 'vertical') {
                    result = this._draw_visual_line_vertical;
                }
                else
                    if (orientation == 'horizontal') {
                        console.info("sn.visual: !!! Visual 'line-horizontal' is yet to be implemented !!!")
                        //do-5: implement _draw_visual_line_horizontal
                        //this._core_lib._draw_visual_line_horizontal;
                    }
            }
            else if (type == 'bubble') {
                if (orientation == 'vertical') {
                    result = this._draw_visual_bubble_vertical;
                }
                else
                    if (orientation == 'horizontal') {
                        console.info("sn.visual: !!! Visual 'bubble-horizontal' is yet to be implemented !!!");
                        //do-5: implement _draw_visual_bubble_horizontal
                        //this._core_lib._draw_visual_bubble_horizontal(param);
                    }
            }
            else if (type == 'pie' || type == 'doughnut') {
                result = this._draw_visual_pie_doughnut;
            }
            else if (type == 'table') {
                result = this._draw_visual_table_vertical;
            }
            else if (type == 'check-list') {
                result = this._draw_visual_check_list_vertical;
            }
            return result;
        },

        _refresh_children: function (visualsToRefresh, selectedData) {
            visualsToRefresh.forEach(function (visual) {
                var _draw_visual = sn.visual._core_lib._get_draw_visual_routine(visual.param.pattern.type, visual.param.pattern.orientation);

                var primaryKey = visual.param.pattern.dataKey.baseAxis.key;
                var valueKey = visual.param.pattern.dataKey.plotAxis.key;
                var dataToDraw = [];
                const dataSourceKey = visual.param.pattern.dataKey.baseAxis.dataSourceKey;

                //do-0: REVIEW - ***CODE BREAKS*** here dataSourceKey is null when 'bar-grouped' is the leaf-most-node. this happens in 'auto' (may be also in 'semi-auto') mode

                selectedData.forEach(function (selectedItem) {
                    let fromSelected;
                    if (_isNotNullOrEmpty(selectedItem[dataSourceKey])) {
                        fromSelected = selectedItem[dataSourceKey];
                    }
                    else if (_isNotNullOrEmpty(selectedItem.data[dataSourceKey])) {
                        fromSelected = selectedItem.data[dataSourceKey];
                    }

                    fromSelected.forEach(function (item) {
                        dataToDraw.push(item);
                    });
                });

                var parentVisualIndex = sn.visual._model.findIndex(item => 'visual' + item._id == visualsToRefresh[0]["parent-visual-id"]);

                visual.param._data = sn.visual.helper.reduceSelectedAll(visual.param, dataToDraw);

                visual.param.pattern.dataKey.plotAxis.colors = visual.param.option.suppress.randomColors == true ?
                        sn.visual._core_lib._get_colors_of_scheme(visual.param.option.colorScheme || sn.visual.default.param.option.colorScheme)
                    : sn.visual._core_lib._get_colors_random(visual.param.option.colorScheme || sn.visual.default.param.option.colorScheme, visual.param._data.length);

                d3.selectAll('#' + visual['visual-id'] + ' > *').remove();
                _draw_visual(visual);

                if (visual.param.children != null && visual.param.children.length > 0) {
                    this.childVisualsToRefresh = sn.visual._model.filter(item => item['parent-visual-id'] == visual['visual-id']);
                    sn.visual._core_lib._refresh_children(
                        this.childVisualsToRefresh,
                        dataToDraw
                    );
                }
            });
        },

        _sort_raw_data: function (param) {
            var sortKeyList = this._get_all_base_keys(param);
            var sortComparePhrase_1 = '';
            var sortComparePhrase_2 = '';
            sortKeyList.forEach(function (sortKey) {
                sortComparePhrase_1 += `sck_1['${sortKey}'] + `;
            });
            sortComparePhrase_1 += ' ""';
            sortComparePhrase_2 = sortComparePhrase_1.replace(/sck_1/g, 'sck_2');

            param.dataSource.data = param.dataSource.data.sort(function (sck_1, sck_2) {
                return d3.ascending(
                    eval(sortComparePhrase_1),
                    eval(sortComparePhrase_2)
                );
            });
        },

        // done: (introduced in plotAxis level hence can be different for each child in param) *** HIGH *** make a 'baseValueKey' in every level of param to pickup the plot value from the raw data.
        // : precautions to take :
        // 1) if the element selector has the visual already rendered, ensure to clear that before rendering the new visual.
        // 2) from the model ensure to remove ( or mark removed and remove later) the visual reference.

        _internal_traverse_to_transform: function (param, currentRow, result) {
            if (param) {
                let resultIndex = result.findIndex(r => r[param.pattern.dataKey.baseAxis.key] == currentRow[param.pattern.dataKey.baseAxis.key]);

                if (resultIndex == -1) {
                    let newDatum = {};
                    newDatum[param.pattern.dataKey.baseAxis.key] = currentRow[param.pattern.dataKey.baseAxis.key];
                    newDatum[param.pattern.dataKey.plotAxis.key] = currentRow[param.pattern.dataKey.plotAxis.valueKey];

                    // handling more keys
                    if (_isNotNullOrEmpty(param.pattern.dataKey.baseAxis.moreKey) == true) {
                        param.pattern.dataKey.baseAxis.moreKey.forEach(function (item) {
                            newDatum[item.key] = currentRow[item.key];
                        });
                    }

                    result.push(newDatum);

                    resultIndex = result.length - 1;
                }
                else {
                    result[resultIndex][param.pattern.dataKey.plotAxis.key] += (currentRow[param.pattern.dataKey.plotAxis.valueKey] == null ? 0 : currentRow[param.pattern.dataKey.plotAxis.valueKey]);
                }

                if (param.children) {
                    for (var childIndex = 0; childIndex < param.children.length; childIndex++) {
                        child = param.children[childIndex];

                        if (_isNotNullOrEmpty(child.pattern.dataKey.baseAxis.dataSourceKey)) {
                            if (result[resultIndex][child.pattern.dataKey.baseAxis.dataSourceKey] == null) {
                                result[resultIndex][child.pattern.dataKey.baseAxis.dataSourceKey] = [];
                            }
                            this._internal_traverse_to_transform(child, currentRow, result[resultIndex][child.pattern.dataKey.baseAxis.dataSourceKey]);
                        }
                        else if (_isNotNullOrEmpty(child.pattern.dataKey.baseAxis.groupKey)) {
                            if (result[resultIndex][child.pattern.dataKey.baseAxis.groupKey] == null) {
                                result[resultIndex][child.pattern.dataKey.baseAxis.groupKey] = [];
                            }

                            let groupData = result[resultIndex][child.pattern.dataKey.baseAxis.groupKey];
                            let groupElementKey = child.pattern.dataKey.baseAxis.groupElementKey;
                            let valueKey = child.pattern.dataKey.plotAxis.valueKey;

                            
                            for (var index = 0; index < groupElementKey.length; index++) {
                                let geIndex = groupData.findIndex(g => g['key'] == currentRow[groupElementKey[index]]);
                                if (geIndex < 0) {
                                    let geData = {};
                                    geData['key'] = currentRow[groupElementKey[index]];
                                    geData['value'] = 0;  // child.pattern.dataKey.plotAxis.key
                                    groupData.push(geData);
                                    geIndex = groupData.length - 1;
                                }
                                groupData[geIndex]['value'] += typeof (currentRow[valueKey[index]]) == 'number' ? currentRow[valueKey[index]] : 0;  // child.pattern.dataKey.plotAxis.key
                            }
                        }
                    };
                }
            }
        },

        _transform_rawdata_to_data: function (param) {
            this.transform_success;

            try {
                // first: sort the raw data as per the order of keys required to transform
                let rawData = param.dataSource.data;

                // second: process the raw data, transform to the required structure
                var result = [];
                var currentIndex = 0;

                while (currentIndex < rawData.length) {
                    this._internal_traverse_to_transform(param, rawData[currentIndex], result);

                    currentIndex++;
                }
                param.data = result;
                this.transform_success = true;
            }
            catch (e) {
                this.transform_success = false;
                console.error(e.message);
            }

            return (this.transform_success);
        },

        _get_all_base_keys: function (param) {
            var result = [];
            if (param) {
                result.push(param.pattern.dataKey.baseAxis.key);
                if (param.children) {
                    param.children.forEach(function (child) {
                        sn.visual._core_lib._get_all_base_keys(child).forEach(function (childResult) {
                            if (result.indexOf(childResult) < 0) {
                                result.push(childResult);
                            }
                        });
                    })
                }
            }
            return result;
        },

        _is_group_key_one_of_data_source_key: function (param) {
            let statement;
            if (_isNotNullOrEmpty(param.pattern.dataKey.baseAxis.groupKey) && param.children.length > 0) {
                statement = false;
                for (var index = 0; index < param.children.length; index++) {
                    if (this._is_group_key_one_of_data_source_key(param.children[index]) == true) {
                        if (param.pattern.dataKey.baseAxis.groupKey == param.children[index].pattern.dataKey.baseAxis.dataSourceKey) {
                            statement = true;
                            break;
                        }
                    }
                }
            }
            else {
                statement = true;
            }

            return (statement);
        },

        // base ground to draw visual
        _get_visual_ground_created: function (param, _draw_visual) {
            var _unique_id = sn.visual.helper.getUniqueId();

            // handle this scale manipulation through standardization
            if (param.pattern.type == 'check-list') {
                param.scale.width = 75;
                param.scale.height = 150;
                param.scale.margin = 2;
                param.scale.padding = 2;
            }

            var visualHolderSize = this._get_visual_holder_size(param);
            let _draw_visual_title = function () {
                d3.selectAll('#' + _get.id.titleContainer(_unique_id) + ' > *').remove();

                if (_get.d3.holder(_unique_id).classed('vh-full') == true) {
                    _get.d3.titleContainer(_unique_id).attr('style', _emptyStr);
                    let visualTitle = _get.d3.titleContainer(_unique_id)
                        .append('div')
                        .html(` <table class="v-text-noselect" style="z-order: 9999999;">
                                <tr>
                                <td id="${_get.id.resizeSmall(_unique_id)}_1" title="Restore back to normal size"><span class="fa fa-angle-left" style="color:gray;"></span></td>
                                <td id="${_get.id.resizeSmall(_unique_id)}_2" title="Restore back to normal size" style="font-size: x-small; opacity: 0.5;">&nbsp;Back</td>
                                <td>&nbsp;&nbsp;&nbsp;&nbsp;|&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                <td class="v-head-text-holder" style="font-size: small; font-weight:800;">${param.basic.title}</td>
                                </tr>
                                </table>
                        `);
                    visualTitle.selectAll('#' + _get.id.resizeSmall(_unique_id) + '_1').on('click', function () {
                        _tile_min_max();
                    })
                    visualTitle.selectAll('#' + _get.id.resizeSmall(_unique_id) + '_2').on('click', function () {
                        _tile_min_max();
                    })

                    visualHeader
                        .style('background-color', param.scale.tile.backgroundColor)
                        .style('color', sn.visual._core_lib._get_color_readable(param.scale.tile.backgroundColor));
                }
                else {
                    //_get.d3.titleContainer(_unique_id)
                    visualHeader
                        .style('background-color', _get.d3.holder(_unique_id).style('border-color')) //do-9: set backgound color of header as per the theme
                        .style('margin', '-' + param.scale.tile.padding + 'px')
                        .style('border', '1px solid ' + _get.d3.holder(_unique_id).style('background-color'))
                        .style('padding', '2px')
                        .style('border-radius', _get.d3.holder(_unique_id).style('border-radius'))
                        .style('font-size', 'x-small')
                        .style('font-weight', '900');
                        

                    let visualTitle = _get.d3.titleContainer(_unique_id)
                        .append('div')
                        .attr('id', _get.id.title(_unique_id));

                    visualTitle
                        .attr('class', 'vv-text-title v-text-noselect v-text-overflow-hide')
                        .style('width', (visualHolderSize.width - 50) + 'px')
                        .style('float', 'left')
                        .on('mouseenter', function () {
                            _set.element.text_as_title(visualTitle.attr('id'));
                        })
                        .text(param.basic.title);

                    _set.element.text_as_title(visualTitle.attr('id')); // to show tooltip of title only if exceeds

                    _get.d3.titleContainer(_unique_id)
                        //.style('color', sn.visual._core_lib._get_color_readable(_get.d3.titleContainer(_unique_id).style('background-color')))  // param.scale.tile.backgroundColor
                        .style('height', param.scale.tile.headerBarHeight + 'px')
                        .style('float', 'left')
                        .style('width', (visualHolderSize.width - 50) + 'px');

                    visualHeader
                        .style('color', sn.visual._core_lib._get_color_readable(_get.d3.titleContainer(_unique_id).style('background-color')))  // param.scale.tile.backgroundColor
                }
            }
            let _tile_min_max = function () {
                d3.selectAll('#' + _get.id.visual(_unique_id) + ' > *').remove();

                if (_get.d3.holder(_unique_id).classed('vh-full') == false) /* proceeding to maximize */ {
                    param.scale._prev_width = param.scale.width;
                    param.scale._prev_height = param.scale.height;

                    param.scale.width = window.innerWidth - window.innerWidth * 0.25;
                    param.scale.height = window.innerHeight - window.innerHeight * 0.25;
                    
                    _get.d3.resizeFull(_unique_id)
                    /*  -- this is required for old style
                        .classed('fa-window-maximize', false)
                        .classed('fa-window-restore', true)
                        .attr('title', 'Resize small');
                    */
                        .style('display', 'none');  // this is for new style

                    _get.d3.holder(_unique_id)
                        .style('z-index', sn.visual.default.maxZIndex)
                        .classed('vh-full', true);

                    window.onresize =
                        function () {
                            _get.d3.holder(_unique_id)
                                .style('width', window.innerWidth + 'px')
                                .style('height', window.innerHeight + 'px')
                        };
                }
                else
                /* proceeding to restore */ 
                {
                    param.scale.width = param.scale._prev_width != null ? param.scale._prev_width : sn.visual.default.param.scale.width;
                    param.scale.height = param.scale._prev_height != null ? param.scale._prev_height : sn.visual.default.param.scale.height;
                    _get.d3.resizeFull(_unique_id)
                        .classed('fa-window-maximize', true)
                        .classed('fa-window-restore', false)
                        .attr('title', 'Resize to full')
                        .style('display', 'block') // this is for new style

                    _get.d3.holder(_unique_id)
                        .classed('vh-full', false);

                    window.onresize = null;
                }

                visualHolderSize = sn.visual._core_lib._get_visual_holder_size(param);
                if (_get.d3.holder(_unique_id).classed('vh-full') == true) {
                    visualHolder
                        .style('width', window.innerWidth + 'px')
                        .style('height', window.innerHeight + 'px')
                        .style('top', '-5px')
                        .style('left', '-5px');

                    //visualHolder
                    //    .style('min-width', visualHolder.style('width'))
                    //    .style('min-height', visualHolder.style('height'))
                }
                else {
                    visualHolder
                        .style('width', visualHolderSize.width + 'px')
                        .style('height', visualHolderSize.height + 'px')
                        .style('top', (window.innerHeight - visualHolderSize.height) / 2 + 'px')
                        .style('left', (window.innerWidth - visualHolderSize.width) / 2 + 'px');
                    _get.d3.title(_unique_id).style('width', (visualHolderSize.width - 30) + 'px');
                }

                _draw_visual_title();
                _draw_visual(visual_ground);

                _get.d3.visual(_unique_id)
                    .style('width', (visualHolderSize.width - 2 * param.scale.tile.padding - 2 * param.scale.tile.padding) + 'px')
                    .style('height', (visualHolderSize.height + (_get.d3.holder(_unique_id).classed('vh-full') ? param.scale.margin : 0) - 2 * param.scale.tile.margin - 2 * param.scale.tile.padding) + 'px');

                if (sn.visual._core_lib._pattern_types._isVisualChartType(param.pattern.type)) {
                    sn.visual._core_lib._decorate_visual_based_on_option(visual_ground);
                }
            }

            // note: this anchor is just to locate the visual. shall remove later.
            d3.select(param.holder.elementSelector)
                .append('a')
                .attr('name', _get.id.holder(_unique_id));

            var visualHolder = d3.select(param.holder.elementSelector)
                .append('div')
                .attr('id', _get.id.holder(_unique_id))
                .attr('class', 'vh-default')
                .style('width', visualHolderSize.width + 'px')
                .style('height', visualHolderSize.height + 'px')
                .style('margin', param.scale.tile.margin + 'px')
                .style('padding', param.scale.tile.padding + 'px')
                .style('background-color', param.scale.tile.backgroundColor);

            _get.d3.holder(_unique_id).style('border-color', param.scale.tile.hostColor);

            var visualTooltip = visualHolder
                .append("div")
                .attr('id', _get.id.tooltip(_unique_id))
                .attr("class", "ve-tooltip-compact")

            var visualHeader = visualHolder
                .append('div')
                .attr('id', _get.id.header(_unique_id))
                //.style('width', (visualHolderSize.width - 30) + 'px')
                //.style('float', 'left');

            visualHeader.append('div')
                .attr('id', _get.id.titleContainer(_unique_id));

            _get.d3.header(_unique_id)
                .style('height', param.scale.tile.headerBarHeight + 'px');

            if (param.option.suppress.tileHeader == false) {
                _draw_visual_title();
            }

            var visual;
                //do-0: introduce a method to check in the pattern list and use.
             if (param.pattern.type == 'table' || param.pattern.type == 'check-list') {
                visual = visualHolder.append('div');
             }
             else {
                visual = visualHolder.append('svg');
                 //do-4: save svg as file, automatically downloading all svgs. do fix.
                 // visual.on('click', sn.visual.helper.saveAsSvg(visual, 'test.svg'));
            };

            visual.attr('id', _get.id.visual(_unique_id))
                 // .attr('width', (visualHolderSize.width + sn.visual._core_lib._getLegendWidth(param) - 2 * param.scale.tile.padding - 2 * param.scale.margin) + 'px')
                  .style('width', sn.visual._core_lib._get_visual_size(param).width + 'px')
                  .style('height', sn.visual._core_lib._get_visual_size(param).height + 'px')
                  .on('click', function () {
                      if (this == d3.event.target) {
                          sn.visual._core_lib._reset_current_selection_state();
                      }
                  });

            var visual_ground = {
                '_id': _unique_id,
                'visual-holder': visualHolder,
                'visual-header': visualHeader,
                'visual-tooltip': visualTooltip,
                'visual': visual,
                'param': param
                  };

                      // do-0: replace the bootstrap dropdown with another one
                      /*
                                  let dd_menu_container = visualHeader.append('div')
                                          .attr('class', 'dropdown v-text-noselect')
                                          .style('font-size', '8px');
                                  let dd_menu_dropdown = dd_menu_container.append('span')
                                          .attr('class', 'dropdown-toggle')
                                          .attr('data-toggle', 'dropdown');
                                  dd_menu_dropdown.append('span')
                                      .attr('title', 'More...')
                                      .attr('class', 'fa fa-ellipsis-v')
                                      .attr('style', 'float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer;')
                                      .style('color', _get.d3.header(_unique_id).style('color'))
                                      .on('click', function () {
                                          //menu_list
                                          //    .style('top', this.offsetTop)
                                          //    .style('left', this.offsetLeft);
                                      });
                      
                                  let menu_list = dd_menu_container.append('ul').attr('class', 'dropdown-menu')
                                      .style('min-width', '100px')
                                      .style('left', param.scale.width + (2 * param.scale.margin) + 'px')
                                      .style('top', '14px')
                                      .style('font-size', 'xx-small')
                                  let menu_item;
                                 
                                  menu_list.append('li').attr('class', 'disabled').html('<a href="#">Show data</a>');
                      
                                  menu_list.append('li').attr('class', 'divider');
                      
                                  // expand
                                  if (_isNotNullOrEmpty(param.basic.expandUrl)) {
                                      menu_list.append('li')
                                          .html('<a href="#">Expand</a>')
                                          .attr('onclick', `window.location = "${param.basic.expandUrl}";`)
                                          .attr('target', '_top');
                                  }
                      
                                  // resize to full
                                  menu_item = menu_list.append('li')
                                  menu_item.append('a')
                                      .attr('href', '#')
                                      .text('Resize to full')
                                      .on('click', function () {
                                          _tile_min_max();
                                      });
                      
                                  menu_list.append('li').attr('class', 'divider');
                                  menu_list.append('li').attr('class', 'disabled').html('<a href="#">About</a>');
                      */
                      //if (_isNotNullOrEmpty(param.basic.expandUrl)) {
            if (param.listener && param.listener.onExpandClick) {
                let expand = visualHeader.append('span')  // button to expand 
                    .attr('id', 'expand_' + _unique_id)
                    .attr('title', 'Expand')
                    .attr('class', 'fa fa-external-link')  //-square
                    .attr('style', 'float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer;')
                    .style('color', _get.d3.header(_unique_id).style('color'))
                    .on('click', function () {
                        param.listener.onExpandClick();
            })

                //expand.attr('onclick', `window.location = "${param.basic.expandUrl}";`)
                //    .attr('target', '_blank');
                  }

            if (param.option.suppress.toolBar != true) {
                visualHeader.append('span') // maximize / restore
                    .attr('id', _get.id.resizeFull(_unique_id))
                    .attr('title', 'Resize to full')
                    .attr('class', 'fa fa-window-maximize')
                    .attr('style', 'float: right; opacity: 0.7; padding: 5px; margin: -2px; top: -2px; cursor: pointer;')
                    .style('color', _get.d3.header(_unique_id).style('color'))
                    .on('click', function () {
                        _tile_min_max();
            });
                  };
            if (sn.visual._core_lib._pattern_types._isVisualChartType(param.pattern.type)) {
                sn.visual._core_lib._decorate_visual_based_on_option(visual_ground);
                  }

            return visual_ground;
        },
        _decorate_visual_based_on_option: function (visual_ground) {
            var param = visual_ground['param'],
                visual = visual_ground['visual'],
                visualHolder = visual_ground['visual-holder'],
                visualHeader = visual_ground['visual-header'];

            if (param.option.suppress.visualBorder == false) {
                visual.style('border', '1px dashed lightblue')
            }

            visualHolder.classed('vh-invisible', param.option.suppress.tileBorder == true);

            if (param.option.suppress.tileHeader == true) { // if tile header is not going to be shown, means the title should be displayed part of the visual
                visual.append("text")
                    .attr("x", 2 * param.scale.margin + ((param.scale.width - 2 * param.scale.margin) / 2))
                    .attr("y", param.scale.margin / 1.5)
                    .attr("class", "vv-text-title v-text-noselect")
                    //do-3: perform title decoration as per the font option provided. *** make all the decorations through css classes ****
                    //.style("font-size", "16px")
                    //.style("text-decoration", "underline")
                    .text(param.basic.title);

                visualHeader
                    .style('background-color', visualHolder.attr('background-color'))
                    .style('color', visualHolder.attr('background-color'));
            }
        },
        _visual_element_click_handling: function (context) {
            _switch_selection = function (visualId, selectAll) {
                this.group = d3.select('#' + visualId + ' .ve-inter-group-default');

                this.group.selectAll('.ve-inter-default').classed('ve-inter-unselected', selectAll == false);
                this.group.selectAll('.ve-inter-default').classed('ve-inter-selected', selectAll == true);
            };
            _invoke_child_visuals = function (visualId) {
                this.visualsToRefresh = sn.visual._model.filter(item => item['parent-visual-id'] == visualId);
                this.group = d3.select('#' + visualId + ' .ve-inter-group-default');

                var selectedData = [];
                var selectedRows;
                if (this.group.selectAll('.ve-inter-default').filter('.ve-inter-unselected').size() == 0) { // if not specific row is selected by click / control click
                    selectedRows = this.group.selectAll('.ve-inter-default') // select all rows
                }
                else {
                    selectedRows = this.group.selectAll('.ve-inter-default').filter('.ve-inter-selected'); // select only the selected rows
                }

                selectedRows.each(function (rowAsData) {
                    selectedData.push(rowAsData);
                });

                sn.visual._core_lib._refresh_children(
                    this.visualsToRefresh,
                    selectedData
                );
            };

            if (_get.is_visual_interactable(context['param'])) {
                this.group = d3.select('#' + context['visualId'] + ' .ve-inter-group-default');

                if (d3.select(context['this']).classed('ve-inter-selected') == true && this.group.selectAll('.ve-inter-selected').size() == 1) {
                    _switch_selection(d3.select(context['this']).attr('data-visual-id'), true);  // select all
                }
                else
                    if (d3.event.ctrlKey == true) {
                        // add to selection
                        d3.select(context['this']).classed('ve-inter-selected', d3.select(context['this']).classed('ve-inter-selected') == true ? false : true);
                        d3.select(context['this']).classed('ve-inter-unselected', d3.select(context['this']).classed('ve-inter-selected') == false);
                    }
                    else {
                        _switch_selection(d3.select(context['this']).attr('data-visual-id'), false); // un-select all 
                        d3.select(context['this']).classed('ve-inter-selected', true); // select current
                    }

                _invoke_child_visuals(d3.select(context['this']).attr('data-visual-id'));
            }

            if (context['param'].listener && context['param'].listener.onVisualElementClick) {
                let userEventContext = {
                    selected: []
                };

                // prepare selected data
                try {
                    if (this.group) {
                        let selectedSource;
                        let newSelected;

                        this.group.selectAll('.ve-inter-default').filter('.ve-inter-selected').each(function (row) {
                            selectedSource = _isNotNullOrEmpty(row.data) ? row.data : row;
                            newSelected = _get.json_cloned(selectedSource);

                            // delete newSelected.key;
                            // delete newSelected.value;
                            delete newSelected._id;

                            newSelected['key'] = _isNullOrEmpty(newSelected['key']) ? selectedSource[context['param'].pattern.dataKey.baseAxis.key] : newSelected['key'];
                            newSelected['value'] = _isNullOrEmpty(newSelected['value']) ? selectedSource[context['param'].pattern.dataKey.plotAxis.key] : newSelected['value'];

                            userEventContext.selected.push(newSelected);
                        });
                    }
                }
                catch (e) {
                    if (e.message || e.stack) {
                        console.error(e.message + e.stack);
                    }
                }

                context['param'].listener.onVisualElementClick(userEventContext);
            }
        },

        _visual_element_mousemove_handling: function (context) {
            // d, i, visual, visualToolTip 
            let visualToolTip = context['visualToolTip'];
            let colorScale = context['colorScale'];
            let baseAxisKey = context['param'].pattern.dataKey.baseAxis.key;
            let plotAxisKey = context['param'].pattern.dataKey.plotAxis.key;
            let ttDistance = context['ttDistance'];  // do-0: ttDistance is no longer in use. invetigate and remove
            let _unique_id = context['id'];

            let tile_maximized = _get.d3.holder(_unique_id).classed('vh-full');

            // do-0: improve mouse context utilization for smart tooltip.
            var mouseContext = sn.visual.helper.getMouseAtContext(context['visual'].attr('id'));            

            var tooltipRect = document.getElementById(visualToolTip.attr('id')).getBoundingClientRect();

            visualToolTip
                .html(`<label class="ve-inner-left-tooltip" style='background-color: ${colorScale(context['i'])}; color: ${sn.visual._core_lib._get_color_readable(colorScale(context['i']))};'>${context['d'][baseAxisKey] != null ? context['d'][baseAxisKey] : (context['d'].data[baseAxisKey] != null ? context['d'].data[baseAxisKey] : '')}</label>&nbsp;<label class="ve-inner-right-tooltip">${context['d'][plotAxisKey] != null ? context['d'][plotAxisKey] : (context['d'].data[plotAxisKey] != null ? context['d'].data[plotAxisKey] : '')}</label>`)
                .style("left", function () {
                    var left = (tile_maximized ? d3.event.offsetX : d3.event.layerX)
                        + 20
                        //+ (mouseContext['at-right-top'] == true || mouseContext['at-right-bottom'] == true ? ttDistance :
                        //        mouseContext['at-left-top'] == true || mouseContext['at-left-bottom'] == true ? -ttDistance - tooltipRect.width : -ttDistance)
                        + "px";
                    return left;

                    //return (d3.event.layerX) + 'px';
                })
                .style("top", function () {
                    var top = (tile_maximized ? d3.event.offsetY : d3.event.layerY)
                                - tooltipRect.height //- ttDistance
                                + "px";
                    return top;

                    //return (d3.event.layerY) + 'px';
                })
                .transition()
                    .duration(1500)
                    .style("display", "block"); // "inline-block");
        },

        // horizontal - bar
        _draw_visual_bar_horizontal: function (draw_param) {
            var visual = draw_param['visual'],
                visualHolder = draw_param['visual-holder'],
                visualTooltip = draw_param['visual-tooltip'],
                param = draw_param['param'];

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            let zeroPlotValue = sn.visual.default.zeroPlotValue;
            let areThereMoreKeysToPlot = (param.pattern.dataKey.plotAxis.moreKey && param.pattern.dataKey.plotAxis.moreKey.length > 0);

            var baseAxisKey = param.pattern.dataKey.baseAxis.key;
            var plotAxisKey = param.pattern.dataKey.plotAxis.key;

            if (param.option.suppress.elementShadow == false) {
                sn.visual._core_lib._define_visual_bar_element_shadow(draw_param);  //do-5: change _define_visual_bar_element_shadow for horizontal
            }

            function is_small_visual() {
                return (param.scale.width <= 100 || param.scale.height <= 100);
            }
            function get_normal_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 12 : 9);
            }
            function get_transition_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 14 : 12);
            }

            // axis handling
            var baseScale = d3.scaleBand().range([param.scale.height, param.scale.margin]).padding(0.4)
            var plotScale = d3.scaleLinear().range([param.scale.margin + 1, param.scale.width]);
            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var g = visual.append("g")
                    .attr('class', 've-inter-group-default')
                    .attr("transform", "translate(" + (param.scale.margin * 1) + "," + 0 + ")");

            baseScale.domain(param._data.map(function (d) {
                return eval('d["' + param.pattern.dataKey.baseAxis.key + '"]');
            }));
            plotScale.domain([0, d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            })]).nice();

            var leftAxis = g.append('g')
                .attr('transform', 'translate(1, 0)')
                .attr('class', 'va-text-baseaxis v-text-noselect')
                .style('font-size', get_normal_font_size() + 'px') // '6px')
                .style('cursor', 'default')
                .style('fill-opacity', '1')
                .call(d3.axisLeft(baseScale).tickFormat(function (d) {
                    return (param.pattern.dataKey.baseAxis.prefix != null ? param.pattern.dataKey.baseAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.baseAxis.suffix != null ? param.pattern.dataKey.baseAxis.suffix : '');
                })
            );
            sn.visual._core_lib._set_axis_text_direction(leftAxis, 'left', param.pattern.dataKey.baseAxis.textDirection);

            leftAxis
                .on('mousemove', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            var bottomAxis = g.append("g")
                .attr("transform", "translate(" + -1 * param.scale.margin + ", " + param.scale.height + ")")
                .style('font-size', get_normal_font_size() + 'px') // '6px')
                .style('cursor', 'default')
                .attr('class', 'va-text-plotaxis v-text-noselect')
                .call(d3.axisBottom(plotScale).tickFormat(function (d) {
                    return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                }));
            sn.visual._core_lib._set_axis_text_direction(bottomAxis, 'bottom', param.pattern.dataKey.plotAxis.textDirection);

            bottomAxis
                .on('mousemove', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px') // '10px')
                })
                .on('mouseout', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px') // '6px')
                });

            let baseTitle = visual.append("text")
                .attr("class", "va-text-baseaxis-title v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin / 7) + "," + ((param.scale.height + 2 * param.scale.margin) / 2) + ") rotate(270)")
                .style('font-size', get_normal_font_size() + 'px') // '6px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.baseAxis.title);

            baseTitle
                .on('mousemove', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let plotTitle = visual.append("text")
                .attr("class", "va-text-plotaxis-title v-text-noselect")
                .attr("transform", "translate(" + ((param.scale.width + 2 * param.scale.margin) / 2) + "," + (param.scale.height + 1.5 * param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.plotAxis.title);

            plotTitle
                .on('mousemove', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            var minValue = d3.min(param._data, function (d) { return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'); });
            var minPlotValue = d3.min(param._data, function (d) { return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')); });

            let completelyRendered = false;
            g.selectAll(".ve-default").data(param._data).enter()
                .append("rect")
                .attr("class", function () {
                    return (_get.is_visual_interactable(param) == true ? 've-inter-default ve-default ' : ' ve-default ')
                })
                // do-9: bar-horizontal - analyze the _id assignment to the visual element is required or not
                //.attr("id", function (d, i) {
                //    return d._id;
                //})
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .on('click', function (element) {
                    sn.visual._core_lib._visual_element_click_handling({
                        'this': this,
                        'visualId': d3.select(this).attr('data-visual-id'),
                        'param': param
                    });
                })
                .on('mousemove', function (d, i) {
                    if (completelyRendered == true) {
                        sn.visual._core_lib._visual_element_mousemove_handling({
                            'd': d,
                            'i': i,
                            'ttDistance': baseScale.bandwidth() / 3,
                            'colorScale': colorScale,
                            'visual': visual,
                            'visualToolTip': visualTooltip,
                            'param': param,
                            'id': draw_param['_id']
                        });
                        d3.select(this)
                            .transition()
                            .style('height', baseScale.bandwidth() + 2 + 'px')
                            .style('width', plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin + 2 + 'px')
                            .style('y', baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - 1 + 'px')
                    }
                })
                .on("mouseout", function (d) {
                    visualTooltip.style("display", "none");
                    d3.select(this)
                        .transition()
                        .style('height', baseScale.bandwidth() - 2 + 'px')
                        .style('width', plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin - 2 + 'px')
                        .style('y', baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + 1 + 'px')
                })
                .style("fill", function (d, i) { return colorScale(i); })
                .classed('ve-default-if-zero', function (d) {
                    return d[param.pattern.dataKey.plotAxis.key] <= 0;
                })
                .attr("x", 2)
                .attr("y", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]'));
                })
                .attr("height", baseScale.bandwidth())
                .attr("width", function (d) {
                    return 0;
                })
                .transition()
                .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("width", function (d) {
                        return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin;
                    })
                    .on('end', function () {
                        completelyRendered = true;
                    });

            g.selectAll(".ve-text-default").data(param._data).enter()
                .append('text')
                .attr('class', 've-text-default v-text-noselect')
                .style("fill", function (d, i) {
                    return (plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin < baseScale.bandwidth()?
                             'black'
                            : sn.visual._core_lib._get_color_readable(colorScale(i)));
                })
                .style("text-anchor", 'end')
                .style('font-size', function () {
                    return (Math.round(baseScale.bandwidth() / 10, 0) * 5) + 'px';
                })
                .text(function (d) {
                    return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
                })
                .attr("x", function (d) {
                    return this.getComputedTextLength();
                })
                .attr("y", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 1.5
                })
                .on('mousemove', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', (Math.round(baseScale.bandwidth() / 10, 0) * 5) + 4 + 'px')
                        .attr("y", function (d) {
                            return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 1.35
                        })
                })
                .on('mouseout', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', (Math.round(baseScale.bandwidth() / 10, 0) * 5) + 'px')
                        .attr("y", function (d) {
                            return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 1.5
                        })
                })
                .style('opacity', '0')
                .transition()
                .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("x", function (d) {
                        return (plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin < baseScale.bandwidth()?
                              plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 2
                            : plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin - baseScale.bandwidth() / 4);
                    })
                    .style('opacity', '1');

            // do-0: bar-horozontal - *** HIGH *** - transformation does not take care of consolidating the more keys as of now.
            // plotting if more keys mentioned in plot axis
            if (areThereMoreKeysToPlot) {
                g.selectAll(".ve-default-1").data(param._data).enter()
                   .append("circle")
                   .attr("class", "ve-default-1")
                   .attr("id", function (d, i) {
                       return d._id;
                   })
                   .style("fill", function (d, i) { return '#FFFFFF' /*'#46b4af'*/ })
                   .style("opacity", 0)
                    //do-0: bar-horizontal - more keys to plot - need to variate the cx position instead cy position
                   .attr("cx", function (d) {
                       return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin + baseScale.bandwidth() / 2;
                   })
                   .attr("cy", function (d) {
                       return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 2
                           + (param.option.suppress.transition == false ?
                                   param.scale.height - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) : 0);
                   })
                   .attr('r', function (d) {
                       return baseScale.bandwidth() / 2;
                   })
                   .transition()
                       .duration(param.option.suppress.transition == false ? 1500 : 0)
                       .style("opacity", 1)
                        //do-0: bar-horizontal - more keys to plot - need to variate the cx position instead cy position after transition as well
                       .attr("cx", function (d) {
                           return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 2 - param.scale.margin;
                       })
                       .attr("cy", function (d) {
                           let y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                       - (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize + 1 /* +1 is to keep clean bottom axis line. */ : 0);
                           return y;
                       })
                       .attr('r', function (d) {
                           return baseScale.bandwidth() / 2;
                       });

                g.selectAll(".ve-text-default-1").data(param._data).enter()
                   .append('text')
                   .text(function (d) {
                       return eval('d["'
                           + (param.pattern.dataKey.plotAxis.moreKey
                               && param.pattern.dataKey.plotAxis.moreKey.length > 0 ?
                               param.pattern.dataKey.plotAxis.moreKey[0] :
                               param.pattern.dataKey.plotAxis.key)
                           + '"]');
                   })
                   .attr("class", "ve-text-default v-text-noselect")
                   .style('font-size', function () {
                       return (Math.round(baseScale.bandwidth() / 10, 0) * 3) + 'px';
                   })
                   .style("fill", function (d, i) {
                       return '#000000'; // d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.textColor : sn.visual._core_lib._get_color_readable(colorScale(i));
                   })
                   .attr("x", function (d) {
                       return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 2 - param.scale.margin;
                   })
                   .attr("y", function (d) {
                       if (param.option.suppress.transition == false) {
                           return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                                       + (param.scale.height - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')));
                       }
                       else {
                           return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                       + baseScale.bandwidth() / 2
                       }
                   })
                   .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                   .transition()
                       .duration(param.option.suppress.transition == false ? 1500 : 0)
                       .attr("y", function (d) {
                           let y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 9
                                       - (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize + 1 /* +1 is to keep clean bottom axis line. */ : 0);

                           return y;
                       })
                       .style('opacity', '1');
            }

            // note: include the shadow as option
            g.selectAll('rect')
                .each(function () {
                    if (param.option.suppress.elementShadow == false) {
                        d3.select(this)
                            .attr("stroke-width", 1)
                            .style("filter", "url(#drop-shadow)");
                    }
                });

            /*
                        sn.visual._core_lib._set_ui_mouse_behavior({
                            currentElementSelector: '.ve-default'
                        });
            */
            // legend
            if (param.option.suppress.legend != true) {
                sn.visual._core_lib._set_legend({
                    'param': param,
                    'legendHolder': g,
                    // 'keys': param.pattern.legend._values,
                    'keys': sn.visual.helper.getLegendValues(param),
                    'colorScale': colorScale,
                    //  'shapeSize': baseScale.bandwidth()
                });
            }
        },

        // vertical - bar
        _draw_visual_bar_vertical: function (draw_param) {
            var visual = draw_param['visual'],
                visualHolder = draw_param['visual-holder'],
                visualTooltip = draw_param['visual-tooltip'],
                param = draw_param['param'];

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            let zeroPlotValue = sn.visual.default.zeroPlotValue;
            let areThereMoreKeysToPlot = (param.pattern.dataKey.plotAxis.moreKey && param.pattern.dataKey.plotAxis.moreKey.length > 0);

            var baseAxisKey = param.pattern.dataKey.baseAxis.key;
            var plotAxisKey = param.pattern.dataKey.plotAxis.key;

            if (param.option.suppress.elementShadow == false) {
                sn.visual._core_lib._define_visual_bar_element_shadow(draw_param);
            }

            function is_small_visual() {
                return (param.scale.width <= 100 || param.scale.height <= 100);
            }
            function get_normal_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 12 : 9);
            }
            function get_transition_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 14 : 12);
            }

            // axis handling
            var baseScale = d3.scaleBand().range([param.scale.margin + 1, param.scale.width + 3 * param.scale.margin]).padding(0.4);
            var plotScale = d3.scaleLinear().range([param.scale.height + param.scale.margin, param.scale.margin / 2]);
            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var g = visual.append("g")
                    .attr('class', 've-inter-group-default')
                    .attr('transform', 'translate(' + (param.scale.margin * 1) + ', 0)');

            var plotMax = d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            });

            baseScale.domain(param._data.map(function (d) {
                return eval('d["' + param.pattern.dataKey.baseAxis.key + '"]');
            }));
            plotScale.domain([0, plotMax]).nice();

            var bottomAxis = g.append("g")
                .attr("transform", "translate(" + -1 * param.scale.margin + ", " + (param.scale.height + param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px') // '6px')
                .style('cursor', 'default')
                .attr('class', 'va-text-plotaxis v-text-noselect')
                .call(d3.axisBottom(baseScale).tickFormat(function (d) {
                    return (param.pattern.dataKey.baseAxis.prefix != null ? param.pattern.dataKey.baseAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.baseAxis.suffix != null ? param.pattern.dataKey.baseAxis.suffix : '');
                })
            );
            sn.visual._core_lib._set_axis_text_direction(bottomAxis, 'bottom', param.pattern.dataKey.baseAxis.textDirection);

            bottomAxis
                .on('mousemove', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px') // '10px')
                })
                .on('mouseout', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px') // '6px')
                });
            //done: tickStyle, blankTicks, tickIncrement - to be introduced for all axis type visual like bar-horizontal
            /*
                // injection test - to me removed
                param.pattern.dataKey.plotAxis.tickStyle = 'auto'; // this includes decimals
                param.option.suppress.blankTicks = true;
                param.pattern.dataKey.plotAxis.tickIncrement = 3;
            */
            var leftAxis;
            if (param.pattern.dataKey.plotAxis.tickStyle == 'auto') {
                leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .style('font-size', get_normal_font_size() + 'px') // '6px')
                    .style('cursor', 'default')
                    .attr('class', 'va-text-baseaxis v-text-noselect')
                    .call(d3.axisLeft(plotScale)
                    .tickFormat(function (d) {
                        return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                + d
                                + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                    }));
            }
            else {
                var customPlotScale = d3.axisLeft(plotScale);

                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal') {
                    if (param.option.suppress.blankTicks == false) {
                        customPlotScale
                            .tickFormat(function (d) {
                                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal' && (d.toString().indexOf('.') >= 0 || d % param.pattern.dataKey.plotAxis.tickIncrement != 0)) {
                                    // means: either (decimal presents) or (not multiples of tickIncrement) then => return empty value
                                    return '';
                                }
                                else {
                                    return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                    + d
                                    + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                                }
                            });
                    }
                    else {
                        customPlotScale
                            .tickFormat(d3.format('.0f'))
                            .tickValues(d3.range(0, plotMax + 1, param.pattern.dataKey.plotAxis.tickIncrement));
                    }
                }
                else if (param.pattern.dataKey.plotAxis.tickStyle == 'blank') {
                    customPlotScale
                        .tickValues(d3.range(0, 0, 0));
                }

                leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .call(customPlotScale);
            }
            sn.visual._core_lib._set_axis_text_direction(leftAxis, 'left', param.pattern.dataKey.plotAxis.textDirection);

            leftAxis
                .on('mousemove', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let plotTitle = visual.append("text")
                .attr("class", "va-text-plotaxis v-text-noselect")
                .attr("transform", "translate(" + param.scale.margin / 3 + "," + ((param.scale.height + 2 * param.scale.margin) / 2) + ") rotate(270)")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.plotAxis.title);

            plotTitle
                .on('mousemove', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let baseTitle = visual.append("text")
                .attr("class", "va-text-baseaxis v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin + ((param.scale.width + param.scale.margin) / 2)) + "," + (param.scale.height
                    + (_get.d3.holder(draw_param._id).classed('vh-full')? 3 : 1.75) * param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.baseAxis.title);

            baseTitle
                .on('mousemove', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            var minValue = d3.min(param._data, function (d) { return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'); });
            var minPlotValue = d3.min(param._data, function (d) { return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')); });

            let _this_visual = {
                get_baseScale_bandwidth: function () {
                    const max_bar_width = _get.d3.holder(draw_param._id).classed('vh-full') ? 50 : 20; //100;
                    return baseScale.bandwidth() > max_bar_width ? max_bar_width : baseScale.bandwidth();
                },
                get_rect_height: function (d) {
                    let height = param.scale.height + param.scale.margin - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'));
                    return (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize : height);
                },
                get_rect_y: function(d) {
                    let y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                - (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize + 1 /* +1 is to keep clean bottom axis line. */ : 0);
                    return y;
                },
                get_text_y: function (d) {
                    let y;

                    if (areThereMoreKeysToPlot) {
                        y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + (_this_visual.get_baseScale_bandwidth() - _this_visual.get_baseScale_bandwidth() / 10) // (baseScale.bandwidth() - baseScale.bandwidth() / 10);
                    }
                    else {
                        /*
                        y = (_this_visual.get_rect_height(d) < baseScale.bandwidth()?
                                plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - (eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') == 0 ? baseScale.bandwidth() / 8 : 0)
                            :   plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 2);
                        */
                        y = (_this_visual.get_rect_height(d) < _this_visual.get_baseScale_bandwidth() ?
                                plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - (eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') == 0 ? _this_visual.get_baseScale_bandwidth() / 8 : 0)
                            : plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + _this_visual.get_baseScale_bandwidth() / 2);
                    }

                    return y;
                },

            };

            let completelyRendered = false;
            g.selectAll(".ve-default")
                .data(param._data).enter()
                .append("rect")
                .attr("class", function () {
                    return (_get.is_visual_interactable(param) == true ? 've-inter-default ve-default ' : ' ve-default ')
                })
                // do-9: bar-vertical - analyze the _id assignment to the visual element is required or not
                //.attr("id", function (d, i) {
                //    return d._id;
                //})
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .on('click', function (element) {
                    sn.visual._core_lib._visual_element_click_handling({
                        'this': this,
                        'visualId': d3.select(this).attr('data-visual-id'),
                        'param': param
                    });
                })
                .on('mousemove', function (d, i) {
                    if (completelyRendered == true) {
                        sn.visual._core_lib._visual_element_mousemove_handling({
                            'd': d,
                            'i': i,
                            'ttDistance': baseScale.bandwidth() / 3,
                            'colorScale': colorScale,
                            'visual': visual,
                            'visualToolTip': visualTooltip,
                            'param': param,
                            'id': draw_param['_id']
                        });

                        d3.select(this)
                            .transition()
                            .style('width', _this_visual.get_baseScale_bandwidth() + 2 + 'px')
                            .style('height', _this_visual.get_rect_height(d) + 1 + 'px')
                            .style('x', baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + ((baseScale.bandwidth() - _this_visual.get_baseScale_bandwidth()) / 2) - param.scale.margin - 1 + 'px')
                            .style('y', _this_visual.get_rect_y(d) - 1 + 'px')
                    }
                })
                .on("mouseout", function (d) {
                    visualTooltip.style("display", "none");

                    d3.select(this)
                        .transition()
                        .style('width', _this_visual.get_baseScale_bandwidth() + 'px')
                        .style('height', _this_visual.get_rect_height(d) + 'px')
                        .style('x', baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + ((baseScale.bandwidth() - _this_visual.get_baseScale_bandwidth()) / 2) - param.scale.margin + 'px')
                        .style('y', _this_visual.get_rect_y(d) + 'px')
                })
                .style("fill", function (d, i) {
                    return d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementColor : colorScale(i);
                })
                .classed('ve-default-if-zero', function (d) {
                    return d[param.pattern.dataKey.plotAxis.key] <= 0;
                })
                .attr("width", _this_visual.get_baseScale_bandwidth())
                .attr("x", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + ((baseScale.bandwidth() - _this_visual.get_baseScale_bandwidth()) / 2) - param.scale.margin;
                })
                .attr("y", function (d) {
                    return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                        + (param.option.suppress.transition == false ?
                                param.scale.height + param.scale.margin - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) : 0);
                })
                .attr("height", function (d) {
                    return 0;
                })
                .transition()
                .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("y", function (d) {
                        return _this_visual.get_rect_y(d)
                    })
                    .attr("height", function (d) {
                        return _this_visual.get_rect_height(d);
                    })
                    .on('end', function () {
                        completelyRendered = true;
                    });

            g.selectAll(".ve-text-default").data(param._data).enter()
                .append('text')
                .text(function (d) {
                    return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
                })
                .attr("class", "ve-text-default v-text-noselect")
                .style('font-size', function () {
                    return (Math.round(_this_visual.get_baseScale_bandwidth() / 10, 0) * 3) + 'px';
                })
                .style("fill", function (d, i) {
                    return (_this_visual.get_rect_height(d) < _this_visual.get_baseScale_bandwidth() || eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') == 0 ?
                            zeroPlotValue.textColor
                        :   sn.visual._core_lib._get_color_readable(colorScale(i)));
                })
                .attr("x", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + (baseScale.bandwidth() - _this_visual.get_baseScale_bandwidth()) / 2 + (_this_visual.get_baseScale_bandwidth() / 2) - (param.scale.margin);
                })
                .attr("y", function (d) {
                    if (param.option.suppress.transition == false) {
                        return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                                    + (param.scale.height - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')));
                    }
                    else {
                        return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + _this_visual.get_baseScale_bandwidth() / 2;
                    }
                })
                .on('mousemove', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', (Math.round(_this_visual.get_baseScale_bandwidth() / 10, 0) * 3) + 4 + 'px')
                        .attr("y", function (d) {
                            return _this_visual.get_text_y(d);
                        })
                })
                .on('mouseout', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', (Math.round(_this_visual.get_baseScale_bandwidth() / 10, 0) * 3) + 'px')
                        .attr("y", function (d) {
                            return _this_visual.get_text_y(d);
                        })
                })
                .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("y", function (d) {
                        return _this_visual.get_text_y(d)
                    })
                    .style('opacity', '1');

            // do-0: *** HIGH *** - transformation does not take care of consolidating the more keys as of now.
            // plotting if more keys mentioned in plot axis
            if (areThereMoreKeysToPlot) {
                g.selectAll(".ve-default-1").data(param._data).enter()
                   .append("circle")
                   .attr("class", "ve-default-1")
                   .attr("id", function (d, i) {
                       return d._id;
                   })
                   .style("fill", function (d, i) { return '#FFFFFF' /*'#46b4af'*/ })
                   .style("opacity", 0)
                   .attr("cx", function (d) {
                       return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin + baseScale.bandwidth() / 2;
                   })
                   .attr("cy", function (d) {
                       return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 2
                           + (param.option.suppress.transition == false ?
                                   param.scale.height - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) : 0);
                   })
                   .attr('r', function (d) {
                       return baseScale.bandwidth() / 2;
                   })
                   .transition()
                       .duration(param.option.suppress.transition == false ? 1500 : 0)
                       .style("opacity", 1)
                       .attr("cx", function (d) {
                           return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 2 - param.scale.margin;
                       })
                       .attr("cy", function (d) {
                           let y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                       - (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize + 1 /* +1 is to keep clean bottom axis line. */ : 0);
                           return y;
                       })
                       .attr('r', function (d) {
                           return baseScale.bandwidth() / 2;
                       });

                g.selectAll(".ve-text-default-1").data(param._data).enter()
                   .append('text')
                   .text(function (d) {
                       return eval('d["'
                           + (param.pattern.dataKey.plotAxis.moreKey
                               && param.pattern.dataKey.plotAxis.moreKey.length > 0 ?
                               param.pattern.dataKey.plotAxis.moreKey[0] :
                               param.pattern.dataKey.plotAxis.key)
                           + '"]');
                   })
                   .attr("class", "ve-text-default v-text-noselect")
                   .style('font-size', function () {
                       return (Math.round(baseScale.bandwidth() / 10, 0) * 3) + 'px';
                   })
                   .style("fill", function (d, i) {
                       return '#000000'; // d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.textColor : sn.visual._core_lib._get_color_readable(colorScale(i));
                   })
                   .attr("x", function (d) {
                       return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) + baseScale.bandwidth() / 2 - param.scale.margin;
                   })
                   .attr("y", function (d) {
                       if (param.option.suppress.transition == false) {
                           return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                                       + (param.scale.height - plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')));
                       }
                       else {
                           return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                                       + baseScale.bandwidth() / 2
                       }
                   })
                   .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                   .transition()
                       .duration(param.option.suppress.transition == false ? 1500 : 0)
                       .attr("y", function (d) {
                           let y = plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) + baseScale.bandwidth() / 9
                                       - (d[param.pattern.dataKey.plotAxis.key] <= 0 ? zeroPlotValue.elementSize + 1 /* +1 is to keep clean bottom axis line. */ : 0);

                           return y;
                       })
                       .style('opacity', '1');
            }

            // note: include shadow as option
            g.selectAll('rect')
                .each(function () {
                    if (param.option.suppress.elementShadow == false) {
                        d3.select(this)
                            .attr("stroke-width", 1)
                            .style("filter", "url(#drop-shadow)");
                    }
                });
            /*
                        sn.visual._core_lib._set_ui_mouse_behavior({
                                currentElementSelector: '.ve-default',
                        });
            */
            // legend
            if (param.option.suppress.legend != true) {
                sn.visual._core_lib._set_legend({
                    'param': param,
                    'legendHolder': g,
                    'keys': sn.visual.helper.getLegendValues(param),
                    'colorScale': colorScale
                });
            };
        },

        // vertical - bar grouped
        _draw_visual_bar_grouped_vertical: function (draw_param) {
            var visualHolder = draw_param['visual-holder'],
                visual = draw_param['visual'],
                param = draw_param['param'];

            if (param.option.suppress.elementShadow == false) {
                sn.visual._core_lib._define_visual_bar_element_shadow(draw_param);
            }

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            // ----------------- review below
            //do-0: need to make this available restoring back after the maximization as well
            // scale adjustment before rendering starts
            //param.scale.width = param.scale.width + param.scale.margin;
            //param.scale.height = param.scale.height + param.scale.margin;
            // scale adjustment before rendering ends

            // axis handling
            var baseScale_0 = d3.scaleBand().rangeRound([1, param.scale.width + param.scale.margin + param.scale.margin]).paddingInner(0.2);
            var baseScale_1 = d3.scaleBand().paddingInner((param.option.suppress.elementShadow == false ? 0.15 : 0.1));
            var plotScale = d3.scaleLinear().range([param.scale.height + param.scale.margin, param.scale.margin / 2]);
            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var baseKey = param.pattern.dataKey.baseAxis;
            var groupKey = param.pattern.dataKey.baseAxis.groupKey;
            var plotKey = param.pattern.dataKey.plotAxis.key;

            var g = visual
                    .append("g")
                    .attr("transform", "translate(" + (param.scale.margin) + ", 0)");

            var plotMax = d3.max(param._data, function (d) {
                // return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
                return d3.max(eval('d["' + param.pattern.dataKey.baseAxis.groupKey + '"]'), function (v) {
                    return eval('v["value"]');
                });
            });

            // do-9: [refactor] this special legend handling for grouped bar, parameterize along with common handling
            var getAllGroupElementValues = function () {
                var result = [];
                param._data.forEach(function (baseData) {
                    baseData[groupKey].forEach(function (groupData) {
                        if (result.indexOf(groupData['key']) < 0) {
                            result.push(groupData['key']);
                        }
                    });
                });

                return result;
            }
            var allGroupElementValues = getAllGroupElementValues();

            baseScale_0.domain(param._data.map(function (d) {
                return eval('d["' + param.pattern.dataKey.baseAxis.key + '"]');
            }));

            this.stretchGroupAxis = (param.scale.width + 2 * param.scale.margin) / (param.scale.width / 12);
            baseScale_1.domain(allGroupElementValues).rangeRound([param.scale.margin - this.stretchGroupAxis, baseScale_0.bandwidth() + this.stretchGroupAxis]);

            plotScale.domain([0, d3.max(param._data, function (d) {
                return d3.max(allGroupElementValues, function (key) {
                    this.groupData = d[param.pattern.dataKey.baseAxis.groupKey];
                    for (var index = 0; index < this.groupData.length; index++) {
                        if (this.groupData[index]['key'] == key) {
                            return this.groupData[index]['value'];
                        }
                    }
                });
            })]).nice();

            function is_small_visual() {
                return (param.scale.width <= 100 || param.scale.height <= 100);
            }
            function get_normal_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 12 : 9);
            }
            function get_transition_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full')? 14 : 12);
            }
            function get_baseScale_1_bandWidth() {
                const max_bar_width = _get.d3.holder(draw_param._id).classed('vh-full') ? 30 : 20;
                const min_bar_width = _get.d3.holder(draw_param._id).classed('vh-full') ? baseScale_1.bandwidth(): 2.5;

                return (is_small_visual() ?
                            min_bar_width :
                            (baseScale_1.bandwidth() > max_bar_width ?
                                max_bar_width :
                                baseScale_1.bandwidth()));
            }

            var sub_g = g.append("g")
                .selectAll("g")
                .data(param._data).enter()
                .append("g")
                .attr("transform", function (d) {
                    return "translate(" + (baseScale_0(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - 1 * param.scale.margin / 1.75) + ", 0)";
                });

            let firstRect;
            sub_g.selectAll("rect")
                .data(function (d) {
                    return allGroupElementValues.map(function (elementValue) {
                        this.plotValue = null;
                        this.groupData = d[param.pattern.dataKey.baseAxis.groupKey];
                        for (var index = 0; index < this.groupData.length; index++) {
                            if (this.groupData[index]['key'] == elementValue) {
                                this.plotValue = this.groupData[index]['value'];
                                break;
                            };
                        };

                        return { key: elementValue, value: this.plotValue };
                    })
                }).enter()
                .append("rect")
                .attr('class', 've-default')
                .attr("x", function (d, i) {
                    if (i == 0) {
                        let gap = baseScale_0.bandwidth() - param.data[0][param.pattern.dataKey.baseAxis.groupKey].length * (get_baseScale_1_bandWidth() + 2)
                        firstRect = (gap > 0 ? gap / 2: 0);
                    }
                    return firstRect + (i + 0) * (get_baseScale_1_bandWidth() + 2) + param.scale.margin /2;
                })
                .attr("y", function (d) {
                    return plotScale(d.value) + (param.scale.height + param.scale.margin - plotScale(d.value));
                })
                .attr("width", get_baseScale_1_bandWidth())
                .attr("height", function (d) {
                    return (param.option.suppress.transition == false ? 0 : param.scale.height + param.scale.margin - plotScale(d.value));
                })
                .style("fill", function (d, i) {
                    return colorScale(i);
                })
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("height", function (d) {
                        return param.scale.height + param.scale.margin - plotScale(d.value);
                    })
                    .attr("y", function (d) {
                        return plotScale(d.value);
                    });

            // text add
            sub_g.selectAll('text')
                .data(function (d) {
                    return allGroupElementValues.map(function (elementValue) {
                        this.plotValue = null;
                        this.groupData = d[param.pattern.dataKey.baseAxis.groupKey];
                        for (var index = 0; index < this.groupData.length; index++) {
                            if (this.groupData[index]['key'] == elementValue) {
                                this.plotValue = this.groupData[index]['value'];
                                break;
                            };
                        };

                        return { key: elementValue, value: this.plotValue };
                    })
                }).enter()
                .append('text')
                .attr('class', 've-text-default v-text-noselect')
                .style("fill", function (d, i) {
                    return (d.value == 0 || (param.scale.height - plotScale(d.value) < get_baseScale_1_bandWidth())? 'black' : sn.visual._core_lib._get_color_readable(colorScale(i)));
                })
                .style('text-anchor', 'middle')
                .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                .style('font-size', function () {
                    return Math.round(get_baseScale_1_bandWidth() / 10, 0) * 5;
                })
                .attr("x", function (d, i) {
                    if (i == 0) {
                        let gap = baseScale_0.bandwidth() - param.data[0][param.pattern.dataKey.baseAxis.groupKey].length * (get_baseScale_1_bandWidth() + 2)
                        firstRect = (gap > 0? gap / 2: 0);
                    }
                    return firstRect + (i + 0) * (get_baseScale_1_bandWidth() + 2) + param.scale.margin / 2
                        + get_baseScale_1_bandWidth() / 2;
                })
                .attr("y", function (d) {
                    return plotScale(d.value) + get_baseScale_1_bandWidth() / 1.5 + (param.scale.height - plotScale(d.value));
                })
                .text(function (d) {
                    return d.value;
                })
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .style('opacity', '1')
                    .attr("y", function (d) {
                        return plotScale(d.value) + get_baseScale_1_bandWidth() / 1.5
                            + (d.value == 0 ? (param.scale.height + param.scale.margin - plotScale(d.value)) - get_baseScale_1_bandWidth() / 1.25
                            : (param.scale.height - plotScale(d.value) < get_baseScale_1_bandWidth() ? -1 * get_baseScale_1_bandWidth() / 1.25 : -1 * get_baseScale_1_bandWidth() / 8));
                    });

            // note: include the shadow as option
            g.selectAll('rect')
                .each(function () {
                    if (param.option.suppress.elementShadow == false) {
                        d3.select(this)
                            .attr("stroke-width", 1)
                            .style("filter", "url(#drop-shadow)");
                    }
                });

            var bottomAxis = g.append("g")
                .attr("transform", "translate(0, " + (param.scale.height + param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .attr('class', 'va-text-plotaxis v-text-noselect')
                .call(d3.axisBottom(baseScale_0).tickFormat(function (d) {
                    return (param.pattern.dataKey.baseAxis.prefix != null ? param.pattern.dataKey.baseAxis.prefix : '')
                        + d
                        + (param.pattern.dataKey.baseAxis.suffix != null ? param.pattern.dataKey.baseAxis.suffix : '');
                })
            );
            sn.visual._core_lib._set_axis_text_direction(bottomAxis, 'bottom', param.pattern.dataKey.baseAxis.textDirection);

            bottomAxis
                .on('mousemove', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            var leftAxis;
            if (param.pattern.dataKey.plotAxis.tickStyle == 'auto') {
                leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .style('font-size', get_normal_font_size() + 'px')
                    .style('cursor', 'default')
                    .attr('class', 'va-text-baseaxis v-text-noselect')
                    .call(d3.axisLeft(plotScale).tickFormat(function (d) {
                        return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                    })
                );
            }
            else {
                var customPlotScale = d3.axisLeft(plotScale);

                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal') {
                    if (param.option.suppress.blankTicks == false) {
                        customPlotScale
                            .tickFormat(function (d) {
                                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal' && (d.toString().indexOf('.') >= 0 || d % param.pattern.dataKey.plotAxis.tickIncrement != 0)) {
                                    // means: either (decimal presents) or (not multiples of tickIncrement) then => return empty value
                                    return '';
                                }
                                else {
                                    return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                    + d
                                    + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                                }
                            });
                    }
                    else {
                        customPlotScale
                            .tickFormat(d3.format('.0f'))
                            .tickValues(d3.range(0, plotMax + 1, param.pattern.dataKey.plotAxis.tickIncrement));
                    }
                }
                else if (param.pattern.dataKey.plotAxis.tickStyle == 'blank') {
                    customPlotScale
                        .tickValues(d3.range(0, 0, 0));
                }

                leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .call(customPlotScale);
            }
            sn.visual._core_lib._set_axis_text_direction(leftAxis, 'left', param.pattern.dataKey.plotAxis.textDirection);

            leftAxis
                .on('mousemove', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let plotTitle = visual.append("text")
                .attr("class", "va-text-plotaxis v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin / 7) + "," + ((param.scale.height + 2 * param.scale.margin) / 2) + ") rotate(270)")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.plotAxis.title);

            plotTitle
                .on('mousemove', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let baseTitle = visual.append("text")
                .attr("class", "va-text-baseaxis v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin + ((param.scale.width + param.scale.margin) / 2)) + "," + (param.scale.height
                    + (_get.d3.holder(draw_param._id).classed('vh-full')? 3 : 1.75) * param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px')
                .text(param.pattern.dataKey.baseAxis.title);

            baseTitle
                .on('mousemove', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            sn.visual._core_lib._set_legend({
                'param': param,
                'legendHolder': g,
                'keys': allGroupElementValues,
                'colorScale': colorScale
            });
        },

        // vertical - bubble
        _draw_visual_bubble_vertical: function (draw_param) {
            var visual = draw_param['visual'],
                visualHolder = draw_param['visual-holder'],
                visualToolTip = draw_param['visual-tooltip'],
                param = draw_param['param'];

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            if (param.option.suppress.elementShadow == false) {
                sn.visual._core_lib._define_visual_bar_element_shadow(draw_param);
            }

            visual.attr('data-mouse-in-point', '0');

            function is_small_visual() {
                return (param.scale.width <= 100 || param.scale.height <= 100);
            }
            function get_normal_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 12 : 9);
            }
            function get_transition_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 14 : 12);
            }

            // axis handling
            var baseScale = d3.scaleBand().range([param.scale.margin + 1, param.scale.width + 3 * param.scale.margin]).padding(0.4)
            var plotScale = d3.scaleLinear().range([param.scale.height + param.scale.margin, param.scale.margin / 2]);
            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var g = visual.append("g")
                .attr('class', 've-inter-group-default')
                .attr('transform', 'translate(' + param.scale.margin + ',' + (param.scale.margin / 2) + ')');

            var plotMax = d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            });

            baseScale.domain(param._data.map(function (d) {
                return eval('d["' + param.pattern.dataKey.baseAxis.key + '"]');
            }));
            plotScale.domain([0, d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            })]).nice();

            var bottomAxis = g.append("g")
                .attr("transform", "translate(" + -1 * param.scale.margin + ", " + (param.scale.height + param.scale.margin / 2) + ")")
                .style('font-size',  get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .call(d3.axisBottom(baseScale).tickFormat(function (d) {
                    return (param.pattern.dataKey.baseAxis.prefix != null ? param.pattern.dataKey.baseAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.baseAxis.suffix != null ? param.pattern.dataKey.baseAxis.suffix : '');
                })
            );
            sn.visual._core_lib._set_axis_text_direction(bottomAxis, 'bottom', param.pattern.dataKey.baseAxis.textDirection);

            bottomAxis
                .on('mousemove', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let horLine = visual.append('path')
                    .style('stroke', 'orange')
                    .style('stroke-width', 1)
                    .style('stroke-dasharray', ('2, 2'));

            let verLine = visual.append('path')
                    .style('stroke', 'orange')
                    .style('stroke-width', 1)
                    .style('stroke-dasharray', ('2, 2'))

            let movingCoordinate = visual.append('g')
                .attr('transform', 'translate(0, 0)');

            let movingText = movingCoordinate.append('text')
                .style('fill', 'orangered')
                .style('font-family', 'Segoe UI')
                .attr('class', 'v-text-noselect');

            movingText.append('tspan')
                .attr('id', 't1')
                .style('font-size', '7px')
                .style('font-weight', 'bold')
                .style('opacity', '0.5');

            movingText.append('tspan')
                .attr('id', 't2')
                .style('font-weight', 'bold')
                .style('font-size', '11px')

            function _set_tracker_visibility(visibility) {
                if (visibility == 'show') {
                    horLine.style('display', 'inline');
                    verLine.style('display', 'inline');
                    movingCoordinate.style('display', 'inline');
                }
                else if (visibility == 'hide') {
                    horLine.style('display', 'none');
                    verLine.style('display', 'none');
                    movingCoordinate.style('display', 'none');
                }
            };

            var leftAxis;
            if (param.pattern.dataKey.plotAxis.tickStyle == 'auto') {
                leftAxis = g.append("g")
                    .attr("transform", `translate(1, ${-param.scale.margin / 2})`)
                    .style('font-size', get_normal_font_size() + 'px')
                    .style('cursor', 'default')
                    .call(d3.axisLeft(plotScale).tickFormat(function (d) {
                        return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                + d
                                + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                    }));
            }
            else {
                var customPlotScale = d3.axisLeft(plotScale);

                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal') {
                    if (param.option.suppress.blankTicks == false) {
                        customPlotScale
                            .tickFormat(function (d) {
                                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal' && (d.toString().indexOf('.') >= 0 || d % param.pattern.dataKey.plotAxis.tickIncrement != 0)) {
                                    // means: either (decimal presents) or (not multiples of tickIncrement) then => return empty value
                                    return '';
                                }
                                else {
                                    return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                    + d
                                    + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                                }
                            });
                    }
                    else {
                        customPlotScale
                            .tickFormat(d3.format('.0f'))
                            .tickValues(d3.range(0, plotMax + 1, param.pattern.dataKey.plotAxis.tickIncrement));
                    }
                }
                else if (param.pattern.dataKey.plotAxis.tickStyle == 'blank') {
                    customPlotScale
                        .tickValues(d3.range(0, 0, 0));
                }

                leftAxis = g.append("g")
                    //.attr("transform", "translate(1, 0)")
                    .attr("transform", `translate(1, ${-param.scale.margin / 2})`)
                    .call(customPlotScale);
            }
            sn.visual._core_lib._set_axis_text_direction(leftAxis, 'left', param.pattern.dataKey.plotAxis.textDirection);

            leftAxis
                .on('mousemove', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let plotTitle = visual.append("text")
                .attr("class", "va-text-plotaxis v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin / 4 )+ "," + (param.scale.height / 2 + param.scale.margin) + ") rotate(270)")
                .style('font-size', get_normal_font_size() + 'px')
                .style('cursor', 'default')
                .text(param.pattern.dataKey.plotAxis.title);

            plotTitle
                .on('mousemove', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let baseTitle = visual.append("text")
                .attr("class", "va-text-baseaxis v-text-noselect")
                .attr("transform", "translate(" + (param.scale.margin + (param.scale.width + param.scale.margin) / 2) + "," + (param.scale.height
                    + (_get.d3.holder(draw_param._id).classed('vh-full') ? 3 : 1.75) * param.scale.margin) + ")")
                .text(param.pattern.dataKey.baseAxis.title);

            baseTitle
                .on('mousemove', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let minRadius = (param.scale.width + param.scale.height) / 100;
            let minPlotValue = d3.entries(param._data)
                    .sort(function (a, b) {
                        return d3.ascending(a.value[param.pattern.dataKey.plotAxis.key], b.value[param.pattern.dataKey.plotAxis.key]);
                    })[0].value[param.pattern.dataKey.plotAxis.key];

            if (minPlotValue == 0) {
                let maxPlotValue = d3.entries(param._data)
                        .sort(function (a, b) {
                            return d3.descending(a.value[param.pattern.dataKey.plotAxis.key], b.value[param.pattern.dataKey.plotAxis.key]);
                        })[0].value[param.pattern.dataKey.plotAxis.key];
                minPlotValue = (minPlotValue + maxPlotValue) / 2;
            }

            let completelyRendered = false;
            g.selectAll(".ve-default").data(param._data).enter()
                .append("circle")
                //.attr("class", "ve-default")
                .attr("class", function () {
                    return (_get.is_visual_interactable(param) == true ? 've-inter-default ve-default ' : ' ve-default ')
                })
                //.attr("id", function (d, i) {
                //    return d._id;
                //})
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .on('click', function (element) {
                    sn.visual._core_lib._visual_element_click_handling({
                        'this': this,
                        'visualId': d3.select(this).attr('data-visual-id'),
                        'param': param
                    });
                })
                .on('mousemove', function (d, i) {
                    if (completelyRendered == true) {
                        _set_tracker_visibility('hide');
                        visual.attr('data-mouse-in-point', 1);

                        sn.visual._core_lib._visual_element_mousemove_handling({
                            'd': d,
                            'i': i,
                            'ttDistance': baseScale.bandwidth() / 3,
                            'colorScale': colorScale,
                            'visual': visual,
                            'visualToolTip': visualToolTip,
                            'param': param,
                            'id': draw_param['_id']
                        });
                        d3.select(this).attr('r', minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue + 2);
                    }
                })
                .on('mouseout', function (d) {
                    visualToolTip.style('display', 'none');
                    d3.select(this).attr('r', minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue - 2);
                    _set_tracker_visibility('show');
                    visual.attr('data-mouse-in-point', 0);
                })
                .style("fill", function (d, i) { return colorScale(i); })
                .attr("cx", function (d, i) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin / 1.5;
                })
                .attr("cy", function (d) {
                    return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'));
                })
                .attr('r', function (d) {
                    return (param.option.suppress.transition == false ? 1 : minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue);
                })
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr('r', function (d) {
                        return (d[param.pattern.dataKey.plotAxis.key] == 0? 0: minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue);
                    })
                    .on('end', function () {
                        completelyRendered = true;

                        _get.d3.visual(draw_param['_id'])
                            .on('mousemove', function (e) {
                                if (d3.event.offsetX < param.scale.margin // if goes beyond left axis
                                    || d3.event.offsetY < param.scale.margin / 2 //param.scale.margin // if goes beyond top portion
                                    || d3.event.offsetX > param.scale.width + 3 * param.scale.margin // if goes beyond right portion
                                    || d3.event.offsetY > param.scale.height + 1 * param.scale.margin // if goes beyond bottom portion
                                    ) {
                                    _set_tracker_visibility('hide');
                                }
                                else if (visual.attr('data-mouse-in-point') == '0') { // else if mouse is not on top of any pointer 
                                    _set_tracker_visibility('show');
                                }
                                else {
                                    _set_tracker_visibility('hide');
                                }

                                if (/* visual.attr('data-drawing-completed') == 1 && */horLine.style('display') != 'none') {
                                    horLine
                                        .attr('d', `M${param.scale.margin + 1} ${d3.event.offsetY} l${param.scale.width + 2 * param.scale.margin} 0`);

                                    verLine
                                        .attr('d', `M${d3.event.offsetX} ${param.scale.margin / 2} v${param.scale.height + param.scale.margin / 2} 0`);

                                    movingCoordinate
                                        .attr('transform', 'translate(' + (d3.event.offsetX + 5) + ', ' + (d3.event.offsetY - 5) + ')');
                                    let plotAxis = param.pattern.dataKey.plotAxis;
                                    movingCoordinate.select('#t1').text(`${_isNotNullOrEmpty(plotAxis.title) ? plotAxis.title : plotAxis.key}: `);
                                    //do-0: the dummy value added to show the proper plot value should be removed and the axis should be fixed instead of that.
                                    movingCoordinate.select('#t2').text(`${Math.round(plotScale.invert(d3.event.offsetY), 0)}`); 
                                }
                            });
                    });

            g.selectAll(".ve-text-default").data(param._data).enter()
                .append('text')
                .attr("class", "ve-text-default v-text-noselect")
                .style('text-align', 'middle')
                .style('font-size', function () {
                    return (Math.round(minRadius / 2, 0) * 1.8) + 'px';
                })
                .text(function (d) {
                    return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
                })
                .attr("x", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin / 1.5;
                })
                .attr("y", function (d) {
                    return (param.option.suppress.transition == false ?
                            plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))
                        : plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - (minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue) * 1.25);
                })
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attr("y", function (d) {
                        return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - (minRadius + eval('d["' + param.pattern.dataKey.plotAxis.key + '"]') / minPlotValue) * 1.25;
                    });

            
            //do-5: shadow for circle - the below code need to modified
            /*
            g.selectAll('rect')
                .each(function () {
                    if (param.option.suppress.elementShadow == false) {
                        d3.select(this)
                            .attr("stroke-width", 1)
                            .style("filter", "url(#drop-shadow)")
                            .attr("transform", function (d) {
                                return "translate(" + (baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin) + "," + (plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'))) - 100 + ")";
                            });
                    }
                });
            */

            /*
                        sn.visual._core_lib._set_ui_mouse_behavior({
                            currentElementSelector: '.ve-default'
                        });
            */

            // legend
            if (param.option.suppress.legend != true) {
                sn.visual._core_lib._set_legend({
                    'param': param,
                    'legendHolder': g,
                    'keys': sn.visual.helper.getLegendValues(param),
                    'colorScale': colorScale
                });
            };
        },

        // vertical - line
        _draw_visual_line_vertical: function (draw_param) {
            var visualHolder = draw_param['visual-holder'],
                visualToolTip = draw_param['visual-tooltip'],
                visual = draw_param['visual'],
                param = draw_param['param'];

            if (param.option.suppress.elementShadow == false) {
                // this._define_visual_line_element_shadow(visual);
            }

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            visual.attr('data-mouse-in-point', '0');

            function is_small_visual() {
                return (param.scale.width <= 100 || param.scale.height <= 100);
            }
            function get_normal_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 12 : 9);
            }
            function get_transition_font_size() {
                return is_small_visual() ? 9 : (_get.d3.holder(draw_param._id).classed('vh-full') ? 14 : 12);
            }

            // axis handling
            // var baseScale = d3.scaleBand().range([param.scale.margin + 1, param.scale.width + 2 * param.scale.margin]).padding(0.4);
            var baseScale = d3.scaleBand().range([param.scale.margin + 1, param.scale.width + 2 * param.scale.margin]).padding(0.4);
            var plotScale = d3.scaleLinear().range([param.scale.height + param.scale.margin, param.scale.margin / 2]);
            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var g = visual.append("g")
                    .attr('class', 've-inter-group-default')
                    .attr("transform", "translate(" + (param.scale.margin * 1) + "," + 0 + ")");

            var plotMax = d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            });

            baseScale.domain(param._data.map(function (d) {
                return eval('d["' + param.pattern.dataKey.baseAxis.key + '"]');
            }));
            plotScale.domain([0, d3.max(param._data, function (d) {
                return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
            })]).nice();

            var bottomAxis = g.append("g")
                .attr("transform", "translate(" + -1 * param.scale.margin + ", " + (param.scale.height + param.scale.margin) + ")")
                .style('font-size', get_normal_font_size() + 'px') // '6px')
                .style('cursor', 'default')
                .call(d3.axisBottom(baseScale).tickFormat(function (d) {
                    return (param.pattern.dataKey.baseAxis.prefix != null ? param.pattern.dataKey.baseAxis.prefix : '')
                            + d
                            + (param.pattern.dataKey.baseAxis.suffix != null ? param.pattern.dataKey.baseAxis.suffix : '');
                })
            );
            sn.visual._core_lib._set_axis_text_direction(bottomAxis, 'bottom', param.pattern.dataKey.baseAxis.textDirection);

            bottomAxis
                .on('mousemove', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    bottomAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let horLine = visual.append('path')
                    .style('stroke', 'orange')
                    .style('stroke-width', 1)
                    .style('stroke-dasharray', ('2, 2'));

            let verLine = visual.append('path')
                    .style('stroke', 'orange')
                    .style('stroke-width', 1)
                    .style('stroke-dasharray', ('2, 2'))

            let movingCoordinate = visual.append('g')
                .attr('transform', 'translate(0, 0)');

            let movingText = movingCoordinate.append('text')
                .style('fill', 'orangered')
                .style('font-family', 'Segoe UI')
                .attr('class', 'v-text-noselect');

            movingText.append('tspan')
                .attr('id', 't1')
                .style('font-size', '7px')
                .style('font-weight', 'bold')
                .style('opacity', '0.5');

            movingText.append('tspan')
                .attr('id', 't2')
                .style('font-weight', 'bold')
                .style('font-size', '11px')

             function _set_tracker_visibility(visibility) {
                if (visibility == 'show') {
                    horLine.style('display', 'inline');
                    verLine.style('display', 'inline');
                    movingCoordinate.style('display', 'inline');
                }
                else if (visibility == 'hide') {
                    horLine.style('display', 'none');
                    verLine.style('display', 'none');
                    movingCoordinate.style('display', 'none');
                }
            };

            var leftAxis;
            if (param.pattern.dataKey.plotAxis.tickStyle == 'auto') {
                 leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .style('font-size', get_normal_font_size() + 'px')
                    .style('cursor', 'default')
                    .call(d3.axisLeft(plotScale).tickFormat(function (d) {
                        return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                + d
                                + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                    }));
            }
            else {
                var customPlotScale = d3.axisLeft(plotScale);

                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal') {
                    if (param.option.suppress.blankTicks == false) {
                        customPlotScale
                            .tickFormat(function (d) {
                                if (param.pattern.dataKey.plotAxis.tickStyle == 'no-decimal' && (d.toString().indexOf('.') >= 0 || d % param.pattern.dataKey.plotAxis.tickIncrement != 0)) {
                                    // means: either (decimal presents) or (not multiples of tickIncrement) then => return empty value
                                    return '';
                                }
                                else {
                                    return (param.pattern.dataKey.plotAxis.prefix != null ? param.pattern.dataKey.plotAxis.prefix : '')
                                    + d
                                    + (param.pattern.dataKey.plotAxis.suffix != null ? param.pattern.dataKey.plotAxis.suffix : '');
                                }
                            });
                    }
                    else {
                        customPlotScale
                            .tickFormat(d3.format('.0f'))
                            .tickValues(d3.range(0, plotMax + 1, param.pattern.dataKey.plotAxis.tickIncrement));
                    }
                }
                else if (param.pattern.dataKey.plotAxis.tickStyle == 'blank') {
                    customPlotScale
                        .tickValues(d3.range(0, 0, 0));
                }

                leftAxis = g.append("g")
                    .attr("transform", "translate(1, 0)")
                    .call(customPlotScale);
            }
            sn.visual._core_lib._set_axis_text_direction(leftAxis, 'left', param.pattern.dataKey.plotAxis.textDirection);

            leftAxis
                .on('mousemove', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    leftAxis
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let plotTitle = visual.append("text")
                .attr("class", "va-text-plotaxis v-text-noselect")
                .attr("transform", function () {
                    return "translate(" + (param.scale.margin / 4) + "," + (param.scale.height / 2 + param.scale.margin) + ") rotate(270)";
                })
                .style('font-size', get_normal_font_size() + 'px')
                .text(param.pattern.dataKey.plotAxis.title);

            plotTitle
                .on('mousemove', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    plotTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            let baseTitle = visual.append("text")
                .attr("class", "va-text-baseaxis v-text-noselect")
                .attr("transform", function () {
                    return "translate(" + (param.scale.margin + ((param.scale.width + param.scale.margin) / 2)) + "," + (param.scale.height
                        + (_get.d3.holder(draw_param._id).classed('vh-full') ? 3 : 1.75) * param.scale.margin) + ")";
                })
                .style('font-size', get_normal_font_size() + 'px')
                .text(param.pattern.dataKey.baseAxis.title);

            baseTitle
                .on('mousemove', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_transition_font_size() + 'px')
                })
                .on('mouseout', function () {
                    baseTitle
                        .transition()
                        .style('font-size', get_normal_font_size() + 'px')
                });

            var line = d3.line()
                .x(function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin + baseScale.bandwidth() / 2;
                })
                .y(function (d) {
                    return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'));
                });

            if (param.pattern.type == 'curve-line') {
                line.curve(d3.curveMonotoneX);
            }

            var path = g.append("path")
                .datum(param._data)
                .attr('class', 've-connector-default')
                .style('fill', (param.pattern.fillArea == true ? 'steelblue' : 'none'))
                .style('stroke-linejoin', 'round')
                .style('stroke-linecap', 'round')
                .style('stroke-opacity', '0.5')
                .style('stroke-width', '1px') // '0.25px' is not visible clearly
                .attr('d', line);

            var totalLength = path.node().getTotalLength();
            path.style("stroke-dasharray", totalLength + " " + totalLength)
                .style("stroke-dashoffset", totalLength)
                .transition()
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .ease(d3.easeLinear)
                    .style("stroke-dashoffset", param.option.suppress.transition == false ? 0 : totalLength);

            let defaultPointRadius = 3;  // previous value was '1'
            let completelyRendered = false;
            g.selectAll(".ve-default").data(param._data).enter()
                .append("circle")
                .attr("class", function () {
                    return (_get.is_visual_interactable(param) == true ? 've-inter-default ve-default ' : ' ve-default ')
                })
                // do-9: line - analyze the _id assignment to the visual element is required or not
                //.attr("id", function (d, i) {
                //    return d._id;
                //})
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .on('click', function () {
                    sn.visual._core_lib._visual_element_click_handling({
                        'this': this,
                        'visualId': d3.select(this).attr('data-visual-id'),
                        'param': param
                    });
                })
                .on('mousemove', function (d, i) {
                    if (completelyRendered == true) {
                        _set_tracker_visibility('hide');
                        visual.attr('data-mouse-in-point', 1);

                        sn.visual._core_lib._visual_element_mousemove_handling({
                            'd': d,
                            'i': i,
                            'ttDistance': baseScale.bandwidth() / 3,
                            'colorScale': colorScale,
                            'visual': visual,
                            'visualToolTip': visualToolTip,
                            'param': param,
                            'id': draw_param['_id']
                        });
                        d3.select(this)
                            .attr('r', 5)
                            .classed('ve-mouse-on', true)
                            .style('fill', colorScale(i));
                    }
                })
                .on("mouseout", function () {
                    visualToolTip.style("display", "none");
                    d3.select(this)
                        .attr('r', defaultPointRadius)
                        .classed('ve-mouse-on', false)
                        .style('fill', 'white');
                    _set_tracker_visibility('show');
                    visual.attr('data-mouse-in-point', 0);
                })
                .attr("cx", function (d) {
                    return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin + baseScale.bandwidth() / 2;
                })
                .attr("cy", function (d) {
                    return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]'));
                })
                .attr("r", 0)
                .transition()
                .duration(param.option.suppress.transition == false ? 2 * 1500 : 0)
                .attr("r", defaultPointRadius)
                .style('fill', function (d, i) {
                    //return colorScale(i);
                    //visual.attr('data-drawing-completed', 1);
                    return 'white';
                })
                .style('stroke', 'steelblue')
                .style('stroke-width', '2px')
                .on('end', function () {
                    completelyRendered = true;
                    _get.d3.visual(draw_param['_id'])
                        .on('mousemove', function (e) {
                            if (d3.event.offsetX < param.scale.margin // if goes beyond left axis
                                || d3.event.offsetY < param.scale.margin / 2 //param.scale.margin // if goes beyond top portion
                                || d3.event.offsetX > param.scale.width + 2 * param.scale.margin // if goes beyond right portion
                                || d3.event.offsetY > param.scale.height + param.scale.margin // if goes beyond bottom portion
                                ) {
                                _set_tracker_visibility('hide');
                            }
                            else if (visual.attr('data-mouse-in-point') == '0') { // else if mouse is not on top of any pointer 
                                _set_tracker_visibility('show');
                            }
                            else {
                                 _set_tracker_visibility('hide');
                            }

                            if (/* visual.attr('data-drawing-completed') == 1 && */horLine.style('display') != 'none') {
                                horLine
                                    .attr('d', `M${param.scale.margin + 1} ${d3.event.offsetY} l${param.scale.width + param.scale.margin} 0`);

                                verLine
                                    .attr('d', `M${d3.event.offsetX} ${param.scale.margin / 2} v${param.scale.height + param.scale.margin / 2} 0`);

                                movingCoordinate
                                    .attr('transform', 'translate(' + (d3.event.offsetX + 5) + ', ' + (d3.event.offsetY - 5) + ')');
                                let plotAxis = param.pattern.dataKey.plotAxis;
                                movingCoordinate.select('#t1').text(`${_isNotNullOrEmpty(plotAxis.title)? plotAxis.title : plotAxis.key}: `);
                                movingCoordinate.select('#t2').text(`${Math.round(plotScale.invert(d3.event.offsetY), 0)}`);
                            }
                        });
                });

            if (param.option.suppress.textOnElement == false) {
                g.selectAll(".ve-text-default").data(param._data).enter()
                    .append('text')
                    .attr("class", "ve-text-default v-text-noselect")
                    .style('font-size', function () {
                        return 'x-small';
                    })
                    .style("fill", function (d, i) {
                        return 'gray';
                    })
                    .attr("x", function (d) {
                        return baseScale(eval('d["' + param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin + baseScale.bandwidth() / 2;
                    })
                    .attr("y", function (d) {
                        return plotScale(eval('d["' + param.pattern.dataKey.plotAxis.key + '"]')) - param.scale.margin / 4;
                    })
                    .transition()
                        .duration(param.option.suppress.transition == false ? 2 * 1500 : 0)
                        .text(function (d) {
                            return eval('d["' + param.pattern.dataKey.plotAxis.key + '"]');
                     });
                }

            // note: include shadow as option
            //do-5: work on shadow for line - vertical
            g.selectAll('rect')
                .each(function () {
                if (param.option.suppress.elementShadow == false) {
                    d3.select(this)
                        .attr("stroke-width", 1)
                        .style("filter", "url(#drop-shadow)")
                        .attr("transform", function (d) {
                            return "translate(" + (baseScale(eval('d["' +param.pattern.dataKey.baseAxis.key + '"]')) - param.scale.margin) + "," +(plotScale(eval('d["' +param.pattern.dataKey.plotAxis.key + '"]'))) -100 + ")";
                        });
                    }
                });

                /*
                            sn.visual._core_lib._set_ui_mouse_behavior({
                                currentElementSelector: '.ve-default'
                            });
                */
                // legend
                if (param.option.suppress.legend != true) {
                    sn.visual._core_lib._set_legend({
                        'param': param,
                        'legendHolder': g,
                        'keys': sn.visual.helper.getLegendValues(param),
                        'colorScale': colorScale
                    });
                };
        },

        // pie
        _draw_visual_pie_doughnut: function (draw_param) {
            var visual = draw_param['visual'],
                visualHolder = draw_param['visual-holder'],
                visualTooltip = draw_param['visual-tooltip'],
                param = draw_param['param'];

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            var baseAxisKey = param.pattern.dataKey.baseAxis.key;
            var plotAxisKey = param.pattern.dataKey.plotAxis.key;

            let noData = (param._data.length == 0);

            if (noData) {
                let dummy = {};
                dummy[baseAxisKey] = '(no data)';
                dummy[plotAxisKey] = 1;
                param._data.push(dummy);
                param.pattern.dataKey.plotAxis.colors = ['#F0F0F0']; //do-1: move the default 'no-data' color to default
            }
            else {
                sn.visual._core_lib._set_colors_from_scheme(param, true);
            }

            //do-1: move the 'textOnElement' property to param - true will put the text on the element and false puts outside. extend this to all visual types.
            var textOnElement = true; // if false the text should be outside the element

            if (param.option.suppress.elementShadow == false) {
                sn.visual._core_lib._define_visual_bar_element_shadow(draw_param);  //do-5: change _define_visual_bar_element_shadow for horizontal
            }

            let minOfScale = Math.min(param.scale.width, param.scale.height);
            var radius = minOfScale <= 100 ? minOfScale * 0.85 : minOfScale * 0.5;

            var colorScale = d3.scaleOrdinal().range(param.pattern.dataKey.plotAxis.colors);

            var g = visual.append("g")
                .attr('class', 've-inter-group-default')
                .attr("transform", "translate("
                        + (param.scale.width / 2 + param.scale.margin) + ","
                        + (param.scale.height / 2 + param.scale.margin /*+ (param.option.suppress.tileHeader == false? param.scale.tile.headerBarHeight: 0)*/ ) + ")")
                .attr('width', param.scale.width + 'px');

            if (param.pattern.type == 'doughnut') {
                let summaryFontSize = (9 + Math.max(param.scale.width, param.scale.height) / 100);

                g.append('text')
                    .attr('class', 'v-text-noselect')
                    .style('text-anchor', 'middle')
                    .style('font-size', summaryFontSize - 1)
                    .text(function () {
                        return noData? '': 'Total';
                    })
                    .attr('y', '-' + (summaryFontSize / 2) + 'px')
                    .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                    .transition()
                    .duration(param.option.suppress.transition == false ? 1500 + 750 : 0)
                        .style('opacity', '0.5');

                g.append('text')
                    .attr('class', 'v-text-noselect')
                    .style('text-anchor', 'middle')
                    .style('font-weight', 'bold')
                    .style('font-size', (summaryFontSize < 14? 14: summaryFontSize))
                    .text(function () {
                        return noData ? '' : d3.sum(param._data, function (d) { return d[plotAxisKey]; });
                    })
                    .attr('y', (summaryFontSize / 2) + 2 + 'px')
                    .style('opacity', param.option.suppress.transition == false ? '0' : '1')
                    .on('mouseenter', function () {
                        d3.select(this)
                            .transition()
                            .style('font-size', (summaryFontSize < 14 ? 14 : summaryFontSize) + (summaryFontSize / 2) + 'px')
                    })
                    .on('mouseout', function () {
                        d3.select(this)
                            .transition()
                            .style('font-size', (summaryFontSize < 14 ? 14 : summaryFontSize) + 'px')
                    })
                    .transition()
                    .duration(param.option.suppress.transition == false ? 1500 + 750 : 0)
                        .style('opacity', '1');
            }

            var pie = d3.pie()
                .sort(null)
                .value(function (d) { return eval("d['" + plotAxisKey + "']"); });

            function tweenPie(b) {
                b.innerRadius = 0;
                var i = d3.interpolate({ startAngle: 0, endAngle: 0 }, b);
                return function (t) { return path(i(t)); };
            }

            var path = d3.arc()
                .outerRadius(radius)
                .innerRadius(param.pattern.type == 'doughnut' ? radius - (radius / 1.5) : 0.1 /* default value is for -pie */);

            var pathIn = d3.arc()
                .outerRadius(radius + 7)
                .innerRadius(7 + (param.pattern.type == 'doughnut' ? radius - (radius / 1.5) : 0.1) /* default value is for -pie */);

            function pathEnter() {
                t = d3.select(this);
                t.transition()
                  .attr('d', pathIn);
            }

            function pathOut() {
                t = d3.select(this);
                t.transition()
                  .attr('d', path);
            }

            this._fn_label_radius = function () {
                return radius + ((textOnElement == true? -1 : 1) * radius / (param.pattern.type == 'doughnut' ? 4 : (textOnElement == true ? 2 : 3)))
            };

            var label = d3.arc()
                .outerRadius(this._fn_label_radius())
                .innerRadius(this._fn_label_radius());

            var arc = g.selectAll('.ve-default')
                .data(pie(param._data)).enter()
                .append('g')
                .attr('class', function () {
                    let classStr =
                          (_get.is_visual_interactable(param) == true ? 've-inter-default ve-default ' : ' ve-default ')
                        + (param.pattern.type == 'doughnut' ? ' ve-doughnut-default ' : ' ve-pie-default ')
                    return classStr;
                })
                .style("fill", function (d, i) { return colorScale(i); })
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .on('click', function (element) {
                    sn.visual._core_lib._visual_element_click_handling({
                        'this': this,
                        'visualId': d3.select(this).attr('data-visual-id'),
                        'param': param
                    });
                });

                // done: *** default is all elements selected - hence refresh all children as well.

            if (textOnElement == false) {
                var outerArc = d3.arc()
                    .outerRadius(this._fn_label_radius() * 1 / 3) // * 1.1)
                    .innerRadius(this._fn_label_radius() * 1 / 3); // * 1.5);

                function midAngle(d) {
                    return d.startAngle + (d.endAngle - d.startAngle) / 2;
                }
            }

            let completelyRendered = false;
            arc.append('path')
                .attr('d', path)
                .on('mouseenter', pathEnter)
                .on('mouseout.animate', pathOut)
                .on('mousemove', function (d, i) {
                    if (noData == false && completelyRendered == true) {
                        //visualTooltip.style("display", "inline");
                        sn.visual._core_lib._visual_element_mousemove_handling({
                            'd': d,
                            'i': i,
                            'ttDistance': 30,
                            'colorScale': colorScale,
                            'visual': visual,
                            'visualToolTip': visualTooltip,
                            'param': param,
                            'id': draw_param['_id']
                        });
                    }
                })
                .on('mouseout.tooltip', function (d) {
                    if (noData == false) {
                        visualTooltip.style("display", "none");
                    }
                })
                .transition()
                    .ease(d3.easeCircle)
                    .duration(param.option.suppress.transition == false ? 1500 : 0)
                    .attrTween("d", param.option.suppress.transition == false ? tweenPie : null)
                    .on('end', function () {
                        completelyRendered = true;
                    });

            if (textOnElement == false) {
                var legendRect = arc.append('rect')
                    .attr('style', 'width: 20px; height: 20px; fill-opacity: 1; fill: #efffdd; stroke-width: 1; stroke: lightgray; stroke-linejoin: round;')
                    .attr('transform', function (d) {
                        this.centroid = label.centroid(d);
                        this.centroid[0] -= 20 / 2;
                        this.centroid[1] -= 20 / 1.5;

                        return 'translate(' + this.centroid[0] + ', ' + this.centroid[1] + ')';
                });
            }

            let _get_font_size_based_on_radius = function () {
                let fontSize = Math.round(radius / 10, 0);
                fontSize = (fontSize < 9 ? 9 : fontSize);  // previously 12
                return fontSize;
            }

            arc.append('text')
                .text(function (d) {
                    return noData ?
                        '0' :
                        (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.prefix)? param.pattern.dataKey.plotAxis.prefix + ' ': _emptyStr)
                            + eval("d.data['" + plotAxisKey + "']") 
                            + (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.suffix) ? ' ' + param.pattern.dataKey.plotAxis.suffix : _emptyStr);
                })
                .attr('class', 've-text-default v-text-noselect')
                .style('font-weight', 'normal')
                .style('stroke', 'white')
                .style('text-align', 'end')
                .style('font-size', function () {
                    return _get_font_size_based_on_radius() + 'px';
                })
                .attr('transform', function (d) {
                    return param.option.suppress.transition == false ? 'translate(0)' : 'translate(' + (label.centroid(d)) + ')';
                })
                .on('mouseenter', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', _get_font_size_based_on_radius() + 9 + 'px')
                })
                .on('mouseout', function () {
                    d3.select(this)
                        .transition()
                        .style('font-size', _get_font_size_based_on_radius() + 'px')
                })
                .transition()
                .ease(d3.easeCircle)
                .duration(1500)
                    .style("stroke", function (d, i) {
                        return (textOnElement == true ? (noData? 'darkgray' : sn.visual._core_lib._get_color_readable(colorScale(i))) : 'black');
                })
                .attr('transform', function (d) {
                    return 'translate(' + (label.centroid(d)) + ')';
                })
                .on('end', function () {
                    if (textOnElement == false) {
                        // preferable is curve from the edge of the element

                        // this is one cut line
                        var legendKey = baseAxisKey;
                        var polyline = arc.selectAll(".v-polyline")
                            .data(pie(param._data), function (d) {
                                return d.data[legendKey];
                    });

                    polyline.enter()
                        .append("polyline")
                        .attr('class', 'v-polyline')
                        .attr('style', 'opacity: .3; stroke: black; stroke-width: 2px; fill: none;');

                    polyline.transition().duration(1000)
                        .attrTween("points", function (d) {
                            this._current = this._current || d;
                            var interpolate = d3.interpolate(this._current, d);
                            this._current = interpolate(0);
                            return function (t) {
                                var d2 = interpolate(t);
                                var pos = outerArc.centroid(d2);
                                pos[0] = radius * 0.95 * (midAngle(d2) < Math.PI ? 1 : -1);
                                return [label.centroid(d2), outerArc.centroid(d2), pos];
                        };
                    });

                    polyline.exit().remove();
                }
            });

            // note: include the shadow as option
            g.selectAll('rect')
                .each(function () {
                    if (param.option.suppress.elementShadow == false) {
                        d3.select(this)
                            .attr("stroke-width", 1)
                            .style("filter", "url(#drop-shadow)");
                }
            });

                /*
                            sn.visual._core_lib._set_ui_mouse_behavior({
                                currentElementSelector: '.ve-default'
                            });
                */

                // legend
            if (param.option.suppress.legend != true) { // && (noData == false)) {
                sn.visual._core_lib._set_legend({
                    'param': param,
                    'legendHolder': g,
                    'keys': sn.visual.helper.getLegendValues(param),
                    'colorScale': colorScale
                });
            }
        },

        // table
        _draw_visual_table_vertical: function (draw_param) {
            var visualHolder = draw_param['visual-holder'],
                visual = draw_param['visual'],
                param = draw_param['param'];

            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }

            const _scroll_fill = '_scroll_fill';
            const _check_box = '_check_box';

            var _do = {
                _invoke_child_visuals: function (visualId) {
                    this.visualsToRefresh = sn.visual._model.filter(item => item['parent-visual-id'] == visualId);
                    this.tbody = d3.select('#' + visualId + ' tbody');

                    var selectedData = [];
                    var selectedRows;
                    if (this.tbody.selectAll('tr').filter('.ve-inter-unselected').size() == 0) { // if not specific row is selected by click / control click
                        selectedRows = this.tbody.selectAll('tr').filter('.ve-inter-default') // select all rows
                    }
                    else {
                        selectedRows = this.tbody.selectAll('tr').filter('.ve-inter-selected'); // select only the selected rows
                    }

                    selectedRows.each(function (row) {
                        selectedData.push(row);
                    });

                    sn.visual._core_lib._refresh_children(
                        this.visualsToRefresh,
                        selectedData
                    );
                },
                _switch_selection: function (visualId, selectAll) {
                    this.tbody = d3.select('#' + visualId + ' tbody');
                    this.tfoot = d3.select('#' + visualId + ' tfoot');

                    this.tbody.selectAll('.ve-inter-default').classed('ve-inter-unselected', selectAll == false);
                    this.tbody.selectAll('.ve-inter-default').classed('ve-inter-table-unselected', selectAll == false);
                    this.tbody.selectAll('.ve-inter-default').classed('ve-inter-selected', selectAll == true);
                    this.tfoot.selectAll('tr').classed('ve-inter-unselected', selectAll == false);
                    this.tfoot.selectAll('tr').classed('ve-inter-table-unselected', selectAll == false);
                    this.tfoot.selectAll('tr').classed('ve-inter-selected', selectAll == true);
                },
                _get_col_head_text: function (d) {
                    let result;

                    if (d.name == _scroll_fill) {
                        result = _emptyStr;
                    }
                    else if (d.name == param.pattern.dataKey.baseAxis.key) {
                        result = param.pattern.dataKey.baseAxis.title ? param.pattern.dataKey.baseAxis.title : param.pattern.dataKey.baseAxis.key;
                    }
                    else if (d.name == param.pattern.dataKey.plotAxis.key) {
                        result = param.pattern.dataKey.plotAxis.title ? param.pattern.dataKey.plotAxis.title : param.pattern.dataKey.plotAxis.key;
                    }
                    else {
                        let found = param.pattern.dataKey.baseAxis.moreKey.filter(m => m.key == d.name)[0];
                        result = (_isNotNullOrEmpty(found) ? found.title : d.name);
                        result = (_isNullOrEmpty(result) ? d.name : result);
                    }

                    return result;
                },
                _invoke_listeners: function (element) {
                    let context = {
                        'this': element,
                        'visualId': d3.select(element).attr('data-visual-id'),
                        'param': param
                    };

//                    this.tbody = d3.select('#' + context['visualId'] + ' tbody');

                    if (context['param'].listener && context['param'].listener.onVisualElementClick) {
                        let userEventContext = {
                            selected: []
                        };

                        // prepare selected data
                        try {
                            if (this.tbody) {
                                let selectedSource;
                                let newSelected;

                                this.tbody.selectAll('.ve-inter-default').filter('.ve-inter-selected').each(function (row) {
                                    selectedSource = _isNotNullOrEmpty(row.data) ? row.data : row;
                                    newSelected = _get.json_cloned(selectedSource);

                                    // delete newSelected.key;
                                    // delete newSelected.value;
                                    delete newSelected._id;

                                    newSelected['key'] = _isNullOrEmpty(newSelected['key']) ? selectedSource[context['param'].pattern.dataKey.baseAxis.key] : newSelected['key'];
                                    newSelected['value'] = _isNullOrEmpty(newSelected['value']) ? selectedSource[context['param'].pattern.dataKey.plotAxis.key] : newSelected['value'];

                                    userEventContext.selected.push(newSelected);
                                });
                            }
                        }
                        catch (e) {
                            if (e.message || e.stack) {
                                console.error(e.message + e.stack);
                            }
                        }

                        context['param'].listener.onVisualElementClick(userEventContext);
                    }
                },
                _switch_foot_selection: function (visualId) {
                    d3.select('#' + visualId + ' tbody')
                        .selectAll('tr').classed('ve-inter-selected', true)
                        .selectAll('tr').classed('ve-inter-unselected', false)
                        .selectAll('tr').classed('ve-inter-table-unselected', false);

                    d3.select('#' + visualId + ' tfoot')
                        .selectAll('tr').classed('ve-inter-selected', true)
                        .selectAll('tr').classed('ve-inter-unselected', false)
                        .selectAll('tr').classed('ve-inter-table-unselected', false);
                }
            };

            let table = visual.append('table')
                        .attr('class', 'vt-default v-text-noselect')
                        .style('display', 'grid') 
                        .style('border-spacing', '0px')
                        .style('font-size', param.decoration.textSize + 'px')
                        .style('width', '100%');

            if (param.option.suppress.toolBar == true) {
                let tcaption = table.append('caption')
                    .text(param.basic.title)
                    .attr('id', _get.id.tcaption(draw_param['_id']))
                    .attr('class', 'vt-caption v-text-noselect v-text-overflow-hide')
                    .style('width', visualHolder.style('width'))
                    .style('background-color', param.scale.tile.hostColor)
                    .style('color', sn.visual._core_lib._get_color_readable(param.scale.tile.hostColor));

                _set.element.text_as_title(tcaption.attr('id'));
            }

            var thead = table.append('thead')
                    .attr('class', 'vt-head')
                    .style('display', 'table')
                    .style('width', '100%')
                    .style('table-layout', 'fixed')
                    .style('border-bottom-color', param.scale.tile.hostColor);

            var tbody = table.append('tbody')
                    .attr('class', 've-inter-group-default')
                    .style('height', param.scale.height + (_isNullOrEmpty(param.pattern.dataKey.plotAxis.summaryReduceMethod)? param.scale.margin + param.scale.tile.margin : 0) + 'px')
                    .style('display', 'block')
                    .style('overflow-y', 'scroll');

            var tfoot = table.append('tfoot')
                    .attr('class', 'vt-foot')
                    .style('display', 'table')
                    .style('width', '100%')
                    .style('table-layout', 'fixed')
                    .style('border-top-color', param.scale.tile.hostColor);

            let columns = [];
            if (param.option.suppress.checkBox == false) {
                columns.push({ 'name': _check_box, 'title': _emptyStr, '_internal': true});
            }
            
            // dynamic formation of table columns and properties
            columns.push({ 'name': param.pattern.dataKey.baseAxis.key, 'title': param.pattern.dataKey.baseAxis.title, '_internal': false });
            if (_isNotNullOrEmpty(param.pattern.dataKey.baseAxis.moreKey) == true) {
                param.pattern.dataKey.baseAxis.moreKey.forEach(function (item) {
                    columns.push({ 'name': item.key, 'title': item.title? item.title: item.key, '_internal': false });
                });
            }
            columns.push({ 'name': param.pattern.dataKey.plotAxis.key, 'title': param.pattern.dataKey.plotAxis.title, '_internal': false });
            columns.push({ 'name': _scroll_fill, 'title': _emptyStr, '_internal': true });

            let sortColumns = [];
            let multiColumnSort = false;

            var defColWidth = param.scale.width / columns.length;

            let col_head = thead.append('tr') // .attr('class', 'table-row')
                .selectAll('th')
                .data(columns).enter()
                .append('th')
                .on('click', function (d) {
                    if (d._internal == false) {
                        // sort and render
                        let prior_cursor_value = visualHolder.style('cursor');
                        try {
                            // set column sort status
                            
                            let sortIndex = sortColumns.findIndex(c => c[d.name] != null);
                            if (sortIndex >= 0) {
                                // do-0: handle combination of ascending and descending sort later.
                                sortColumns[sortIndex][d.name].newSortState = (sortColumns[sortIndex][d.name].sortedState == 'asc' ? 'asc': 'asc') // 'desc' : 'asc');
                            }

                            // do-0: need to fix multi column sort.
                            if (d3.event.shiftKey == true) {
                                if (multiColumnSort == true) {
                                    let sortIndex = sortColumns.findIndex(c => c[d.name] != null);
                                    if (sortIndex > -1 && sortColumns[sortIndex][d.name].sortedState == sortColumns[sortIndex][d.name].newSortState) {
                                        sortColumns.splice(sortIndex, 1)
                                    }
                                }                            
                                multiColumnSort = true;
                            }
                            else {
                                multiColumnSort = false;
                                
                                if (sortColumns.length > 1) {
                                    sortColumns = [sortColumns[sortColumns.length - 1]];
                                }
                                else if (sortColumns.length == 1 && sortColumns.filter(c => c[d.name] != null).length == 0) {
                                    sortColumns = [];
                                } 
                            }

                            sortIndex = sortColumns.findIndex(c => c[d.name] != null);

                            if (sortIndex == -1) {
                                let sortColumn = {};
                                sortColumn[d.name] = { sortedState: 'asc' };
                                sortColumns.push(sortColumn);
                                sortIndex = sortColumns.findIndex(c => c[d.name] != null);
                            }

                            // prepare sort expression
                            let newSortState = (sortIndex == -1 ? '' : sortColumns[sortIndex][d.name].newSortState);
                            if (_isNullOrEmpty(newSortState) == true) {
                                newSortState = 'asc';
                            }

                            var sortComparePhrase_1 = _emptyStr;
                            var sortComparePhrase_2 = _emptyStr;
                            sortColumns.map(function (sc) { return Object.keys(sc)[0]; }).forEach(function (sortColumn) {
                                sortComparePhrase_1 += (_isNotNullOrEmpty(sortComparePhrase_1)? ' + ': '') + `scp_1['${sortColumn}']`;
                            });
                            sortComparePhrase_2 = sortComparePhrase_1.replace(/scp_1/g, 'scp_2');
                            
                            if (_isNullOrEmpty(newSortState) || newSortState == 'asc') {
                                param._data = param._data.sort(function (scp_1, scp_2) {
                                    return eval('d3.ascending(eval(sortComparePhrase_1), eval(sortComparePhrase_2))');
                                });
                            }
                            else {
                                param._data = param._data.sort(function (scp_1, scp_2) {
                                    return eval('d3.descending(eval(sortComparePhrase_1), eval(sortComparePhrase_2))');
                                });
                            }

                            // clean present
                            table.selectAll('tbody > tr').remove();
                            // render new order
                            render_data_rows(columns, param, draw_param);
                            _do._switch_foot_selection(draw_param['visual-id']);

                            // update the currently sorted state for the column
                            sortColumns[sortIndex][d.name].sortedState = newSortState;
                        }
                        finally {
                            visualHolder.style('cursor', prior_cursor_value);
                        }

                        // render column sort status
                        col_head
                            .html(function (d) {
                                let sortIndex = sortColumns.findIndex(c => c[d.name] != null);
                                let sortedState = (sortIndex == -1? '' : sortColumns[sortIndex][d.name].sortedState);

                                return `<sub style="color: black; font-weight: bolder; font-size: xx-small;">`
                                    + `${sortIndex >= 0 && sortColumns.length > 1 ? sortIndex + 1 : ''}</sub>`
                                    //do-0: include next sort state symbol once the descending also implemented
                                   // + `<span class="fa ${sortedState == 'asc' ? 'fa-sort-alpha-up' : (sortedState == 'desc' ? 'fa-sort-alpha-down' : '')}"></span>&nbsp;`
                                    + `<span class="fa ${sortedState == 'asc' ? 'fa-sort-alpha-down' : ''}"></span>&nbsp;` // as of now it shows the current sorted state
                                    + `<b>${d.title}</b>`;
                            });
                    }
                })
                .attr('class', function (d) {
                    //do-0: table data type - consider including data type in param and handle the alignment accordingly. default sense it from the first row of data.
                    return  (d.name == param.pattern.dataKey.baseAxis.key ?
                        'vt-c-text' :
                        (param._data.length > 0 && _isNotNullOrEmpty(d.name) && typeof (param._data[0][d.name]) == 'number' ? 'vt-c-number' : 'vt-c-text'));
                })
                //.style('width', defColWidth + 'px')
                .style('border-right', function (d) {
                    if (d.name == _scroll_fill) {
                        return '0px'
                    }
                    else {
                        return d3.select(this).attr('border-right');
                    }
                })
                .style('width', function (d) {
                    if (d.name == _scroll_fill) {
                        return '14px'
                    }
                    else {
                        return d3.select(this).attr('width');
                    }
                })
                .attr('title', function (d) {
                    return _do._get_col_head_text(d);
                })
                .text(function (d) {
                    return _do._get_col_head_text(d);
                })
                .html(function (d) {
                    if (param.option.suppress.checkBox == false && d.name == _check_box) {
                        return `<input id="checkAll${draw_param['_id']}" type="checkbox" style="width: 9px; height: 9px;" title="Check / Uncheck All"/>`
                    }
                    else {
                        return d3.select(this).html();
                    }
                });

            d3.select(`#checkAll${draw_param['_id']}`).on('click', function () {
                console.log('check-all-clicked');
            });
                    /*  //do-: apply column sort on table
                        .append('span')
                        .attr('class', function (d) {
                            return 'glyphicon ' + (d == param.pattern.dataKey.baseAxis.key ? 'glyphicon-sort-by-alphabet' :
                                        (param._data.length > 0 && typeof (param._data[0][d]) == 'number' ? 'glyphicon-sort-by-order' : 'glyphicon-sort-by-alphabet'));
                        })
                        .attr('style', 'opacity: 0.5;');
                        */

            // do-0: table visual type - review the need of 'table-row' class and fix.
            render_data_rows = function (columns, param, draw_param) {
                let table = d3.select('#' + draw_param['visual-id'] + ' table');
                let rows = table.selectAll('tbody').selectAll('tr') 
                    .data(param._data)
                    .enter()
                    .append('tr')
                    .style('display', 'table')
                    .style('width', '100%')
                    .style('table-layout', 'fixed')
                    .attr('class', function (d, i) {
                        return 'table-row ' + (_get.is_visual_interactable(param) ? 've-inter-default ' : '') + ' ve-inter-table-default vt-r-data ' + (i % 2 == 1 ? 'vt-r-data-even' : '');
                    })
                    //.attr('data-data-id', function (d) { return d._id; })  //do-0: analyze whether 'data-data-id' is really required to be assigned to the element.
                    .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                    .on('mousedown', function () {
                        if (_get.is_visual_interactable(param)) {
                            d3.select(this).classed('ve-grabbing', d3.event.ctrlKey == true)
                        }
                    })
                    .on('mouseup', function () {
                        if (_get.is_visual_interactable(param)) {
                            d3.select(this).classed('ve-grabbing', false)
                        }
                    })
                    .on('click', function () {
                        if (_get.is_visual_interactable(param)) {
                            this.tbody = d3.select('#' + d3.select(this).attr('data-visual-id') + ' tbody');
                            this.tfoot = d3.select('#' + d3.select(this).attr('data-visual-id') + ' tfoot');

                            if (d3.select(this).classed('ve-inter-selected') == true && this.tbody.selectAll('.ve-inter-selected').size() == 1) {
                                _do._switch_selection(d3.select(this).attr('data-visual-id'), true);  // select all
                            }
                            else
                                if (d3.event.ctrlKey == true) {
                                    // switch selection
                                    d3.select(this).classed('ve-inter-selected', d3.select(this).classed('ve-inter-selected') == true ? false : true);
                                    d3.select(this).classed('ve-inter-unselected', d3.select(this).classed('ve-inter-selected') == false);
                                    d3.select(this).classed('ve-inter-table-unselected', d3.select(this).classed('ve-inter-selected') == false);

                                    if (this.tbody.selectAll('.ve-inter-default').size() == this.tbody.selectAll('.ve-inter-selected').size()) {
                                        this.tfoot.selectAll('tr').classed('ve-inter-unselected', false);
                                        this.tfoot.selectAll('tr').classed('ve-inter-table-unselected', false);
                                        this.tfoot.selectAll('tr').classed('ve-inter-selected', true);
                                    }
                                    else {
                                        this.tfoot.selectAll('tr').classed('ve-inter-unselected', true);
                                        this.tfoot.selectAll('tr').classed('ve-inter-table-unselected', true);
                                        this.tfoot.selectAll('tr').classed('ve-inter-selected', false);
                                    }
                                }
                                else {
                                    _do._switch_selection(d3.select(this).attr('data-visual-id'), false); // un-select all 
                                    d3.select(this).classed('ve-inter-selected', true); // select current
                                }

                            _do._invoke_child_visuals(d3.select(this).attr('data-visual-id'));
                        }

                        _do._invoke_listeners(this);
                    });

                var cells = rows.selectAll('td')
                    .data(function (row) {
                        return columns.filter(c => c.name != _scroll_fill).map(function (column) {
                            return {
                                key: column, value: row[column.name], _id: row['_id']
                            };
                        });
                    })
                    .enter()
                    .append('td')
                    .style('border-right', '1px solid lightgray')
                    .attr('nowrap', '1')
                    .attr('class', function (d, i) {
                        return (d.key.name == param.pattern.dataKey.baseAxis.key ? 'vt-c-text' : // note: this condition is required, other wise based on the data the text column will be right aligned.
                                    (typeof (d.value) == 'number' ? 'vt-c-number' : 'vt-c-text')
                            );
                    })
               //     .style('display', 'table-cell')
                    //do-0: calculate the size of the text and cut text based on the cell size
                    //.style('width', defColWidth + 'px') 
                    .attr('title', function (d) {
                        return d.value;
                    })
                    .text(function (d) {
                        return d.value;
                    })
                    .attr('data-check-box-id', function (d) {
                        return (param.option.suppress.checkBox == false && _isNullOrEmpty(d.value) ? `check${d._id}` : '');
                    })
                    .on('mouseenter', function () {
                        d3.select(this).classed('vt-cell-mouseover', true);
                    })
                    .on('mouseleave', function () {
                        d3.select(this).classed('vt-cell-mouseover', false);
                    });

                cells
                    .html(function () {
                        if (d3.select(this).attr('data-check-box-id') != '') {
                            return `<input id="${d3.select(this).attr('data-check-box-id')}" type="checkbox" style="width: 9px; height: 9px;" />`
                        }
                        else {
                            return d3.select(this).html();
                        }
                    });
            }

            render_data_rows(columns, param, draw_param);

            if (_isNotNullOrEmpty(param.pattern.dataKey.plotAxis.summaryReduceMethod)) {
                tfoot.append('tr')
                    .attr('data-visual-id', 'visual' + draw_param['_id'])
                    .on('click', function () {
                        this.tbody = d3.select('#' + d3.select(this).attr('data-visual-id') + ' tbody');

                        if (_get.is_visual_interactable(param)) {
                            _do._switch_foot_selection(d3.select(this).attr('data-visual-id'));
                            _do._invoke_child_visuals(d3.select(this).attr('data-visual-id'));
                        }

                        _do._invoke_listeners(this);
                })
                .selectAll('th')
                .data(columns).enter()
                .append('th')
                .attr('class', function (d) {
                    return (d.name == param.pattern.dataKey.baseAxis.key ? 'vt-c-text' :
                                (param._data.length > 0 && _isNotNullOrEmpty(d.name) && typeof (param._data[0][d.name]) == 'number' ? 'vt-c-number vt-c-summary' : 'vt-c-text')
                    );
                })
                .style('border-right', function (d) {
                    if (d.name == _scroll_fill) {
                        return '0px'
                    }
                    else {
                        return d3.select(this).attr('border-right');
                    }
                })
                .style('width', function (d) {
                    if (d.name == _scroll_fill) {
                        return '14px'
                    }
                    else {
                        return d3.select(this).attr('width');
                    }
                })
                //.style('width', defColWidth + 'px')
                .text(function (d) {
                    let result;
                    if (_isNotNullOrEmpty(d.name)) {
                        result = sn.visual.helper.reduceToSummaryValue(d.name, param);
                    }
                    else {
                        result = _emptyStr;
                    }
                    return result;
                });
            }
        },

        // chek-list
        _draw_visual_check_list_vertical: function (draw_param) {
            var visualHolder = draw_param['visual-holder'],
                visual = draw_param['visual'],
                param = draw_param['param'];

            /* --do-0: see the applicability of setting the title before proceeding with this check list component
            if (_get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').size() > 0) {
                _get.d3.titleContainer(`${draw_param['_id']}`).select('.v-head-text-holder').text(param.basic.title);
            }
            else {
                _get.d3.title(`${draw_param['_id']}`).text(param.basic.title);
            }
            */

            let _check_list_element_click = function (element) {
                sn.visual._core_lib._visual_element_click_handling({
                    'this': element,
                    'visualId': d3.select(element).attr('data-visual-id'),
                    'param': param
                });

                // additional element click handling
                let selected = d3.select('#' + d3.select(element).attr('data-visual-id') + ' .ve-inter-group-default');
                selected.selectAll('.ve-inter-default').each(function (d) {
                    let sel = d3.select(this);
                    if (sel.classed('ve-inter-selected') == true) {
                        sel.select('input').attr('checked', 'true')
                    }
                    else {
                        sel.select('input').attr('checked', null);
                    }                    
                });
            }
            let _selectall_switch = function () {
                let element = d3.select('#selectall_' + draw_param['_id']);
                
                if (element.attr('data-selectedall') == 0) {
                    element
                        .style('background-color', 'black')
                        .style('color', 'white')
                        .attr('data-selectedall', 1);

                    liList.selectAll('input').attr('checked', true);
                    liList.classed('ve-inter-selected', true);
                }
                else {
                    element
                        .style('background-color', 'white')
                        .style('color', 'black')
                        .attr('data-selectedall', 0);
                    liList.selectAll('input').attr('checked', null);
                    liList.classed('ve-inter-selected', false);
                }
            };

            visual.style('font-size', param.decoration.textSize + 'px');
            if (param.option.suppress.toolBar == true) {
                let title = visual.append('span')
                            .text(param.basic.title)
                            .attr('class', 'v-text-noselect v-text-overflow-hide')
                            .attr('id', _get.id.tcaption(draw_param['_id']))
                            .style('font-size', param.decoration.textSize + 5 + 'px')
                            .style('font-weight', 'bold')
                            .style('width', '100%');
                _set.element.text_as_title(title.attr('id'));

                title.append('label').text('All')
                    .attr('id', 'selectall_' + draw_param['_id'])
                    .style('font-size', param.decoration.textSize + 'px')
                    .style('margin-left', '2px')
                    .style('border', '1px solid black')
                    .style('border-radius', '3px')
                    .style('padding-left', '1px')
                    .style('padding-right', '2px')
                    .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                    .attr('data-selectedall', 0)
                    .on('click', function () {
                        _selectall_switch();
                        // need to invoke element click
                    });
                }

            let ul = visual.append('ul')
                .attr('class', 'v-check-list-item-group ve-inter-group-default')
                .style('list-style', 'none');

            let liList = ul.selectAll('li').data(param._data).enter()
                .append('li')
                .attr('data-visual-id', _get.id.visual(draw_param['_id']))
                .attr("class", function () {
                    return 'v-check-list-item-default ' + (_get.is_visual_interactable(param) == true ? 've-inter-default  ': '')
                })
                .on('click', function () {
                    _check_list_element_click(this);

                    let element = d3.select('#selectall_' + draw_param['_id']);
                    element
                        .style('background-color', 'white')
                        .style('color', 'black')
                        .attr('data-selectedall', 0);
                });

            liList.append('input')
                .attr('class', 'v-check-box')
                .attr('id', function (d, i) {
                    return `cb_${i}_${draw_param['_id']}`;
                })
                .attr('disabled', 'true')
                .attr('type', 'checkbox')
                .on('click', function () {
                    _check_list_element_click(this.parentElement);
                });

            liList.append('label')
                .attr('class', 'v-text-noselect')
                .style('font-size', param.decoration.textSize +2 + 'px')
                .style('margin-left', '2px')
                .attr('for', function (d, i) {
                    return `cb_${i}_${draw_param['_id']}`;
                })
                .text(function (d, i) {
                    return `${d[param.pattern.dataKey.baseAxis.key]}`;
                });
        },
    },
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
            //'#ffffff',  // white  // excluding white color because the default background is white.
            '#01b8aa',
            '#000000',
            '#374649',
            '#fd625e',
            '#f2c80f',
            '#5f6b6d',
            '#8ad4eb',
            '#fe9666'
        ]
    },
    _message_template: {
        '_error_suffix': 'Missing details in parameter will lead to experience visual not rendered, incorrectly rendered or not behaving as expected.',
        '_warning_suffix': 'Missing details in parameter may lead to experience visual not rendered, incorrectly rendered or not behaving as expected.',
        '_simple': 'sn.visual: %s. ',
        '_key_value': 'sn.visual: %s. %s: %o.',
        '_validlist_key_value': 'sn.visual: %s. %o %s: %o.',
        get: function (templateName) {
            switch (templateName) {
                case 'error': {
                    return this._simple + this._error_suffix;
                    break;
                }
                case 'error-key-value': {
                    return this._key_value + this._error_suffix;
                    break;
                }
                case 'error-validlist-key-value': {
                    return this._validlist_key_value + this._error_suffix;
                    break;
                }
                case 'warning': {
                    return this._simple + this._warning_suffix;
                    break;
                }
                case 'warning-key-value': {
                    return this._key_value + this._warning_suffix;
                    break;
                }
                case 'warning-validlist-key-value': {
                    return this._validlist_key_value + this._warning_suffix;
                    break;
                }
            }
        }
    },
    _model: [],
    create: function (param) { 
        let result = null;

        try {
            // step #1: param handling - pre-requisite

            // handling auto interaction input value
            this._core_lib._set_interaction_mode(param, this.default.param.option.behaviour.interactionMode);
            this._core_lib._casacade_down_the_interaction_mode(param, param.option.behaviour.interactionMode);

            // step #2: ensure param is valid in all aspects
            var paramIsValid = this.helper.validateParam(param);

            if (paramIsValid == true) {
                // set default if any required value is missing
                param.pattern.orientation =
                        (param.pattern.type == 'bar' && param.pattern.orientation == null ? 'vertical' : param.pattern.orientation);

                // param handling
                param = this.helper.handleParam(param);
                if (param.data) {
                    // have a copy of 'param.data' in 'param._data' for dynamic data handling and binding to visuals, because internally the calculation and rendering are done based on '_data'
                    param['_data'] = _get.json_cloned(param.data);
                
                    // reduce the parent level figures with consolidation of the leaf node figures using the dynamic _data 
                    this.helper.reduceParamPlotValue(param, param._data);
                    if (param.option.behaviour.interactionMode == 'auto') {
                        this.helper.setUniqueIdToData(param);
                    }

                    // assign the resolved copy of _data to data. henceforth data is treated as the source for all interactions.
                    param['data'] = _get.json_cloned(param._data);

                    // visual invocation
                    var _draw_visual = this._core_lib._get_draw_visual_routine(param.pattern.type, param.pattern.orientation);

                    if (typeof (_draw_visual) == 'function') {
                        var visualGround = this._core_lib._get_visual_ground_created(param, _draw_visual);
                        visualGround['visual-id'] = 'visual' + visualGround._id;

                        _draw_visual(visualGround);

                        if (param.option.behaviour.interactionMode == 'auto' && param.children && param.children.length > 0) {
                            // do-0: properties specific to children, validate properly
                            // done: (this is the right place) *** default is all elements selected - hence refresh all children as well.
                            param.children.forEach(function (child) {
                                var dataSourceKey = child.pattern.dataKey.baseAxis.dataSourceKey;
                                var groupKey = child.pattern.dataKey.baseAxis.groupKey;
                                var dataToDraw = [];

                                if (child.pattern.type == 'bar-grouped' && _isNotNullOrEmpty(groupKey) == true) {
                                    param.data.forEach(function (dataItem) {
                                        let newDatum = {};
                                        newDatum[child.pattern.dataKey.baseAxis.key] = dataItem[child.pattern.dataKey.baseAxis.key];
                                        newDatum[child.pattern.dataKey.baseAxis.groupKey] = dataItem[child.pattern.dataKey.baseAxis.groupKey];
                                        dataToDraw.push(newDatum);
                                    });
                                    child._data = _get.json_cloned(dataToDraw);
                                }
                                else if (_isNotNullOrEmpty(dataSourceKey) == true) {
                                    param.data.forEach(function (dataItem) {
                                        dataItem[dataSourceKey].forEach(function (item) {
                                            dataToDraw.push(item);
                                        });
                                    });
                                    dataToDraw = _get.json_cloned(dataToDraw);
                                    child._data = sn.visual.helper.reduceSelectedAll(child, dataToDraw);
                                }

                                child.data = _get.json_cloned(child._data);

                                var childVisual = sn.visual.create(child);

                                var childInModel = sn.visual._model.filter(item => item['_id'] == childVisual.id)[0];
                                if (_isNotNullOrEmpty(childInModel)) {
                                    childInModel['parent-visual-id'] = _get.id.visual(visualGround._id);
                                }
                            });
                        }

                        sn.visual._model.push(visualGround);
                        result = { id: visualGround._id };
                    }
                }
            }
        }
        catch (e) {
            sn.visual.helper.handleException(e);
        }

        return result;
    },
    update: function (updateParam) {
        /* updateParam = {
            _id: 123, // id of the visual returned by the create method
            data: [...] // json data
        }
        */
        let visualUpdated = false;

        try
        {
            // validation part
            if (updateParam == null) {
                throw { error: 'Parameter is missing' }
            }

            if (updateParam.id == null) {
                throw { error: "Parameter does not have the 'id' of visual", key: 'updateParam', value: updateParam }
            }

            const modelIndex = sn.visual._model.findIndex(m => m._id == updateParam.id);
            if (modelIndex < 0) {
                throw { error: "Unable to find the visual with the 'id' property specified", key: 'updateParam', value: updateParam }
            }

            if (updateParam.data == null) {
                throw { error: "Parameter does not have 'data'", key: 'updateParam', value: updateParam}
            }

            // update visual part
            let modelOfVisual = sn.visual._model[modelIndex];
            modelOfVisual.param['_data'] = _get.json_cloned(updateParam.data);

            modelOfVisual.param.basic.title = (updateParam.basic && updateParam.basic.title ? updateParam.basic.title : modelOfVisual.param.basic.title);
            modelOfVisual.param.pattern.dataKey.baseAxis.title = (updateParam.pattern && updateParam.pattern.dataKey && updateParam.pattern.dataKey.baseAxis ? updateParam.pattern.dataKey.baseAxis.title : modelOfVisual.param.pattern.dataKey.baseAxis.title);
            modelOfVisual.param.pattern.dataKey.plotAxis.title = (updateParam.pattern && updateParam.pattern.dataKey && updateParam.pattern.dataKey.plotAxis ? updateParam.pattern.dataKey.plotAxis.title : modelOfVisual.param.pattern.dataKey.plotAxis.title);

            let _draw_visual = this._core_lib._get_draw_visual_routine(modelOfVisual.param.pattern.type, modelOfVisual.param.pattern.orientation);

            if (typeof (_draw_visual) == 'function') {
                d3.selectAll('#' + modelOfVisual['visual-id'] + ' > *').remove();
                _draw_visual(modelOfVisual);
                visualUpdated = true;
            }
        }
        catch (e) {
            if (e.error) {
                e.error += '. Unable to proceed with update';
            }
            sn.visual.helper.handleException(e);
        }

        return (visualUpdated);
    }
};
