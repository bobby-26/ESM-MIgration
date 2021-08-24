<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersVesselMedicalTestMap.aspx.cs"
    Inherits="RegistersVesselMedicalTestMap" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Course" Src="~/UserControls/UserControlCourse.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Documents Required</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="certificateslink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersDocumentsRequired" runat="server" submitdisabledcontrols="true">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDocumentsRequiredEntry">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%;
                position: absolute;">
                <div class="subHeader">
                    <div id="divHeading">
                        <asp:Literal ID="lblDocumentsRequiredCourse" runat="server" Text="Medical Test"></asp:Literal>
                    </div>
                </div>
                <div style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFlag" runat="server" OnTabStripCommand="Flag_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <div id="divFind">
                    <table id="tblConfigureDocumentsRequired" width="80%">
                        <tr>
                            <td style="width: 10%">
                                <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td align="left">
                                <asp:TextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" 
                                    ReadOnly="true" Text=""></asp:TextBox>
                            </td>
                            <td>
                                <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                            </td>
                            <td>
                                <eluc:VesselType runat="server" ID="ucVesselType" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                                    AutoPostBack="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                            </td>
                            <td>
                                <eluc:Rank runat="server" ID="ucRank" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    AutoPostBack="true" />
                            </td>
                            <td valign="baseline"  style="width: 10%">
                                Medical Type
                            </td>
                           <td valign="baseline"  style="width: 20%">
                                 <eluc:Hard runat="server" ID="ucMedicals" AppendDataBoundItems="true" CssClass="dropdown_mandatory" HardTypeCode="95"
                                     HardList='<%# PhoenixRegistersHard.ListHard(1, 95, 0, "UKP,P&I,PMU") %>' ShortNameFilter="UKP,P&I,PMU" />
                            </td>                            
                        </tr>
                    </table>
                </div>
                <div style="position: relative; width: 15px;">
                    <eluc:TabStrip ID="MenuRegistersDocumentsRequired" runat="server" OnTabStripCommand="RegistersDocumentsRequired_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: +1;">
                    <asp:GridView ID="gvDocumentsRequired" runat="server" AutoGenerateColumns="False"
                        Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvDocumentsRequired_RowCommand"
                        OnRowDataBound="gvDocumentsRequired_ItemDataBound" ShowFooter="true" ShowHeader="true"
                        EnableViewState="false" OnSorting="gvDocumentsRequired_Sorting" AllowSorting="true"
                        OnRowCreated="gvDocumentsRequired_RowCreated" OnRowCancelingEdit="gvDocumentsRequired_RowCancelingEdit"
                        OnRowDeleting="gvDocumentsRequired_RowDeleting" OnRowEditing="gvDocumentsRequired_RowEditing"
                        OnSelectedIndexChanging="gvDocumentsRequired_SelectedIndexChanging" OnRowUpdating="gvDocumentsRequired_RowUpdating">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField HeaderText="Document Type">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMedicalTestCode" runat="server">Code&nbsp;
                                    </asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMedicalCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCODE") %>'></asp:Label>
                                    <asp:Label ID="lblVesselMdicalTestMapid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELMEDICALTESTMAPID") %>'></asp:Label>
                                    <asp:Label ID="lblMedicalTestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALTESTID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Medical Test">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblMedicalTest" runat="server" Text="Name"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblMedicalTestName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></asp:Label>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:DropDownList runat="server" ID="ddlMedicalTestAdd" CssClass="input_mandatory" OnSelectedIndexChanged="ddlMedicalTestAdd_SelectedIndexChanged" 
                                        DataTextField="FLDNAMEOFMEDICAL" DataValueField="FLDDOCUMENTMEDICALID" Width="300px" AppendDataBoundItems="true" AutoPostBack="true">
                                    </asp:DropDownList>
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Expiry Period">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblExpiryPeriodHeader" runat="server" Text="Expiry Period (month)"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblExpiryPeriod" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYPERIOD") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucExpiryPeriodEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYPERIOD") %>' MaxLength="2" IsPositive="true" DecimalPlace="0" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucExpiryPeriodAdd" runat="server" CssClass="input" MaxLength="2" IsPositive="true" DecimalPlace="0" />                                        
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Age From">
                                <HeaderTemplate>
                                    <asp:Label ID="lblAgeFromHeader" runat="server" Text="Age From"></asp:Label>
                                </HeaderTemplate>
                                <ItemStyle HorizontalAlign="Right" />
                                <ItemTemplate>
                                    <asp:Label ID="lblAgefrom" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGEFROM") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAgefromEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGEFROM") %>' CssClass="input_mandatory" MaxLength="2" IsPositive="true" DecimalPlace="0" />                                        
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucAgefromAdd" runat="server" CssClass="input_mandatory" MaxLength="2" IsPositive="true" DecimalPlace="0" />
                                </FooterTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Age To">
                                <ItemStyle HorizontalAlign="Right" />
                                <HeaderTemplate>
                                    <asp:Label ID="lblAgeToHeader" runat="server" Text="Age To"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAgeTo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGETO") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="ucAgeToEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAGETO") %>' CssClass="input_mandatory"
                                        MaxLength="2" IsPositive="true" DecimalPlace="0" />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="ucAgeToAdd" runat="server" CssClass="input_mandatory"
                                        MaxLength="2" IsPositive="true" DecimalPlace="0" />
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
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="SAVE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
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
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
