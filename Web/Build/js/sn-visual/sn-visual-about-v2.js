if (sn === null || sn === undefined || sn === '') {
    sn ={ visual: {_core_lib: {} } };
}

sn.visual._core_lib._about = {
            _userMessage: {
                emptyMessage: 'Sorry No Visual Details Found!'
            },
            _create: function (param) {
                let visualHolder = _get.d3.holder(param._id);
                visualAbout = visualHolder.append('div');

                visualAbout.attr('id', "about" + param._id)
                    .attr('class', 'v-about')
                    .style('height', `${window.innerHeight - sn.visual.default.margin.right + sn.visual.default.margin.left}px`);
        
                // let editAbout = param.allowEdit == true &&
                //     param.listener &&
                //     param.listener.onAboutVisualEdited &&
                //     typeof (param.listener.onAboutVisualEdited) == 'function';

                // let aboutSaveButtonHtml = (editAbout == true ? `<span class ="fa fa-save" style="font-size: 18px; color: ${hostColor}; cursor: pointer;display: none;" id="about_save${param._id}"></span>` : _emptyStr);
                // let textAreaStartTag = (editAbout == true ? `<textarea id="about_edit${param._id}" style="max-width: 387px; min-width: 387px;">` : _emptyStr);
                // let textAreaEndTag = (editAbout == true ? '</textarea>' : _emptyStr);

                let aboutSaveButtonHtml = '';

                visualAbout.append('div')
                    .html(`
                    <label class="v-text-noselect" style="color: lightgray;font-size: 8px;">Version - ${sn.visual.default.version}</label>
                    <table style="width: 100%; margin_bottom: 15px;">
                            <td>
                                <label class ="v-text-noselect" style="border-radius: 3px; opacity: 0.7; padding-left: 5px; padding-right: 5px; margin-bottom: 0px;">
                                About Visual
                                </label>
                            </td>
                            <td align="right">
                                ${aboutSaveButtonHtml}
                            </td>
                        </tr>
                    <table>
                    <p>
                            ${sn.visual.helper.isNullOrUndefined(param.visualAbout) === true ? this._userMessage.emptyMessage : param.visualAbout}
                    </p>
                `);
            },
            _show: function (param, option) {
                let visualAbout = _get.d3.about(param._id);
                if (visualAbout) {
                    visualAbout
                        .style('display', 'block')
                        .style('height', `${window.innerHeight - sn.visual.default.margin.right + sn.visual.default.margin.left }px`);
                }
    
            },
            _hide: function (param) {
                let visualAbout = _get.d3.about(param._id);
                 visualAbout.style('display', 'none')
            },
}