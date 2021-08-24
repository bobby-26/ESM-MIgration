<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewEmployeeExperienceGanttChart.aspx.cs" Inherits="CrewEmployeeExperienceGanttChart" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Crew Employee Experience Visual</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script lang="javascript" type="text/javascript">

            <%--function onEdit(e) {
                  if ($(e.container).hasClass('k-popup-edit-form')) {
                      e.preventDefault();
                  }
              }

              $(document).ready(function() {
 
 
            $(".rgtTask").on("dblclick", function(e) {
                 
                var gantt = $find("<%=RadGantt1.ClientID%>")._widget;
 
                e.stopPropagation();
                var element = $(e.target);
                if (!element.is(".rgtTask")) {
                    element = element.closest(".rgtTask");
                }
                 
                if (element.length==1) {
                    task = gantt.dataItem($(".rgtTreelistContent").find("[data-uid="+element.data("uid")+"]"));
                    if (task != null) {

                    }
                }
 
            });
        });
               --%>

            <%--script for custom tooltip--%> 
            <%-- var $ = $ || $telerik.$;
    function OnClientDataBound(sender, args) {
        var gantt = sender;
 
        $(sender.get_element()).find(".rgtTask").on("mouseover", function (e) {
            e.stopPropagation();
            var $element = $(e.target);
            if (!$element.is(".rgtTask")) {
                $element = $element.parents(".rgtTask").first();
            }
 
            var task = getTaskByUid(gantt, $element.attr("data-uid"));
 
            var tooltipManager = $find("<%= ToolTipMngr1.ClientID %>");
            var tooltip = tooltipManager.getToolTipByElement(task.get_element());
 
            //Create a tooltip if no tooltip exists for such element
            if (!tooltip) {
                tooltip = tooltipManager.createToolTip(task.get_element());
 
                //use task
                var percentage = task.get_percentComplete() * 100;
                $get("startTime").innerHTML = task.get_start().format("MM/dd/yyyy HH:mm");
                $get("endTime").innerHTML = task.get_end().format("MM/dd/yyyy HH:mm");
                $get("percentageDiv").innerHTML = (100 - percentage) + "%";
                tooltip.set_text($get("contentContainer").innerHTML);
 
                setTimeout(function () {
                    tooltip.show();
                }, 100);
            }                    
        });
    }
 
    function getTaskByUid(gantt, uid) {
        var tasks = gantt.get_allTasks();
 
        for (var i = 0; i < tasks.length; i++) {
            if (tasks[i]._uid === uid) {
                return tasks[i];
                break;
            }
        }
 
        return null;
    }--%>
        </script>

        <style>
            html .RadGantt .rgtActions .radButton {
                display: none;
            }
        </style>


        <%--<style type="text/css">   
    HTML   
    {   
        overflow-x: hidden; 
        overflow-y: hidden;  
    }   
</style>  --%>
    </telerik:RadCodeBlock>
    <%--<link rel="stylesheet" type="text/css" href="styles.css" />--%>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
            <Scripts>
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.Core.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQuery.js" />
                <asp:ScriptReference Assembly="Telerik.Web.UI" Name="Telerik.Web.UI.Common.jQueryInclude.js" />
            </Scripts>
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" ShowChooser="false" />
        <telerik:RadToolTipManager runat="server" ID="ToolTipMngr1">
        </telerik:RadToolTipManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <telerik:RadGantt RenderMode="Lightweight" runat="server" ID="RadGantt1" Height="620px" ListWidth="250px" AllowColumnResize="true"
            SelectedView="YearView" Skin="Silk" AutoGenerateColumns="false" ShowTooltip="false">
            <YearView UserSelectable="true" />

            <Columns>
                <telerik:GanttBoundColumn DataField="ID" Width="90px" Visible="false"></telerik:GanttBoundColumn>
                <telerik:GanttBoundColumn DataField="Title" DataType="String" Width="120px" HeaderText="Name"></telerik:GanttBoundColumn>
                <telerik:GanttBoundColumn DataField="Start" DataType="DateTime" DataFormatString="dd/MM/yy" Width="40px" HeaderText="Start"></telerik:GanttBoundColumn>
                <telerik:GanttBoundColumn DataField="End" DataType="DateTime" DataFormatString="dd/MM/yy" Width="40px" HeaderText="Last"></telerik:GanttBoundColumn>
            </Columns>

            <DataBindings>
                <TasksDataBindings
                    IdField="FLDID" TitleField="FLDVESSELNAME"
                    StartField="FLDSTART" EndField="FLDEND" />

                <DependenciesDataBindings IdField="" PredecessorIdField=""
                    SuccessorIdField="" TypeField="" />
            </DataBindings>

        </telerik:RadGantt>
    </form>
</body>
</html>

