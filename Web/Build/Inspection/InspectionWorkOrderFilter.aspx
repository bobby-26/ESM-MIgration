<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionWorkOrderFilter.aspx.cs" Inherits="InspectionWorkOrderFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselByCompany" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<%@ Register TagPrefix="eluc" TagName="addresstype" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Order Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionWorkOrderScheduleFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">            
            <eluc:Title runat="server" ID="Title1" Text="Work Order Filter" ShowMenu="false">
            </eluc:Title>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuWorkOrderScheduleFilter" runat="server" OnTabStripCommand="MenuWorkOrderScheduleFilter_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlWorkOrderScheduleFilter">
        <ContentTemplate>
            <div id="divFind">
                <table cellpadding="2" cellspacing="2" width="100%">
                    
                     <tr>
                        <td>
                            <asp:Literal ID="lblWorkOrderNo" runat="server" Text="WorkOrder No"></asp:Literal>
                        </td>
                        <td>
                             <asp:TextBox ID="txtWorkOrderNo" runat="server" CssClass="input"></asp:TextBox>
                        </td>
                        <td>
                           <asp:Literal ID="lblDepartment" runat="server" Text="Department"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Department ID="ucDepartment" runat="server" AppendDataBoundItems="true" AutoPostBack="true"
                                CssClass="input" OnTextChanged="selection_Changed" />
                        </td>
                    </tr> 
                     <tr>
                        <td>
                            <asp:Literal ID="lblCompletionDateFrom" runat="server" Text="Completion Date From"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDoneDateFrom" CssClass="input" Width="80px" runat="server" DatePicker="true" />
                        </td>  
                         <td>
                            <asp:Literal ID="lblCompletionDateTo" runat="server" Text="Completion Date To"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtDoneDateTo" CssClass="input" Width="80px" runat="server" DatePicker="true" />
                        </td>                      
                    </tr>
                     <tr>
                        <td>
                           <asp:Literal ID="lblWOStatus" runat="server" Text="WO Status"></asp:Literal>
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlAcceptance" runat="server" AppendDataBoundItems="true" CssClass="input"
                                AutoPostBack="true">
                                <asp:ListItem Text="--Select--" Value=""></asp:ListItem>
                                <asp:ListItem Text="Open" Value="1"></asp:ListItem>
                                <asp:ListItem Text="Closed" Value="2"></asp:ListItem>
                                <asp:ListItem Text="Cancelled" Value="3"></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>                          
                   
                   
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>

