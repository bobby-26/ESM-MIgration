<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaOffice.aspx.cs" Inherits="PreSeaOffice" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Address" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Bank" Src="~/UserControls/UserControlBank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Owner" Src="~/UserControls/UserControlAddressType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQuick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="RFQPreference" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Clinic" Src="~/UserControls/UserControlClinic.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaZone" Src="../UserControls/UserControlPreSeaZone.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Address</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatus" />
    <div class="subHeader" style="position: relative">
        <div id="div1" style="vertical-align: top">
            <asp:Label runat="server" ID="Label1" Font-Bold="true" Text="Address" Width="360px"></asp:Label>
        </div>
    </div>
    <div runat="server" id="divSubHeader" class="subHeader" style="position: relative">
        <div id="divHeading" style="vertical-align: top">
            <asp:Label runat="server" ID="lblCaption" Font-Bold="true" Text="" Width="360px"></asp:Label>
        </div>
    </div>
    <div class="navSelectHeader" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="MenuAddressMain" runat="server" OnTabStripCommand="AddressMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <div class="navSelect" style="top: 28px; right: 0px; position: absolute;">
        <eluc:TabStrip ID="MenuOfficeMain" runat="server" OnTabStripCommand="OfficeMain_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <asp:UpdatePanel ID="pnlAddressEntry" runat="server">
        <ContentTemplate>
            <eluc:Address runat="server" ID="ucAddress" EnableAOH="true"></eluc:Address>
            <br clear="all" />
            <table>
                <tr>
                    <td>
                        Board
                    </td>
                    <td>
                        <eluc:Quick ID="ucAcademicBoard" runat="server" CssClass="dropdown_mandatory" QuickTypeCode="101"
                           Width="350px" AppendDataBoundItems="true" />
                    </td>
                      <td>&nbsp;&nbsp;<asp:Literal ID='lblPreSeaZone' runat="server" Text="Zone" ></asp:Literal></td>
                    <td>
                        <eluc:PreSeaZone ID="ucPreSeaZone" runat="server" CssClass="input" AppendDataBoundItems="true" />
                    </td>
                </tr>               
                <tr>
                    <td>
                        Business Profile
                    </td>
                    <td>
                        <asp:TextBox runat="server" ID="txtBusinessProfile" CssClass="input" Width="350px"
                            Height="75px" TextMode="MultiLine"></asp:TextBox>
                    </td>
                    <td>
                        &nbsp;&nbsp; <b>Grade</b>
                        <asp:Label ID="lblAddressGrade" runat="server" Visible="false"></asp:Label>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlAddressGrade" runat="server" CssClass="input">
                            <asp:ListItem Value="DUMMY" Text="--Select--" />
                            <asp:ListItem Value="A" Text="A" />
                            <asp:ListItem Value="B" Text="B" />
                            <asp:ListItem Value="C" Text="C" />
                            <asp:ListItem Value="D" Text="D" />
                        </asp:DropDownList>
                    </td>
                </tr>
            </table>
            <ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
                HeaderSelectedCssClass="accordionHeaderSelected" ContentCssClass="accordionContent"
                FadeTransitions="false" FramesPerSecond="40" TransitionDuration="50" AutoSize="None"
                RequireOpenedPane="false" SuppressHeaderPostbacks="true">
                <Panes>
                    <ajaxToolkit:AccordionPane ID="AccordionPane4" runat="server">
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
            <br />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
