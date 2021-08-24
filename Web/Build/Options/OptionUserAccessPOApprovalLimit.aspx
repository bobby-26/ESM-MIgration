<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionUserAccessPOApprovalLimit.aspx.cs" Inherits="OptionUserAccessPOApprovalLimit" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="~/UserControls/UserControlUserName.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Principal" Src="~/UserControls/UserControlAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>

    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <eluc:TabStrip ID="MenuUserAccessList" runat="server" OnTabStripCommand="UserAccessList_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="92%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false"></eluc:Status>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="ltrlVessel" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDropDownList ID="ddlVesselName" runat="server" Width="180px">
                        </telerik:RadDropDownList>
                    </td>
                    <td>
                        <telerik:RadLabel ID="ltrlType" runat="server" Text="PO Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucPOType" runat="server" QuickTypeCode="152" AppendDataBoundItems="true" Width="180px" />
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuDivPO" runat="server" OnTabStripCommand="MenuDivPO_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPOApprovalLimitsList" Height="80%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" OnSorting="gvPOApprovalLimitsList_Sorting"
                CellSpacing="0" GridLines="None" OnItemCommand="gvPOApprovalLimitsList_ItemCommand" OnItemDataBound="gvPOApprovalLimitsList_ItemDataBound" OnNeedDataSource="gvPOApprovalLimitsList_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDUSERLIMITID">
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="true" SortExpression="FLDVESSELNAME">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblVesselName" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblLimitId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSERLIMITID") %>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblVesselId" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>' Visible="false"></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <telerik:RadDropDownList runat="server" ID="ddlVesselAdd" DataTextField="FLDVESSELNAME" DataValueField="FLDVESSELID" ExpandDirection="Up" DropDownHeight="300px" Width="180px" CssClass="input_mandatory"></telerik:RadDropDownList>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PO Approval Type">
                            <HeaderStyle Width="25%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblPOApprovalType" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOAPPROVALCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel runat="server" ID="lblPOApprovalCat" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOAPPROVALCATEGORY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick runat="server" ID="ucPOApprovalCategoryAdd" AppendDataBoundItems="true" QuickTypeCode="152" Width="180px" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Primary Limit (USD)">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblPOPrimaryLimit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOPRIMARYAPROVALLIMIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucPrimaryLimitEdit" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOPRIMARYAPROVALLIMIT") %>'
                                    CssClass="gridinput" MaxLength="19" DecimalPlace="2"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucPrimaryLimitAdd" Width="150px" runat="server" CssClass="gridinput"
                                    MaxLength="19" DecimalPlace="2"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Secondary Limit (USD)">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel runat="server" ID="lblSecondaryLimit" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSECONDARYAPROVALLIMIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="ucSecondaryLimitEdit" runat="server" Width="150px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPOSECONDARYAPROVALLIMIT") %>'
                                    CssClass="gridinput" MaxLength="19" DecimalPlace="2"></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="ucSecondaryLimitAdd" runat="server" Width="150px" CssClass="gridinput"
                                    MaxLength="19" DecimalPlace="2"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="10%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <table>
                <asp:Label ID="lbl1" runat="server" Text="* All Amounts are in USD." Font-Bold="true"></asp:Label>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

