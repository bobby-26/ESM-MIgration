///////// ELEARNING1 Jun 29, 2017 ///////////////

function toggleFullscreen(elem, divNam) {
  elem = elem || document.documentElement;
  if (!document.fullscreenElement && !document.mozFullScreenElement &&
    !document.webkitFullscreenElement && !document.msFullscreenElement) {
    $(elem).toggleClass('btnCollapse').toggleClass('btnExpand');
    //elem.display = "block";
    if (elem.requestFullscreen) {
      elem.requestFullscreen();
    } else if (elem.msRequestFullscreen) {
      elem.msRequestFullscreen();
    } else if (elem.mozRequestFullScreen) {
      elem.mozRequestFullScreen();
    } else if (elem.webkitRequestFullscreen) {
      elem.webkitRequestFullscreen(Element.ALLOW_KEYBOARD_INPUT);
    }


    $(elem).find(divNam).removeClass("wrap3rDivNS");
    $(elem).find(divNam).addClass("wrap3rDivFS");

  } else {
    $(elem).toggleClass('btnExpand').toggleClass('btnCollapse');
    if (document.exitFullscreen) {
      document.exitFullscreen();
    } else if (document.msExitFullscreen) {
      document.msExitFullscreen();
    } else if (document.mozCancelFullScreen) {
      document.mozCancelFullScreen();
    } else if (document.webkitExitFullscreen) {
      document.webkitExitFullscreen();
    }

    $(elem).find(divNam).removeClass("wrap3rDivFS");
    $(elem).find(divNam).addClass("wrap3rDivNS");

  }
}


  function showGraph(option, targetGraphDiv) {
    // configure for module loader
    require.config({
      paths: {
        echarts: '../js/echarts'
      }
    });

    require(
      [
        'echarts',
        'echarts/chart/bar'
      ],
      function (ec) {
        var myChart = ec.init(document.getElementById(targetGraphDiv));

        myChart.setOption(option);
      }
    );
  }

  //hearder code
  var svgLnk = "http://www.w3.org/2000/svg";
  var otisHdr = "<div class='otisTitleBar' onclick='toggleFullscreen(lwr3rDiv, otisIframe)'><div id='otisDivLbl' class='titleLabel'>OTIS INFORMATION</div><div id='chartDivBtn'><span class='btnExpand'><svg width='16' height='16' viewBox='0 0 1792 1792' xmlns ='" + svgLnk + "'><path d='M1650 288q0 13-10 23l-332 332 144 144q19 19 19 45t-19 45-45 19h-448q-26 0-45-19t-19-45v-448q0-26 19-45t45-19 45 19l144 144 332-332q10-10 23-10t23 10l114 114q10 10 10 23z'/><path d='M896 960v448q0 26-19 45t-45 19-45-19l-144-144-332 332q-10 10-23 10t-23-10l-114-114q-10-10-10-23t10-23l332-332-144-144q-19-19-19-45t19-45 45-19h448q26 0 45 19t19 45z'/></svg></span></div></div>";


  //icon response code.

  $('.icoBlk').click(function () {
    var tab_id = $(this).attr('data-tab');
    var pgNam = $(this).attr('data-PG') + ".html";

    $('.icoBlk').removeClass('icoActive');
    $('.dataMenuList').removeClass('currentList');

    $(this).addClass('icoActive');
    var toDiv = "#" + tab_id;
    $(toDiv).addClass('currentList');

    if (pgNam != ".html")
      loadMeasuresPg(toDiv, pgNam)
  });


  ///////////////////////////// load html ///////////////////

  function loadMeasuresPg(toDiv, pgNam) {

    $(toDiv).load(pgNam, function (responseTxt, statusTxt, xhr) {
      if (statusTxt == "success")
      //alert("External content loaded successfully!");
        if (statusTxt == "error")
        alert("Error: " + xhr.status + ": " + xhr.statusText);
    });
  }

//////////////////////////// height width calc functions //////////////////

function calcH(divVal){
  var heyt = $(this).height()-365;
  $(divVal).height(heyt+"px");
}

function calcW(divVal){
  var wideval = $(this).width()-365;
  $(divVal).width(wideval+"px");
}

//// color palette for charts - ['#c23531','#2f4554', '#61a0a8', '#d48265', '#91c7ae','#749f83',  '#ca8622', '#bda29a','#6e7074', '#546570', '#c4ccd3']
////  ["#ff7f50", "#87cefa", "#da70d6",  "#32cd32", "#6495ed", "#ff69b4", "#ba55d3", "#cd5c5c", "#ffa500", "#40e0d0", "#1e90ff", "#ff6347", "#7b68ee", "#00fa9a", "#ffd700", "#6699FF", "#ff6666",  "#3cb371", "#b8860b", "#30e0e0" ]

//////////////   popup div call ////////

function popupGenCall(pgInfo){

  $("#gMain").html(pgInfo);

  $("#popupDiv").dialog({
    modal: true,
    width: 600,
    height: 400,
    dialogClass: 'ui-widget-shadow'
  });
}

function callDia(titleText, subText, seriesName, dataSName, dataValues, colourList) {
  $("#popupDiv").dialog({
    modal: true,
    width: 1000,
    height: 600,
    dialogClass: 'ui-widget-shadow'
  });

  var graph01 = {
    title: {
      x: 'center',
      text: titleText,
      subtext: subText
    },
    tooltip: {
      trigger: 'item'
    },
    toolbox: {
      show: true,
      feature: {
        restore: {
          show: true,
          title: 'Refresh'
        },
        saveAsImage: {
          show: true,
          title: 'Save'
        }
      }
    },
    calculable: true,
    grid: {
      borderWidth: 1,
      x: 40,
      x2: 40,
      y: 50,
      y2: 130
    },
    xAxis: [
      {
        type: 'category',
        show: true,
        axisLabel: {rotate: 90},
        data: dataSName
      }
    ],
    yAxis: [
      {
        type: 'value',
        show: true
      }
    ],
    series: [
      {
        name: seriesName,
        type: 'bar',
        itemStyle: {
          normal: {
            color: function(params) {
              var colorList = colourList;
              return colorList[params.dataIndex]
            },
            label: {
                show: false,
                position: 'top',
                formatter: '{c}'
            }
          }
        },
        data: dataValues
      }
    ]
  };


  showGraph(graph01, 'popupDiv');
}

$('#popupDiv').on('dialogclose', function(event) {
  $(this).html("<div id='gMain'> 2 </div>");
});

function resizeDashboard() {
    if ($("#pnl").attr("style") != null)
        $("#pnl").removeAttr("style");
    else
        $("#pnl").attr("style", "position:absolute;top:0px;height:100%;width:100%;background-color:white;z-index:99")
}
