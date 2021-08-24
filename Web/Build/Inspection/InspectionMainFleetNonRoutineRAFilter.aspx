<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMainFleetNonRoutineRAFilter.aspx.cs" Inherits="InspectionMainFleetNonRoutineRAFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Inspection RA Generic Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </div>
    <script type="text/javascript">
        function resize() {
            var obj = document.getElementById("ifMoreInfo");
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight)-95 + "px";

        }
        </script> 
</telerik:RadCodeBlock></head>
<body onload="resize()" onresize="resize()">
    <form id="frmSealUsageFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <%--<asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Non Routine RA Filter"></asp:Label>--%>
             <eluc:Title runat="server" ID="ucTitle" Text="Non Routine RA" ShowMenu="true" />
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuRAGenericFilter" runat="server" OnTabStripCommand="MenuRAGenericFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>    
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlRAGeneric">
        <ContentTemplate>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblRAType" runat="server" Text="Type"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlRAType" runat="server" CssClass="input" Width="240px" OnSelectedIndexChanged = "ddlRAType_Changed">
                        </asp:DropDownList>
                    </td>
                    <td>
                        <asp:Literal ID="lblRefNo" runat="server" Text="Ref Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Fleet runat="server" ID="ucTechFleet" Width="240px" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                    <td>
                        <asp:Literal ID="lblIntendedWork" runat="server" Text="Intended Work Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="ucDateIntendedWorkFrom" runat="server" CssClass="input" />
                        &nbsp;-&nbsp;
                        <eluc:Date ID="ucDateIntendedWorkTo" runat="server" CssClass="input" />
                    </td>                
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel" ></asp:Literal>
                    </td>
                    <td>                                            
                         <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" Width="240px" CssClass="input" AssignedVessels="true" />                                
                    </td>
                    <td>
                        <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="240px"
                            CssClass="input">                            
                        </asp:DropDownList>
                    </td>
                </tr>                              
            </table> 
             <iframe runat="server" id="ifMoreInfo" scrolling="yes" frameborder="0" style="width:100%" ></iframe>           
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
 
