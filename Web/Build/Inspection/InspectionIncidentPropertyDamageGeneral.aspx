<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionIncidentPropertyDamageGeneral.aspx.cs" Inherits="InspectionIncidentPropertyDamageGeneral" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Incident Property Damage General</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="IncidentDamagelink" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmIncidentDamageGeneral" runat="server">
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <eluc:Status runat="server" ID="ucStatus" />
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader" style="position: relative">
            <div id="divHeading">
                <eluc:Title runat="server" ID="ucTitle" Text="Property Damage" ShowMenu="false">
                </eluc:Title>
            </div>
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="MenuIncidentDamageGeneral" runat="server" OnTabStripCommand="IncidentDamageGeneral_TabStripCommand">
                </eluc:TabStrip>
        </div>
        <%--<div class="subHeader" style="position: relative;">
            <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                <eluc:TabStrip ID="MenuIncidentDamageGeneral" runat="server" OnTabStripCommand="IncidentDamageGeneral_TabStripCommand">
                </eluc:TabStrip>
            </span>
        </div>--%>
        <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
        </ajaxToolkit:ToolkitScriptManager>
        <asp:UpdatePanel runat="server" ID="pnlIncidentDamageGeneral">
            <ContentTemplate>
                <div id="divFind" style="position: relative; z-index: 2">
                    <table id="tblConfigureIncidentDamage" width="100%">
                        <tr>
                            <td style="width: 15%">
                                <asp:Label ID="lblTypeofDamageHeader" runat="server" >Type of Damage</asp:Label>
                            </td>
                            <td style="width: 35%">
                                <%--<eluc:Hard ID="ucTypeOfPropertyDamage" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory" AutoPostBack="true" OnTextChangedEvent="ucTypeOfPropertyDamage_Changed"
                                    HardList="<%# PhoenixRegistersHard.ListHard(1,203) %>" HardTypeCode="203" />--%>  
                                <asp:DropDownList ID="ddlPropertyDamage" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlPropertyDamage_Changed" 
                                    DataTextField="FLDNAME" DataValueField="FLDHAZARDID" Width="150px"></asp:DropDownList>
                            </td>
                            <td style="width: 15%">
                                <asp:Label ID="lblDamagedPropertyHeader" runat="server">Damaged Property</asp:Label>
                            </td>
                            <td style="width: 35%">
                                <eluc:Quick runat="server" ID="ucProperty" AppendDataBoundItems="true" CssClass="input_mandatory"
                                    Width="200px" QuickTypeCode="52" />
                                <span id="spnPickListComponent">
                                    <asp:TextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                       MaxLength="20" Width="60px"></asp:TextBox>
                                    <asp:TextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Width="210px"></asp:TextBox>
                                    <img id="imgComponent" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                        style="cursor: pointer; vertical-align: top" />
                                    <asp:TextBox ID="txtComponentId" runat="server" CssClass="hidden" Width="10px"></asp:TextBox>
                                </span>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblsubtypeofDamage" runat="server" Text="Subtype of Damage"></asp:Literal>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSubPropertyDamage" Width="200px" AppendDataBoundItems="true" runat="server" 
                                    CssClass="input_mandatory" DataTextField="FLDNAME" DataValueField="FLDSUBHAZARDID"></asp:DropDownList>
                            </td>
                            <td>
                                <asp:Literal ID="lblEstimatedCostinUSD" runat="server" Text="Estimated cost in USD"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Number ID="txtEstimatedCost" runat="server" CssClass="input" MaxLength="7"
                                    DecimalPlace="2" IsPositive="true" Width="120px"></eluc:Number>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblConsequenceCategory" runat="server" Text="Consequence Category"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCategory" runat="server" Width="30px" Enabled="false" ReadOnly="true" CssClass="input"></asp:TextBox>
                            </td>
                        </tr>
                        <%--<tr>
                                <td>
                                    Time of Incident
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTimeOfIncident" runat="server" CssClass="input_mandatory" Width="50px" />  
                                    <ajaxToolkit:MaskedEditExtender ID="txtTimeMask" runat="server" TargetControlID="txtTimeOfIncident"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="false" UserTimeFormat="TwentyFourHour" ClearMaskOnLostFocus="false" ClearTextOnInvalid="true"/>
                                </td>
                            </tr>--%>
                        <%--<tr>
                            <td>
                                Component
                            </td>
                            <td>
                                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                                    <asp:GridView ID="gvIncidentDamage" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                        Width="100%" CellPadding="3" OnRowCommand="gvIncidentDamage_RowCommand" OnRowDataBound="gvIncidentDamage_ItemDataBound"
                                        OnRowDeleting="gvIncidentDamage_RowDeleting" OnSorting="gvIncidentDamage_Sorting"
                                        AllowSorting="true" OnRowEditing="gvIncidentDamage_RowEditing" ShowFooter="true"
                                        ShowHeader="true" EnableViewState="false" DataKeyNames="FLDINSPECTIONINCIDENTDAMAGEID">
                                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                                        <RowStyle Height="10px" />
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblComponentHeader" runat="server">Component Name&nbsp; </asp:Label>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblIncidentDamageId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTDAMAGEID") %>'></asp:Label>
                                                    <asp:Label ID="lblIncidentId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONINCIDENTID") %>'></asp:Label>
                                                    <asp:LinkButton ID="lnkComponentName" runat="server" CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>"
                                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPONENTNAME") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <EditItemTemplate>
                                                <span id="spnPickListComponentEdit">
                                                        <asp:TextBox ID="txtComponentCodeEdit" runat="server" CssClass="input_mandatory" MaxLength="20"
                                                            Enabled="false" Width="60px"></asp:TextBox>
                                                        <asp:TextBox ID="txtComponentNameEdit" runat="server" CssClass="input_mandatory" MaxLength="20"
                                                            Enabled="false" Width="210px"></asp:TextBox>
                                                        <img id="imgComponentEdit" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                                            style="cursor: pointer; vertical-align: top" />
                                                        <asp:TextBox ID="txtComponentIdEdit" runat="server" CssClass="hidden" Width="10px"></asp:TextBox>
                                                    </span>
                                                </EditItemTemplate>
                                                <FooterTemplate>
                                                    <span id="spnPickListComponentAdd">
                                                        <asp:TextBox ID="txtComponentCodeAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                                            Enabled="false" Width="60px"></asp:TextBox>
                                                        <asp:TextBox ID="txtComponentNameAdd" runat="server" CssClass="input_mandatory" MaxLength="20"
                                                            Enabled="false" Width="210px"></asp:TextBox>
                                                        <img id="imgComponentAdd" runat="server" src="<%$ PhoenixTheme:images/picklist.png %>"
                                                            style="cursor: pointer; vertical-align: top" />
                                                        <asp:TextBox ID="txtComponentIdAdd" runat="server" CssClass="hidden" Width="10px"></asp:TextBox>
                                                    </span>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                                <HeaderTemplate>
                                                    <asp:Label ID="lblActionHeader" runat="server">
                                                     Action
                                                    </asp:Label>
                                                </HeaderTemplate>
                                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                                <ItemTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                        CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                                                        ToolTip="Edit Incident"></asp:ImageButton>
                                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                                        ToolTip="Delete Incident"></asp:ImageButton>
                                                </ItemTemplate>
                                                 <FooterStyle HorizontalAlign="Center" />
                                                <FooterTemplate>
                                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                                        CommandName="ADD" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAdd"
                                                        ToolTip="Add New"></asp:ImageButton>
                                                </FooterTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </td>
                        </tr>--%>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </div>
    </form>
</body>
</html>
