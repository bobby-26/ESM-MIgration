<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaEntryRequirementsAndEligibility.aspx.cs"
    Inherits="PreSeaEntryRequirementsAndEligibility" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Course Eligibility</title>
   <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <title>
            <%=Application["softwarename"].ToString() %>
            -
            <%=Session["companyname"]%></title>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
       </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <table width="100%" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td align="left" style="width: 20%">
                <img id="img2" runat="server" alt="" src="<%$ PhoenixTheme:images/sims.png %>" />
            </td>
            <td style="font-size: medium; font-weight: bold; width: 60%;" align="center">
                SAMUNDRA INSTITUTE OF MARITIME STUDIES
                <br />
                <br />
                Enquiry /Query about courses
            </td>
            <td style="width: 20%">
                &nbsp;
            </td>
        </tr>
    </table>
    <hr />
    <table width="100%">
        <tr align="center">
            <td>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvEligibility" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        OnRowCreated="gvEligibility_RowCreated" Width="100%" CellPadding="3" ShowHeader="true"
                        EnableViewState="false">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle BackColor="#5588bb" ForeColor="#ffffff" Font-Bold="true" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblEligibility" runat="server" Text="Requirement"> </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblEligibility" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDELIGIBILITYTYPE") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <table align="center">
        <tr>
            <td>
                <asp:Button ID="btnContinue" runat="server" Text="Continue" OnClick="btnContinue_Click"
                    CssClass="cntxMenuSelect" />
                <asp:Button ID="btnHome" runat="server" Text="Home" OnClick="btnHome_Click" CssClass="cntxMenuSelect" />
            </td>
        </tr>
    </table>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility: hidden" />
    </form>
</body>
</html>
