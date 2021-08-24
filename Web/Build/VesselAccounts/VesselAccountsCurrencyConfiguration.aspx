<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsCurrencyConfiguration.aspx.cs"
    Inherits="VesselAccountsCurrencyConfiguration" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Vessel Currency Configuration</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="frmRegistersRank" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="94%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcurrency" runat="server" Text="Currency Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:TextBox ID="txtCurrencyCode" runat="server" CssClass="ïnput" Text=""></asp:TextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblActive" runat="server" Text="Active"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkactiveyn" runat="server" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuPhoneReq" runat="server" OnTabStripCommand="MenuPhoneReq_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCurrencyConf" Height="92%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" OnItemCommand="gvCurrencyConf_ItemCommand" OnItemDataBound="gvCurrencyConf_ItemDataBound" EnableViewState="false"
                ShowFooter="true" ShowHeader="true" OnNeedDataSource="gvCurrencyConf_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDMAPPINGID">
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Currency">

                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCURRENCYCODE"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblCurrencyConf" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDMAPPINGID"] %>'></telerik:RadLabel>
                                <eluc:Currency ID="ddlCurrencyEdit" runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' CssClass="input_mandatory" AppendDataBoundItems="true" Width="70px" SelectedCurrency='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYID") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Currency ID="ddlCurrencyAdd" runat="server" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>' AppendDataBoundItems="true" CssClass="input_mandatory" Width="70px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="ActiveYN">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDACTIVEYNDESC"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNEdit" runat="server" Checked='<%# ((DataRowView)Container.DataItem)["FLDACTIVEYN"].ToString()=="1" ?true:false%>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActiveYNAdd" runat="server" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="CC Round Off">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDCCROUNDOFFLIMIT"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtCCRoundoffEdit" runat="server" CssClass="input_mandatory" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDCCROUNDOFFLIMIT"]%>'
                                    Width="90px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtCCRoundoffAdd" runat="server" CssClass="input_mandatory" IsInteger="true"
                                    Width="90px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bond & Provision Round Off">
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBONDROUNDOFF"]%>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtBondRoundoffEdit" runat="server" CssClass="input_mandatory" IsInteger="true" Text='<%# ((DataRowView)Container.DataItem)["FLDBONDROUNDOFF"]%>'
                                    Width="90px" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtBondRoundoffAdd" runat="server" CssClass="input_mandatory" IsInteger="true"
                                    Width="90px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>                     
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                             
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Save" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
           
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
