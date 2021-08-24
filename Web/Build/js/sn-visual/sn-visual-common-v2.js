const _emptyStr = String.fromCharCode();

this._isNullOrEmpty = function (value) {
    return (value === undefined || value == null || value.length <= 0) ? true : false;
}
this._isNullThenGetBlank = function (value) {
    return (value == null ? _emptyStr : value)
}
this._isNoneThenGetZero = function (value) {
    return (value == null || isNaN(value) == true ? 0 : value)
}
this._isNotNullOrEmpty = function (value) {
    return _isNullOrEmpty(value) == false;
}

this.getUniqueId = function (prefix, length, suffix) {
    length = (length == null || length > 17) ? 17 : (length.length == 0) ? length + 1 : length;
    prefix = (prefix == null) ? '' : prefix;
    suffix = (suffix == null) ? '' : suffix;
    var randomNumber = Math.round(Math.random() * Math.pow(10, length));
    // reg. exp: slash space slash g (represents a single space)
    return (prefix + randomNumber + suffix).replace(/ /g, '');
}

this._get = {
    d3: {
        header: function (id) { return d3.select(`#header${id}`); },
        svgTitle: function (id) { return d3.select(`#svgtitle${id}`); },
        title: function (id) { return d3.select(`#title${id}`); },
        titleContainer: function (id) { return d3.select(`#title_container${id}`); },
        toolButtonContainer: function (id) { return d3.select(`#tool_button_container${id}`); },
        holder: function (id) { return d3.select(`#holder${id}`); },
        resizeFull: function (id) { return d3.select(`#resize_full${id}`); },
        resizeSmall: function (id) { return d3.select(`#resize_small${id}`); },
        print: function (id) { return d3.select(`#print${id}`); },
        downloadPng: function (id) { return d3.select(`#downloadpng${id}`); },
        // tooltip: function (id) { return d3.select(`#tooltip${id}`); },
        tooltip: function (id) { return d3.select('.sn-tooltip'); },
        visual: function (id) { return d3.select(`#visual${id}`); },
        tcaption: function (id) { return d3.select(`#tcaption${id}`); },
        infoData: function (id) { return d3.select(`#info_data${id}`); },
        info: function (id) { return d3.select(`#info_${id}`); },
        about: function (id) { return d3.select(`#about${id}`); },
        expand: function (id) { return d3.select(`#expand${id}`); },
        refresh: function (id) { return d3.select(`#refresh${id}`); },
        filter: function (id) { return d3.select(`#filter${id}`); },
        loader: function (id) { return d3.select(`#loader${id}`); },
        editable: function (id) { return d3.select(`#editable${id}`); }
    },
    id: {
        header: function (id) { return `header${id}`; },
        svgTitle: function (id) { return `svgtitle${id}`; },
        title: function (id) { return `title${id}`; },
        titleContainer: function (id) { return `title_container${id}`; },
        toolButtonContainer: function (id) { return `tool_button_container${id}`; },
        holder: function (id) { return `holder${id}`; },
        resizeFull: function (id) { return `resize_full${id}`; },
        resizeSmall: function (id) { return `resize_small${id}`; },
        print: function (id) { return `print${id}`; },
        downloadPng: function (id) { return `downloadpng${id}`; },
        tooltip: function (id) { return `tooltip${id}`; },
        visual: function (id) { return `visual${id}`; },
        tcaption: function (id) { return `tcaption${id}`; },
        infoData: function (id) { return `info_data${id}`; },
        about: function (id) { return `about${id}`; },
        expand: function (id) { return `expand${id}`; },
        refresh: function (id) { return `refresh${id}`; },
        filter: function (id) { return `filter${id}`; },
        loader: function (id) { return `loader${id}`; },
        editable: function (id) { return d3.select(`#editable${id}`); }
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
        text_as_title: function (elementorid) {
            let element = document.getElementById(elementorid);
            if (element == null) {
                element = elementorid;
            }

            if (element) { 
                if (element.offsetWidth < element.scrollWidth) {
                    element.setAttribute('title', element.innerText);
                }
                else {
                    element.setAttribute('title', '');
                }
            }
        }
    }
}

this.getBrowserDetails = function (full) {
    let nVer = navigator.appVersion;
    let nAgt = navigator.userAgent;
    let browserName = navigator.appName;
    let fullVersion = '' + parseFloat(navigator.appVersion);
    let majorVersion = parseInt(navigator.appVersion, 10);
    let nameOffset, verOffset, ix;

    // In Opera 15+, the true version is after "OPR/" 
    if ((verOffset = nAgt.indexOf("OPR/")) != -1) {
        browserName = "Opera";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 4);
        }
    }
        // In older Opera, the true version is after "Opera" or after "Version"
    else if ((verOffset = nAgt.indexOf("Opera")) != -1) {
        browserName = "Opera";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 6);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
    }
        // In MSIE, the true version is after "MSIE" in userAgent
    else if ((verOffset = nAgt.indexOf("MSIE")) != -1) {
        browserName = "Microsoft Internet Explorer";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 5);
        }
    }
        // In Chrome, the true version is after "Chrome" 
    else if ((verOffset = nAgt.indexOf("Edge")) != -1) {
        browserName = "Edge";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 5);
        }
    }
        // In Chrome, the true version is after "Chrome" 
    else if ((verOffset = nAgt.indexOf("Chrome")) != -1) {
        browserName = "Chrome";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 7);
        }
    }
        // In Safari, the true version is after "Safari" or after "Version" 
    else if ((verOffset = nAgt.indexOf("Safari")) != -1) {
        browserName = "Safari";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 7);
            if ((verOffset = nAgt.indexOf("Version")) != -1)
                fullVersion = nAgt.substring(verOffset + 8);
        }
    }
        // In Firefox, the true version is after "Firefox" 
    else if ((verOffset = nAgt.indexOf("Firefox")) != -1) {
        browserName = "Firefox";
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 8);
        }
    }
        // In most other browsers, "name/version" is at the end of userAgent 
    else if ((nameOffset = nAgt.lastIndexOf(' ') + 1) <
              (verOffset = nAgt.lastIndexOf('/'))) {
        browserName = nAgt.substring(nameOffset, verOffset);
        if (full == true) {
            fullVersion = nAgt.substring(verOffset + 1);
            if (browserName.toLowerCase() == browserName.toUpperCase()) {
                browserName = navigator.appName;
            }
        }
    }

    if (full == true) {
        // trim the fullVersion string at semicolon/space if present
        if ((ix = fullVersion.indexOf(";")) != -1)
            fullVersion = fullVersion.substring(0, ix);
        if ((ix = fullVersion.indexOf(" ")) != -1)
            fullVersion = fullVersion.substring(0, ix);

        majorVersion = parseInt('' + fullVersion, 10);
        if (isNaN(majorVersion)) {
            fullVersion = '' + parseFloat(navigator.appVersion);
            majorVersion = parseInt(navigator.appVersion, 10);
        }
    }

    let detail = {};
    detail['browser-short-name'] = browserName.toLowerCase()

    if (full == true)
    { 
        detail['browser-name'] = browserName;
        detail['full-version'] = fullVersion;
        detail['major-version'] = majorVersion;
        detail['navigator-appName'] = navigator.appName;
        detail['navigator-userAgent'] = navigator.userAgent;
    }

    return detail;
}

