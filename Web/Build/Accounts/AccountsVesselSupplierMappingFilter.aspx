<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsVesselSupplierMappingFilter.aspx.cs" Inherits="AccountsVesselSupplierMappingFilter" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
    <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

</telerik:RadCodeBlock></head>
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
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts ="false" >
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlAddressEntry">
        <ContentTemplate>
            <div id="divFind">
                <table width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblCode" runat="server" Text="Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtcode" MaxLength="200" CssClass="input" Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblName" runat="server" Text="Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtName" MaxLength="100" CssClass="input" Width="240px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPhone1" runat="server" Text="Phone 1"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPhone1" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblCity" runat="server" Text="City"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtCity" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblCountry" runat="server" Text="Country"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Country runat="server" ID="ucCountry" AppendDataBoundItems="true" CssClass="input" />
                        </td>
                        <td>
                            <asp:Literal ID="lblPostalCode" runat="server" Text="Postal Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtPostalCode" MaxLength="50" CssClass="input"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblEMail" runat="server" Text="E-Mail"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtEMail" CssClass="input" MaxLength="100"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Hard ID="ucStatus" runat="server" CssClass="input" AppendDataBoundItems="true" />
                        </td>
                        <td>
                            <asp:Literal ID="lblQAGrading" runat="server" Text="QA Grading"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Quick ID="ucQAGrading" runat="server" AppendDataBoundItems="true" CssClass="input" />
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
            <ajaxToolkit:AccordionPane ID="AccordionPane1" runat="server">
                <Header>
                    <a href="" class="accordionLink">Address Type</a></Header>
                <Content>
                    <asp:CheckBoxList runat="server" ID="cblAddressType" Height="26px" RepeatColumns="5"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>
            <ajaxToolkit:AccordionPane ID="AccordionPane2" runat="server">
                <Header>
                    <a href="" class="accordionLink">Product/Services</a></Header>
                <Content>
                    <p>
                        <asp:Literal ID="lblSelecttheProductServicesyouoffer" runat="server" Text="Select the Product/Services you offer"></asp:Literal></p>
                    <asp:CheckBoxList runat="server" ID="cblProductType" Height="26px" RepeatColumns="7"
                        RepeatDirection="Horizontal" RepeatLayout="Table">
                    </asp:CheckBoxList>
                </Content>
            </ajaxToolkit:AccordionPane>
        </Panes>
    </ajaxToolkit:Accordion>
    </form>
</body>
</html>

