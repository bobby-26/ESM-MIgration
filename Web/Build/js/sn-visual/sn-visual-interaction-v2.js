if (sn === null || sn === undefined || sn === '') {
    sn = {
        visual: {}
    };
}


sn.visual.interaction = {
    visualList: [],
    host: null,
    filterParam: null,
    create: function(chartTemplates, url, filterParams) {
        this.filterParam = filterParams;
        chartTemplates.forEach(template => {
            // get the chart data
            this.host = url + '/';
            
            d3.json(this.host + template.visualURL, {
                    method: 'POST',
                    body: JSON.stringify(filterParams),
                    headers: { "Content-type": "application/json; charset=UTF-8" }
                })
                .then(function (data) {
                    sn.visual.interaction.getChartData(JSON.parse(data.d), template);
                })
                .catch(function (error) {
                    sn.visual.interaction.errorHandler(error);
                });
           });
    },
    getChartData: function (data, param) {
            this.createChart(param, data, this.eventWireHelper);
            // wire the event with base and other events
    },
    createChart: function (param, data, baseAxisClick) {
        let id = null;
        switch (param.visualType.toLowerCase()) {
            case "bar":
                id = this._barHandler(param, data, baseAxisClick);
                break;
            case "bar-group":
                id = sn.visual.barGrouped.vertical.create(param, data, baseAxisClick);
                break;
            case "pie":
                id = sn.visual.pie.create(param, data, baseAxisClick);
                break;
            case "donut":
                id = sn.visual.donut.create(param, data, baseAxisClick);
                break;
            case "line":
                id = sn.visual.line.create(param, data, baseAxisClick);
                break;
            case "table":
                id = sn.visual.table.create(param, data, baseAxisClick);
                break;
        }
        // update in model
        this.visualList.push({ 'id': id, 'visualName': param.name, 'param': param }); // need to 
        return id;
    },
    errorHandler: function (error) {
        console.log('calling from error handler' + error);
    },
    eventWireHelper: function (selectedData, param) {
        try {
            let interactionVisual = param.interaction.split(',');
            let interModelProp = param.interactionModelProperty.split(',');
            let interProp = param.interactionProperty.split(',');
            let visualList = sn.visual.interaction.visualList;
            let host = sn.visual.interaction.host;
            // copying the filter obj
            let reqParams = JSON.parse(JSON.stringify(sn.visual.interaction.filterParam));
            //get the current selected data
            sn.visual.interaction.getChartSelectedData(interProp, interModelProp, reqParams, selectedData);
            // get previous interaction data
            sn.visual.interaction.getPreviousVisualInteractionData(param, reqParams);

            // send the data 
            interactionVisual.forEach(visual => {
                try {
                    let chartObj = visualList.filter(vis => vis.visualName === visual.trim())[0];
                    if (chartObj === undefined) {
                        return;
                    }
                    chartObj.param._loader.show(chartObj.param);
                    // set the interaction to child
                    sn.visual.interaction.setCurrentVisualInteractionDataToChild(chartObj, interModelProp, reqParams);
                    d3.json(host + chartObj.param.visualURL, {
                        method: "POST",
                        body: JSON.stringify(reqParams),
                        headers: { "Content-type": "application/json; charset=UTF-8" }
                    }).then((data) => {
                        chartObj.param._visual.update(chartObj.id, JSON.parse(data.d)); // for asp.net web method 
                        chartObj.param._loader.hide(chartObj.param);
                    }).catch((error) => {
                        sn.visual.interaction.errorHandler(error);
                        chartObj.param._loader.hide(chartObj.param);
                    });
                } catch (e) {
                    sn.visual.interaction.errorHandler(e);
                }
                
            });
        } catch(error) {
            sn.visual.interaction.errorHandler(error);
        }
    },
    _barHandler: function (param, data, baseAxisClick) {
        let id = null;
        if (param.orientation.toLowerCase() === 'vertical') {
            id = sn.visual.bar.vertical.create(param, data, baseAxisClick);
        } else {
            id = sn.visual.bar.horizontal.create(param, data, baseAxisClick);
        }
        return id;
    },
    getPreviousVisualInteractionData: function (param, reqParams) {
        if (param._interaction === null || param._interaction === undefined) {
            return;
        }
        // get previous chart interaction data if any
        Object.keys(param._interaction).forEach(key => {
            reqParams[key] = param._interaction[key];
        });
    },
    setCurrentVisualInteractionDataToChild: function (chartObj, interModelProp, reqParams) {
        chartObj.param._interaction === undefined ? chartObj.param._interaction = {} : null;
        interModelProp.forEach((prop, index) => {
            chartObj.param._interaction[prop] = reqParams[prop];
        });
    },
    getChartSelectedData: function (interactionProp, interactionModelProp, requestParams, selectedData) {
        // clearing out the parameter
        interactionProp.forEach((prop, index) => {
            requestParams[interactionModelProp[index]] = [];
        });

        // get chart selected data according to paramater
        selectedData.forEach(sel => {
            interactionProp.forEach((prop, index) => {
                requestParams[interactionModelProp[index]].push(sel[interactionProp[index]]);
            });
        });

        // making the selected data comma seperated
        interactionProp.forEach((prop, index) => {
            requestParams[interactionModelProp[index]] = requestParams[interactionModelProp[index]].join();
        });
    }
}