<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaOfficeFilter.aspx.cs"
    Inherits="PreSeaOfficeFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="Address"></asp:Label>
        </div>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            Code
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtcode" MaxLength="200" CssClass="input" Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            Name
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" MaxLength="100" CssClass="input" Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            Phone 1
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPhone1" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            City
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCity" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            Country
                        </td>
                        <td>
                            <eluc:Country runat="server" ID="ucCountry" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            Postal Code
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostalCode" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            E-Mail
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEMail" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            Status
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            QA Grading
                        </td>
                        <td>
                            <eluc:Quick ID="ucQAGrading" runat="server" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            Business Profile
                        </td>
                        <td colspan="5">
                            <asp:TextBox runat="server" ID="txtBusinessProfile" CssClass="input" TextMode="MultiLine" Width="300px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <font color="blue"> Note: To search multiple criteria, please separate them by ',' (comma) </font>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
        HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
        FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
        RequireOpenedPane="false" SuppressHeaderPostbacks="true">
        <Panes>
            <ajaxToolkit:AccordionPane>
                <Header>
                    <a href="" class="accordionLink">Department</a>
                </Header>
                <Content>
                    <asp:CheckBoxList runat="server" ID="cblAddressDepartment" Height="26px" RepeatColumns="7"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <a href="" class="accordionLink">Address Type</a></Header>
                <Content>
                    <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>
    </form>
</body>
</html>
