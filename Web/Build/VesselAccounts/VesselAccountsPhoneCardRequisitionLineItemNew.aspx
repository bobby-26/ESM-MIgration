<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsPhoneCardRequisitionLineItemNew.aspx.cs"
    Inherits="VesselAccountsPhoneCardRequisitionLineItemNew" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneCard1" Src="~/UserControls/UserControlPhoneCard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">

    <title>Requisition of Phone Cards</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .scrolpan {
            overflow-y: auto;
            height: 80%;
        }

        .checkRtl {
            direction: rtl;
        }

        .fon {
            font-size: small !important;
        }
    </style>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmBondReq" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="frmBondReq" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>

        <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="OrderForm_TabStripCommand" TabStrip="true" />
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%" CssClass="scrolpan">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtVesselName" runat="server" CssClass="readonlytextbox" ReadOnly="true" Enabled="false"
                            Text="<%#SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.VesselName %>" Width="240px">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblRequestNo" runat="server" Text="Request No"></asp:Literal>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtRefNo" runat="server" CssClass="readonlytextbox" Width="240px" Enabled="false"
                            ReadOnly="true">
                        </telerik:RadTextBox>
                    </td>
                    <td>
                        <asp:Literal ID="lblRequestDate" runat="server" Text="Request Date"></asp:Literal>
                    </td>
                    <td>
                        <eluc:Date ID="txtRequestDate" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
                <tr>
                    <td colspan="6">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td colspan="3">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="Summary" EnableDrag="false" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuPhoneCardSummary" runat="server" OnTabStripCommand="MenuPhoneCardSummary_TabStripCommand"></eluc:TabStrip>
                                    <telerik:RadGrid ID="gvSummary" Width="100%" runat="server" OnNeedDataSource="gvSummary_NeedDataSource" ShowHeader="true">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Phone Card">
                                                    <ItemStyle Width="50%" Wrap="false" />
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDNAME") %>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Quantity">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right" Width="15%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <%# DataBinder.Eval(Container, "DataItem.FLDQUANTITY")%>
                                                    </ItemTemplate>
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
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                    <td colspan="3"></td>
                </tr>
                <tr>
                    <td colspan="6">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="LineItem" EnableDrag="false" EnableAnimation="true"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuPhoneReq" runat="server" OnTabStripCommand="MenuPhoneReq_TabStripCommand" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPhoneReqLine" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                        CellSpacing="0" GridLines="None" OnItemCommand="gvPhoneReqLine_ItemCommand" OnItemDataBound="gvPhoneReqLine_ItemDataBound"
                                        ShowFooter="true" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvPhoneReqLine_NeedDataSource">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                                            <HeaderStyle Width="102px" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="Phone Card">
                                                    <HeaderStyle Width="25%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRequestLineIdEdit" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTLINEID"] %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblPhoneCardid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblPhoneCardName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:PhoneCard1 ID="ddlPhoneCard" Width="240px" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Quantity">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblQuantity" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Text='<%# ((DataRowView)Container.DataItem)["FLDQUANTITY"]%>'
                                                            Width="90px" />
                                                    </EditItemTemplate>
                                                    <FooterStyle HorizontalAlign="Right" />
                                                    <FooterTemplate>
                                                        <eluc:Number ID="txtQuantity" runat="server" CssClass="input_mandatory" DefaultZero="false"
                                                            MaxLength="5" Width="90px" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Employee">
                                                    <HeaderStyle Width="25%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRequestId" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDREQUESTID"] %>'
                                                            Visible="false">
                                                        </telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblEmpName" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPNAME"]%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:VesselCrew ID="ddlEmployeeEdit" runat="server" AppendDataBoundItems="true" Width="240px"
                                                            CssClass="input" SelectedEmployee='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"] %>' />
                                                    </EditItemTemplate>
                                                    <FooterTemplate>
                                                        <eluc:VesselCrew ID="ddlEmployee" runat="server" AppendDataBoundItems="true" CssClass="input" Width="240px" />
                                                    </FooterTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Card Number-PIN Number" Visible="false">
                                                    <HeaderStyle Width="20%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblCardNumber" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUECARDNO"]%>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <telerik:RadTextBox ID="txtCardNumberEdit" runat="server" CssClass="input" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUECARDNO"]%>'
                                                            ToolTip="CardNo1-PinNo1,CardNo2-PinNo2,...">
                                                        </telerik:RadTextBox>
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Issue On" Visible="false">
                                                    <HeaderStyle Width="10%" />
                                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%# string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDISSUEDDATE"]) %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <eluc:Date ID="txtIssueDateEdit" runat="server" CssClass="input" Text='<%# ((DataRowView)Container.DataItem)["FLDISSUEDDATE"] %>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
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
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
