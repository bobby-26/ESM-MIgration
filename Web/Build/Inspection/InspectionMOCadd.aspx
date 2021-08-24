<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionMOCadd.aspx.cs"
    Inherits="InspectionMOCadd" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Add MOC</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlMOC">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" />
            <telerik:RadFormDecorator ID="rfdinstruction" RenderMode="LightWeight" runat="server"
                DecoratedControls="All" EnableRoundedCorners="true" DecorationZoneID="divFind">
            </telerik:RadFormDecorator>
            <eluc:TabStrip ID="MenuMOC" runat="server" OnTabStripCommand="MOC_TabStripCommand">
            </eluc:TabStrip>
            <div id="divFind" style="position: relative; z-index: 2">
                <table id="tblmoc" width="100%">
                    <tr>
                        <td style="width: 15%">
                            <asp:Literal ID="lblOfficeShip" runat="server" Text="Office/Ship"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Vessel ID="ddlvessel" runat="server" CssClass="input" AppendDataBoundItems="true"
                                OnTextChangedEvent="ddlvessel_textchanged" AssignedVessels="true" AutoPostBack="true"
                                Width="240px" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCompany" runat="server" Text="Company">
                            </telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Company ID="ucCompany" runat="server" Enabled="false" AppendDataBoundItems="true"
                                CssClass="input" Width="240px" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Literal ID="lblProposer" runat="server" Text="Proposer (Name/Rank)"></asp:Literal>
                        </td>
                        <td style="width: 35%">
                            <span id="spnPersonInCharge" runat="server">
                                <asp:TextBox ID="txtCrewName" runat="server" CssClass="input_mandatory" Enabled="false"
                                    MaxLength="50" Width="50%"></asp:TextBox>
                                <asp:TextBox ID="txtCrewRank" runat="server" CssClass="input_mandatory" Enabled="false"
                                    MaxLength="50" Width="20%"></asp:TextBox>
                                <%--<img runat="server" id="imgPersonInCharge" style="cursor: pointer; vertical-align: top"
                                    src="<%$ PhoenixTheme:images/picklist.png %>" />--%>
                                <asp:LinkButton runat="server" ID="imgPersonInCharge"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:TextBox ID="txtCrewId" runat="server" CssClass="input" MaxLength="20" Width="10px"></asp:TextBox>
                            </span><span id="spnPersonInChargeOffice" runat="server">
                                <asp:TextBox ID="txtOfficePersonName" runat="server" CssClass="input_mandatory" Enabled="false"
                                    MaxLength="50" Width="50%"></asp:TextBox>
                                <asp:TextBox ID="txtOfficePersonDesignation" runat="server" CssClass="input_mandatory"
                                    Enabled="false" MaxLength="50" Width="23%"></asp:TextBox>
                                <%--<asp:ImageButton runat="server" ID="imgPersonInChargeOffice" Style="cursor: pointer;
                                    vertical-align: top" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" />--%>
                                <asp:LinkButton runat="server" ID="imgPersonInChargeOffice"><span class="icon"><i class="fas fas fa-tasks"></i></span>
                                </asp:LinkButton>
                                <asp:TextBox runat="server" ID="txtPersonInChargeOfficeId" CssClass="input" Width="0px"
                                    MaxLength="20"></asp:TextBox>
                                <asp:TextBox runat="server" ID="txtPersonInChargeOfficeEmail" CssClass="input" Width="0px"
                                    MaxLength="20"></asp:TextBox>
                            </span>
                            <asp:Label ID="lblDtkey" runat="server" Visible="false"></asp:Label>
                        </td>
                        <td style="width: 15%">
                            <asp:Literal ID="lblstatus" runat="server" Text="Status"></asp:Literal>
                        </td>
                        <td class="style1">
                            <telerik:RadDropDownList ID="ddlstatus" runat="server">
                                <Items>
                                    <telerik:DropDownListItem Text="Draft" Value="1" />
                                </Items>
                            </telerik:RadDropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Literal ID="lblMOCDate" runat="server" Text="Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="ucMOCDate" runat="server" CssClass="input_mandatory" DatePicker="true"
                                AutoPostBack="true" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 15%">
                            <asp:Literal ID="lbltitle" runat="server" Text="Title"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtmoctitle" runat="server" CssClass="input" MaxLength="50" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
