<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOfficeFilter.aspx.cs"
    Inherits="RegistersOfficeFilter" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%--<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>--%>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Filter</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">

            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuOfficeFilterMain" runat="server" OnTabStripCommand="OfficeFilterMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>

            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCode" runat="server" Text="Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtcode" MaxLength="200" CssClass="input" Width="240px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblName" runat="server" Text="Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtName" MaxLength="100" CssClass="input" Width="240px"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPhone1" runat="server" Text="Phone 1"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPhone1" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtCity" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Country runat="server" ID="ucCountry" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblPostalCode" runat="server" Text="Postal Code"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtPostalCode" MaxLength="50" CssClass="input"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEMail" runat="server" Text="E-Mail"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEMail" CssClass="input" MaxLength="100"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblQAGrading" runat="server" Text="QA Grading"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Quick ID="ucQAGrading" runat="server" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblBusinessProfile" runat="server" Text="Business Profile"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <telerik:RadTextBox runat="server" ID="txtBusinessProfile" CssClass="input" TextMode="MultiLine" Width="300px"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <font color="blue">
                                <asp:Literal ID="lblNoteTosearchmultiplecriteriapleaseseparatethembycomma" runat="server" Text="Note: To search multiple criteria, please separate them by ',' (comma)"></asp:Literal>
                            </font>
                        </td>
                    </tr>
                </table>
            </div>

            <%--<ajaxToolkit:Accordion ID="MyAccordion" runat="server" SelectedIndex="-1" HeaderCssClass="accordionHeader"
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
                    </ajaxToolkit:AccordionPane>--%>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="MyAccordion" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Department" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="Department"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblAddressDepartment" Height="90%" Columns="7"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>

            <%-- <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <a href="" class="accordionLink">Address Type</a>
                </Header>
                <Content>
                    <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>--%>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="AccordionPane1" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Address Type" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="Address Type"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblAddressType" Height="90%" Columns="5"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>

            <%--<ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="" class="accordionLink">Product/Services</a>
                </Header>
                <Content>
                    <p>
                        <asp:Literal ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></asp:Literal>
                    </p>
                    <asp:CheckBoxList runat="server" ID="cblProductType" Height="26px" RepeatColumns="7"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>--%>
            <telerik:RadPanelBar RenderMode="Lightweight" ID="AccordionPane2" runat="server" Width="100%">
                <Items>
                    <telerik:RadPanelItem Text="Product" Width="100%">
                        <HeaderTemplate>
                            <telerik:RadLabel ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></telerik:RadLabel>
                        </HeaderTemplate>
                        <ContentTemplate>
                            <telerik:RadCheckBoxList RenderMode="Lightweight" runat="server" ID="cblProductType" Height="96%" Columns="7"
                                Direction="Vertical" Layout="Flow" AutoPostBack="false">
                            </telerik:RadCheckBoxList>
                        </ContentTemplate>
                    </telerik:RadPanelItem>
                </Items>
            </telerik:RadPanelBar>


        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
