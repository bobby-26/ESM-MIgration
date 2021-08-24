<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselAccountsEmployeeBankAccount.aspx.cs"
    Inherits="VesselAccountsEmployeeBankAccount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselCrew" Src="~/UserControls/UserControlVesselEmployee.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Employee Bank Account</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="form1" DecoratedControls="All" EnableRoundedCorners="true" />
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="50%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStaffName" runat="server" Text="Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:VesselCrew ID="ddlEmployee" runat="server" CssClass="input" AppendDataBoundItems="true"
                            AutoPostBack="true" OnTextChangedEvent="ddlEmployee_TextChangedEvent" Width="200px" />
                    </td>
                </tr>
            </table>
            <%--OnItemDataBound="gvCrewSearch_ItemDataBound" --%>
            <eluc:TabStrip ID="CrewBankAcct" runat="server" OnTabStripCommand="CrewBankAcct_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewSearch" Height="90%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCrewSearch_ItemCommand"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" OnNeedDataSource="gvCrewSearch_NeedDataSource">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" GroupsDefaultExpanded="true"
                    AutoGenerateColumns="false" TableLayout="Auto" GroupHeaderItemStyle-Font-Bold="true" EnableGroupsExpandAll="false" GroupLoadMode="Client" GroupHeaderItemStyle-CssClass="center">
                    <HeaderStyle Width="102px" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Default">
                            <HeaderStyle Width="4%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblEmployeeid" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDEMPLOYEEID"]%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblBankAccontId" runat="server" Visible="false" Text='<%# ((DataRowView)Container.DataItem)["FLDBANKACCOUNTID"]%>'></telerik:RadLabel>
                                <asp:LinkButton runat="server" Visible='<%# ((DataRowView)Container.DataItem)["FLDISDEFAULT"].ToString() == "1" ? true : false%>'>
                                    <span class="icon"><i class="fas fa-star"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDFILENO"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="18%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDEMPLOYEENAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Rank">
                            <HeaderStyle Width="5%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDRANKCODE"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Beneficiary">
                            <HeaderStyle Width="18%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDACCOUNTNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account No.">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDACCOUNTNUMBER"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# ((DataRowView)Container.DataItem)["FLDBANKNAME"]%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Bank Address">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%--    <telerik:RadLabel ID="lblEmployeeid" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDADDRESS") +(DataBinder.Eval(Container, "DataItem.FLDCITYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCITYNAME"]
                                                                                    +( DataBinder.Eval(Container, "DataItem.FLDSTATENAME").ToString() == "" ? " " : " , " )+ ((DataRowView)Container.DataItem)["FLDSTATENAME"] +(DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCOUNTRYNAME"]%>'
                                    ToolTip='<%#DataBinder.Eval(Container, "DataItem.FLDADDRESS") +(DataBinder.Eval(Container, "DataItem.FLDCITYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCITYNAME"]
                                                                                    +( DataBinder.Eval(Container, "DataItem.FLDSTATENAME").ToString() == "" ? " " : " , " )+ ((DataRowView)Container.DataItem)["FLDSTATENAME"] +(DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCOUNTRYNAME"]%>'>
                              </telerik:RadLabel>--%>  <%#DataBinder.Eval(Container, "DataItem.FLDADDRESS") +(DataBinder.Eval(Container, "DataItem.FLDCITYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCITYNAME"]
                                                                                    +( DataBinder.Eval(Container, "DataItem.FLDSTATENAME").ToString() == "" ? " " : " , " )+ ((DataRowView)Container.DataItem)["FLDSTATENAME"] +(DataBinder.Eval(Container, "DataItem.FLDCOUNTRYNAME").ToString() == "" ? " " : " , ") + ((DataRowView)Container.DataItem)["FLDCOUNTRYNAME"]%>
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
