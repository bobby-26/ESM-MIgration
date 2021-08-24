<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardHSEQAPlanAdd.aspx.cs" Inherits="Dashboard_DashboardHSEQAPlanAdd" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HSEQA Planner</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
       
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server"
            EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxLoadingPanel runat="server" ID="RadAjaxLoadingPanel1" />
        <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server" DefaultLoadingPanelID="RadAjaxLoadingPanel1">
            <AjaxSettings>
                <telerik:AjaxSetting AjaxControlID="attachments">
                    <UpdatedControls>
                        <telerik:AjaxUpdatedControl ControlID="reportform" />
                    </UpdatedControls>
                </telerik:AjaxSetting>
            </AjaxSettings>
        </telerik:RadAjaxManager>
        <telerik:RadAjaxPanel runat="server" Width="100%" >
        <div style="margin-left: 0px">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <eluc:TabStrip ID="Tabstriphseqaschedulereportmenu" runat="server" OnTabStripCommand="Tabstriphseqaschedulereportmenu_TabStripCommand"
                TabStrip="true" />
            <br />
            <table style="margin-left: 20px; text-wrap: normal;" id="reportform">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="LI" />
                    </td>
                    <th>&nbsp &nbsp &nbsp</th>
                    <td>
                        <telerik:RadMultiColumnComboBox RenderMode="Lightweight" ID="radcobLi" runat="server" Width="290px" Height="150px" NoWrap="true"
                            DataTextField="FLDLINAME" DataValueField="FLDLIID" DropDownWidth="300px"
                            Placeholder="Type to Select the LI" Filter="Contains" FilterFields="FLDLICODE, FLDLINAME" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="radcobLi_SelectedIndexChanged">
                            <NoDataTemplate>
                                 <p style="text-align:center"><b>No Records Found</b>
                                </p> 
                            </NoDataTemplate>
                            <ColumnsCollection>
                                <telerik:MultiColumnComboBoxColumn Field="FLDLICODE" Title="Code" Width="70px" />
                                <telerik:MultiColumnComboBoxColumn Field="FLDLINAME" Title="Name" Width="200px" />
                            </ColumnsCollection>
                        </telerik:RadMultiColumnComboBox>
                    </td>
                    <td>&nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Interval"  Visible="false"/>
                    </td>
                    <th>&nbsp &nbsp &nbsp</th>
                    <td>
                        <telerik:RadLabel runat="server" ID="radlblinterval" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Planned Date" />
                    </td>
                    <th>&nbsp &nbsp &nbsp
                    </th>
                    <td>
                        <eluc:Date runat="server" ID="radplanneddate" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel runat="server" Text="Lastdone Date" Visible="false" />
                    </td>
                    <th>&nbsp &nbsp &nbsp
                    </th>
                    <td colspan="5">
                        <telerik:RadLabel runat="server" Text="" />
                    </td>
                </tr>
                <tr>
                    <td colspan="7">
                        <br />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" Text="Action by" />
                    </td>
                    <td>
                        &nbsp &nbsp &nbsp
                    </td>
                    <td>
                        <telerik:RadComboBox runat="server"  AllowCustomText="true" ID="radcbactionby" CssClass="input_mandatory"/>
                    </td>
                </tr>
            </table>
        </div>
            </telerik:RadAjaxPanel>
    </form>
</body>
</html>