var sn = (typeof (sn) == null || typeof (sn) != 'object') ? {} : sn;

if (typeof (sn) != null && typeof (sn) === 'object') {
    sn.common = (typeof (sn.common) == null || typeof (sn.common) != 'object') ? {} : sn.common;
}

if (typeof (sn) == 'object' && typeof (sn.common) == 'object') {
    sn.common['_color_list'] = {
        'aliceblue': '#F0F8FF',
        'alloyorange': '#C46210',
        'amethyst': '#9966CC',
        'antiquewhite': '#FAEBD7',
        'aqua': '#00FFFF',
        'aquamarine': '#7FFFD4',
        'ashgray': '#B2BEB5',
        'azure': '#F0FFFF',
        'battleshipgray': '#848482',
        'bdazzledblue': '#2E5894',
        'beige': '#F5F5DC',
        'bigdiporuby': '#9C2542',
        'bisque': '#FFE4C4',
        'bistre': '#3D2B1F',
        'bittersweetshimmer': '#BF4F51',
        'black': '#000000',
        'blackbean': '#3D0C02',
        'blackleatherjacket': '#253529',
        'blackolive': '#3B3C36',
        'blanchedalmond': '#FFEBCD',
        'blastoffbronze': '#A57164',
        'blue': '#0000FF',
        'bluegray': '#6699CC',
        'blueviolet': '#8A2BE2',
        'brown': '#A52A2A',
        'burlywood': '#DEB887',
        'cadetblue': '#5F9EA0',
        'cadetgray': '#91A3B0',
        'cafenoir': '#4B3621',
        'charcoal': '#36454F',
        'charlestongreen': '#232B2B',
        'chartreuse': '#7FFF00',
        'chocolate': '#D2691E',
        'cinereous': '#98817B',
        'coolgray': '#8C92AC',  // #9090C0 ???
        'coral': '#FF7F50',
        'cornflowerblue': '#6495ED',
        'cornsilk': '#FFF8DC',
        'crimson': '#DC143C',
        'cyan': '#00FFFF',
        'cybergrape': '#58427C',
        'darkblue': '#00008B',
        'darkcyan': '#008B8B',
        'darkgoldenrod': '#B8860B',
        'darkgray': '#A9A9A9',
        'darkgreen': '#006400',
        'darkkhaki': '#BDB76B',
        'darkmagenta': '#8B008B',
        'darkmediumgray': '#A9A9A9',
        'darkolivegreen': '#556B2F',
        'darkorange': '#FF8C00',
        'darkorchid': '#9932CC',
        'darkred': '#8B0000',
        'darksalmon': '#E9967A',
        'darkseagreen': '#8FBC8F',
        'darkslateblue': '#483D8B',
        'darkslategray': '#2F4F4F',
        'darkturquoise': '#00CED1',
        'darkviolet': '#9400D3',
        'davysgray': '#555555',
        'deeppink': '#FF1493',
        'deepskyblue': '#00BFFF',
        'deepspacesparkle': '#4A646C',
        'dimgray': '#696969',
        'dodgerblue': '#1E90FF',
        'ebony': '#555D50',
        'eerieblack': '#1B1B1B',
        'firebrick': '#B22222',
        'floralwhite': '#FFFAF0',
        'forestgreen': '#228B22',
        'fuchsia': '#FF00FF',
        'gainsboro': '#DCDCDC',
        'ghostwhite': '#F8F8FF',
        'glaucous': '#6082B6',
        'gold': '#FFD700',
        'goldenrod': '#DAA520',
        'goldfusion': '#85754E',
        'gray': '#808080',
        'graygreen': '#5E716A',
        'green': '#008000',
        'greenyellow': '#ADFF2F',
        'gunmetal': '#536267',
        'honeydew': '#F0FFF0',
        'hotpink': '#FF69B4',
        'illuminatingemerald': '#319177',
        'indianred': '#CD5C5C',
        'indigo': '#4B0082',
        'indigo': '#4B0082',
        'ivory': '#FFFFF0',
        'jet': '#343434',
        'khaki': '#F0E68C',

        'lavender': '#E6E6FA',
        'lavenderblush': '#FFF0F5',
        'lawngreen': '#7CFC00',
        'lemonchiffon': '#FFFACD',
        'licorice': '#1A1110',
        'lightblue': '#ADD8E6',
        'lightcoral': '#F08080',
        'lightcyan': '#E0FFFF',
        'lightgoldenrodyellow': '#FAFAD2',
        'lightgray': '#D3D3D3',
        'lightgreen': '#90EE90',

        'lightpink': '#FFB6C1',
        'lightsalmon': '#FFA07A',
        'lightseagreen': '#20B2AA',
        'lightskyblue': '#87CEFA',
        'lightslategray': '#778899',
        'lightsteelblue': '#B0C4DE',
        'lightyellow': '#FFFFE0',
        'lime': '#00FF00',
        'limegreen': '#32CD32',
        'linen': '#FAF0E6',
        'magenta': '#FF00FF',
        'marengo': '#4C5866',
        'maroon': '#800000',
        'mediumaquamarine': '#66CDAA',
        'mediumblue': '#0000CD',
        'mediumgray': '#BEBEBE',
        'mediumorchid': '#BA55D3',
        'mediumpurple': '#9370DB',
        'mediumseagreen': '#3CB371',
        'mediumslateblue': '#7B68EE',
        'mediumspringgreen': '#00FA9A',
        'mediumtaupe': '#674C47',
        'mediumturquoise': '#48D1CC',
        'mediumvioletred': '#C71585',
        'metallicseaweed': '#0A7E8C',
        'metallicsunburst': '#9C7C38',
        'midnightblue': '#191970',
        'mintcream': '#F5FFFA',
        'mistyrose': '#FFE4E1',
        'moccasin': '#FFE4B5',
        'navajowhite': '#FFDEAD',
        'navy': '#000080',
        'nickel': '#727472',
        'oldlace': '#FDF5E6',
        'olive': '#808000',
        'olivedrab': '#6B8E23',
        'onyx': '#353839',
        'orange': '#FFA500',
        'orangered': '#FF4500',
        'orchid': '#DA70D6',
        'outerspace': '#414A4C',
        'palegoldenrod': '#EEE8AA',
        'palegreen': '#98FB98',
        'paleturquoise': '#AFEEEE',
        'palevioletred': '#DB7093',
        'papayawhip': '#FFEFD5',
        'paynesgray': '#536878',
        'peachpuff': '#FFDAB9',
        'peru': '#CD853F',
        'phthalogreen': '#123524',
        'pink': '#FFC0CB',
        'platinum': '#E5E4E2',
        'plum': '#DDA0DD',
        'powderblue': '#B0E0E6',
        'purple': '#800080',
        'purpletaupe': '#50404D',
        'raisinblack': '#242124',
        'razzmicberry': '#8D4E85',
        'red': '#FF0000',
        'rocketmetallic': '#8A7F8D',
        'rosequartz': '#AA98A9',
        'rosybrown': '#BC8F8F',
        'royalblue': '#4169E1',
        'saddlebrown': '#8B4513',
        'salmon': '#FA8072',
        'sandybrown': '#F4A460',
        'seagreen': '#2E8B57',
        'seashell': '#FFF5EE',
        'sheengreen': '#8FD400',
        'shimmeringblush': '#D98695',
        'sienna': '#A0522D',
        'silver': '#C0C0C0',
        'skyblue': '#87CEEB',
        'slateblue': '#6A5ACD',
        'slategray': '#708090',
        'snow': '#FFFAFA',
        'sonicsilver': '#757575',
        'spanishgray': '#989898',
        'springgreen': '#00FF7F',
        'steelblue': '#0081AB',
        'steelblue': '#4682B4',
        'tan': '#D2B48C',
        'taupe': '#483C32',
        'taupegray': '#8B8589',
        'teal': '#008080',
        'thistle': '#D8BFD8',
        'timberwolf': '#DBD7D2',
        'tomato': '#FF6347',
        'turquoise': '#40E0D0',
        'violet': '#800080',
        'violet': '#EE82EE',
        'wheat': '#F5DEB3',
        'white': '#FFFFFF',  // Note: white color is normally excluded in the visuals
        'whitesmoke': '#F5F5F5',
        'yellow': '#FFFF00',
        'yellowgreen': '#9ACD32'
    };
    sn.common['_crayola_crayon_std_cl'] = {
        'almond': '#EED9C4',
        'antique-brass': '#C88A65',
        'apricot': '#FDD5B1',
        'aquamarine': '#95E0E8',
        'asparagus': '#7BA05B',
        'banana-mania': '#FBE7B2',
        'beaver': '#926F5B',
        'bittersweet': '#FE6F5E',
        //'black': '#000000',
        'blue-1': '#4997D0',
        'blue-2': '#4570E6',
        'blue-3': '#0066FF',
        'blue-bell': '#9999CC',
        'blue-gray': '#C8C8CD',
        'blue-green': '#0095B7',
        'bluetiful': '#3C69E7',
        'blue-violet': '#6456B7',
        'blush': '#DB5079',
        'brick-red': '#C62D42',
        'brilliant-rose': '#E667CE',
        'brown': '#AF593E',
        'burnt-orange': '#FF7F49',
        'burnt-sienna': '#E97451',
        'burnt-umber': '#805533',
        'cadet-blue': '#A9B2C3',
        'canary': '#FFFF99',
        'caribbean-green': '#00CC99',
        'carmine': '#E62E6B',
        'carnation-pink': '#FFA6C9',
        'celestial-blue': '#7070CC',
        'cerise': '#DA3287',
        'cerulean': '#02A4D3',
        'cerulean-blue': '#339ACC',
        'charcoal-gray': '#736A62',
        'cobalt-blue': '#8C90C8',
        'copper': '#DA8A67',
        'cornflower': '#93CCEA',
        'cotton-candy': '#FFB7D5',
        'dandelion': '#FED85D',
        'dark-venetian-red': '#B33B24',
        'denim': '#1560BD',
        'desert-sand': '#EDC9AF',
        'eggplant': '#614051',
        'english-vermilion': '#CC474B',
        'fern': '#63B76C',
        'forest-green': '#5FA777',
        'fuchsia': '#C154C1',
        'fuzzy-wuzzy': '#87421F',
        'gold-1': '#92926E',
        'gold-2': '#E6BE8A',
        'goldenrod': '#FCD667',
        'granny-smith-apple': '#9DE093',
        'gray': '#8B8680',
        'green': '#3AA655',
        'green-blue': '#2887C8',
        'green-yellow': '#F1E788',
        'inchworm': '#AFE313',
        'indian-red': '#B94E48',
        'indigo': '#4F69C6',
        'jazzberry-jam': '#A50B5E',
        'jungle-green': '#29AB87',
        'lavender-1': '#BF8FCC',
        'lavender-2': '#FBAED2',
        'lemon-yellow': '#FFFF9F',
        'light-blue': '#8FD8D8',
        'light-chrome-green': '#BEE64B',
        'light-venetian-red': '#E6735C',
        'macaroni-and-cheese': '#FFB97B',
        'madder-lake': '#CC3336',
        'magenta': '#F653A6',
        'mahogany': '#CA3435',
        'maize': '#F2C649',
        'manatee': '#8D90A1',
        'mango-tango': '#E77200',
        'maroon': '#C32148',
        'mauvelous': '#F091A9',
        'maximum-blue': '#47ABCC',
        'maximum-blue-green': '#30BFBF',
        'maximum-blue-purple': '#ACACE6',
        'maximum-green': '#5E8C31',
        'maximum-green-yellow': '#D9E650',
        'maximum-purple': '#733380',
        'maximum-red': '#D92121',
        'maximum-red-purple': '#A63A79',
        'maximum-yellow': '#FAFA37',
        'maximum-yellow-red': '#F2BA49',
        'medium-chrome-green': '#6CA67C',
        'medium-rose': '#D96CBE',
        'medium-violet': '#8F47B3',
        'melon': '#FEBAAD',
        'middle-blue': '#7ED4E6',
        'middle-blue-green': '#8DD9CC',
        'middle-blue-purple': '#8B72BE',
        'middle-green': '#4D8C57',
        'middle-green-yellow': '#ACBF60',
        'middle-purple': '#D982B5',
        'middle-red': '#E58E73',
        'middle-red-purple': '#A55353',
        'middle-yellow': '#FFEB00',
        'middle-yellow-red': '#ECB176',
        'midnight-blue': '#00468C',
        'mountain-meadow': '#1AB385',
        'mulberry': '#C8509B',
        'navy-blue': '#0066CC',
        'olive-green': '#B5B35C',
        'orange': '#FF8833',
        'orange-red': '#FF5349',
        'orange-yellow': '#F8D568',
        'orchid': '#E29CD2',
        'outer-space': '#2D383A',
        'pacific-blue': '#009DC4',
        'peach': '#FFCBA4',
        'periwinkle': '#C3CDE6',
        'permanent-geranium-lake': '#E12C2C',
        'pig-pink': '#FDD7E4',
        'pine-green': '#01786F',
        'pink-flamingo': '#FC74FD',
        'pink-sherbert': '#F7A38E',
        'plum': '#8E3179',
        'purple-heart': '#652DC1',
        'purple-mountains-majesty': '#D6AEDD',
        'raw-sienna-1': '#D27D46',
        'raw-sienna-2': '#E6BC5C',
        'raw-umber': '#665233',
        'razzmatazz': '#E30B5C',
        'red': '#ED0A3F',
        'red-orange': '#FF681F',
        'red-violet': '#BB3385',
        'robins-egg-blue': '#00CCCC',
        'royal-purple': '#6B3FA0',
        'salmon': '#FF91A4',
        'scarlet': '#FD0E35',
        'sea-green': '#93DFB8',
        'sepia': '#9E5B40',
        'shadow': '#837050',
        'shamrock': '#33CC99',
        'silver': '#C9C0BB',
        'sky-blue': '#76D7EA',
        'spring-green': '#ECEBBD',
        'sunset-orange': '#FE4C40',
        'tan': '#D99A6C',
        'teal-blue': '#008080',
        'thistle': '#EBB0D7',
        'tickle-me-pink': '#FC80A5',
        'timberwolf': '#D9D6CF',
        'tropical-rain-forest': '#00755E',
        'tumbleweed': '#DEA681',
        'turquoise-blue': '#6CDAE7',
        'ultramarine-blue': '#3F26BF',
        'van-dyke-brown': '#664228',
        'venetian-red': '#CC553D',
        'violet-1': '#732E6C',
        'violet-2': '#8359A3',
        'violet-blue': '#766EC8',
        'violet-red': '#F7468A',
        'vivid-tangerine': '#FF9980',
        'vivid-violet': '#803790',
        //'white': '#FFFFFF',
        'wild-blue-yonder': '#7A89B8',
        'wild-strawberry': '#FF3399',
        'wisteria': '#C9A0DC',
        'yellow': '#FBE870',
        'yellow-green': '#C5E17A',
        'yellow-orange': '#FFAE42'
    };
    sn.common['_crayola_crayon_fluorescent'] = {
        'outrageous-orange': '#FF6037',
        'atomic-tangerine': '#FF9966',
        'neon-carrot': '#FF9933',
        'sunglow': '#FFCC33',
        'laser-lemon': '#FFFF66',
        'unmellow-yellow': '#FFFF66',
        'electric-lime': '#CCFF00',
        'screamin-green': '#66FF66',
        'magic-mint': '#AAF0D1',
        'blizzard-blue': '#50BFE6',
        'shocking-pink': '#FF6EFF',
        'razzle-dazzle-rose': '#EE34D2',
        'hot-magenta': '#FF00CC',
        'purple-pizzazz': '#FF00CC',
        'radical-red': '#FF355E',
        'wild-watermelon': '#FD5B78'
    };
    sn.common['_crayola_crayon_silver_swirls'] = {
        'aztec-gold': '#C39953',
        'burnished-brown': '#A17A74',
        'cerulean-frost': '#6D9BC3',
        'cinnamon-satin': '#CD607E',
        'copper-penny': '#AD6F69',
        'cosmic-cobalt': '#2E2D88',
        'glossy-grape': '#AB92B3',
        'granite-gray': '#676767',
        'green-sheen': '#6EAEA1',
        'lilac-luster': '#AE98AA',
        'misty-moss': '#BBB477',
        'mystic-maroon': '#AD4379',
        'pearly-purple': '#B768A2',
        'pewter-blue': '#8BA8B7',
        'polished-pine': '#5DA493',
        'quick-silver': '#A6A6A6',
        'rose-dust': '#9E5E6F',
        'rusty-red': '#DA2C43',
        'shadow-blue': '#778BA5',
        'shiny-shamrock': '#5FA778',
        'steel-teal': '#5F8A8B',
        'sugar-plum': '#914E75',
        'twilight-lavender': '#8A496B',
        'wintergreen-dream': '#56887D'
    };
    sn.common['_crayola_crayon_gem_tones'] = {
        'amethyst': '#64609A',
        'citrine': '#933709',
        'emerald': '#14A989',
        'jade': '#469A84',
        'jasper': '#D05340',
        'lapis-lazuli': '#436CB9',
        'malachite': '#469496',
        'moonstone': '#3AA8C1',
        'onyx': '#353839',
        'peridot': '#ABAD48',
        'pink-pearl': '#B07080',
        'rose-quartz': '#BD559C',
        'ruby': '#AA4069',
        'sapphire': '#2D5DA1',
        'smokey-topaz': '#832A0D',
        'tigers-eye': '#B56917'
    };

    sn.common._color_scheme = {};

    //TODO:P2: move all colors to color base, kind of array, and associate the respective colors to the scheme.
    sn.common._color_scheme['shades-of-black'] = [ // source: https://en.wikipedia.org/wiki/Shades_of_black
        sn.common['_color_list']['bistre'],
        sn.common['_color_list']['black'],
        sn.common['_color_list']['blackbean'],
        sn.common['_color_list']['blackleatherjacket'],
        sn.common['_color_list']['blackolive'],
        sn.common['_color_list']['cafenoir'],
        sn.common['_color_list']['charcoal'],
        sn.common['_color_list']['charlestongreen'],
        sn.common['_color_list']['davysgray'],
        sn.common['_color_list']['dimgray'],
        sn.common['_color_list']['ebony'],
        sn.common['_color_list']['eerieblack'],
        sn.common['_color_list']['jet'],
        sn.common['_color_list']['licorice'],
        sn.common['_color_list']['midnightblue'],
        sn.common['_color_list']['onyx'],
        sn.common['_color_list']['outerspace'],
        sn.common['_color_list']['phthalogreen'],
        sn.common['_color_list']['raisinblack'],
        sn.common['_color_list']['taupe']
    ];
    sn.common._color_scheme['shades-of-gray'] = [
        sn.common['_color_list']['ashgray'],
        sn.common['_color_list']['battleshipgray'],
        sn.common['_color_list']['black'],
        sn.common['_color_list']['bluegray'],
        sn.common['_color_list']['cadetgray'],
        sn.common['_color_list']['charcoal'],
        sn.common['_color_list']['cinereous'],
        sn.common['_color_list']['coolgray'],
        sn.common['_color_list']['darkmediumgray'],
        sn.common['_color_list']['davysgray'],
        sn.common['_color_list']['dimgray'],
        sn.common['_color_list']['gainsboro'],
        sn.common['_color_list']['glaucous'],
        sn.common['_color_list']['gray'],
        sn.common['_color_list']['graygreen'],
        sn.common['_color_list']['gunmetal'],
        sn.common['_color_list']['jet'],
        sn.common['_color_list']['lightgray'],
        sn.common['_color_list']['marengo'],
        sn.common['_color_list']['mediumgray'],
        sn.common['_color_list']['mediumtaupe'],
        sn.common['_color_list']['nickel'],
        sn.common['_color_list']['paynesgray'],
        sn.common['_color_list']['platinum'],
        sn.common['_color_list']['purpletaupe'],
        sn.common['_color_list']['rocketmetallic'],
        sn.common['_color_list']['rosequartz'],
        sn.common['_color_list']['silver'],
        sn.common['_color_list']['slategray'],
        sn.common['_color_list']['spanishgray'],
        sn.common['_color_list']['taupe'],
        sn.common['_color_list']['taupegray'],
        sn.common['_color_list']['timberwolf']
    ];

    sn.common._color_scheme['crayon-metallic-fx'] = [ // source: https://en.wikipedia.org/wiki/List_of_Crayola_crayon_colors
        sn.common['_color_list']['alloyorange'],
        sn.common['_color_list']['bdazzledblue'],
        sn.common['_color_list']['bigdiporuby'],
        sn.common['_color_list']['bittersweetshimmer'],
        sn.common['_color_list']['blastoffbronze'],
        sn.common['_color_list']['cybergrape'],
        sn.common['_color_list']['deepspacesparkle'],
        sn.common['_color_list']['goldfusion'],
        sn.common['_color_list']['illuminatingemerald'],
        sn.common['_color_list']['metallicseaweed'],
        sn.common['_color_list']['metallicsunburst'],
        sn.common['_color_list']['razzmicberry'],
        sn.common['_color_list']['sheengreen'],
        sn.common['_color_list']['shimmeringblush'],
        sn.common['_color_list']['sonicsilver'],
        sn.common['_color_list']['steelblue']
    ];
    sn.common._color_scheme['crayon-standard'] = Object.values(sn.common['_crayola_crayon_std_cl']);
    sn.common._color_scheme['crayon-fluorescent'] = Object.values(sn.common['_crayola_crayon_fluorescent']);
    sn.common._color_scheme['crayon-silver-swirls'] = Object.values(sn.common['_crayola_crayon_silver_swirls']);
    sn.common._color_scheme['crayon-gem-tones'] = Object.values(sn.common['_crayola_crayon_gem_tones']);

    sn.common._color_scheme['distinct-to-humaneye'] = [
        sn.common['_color_list']['aqua'],
        sn.common['_color_list']['azure'],
        sn.common['_color_list']['beige'],
        sn.common['_color_list']['black'],
        sn.common['_color_list']['blue'],
        sn.common['_color_list']['brown'],
        sn.common['_color_list']['cyan'],
        sn.common['_color_list']['darkblue'],
        sn.common['_color_list']['darkcyan'],
        sn.common['_color_list']['darkgray'],
        sn.common['_color_list']['darkgreen'],
        sn.common['_color_list']['darkkhaki'],
        sn.common['_color_list']['darkmagenta'],
        sn.common['_color_list']['darkolivegreen'],
        sn.common['_color_list']['darkorange'],
        sn.common['_color_list']['darkorchid'],
        sn.common['_color_list']['darkred'],
        sn.common['_color_list']['darksalmon'],
        sn.common['_color_list']['darkviolet'],
        sn.common['_color_list']['fuchsia'],
        sn.common['_color_list']['gold'],
        sn.common['_color_list']['green'],
        sn.common['_color_list']['indigo'],
        sn.common['_color_list']['khaki'],
        sn.common['_color_list']['lightblue'],
        sn.common['_color_list']['lightcyan'],
        sn.common['_color_list']['lightgreen'],
        sn.common['_color_list']['lightgray'],
        sn.common['_color_list']['lightpink'],
        sn.common['_color_list']['lightyellow'],
        sn.common['_color_list']['lime'],
        sn.common['_color_list']['magenta'],
        sn.common['_color_list']['maroon'],
        sn.common['_color_list']['navy'],
        sn.common['_color_list']['olive'],
        sn.common['_color_list']['orange'],
        sn.common['_color_list']['pink'],
        sn.common['_color_list']['purple'],
        sn.common['_color_list']['violet'],
        sn.common['_color_list']['red'],
        sn.common['_color_list']['silver'],
        sn.common['_color_list']['yellow']
    ];

    sn.common._color_scheme['red-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['indianred'],
        sn.common['_color_list']['lightcoral'],
        sn.common['_color_list']['salmon'],
        sn.common['_color_list']['darksalmon'],
        sn.common['_color_list']['lightsalmon'],
        sn.common['_color_list']['crimson'],
        sn.common['_color_list']['red'],
        sn.common['_color_list']['firebrick'],
        sn.common['_color_list']['darkred']
    ];
    sn.common._color_scheme['pink-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['pink'],
        sn.common['_color_list']['lightpink'],
        sn.common['_color_list']['hotpink'],
        sn.common['_color_list']['deeppink'],
        sn.common['_color_list']['mediumvioletred'],
        sn.common['_color_list']['palevioletred']
    ];
    sn.common._color_scheme['orange-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['lightsalmon'],
        sn.common['_color_list']['coral'],
        sn.common['_color_list']['tomato'],
        sn.common['_color_list']['orangered'],
        sn.common['_color_list']['darkorange'],
        sn.common['_color_list']['orange']
    ];
    sn.common._color_scheme['yellow-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['gold'],
        sn.common['_color_list']['yellow'],
        sn.common['_color_list']['lightyellow'],
        sn.common['_color_list']['lemonchiffon'],
        sn.common['_color_list']['lightgoldenrodyellow'],
        sn.common['_color_list']['papayawhip'],
        sn.common['_color_list']['moccasin'],
        sn.common['_color_list']['peachpuff'],
        sn.common['_color_list']['palegoldenrod'],
        sn.common['_color_list']['khaki'],
        sn.common['_color_list']['darkkhaki']
    ];
    sn.common._color_scheme['purple-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['lavender'],
        sn.common['_color_list']['thistle'],
        sn.common['_color_list']['plum'],
        sn.common['_color_list']['violet'],
        sn.common['_color_list']['orchid'],
        sn.common['_color_list']['fuchsia'],
        sn.common['_color_list']['magenta'],
        sn.common['_color_list']['mediumorchid'],
        sn.common['_color_list']['mediumpurple'],
        sn.common['_color_list']['amethyst'],
        sn.common['_color_list']['blueviolet'],
        sn.common['_color_list']['darkviolet'],
        sn.common['_color_list']['darkorchid'],
        sn.common['_color_list']['darkmagenta'],
        sn.common['_color_list']['purple'],
        sn.common['_color_list']['indigo'],
        sn.common['_color_list']['slateblue'],
        sn.common['_color_list']['darkslateblue'],
        sn.common['_color_list']['mediumslateblue']
    ];
    sn.common._color_scheme['green-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['greenyellow'],
        sn.common['_color_list']['chartreuse'],
        sn.common['_color_list']['lawngreen'],
        sn.common['_color_list']['lime'],
        sn.common['_color_list']['limegreen'],
        sn.common['_color_list']['palegreen'],
        sn.common['_color_list']['lightgreen'],
        sn.common['_color_list']['mediumspringgreen'],
        sn.common['_color_list']['springgreen'],
        sn.common['_color_list']['mediumseagreen'],
        sn.common['_color_list']['seagreen'],
        sn.common['_color_list']['forestgreen'],
        sn.common['_color_list']['green'],
        sn.common['_color_list']['darkgreen'],
        sn.common['_color_list']['yellowgreen'],
        sn.common['_color_list']['olivedrab'],
        sn.common['_color_list']['olive'],
        sn.common['_color_list']['darkolivegreen'],
        sn.common['_color_list']['mediumaquamarine'],
        sn.common['_color_list']['darkseagreen'],
        sn.common['_color_list']['lightseagreen'],
        sn.common['_color_list']['darkcyan'],
        sn.common['_color_list']['teal']
    ];
    sn.common._color_scheme['blue-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['aqua'],
        sn.common['_color_list']['cyan'],
        sn.common['_color_list']['lightcyan'],
        sn.common['_color_list']['paleturquoise'],
        sn.common['_color_list']['aquamarine'],
        sn.common['_color_list']['turquoise'],
        sn.common['_color_list']['mediumturquoise'],
        sn.common['_color_list']['darkturquoise'],
        sn.common['_color_list']['cadetblue'],
        sn.common['_color_list']['steelblue'],
        sn.common['_color_list']['lightsteelblue'],
        sn.common['_color_list']['powderblue'],
        sn.common['_color_list']['lightblue'],
        sn.common['_color_list']['skyblue'],
        sn.common['_color_list']['lightskyblue'],
        sn.common['_color_list']['deepskyblue'],
        sn.common['_color_list']['dodgerblue'],
        sn.common['_color_list']['cornflowerblue'],
        sn.common['_color_list']['mediumslateblue'],
        sn.common['_color_list']['royalblue'],
        sn.common['_color_list']['blue'],
        sn.common['_color_list']['mediumblue'],
        sn.common['_color_list']['darkblue'],
        sn.common['_color_list']['navy'],
        sn.common['_color_list']['midnightblue']
    ];
    sn.common._color_scheme['brown-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['cornsilk'],
        sn.common['_color_list']['blanchedalmond'],
        sn.common['_color_list']['bisque'],
        sn.common['_color_list']['navajowhite'],
        sn.common['_color_list']['wheat'],
        sn.common['_color_list']['burlywood'],
        sn.common['_color_list']['tan'],
        sn.common['_color_list']['rosybrown'],
        sn.common['_color_list']['sandybrown'],
        sn.common['_color_list']['goldenrod'],
        sn.common['_color_list']['darkgoldenrod'],
        sn.common['_color_list']['peru'],
        sn.common['_color_list']['chocolate'],
        sn.common['_color_list']['saddlebrown'],
        sn.common['_color_list']['sienna'],
        sn.common['_color_list']['brown'],
        sn.common['_color_list']['maroon']
    ];
    sn.common._color_scheme['white-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['white'],
        sn.common['_color_list']['snow'],
        sn.common['_color_list']['honeydew'],
        sn.common['_color_list']['mintcream'],
        sn.common['_color_list']['azure'],
        sn.common['_color_list']['aliceblue'],
        sn.common['_color_list']['ghostwhite'],
        sn.common['_color_list']['whitesmoke'],
        sn.common['_color_list']['seashell'],
        sn.common['_color_list']['beige'],
        sn.common['_color_list']['oldlace'],
        sn.common['_color_list']['floralwhite'],
        sn.common['_color_list']['ivory'],
        sn.common['_color_list']['antiquewhite'],
        sn.common['_color_list']['linen'],
        sn.common['_color_list']['lavenderblush'],
        sn.common['_color_list']['mistyrose']
    ];
    sn.common._color_scheme['gray-based'] = [ // source: https://html-color-codes.info/color-names/
        sn.common['_color_list']['gainsboro'],
        sn.common['_color_list']['lightgrey'],
        sn.common['_color_list']['silver'],
        sn.common['_color_list']['darkgray'],
        sn.common['_color_list']['gray'],
        sn.common['_color_list']['dimgray'],
        sn.common['_color_list']['lightslategray'],
        sn.common['_color_list']['slategray'],
        sn.common['_color_list']['darkslategray'],
        sn.common['_color_list']['black']
    ];
    sn.common._color_scheme['pbi-default-darker'] = [
        '#666666',
        '#000000',
        '#015C55',
        '#1C2325',
        '#7F312F',
        '#796408',
        '#303637',
        '#456A76',
        '#7F4B33'
    ];
    sn.common._color_scheme['pbi-default-dark'] = [
        '#808080',
        '#1A1A1A',
        '#018A80',
        '#293537',
        '#BE4A47',
        '#B6960B',
        '#475052',
        '#689FB0',
        '#BF714D'
    ];
    sn.common._color_scheme['pbi-default-normal'] = [
        '#B3B3B3',
        '#333333',
        '#34C6BB',
        '#5F6B6D',
        '#FD817E',
        '#F5D33F',
        '#7F898A',
        '#A1DDEF',
        '#FEAB85'
    ];
    sn.common._color_scheme['pbi-default-light'] = [
        '#CCCCCC',
        '#666666',
        '#67D4CC',
        '#879092',
        '#FEA19E',
        '#F7DE6F',
        '#9FA6A7',
        '#B9E5F3',
        '#FEC0A3'
    ];
    sn.common._color_scheme['pbi-default-lighter'] = [
        '#e6e6e6',
        '#999999',
        '#99e3dd',
        '#afb5b6',
        '#fec0bf',
        '#fae99f',
        '#bfc4c5',
        '#d0eef7',
        '#ffd5c2'
    ];
    sn.common._color_scheme['pbi-default-bright'] = [
        //'#ffffff',  // white  // excluding white color because the default background is white.
     /*   '#01b8aa',
        '#4a828e',
        '#374649',
        '#5f6b6d',
        '#fd625e',
        '#8ad4eb',
        '#f2c80f',
        '#fe9666',
        '#000000'
        */
        '#01b8aa',
        '#374649',
        '#fd625e',
        '#f2c80f',
        '#5f6b6d',
        '#8ad4eb',
        '#fe9666',
        '#000000'

    ];
    sn.common._color_scheme['web-default'] = [
        '#F0F8FF', // aliceblue
        '#FAEBD7', // antiquewhite
        '#00FFFF', // aqua
        '#7FFFD4', // aquamarine
        '#F0FFFF', // azure
        '#F5F5DC', // beige
        '#FFE4C4', // bisque
        '#000000', // black
        '#FFEBCD', // blanchedalmond
        '#0000FF', // blue
        '#8A2BE2', // blueviolet
        '#A52A2A', // brown
        '#DEB887', // burlywood
        '#5F9EA0', // cadetblue
        '#7FFF00', // chartreuse
        '#D2691E', // chocolate
        '#FF7F50', // coral
        '#6495ED', // cornflowerblue
        '#FFF8DC', // cornsilk
        '#DC143C', // crimson
        '#00FFFF', // cyan
        '#00008B', // darkblue
        '#008B8B', // darkcyan
        '#B8860B', // darkgoldenrod
        '#A9A9A9', // darkgray
        '#006400', // darkgreen
        '#BDB76B', // darkkhaki
        '#8B008B', // darkmagenta
        '#556B2F', // darkolivegreen
        '#FF8C00', // darkorange
        '#9932CC', // darkorchid
        '#8B0000', // darkred
        '#E9967A', // darksalmon
        '#8FBC8F', // darkseagreen
        '#483D8B', // darkslateblue
        '#2F4F4F', // darkslategray
        '#00CED1', // darkturquoise
        '#9400D3', // darkviolet
        '#FF1493', // deeppink
        '#00BFFF', // deepskyblue
        '#696969', // dimgray
        '#1E90FF', // dodgerblue
        '#B22222', // firebrick
        '#FFFAF0', // floralwhite
        '#228B22', // forestgreen
        '#FF00FF', // fuchsia
        '#DCDCDC', // gainsboro
        '#F8F8FF', // ghostwhite
        '#FFD700', // gold
        '#DAA520', // goldenrod
        '#808080', // gray
        '#008000', // green
        '#ADFF2F', // greenyellow
        '#F0FFF0', // honeydew
        '#FF69B4', // hotpink
        '#CD5C5C', // indianred
        '#4B0082', // indigo
        '#FFFFF0', // ivory
        '#F0E68C', // khaki
        '#E6E6FA', // lavender
        '#FFF0F5', // lavenderblush
        '#7CFC00', // lawngreen
        '#FFFACD', // lemonchiffon
        '#ADD8E6', // lightblue
        '#F08080', // lightcoral
        '#E0FFFF', // lightcyan
        '#FAFAD2', // lightgoldenrodyellow
        '#90EE90', // lightgreen
        '#D3D3D3', // lightgrey
        '#FFB6C1', // lightpink
        '#FFA07A', // lightsalmon
        '#20B2AA', // lightseagreen
        '#87CEFA', // lightskyblue
        '#778899', // lightslategray
        '#B0C4DE', // lightsteelblue
        '#FFFFE0', // lightyellow
        '#00FF00', // lime
        '#32CD32', // limegreen
        '#FAF0E6', // linen
        '#FF00FF', // magenta
        '#800000', // maroon
        '#66CDAA', // mediumaquamarine
        '#0000CD', // mediumblue
        '#BA55D3', // mediumorchid
        '#9370DB', // mediumpurple
        '#3CB371', // mediumseagreen
        '#7B68EE', // mediumslateblue
        '#00FA9A', // mediumspringgreen
        '#48D1CC', // mediumturquoise
        '#C71585', // mediumvioletred
        '#191970', // midnightblue
        '#F5FFFA', // mintcream
        '#FFE4E1', // mistyrose
        '#FFE4B5', // moccasin
        '#FFDEAD', // navajowhite
        '#000080', // navy
        '#FDF5E6', // oldlace
        '#808000', // olive
        '#6B8E23', // olivedrab
        '#FFA500', // orange
        '#FF4500', // orangered
        '#DA70D6', // orchid
        '#EEE8AA', // palegoldenrod
        '#98FB98', // palegreen
        '#AFEEEE', // paleturquoise
        '#DB7093', // palevioletred
        '#FFEFD5', // papayawhip
        '#FFDAB9', // peachpuff
        '#CD853F', // peru
        '#FFC0CB', // pink
        '#DDA0DD', // plum
        '#B0E0E6', // powderblue
        '#800080', // purple
        '#FF0000', // red
        '#BC8F8F', // rosybrown
        '#4169E1', // royalblue
        '#8B4513', // saddlebrown
        '#FA8072', // salmon
        '#FAA460', // sandybrown
        '#2E8B57', // seagreen
        '#FFF5EE', // seashell
        '#A0522D', // sienna
        '#C0C0C0', // silver
        '#87CEEB', // skyblue
        '#6A5ACD', // slateblue
        '#708090', // slategray
        '#FFFAFA', // snow
        '#00FF7F', // springgreen
        '#4682B4', // steelblue
        '#D2B48C', // tan
        '#008080', // teal
        '#D8BFD8', // thistle
        '#FF6347', // tomato
        '#40E0D0', // turquoise
        '#EE82EE', // violet
        '#F5DEB3', // wheat
        '#FFFFFF', // white
        '#F5F5F5', // whitesmoke
        '#FFFF00', // yellow
        '#9ACD32' // yellowgreen
    ]
}
