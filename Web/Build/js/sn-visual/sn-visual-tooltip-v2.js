if (sn === null || sn === undefined || sn === '') {
    sn ={ visual: {_core_lib: {}} };
}

sn.visual._core_lib._tooltip = {
    _tooltip: null,
    _create: function() {
        if (d3.select('.sn-tooltip').size() === 0) {
             this._tooltip = d3.select("body")
                        .append("div")
                        .attr('class', 'sn-tooltip v-text-noselect');    
        } 
            return this;
    },
    _show: function (data, param) {
        if (param.toolTipProperty === null || param.toolTipProperty === undefined || param.toolTipProperty === '') {
            this._tooltip
                .style("left", d3.event.pageX - 50 + "px")
                .style("top", d3.event.pageY - 100 + "px")
                .style("display", "inline-block")
                .style("opacity", .9)
                .html(data[param.baseAxisKey] + ' - ' + data[param.plotAxisKey]);
        } else {
            this._customTooltip(data, param)
        }
    },
    _hide: function () {
        this._tooltip.style("display", "none");
    },
    _customTooltipCreate: function (data, param) {
        let toolTipTitle = param.toolTipTitleProperty.split(',');
        let toolTip = param.toolTipProperty.split(',');

        const divOpen = '<div class="chart-toolTip">';
        const divClose = '</div>';
        let table = '<table class="tooltipTable"  style="margin-bottom:0;">';
        const openHeader = '<td class="chart-toolTip-header" style="opacity:0.9"><span>';
        const closeHeader = '</span></td>';
        const openRow = '<tr>';
        const closeRow = '</tr>';
        const closeTable = '</tbody></table>';
        const openBody = '<td>';
        const closeBody = '</td>';


        table += openRow;
        toolTipTitle.forEach((title, index) => {
            if (title === 'key') {
                table += openHeader + ' ' + data['key'] + ' ';
            }
            else if (data[title] != '' && data[title] != undefined && data[title] != null) {
                table += openHeader + ' ' + data[title] + ' ';
            } 
            else 
            {
                table += openHeader + ' ' + title + ' ';
            }
            table += closeHeader;
        });
        table += closeRow;

        table += openRow;
        toolTip.forEach((tip, index) => {
            table += openBody + ' ' + data[tip] + ' ';
            table += closeBody;
        });
        table += closeRow;
        table += closeTable;

        param._tooltipHtml = divOpen + table + divClose;
        return true;

    },
    _customTooltip: function (data, param) {
        this._customTooltipCreate(data, param);
        this._tooltip
            .style("left", d3.event.pageX - 50 + "px")
            .style("top", d3.event.pageY - 100 + "px")
            .style("display", "inline-block")
            .style("opacity", .9)
            .html(param._tooltipHtml);
    }
};