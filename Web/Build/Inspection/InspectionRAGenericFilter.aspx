<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAGenericFilter.aspx.cs"
    Inherits="InspectionRAGenericFilter" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselByOwner.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Fleet" Src="~/UserControls/UserControlFleet.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Inspection RA Generic Filter</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmSealUsageFilter" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Generic RA Filter"></asp:Label>
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
        <br />
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <asp:Literal ID="lblRefNo" runat="server" Text="Ref Number"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtRefNo" runat="server" CssClass="input" Width="150px"></asp:TextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblActivityConditions" runat="server" Text="Activity Conditions"></asp:Literal>
                    </td>
                    <td>
                        <asp:TextBox ID="txtActivityConditions" runat="server" CssClass="input" Width="205px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>                                            
                         <eluc:Vessel runat="server" ID="ucVessel" AppendDataBoundItems="true" CssClass="input" AssignedVessels="true" />                                
                    </td>
                    <td>
                        <asp:Literal ID="lblPreparedDate" runat="server" Text="Prepared Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="ucDatePreparedFrom" runat="server" CssClass="input" />
                        &nbsp;-&nbsp;
                        <eluc:Date ID="ucDatePreparedTo" runat="server" CssClass="input" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true" Width="150px"
                            CssClass="input">                            
                        </asp:DropDownList>
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
                         <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                      </td>
                      <td>
                          <eluc:Fleet runat="server" ID="ucTechFleet" Width="150px" CssClass="input" AppendDataBoundItems="true" />
                     </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
