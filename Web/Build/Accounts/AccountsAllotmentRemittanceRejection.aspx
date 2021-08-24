<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsAllotmentRemittanceRejection.aspx.cs" Inherits="AccountsAllotmentRemittanceRejection" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Allotment Remittance Rejection</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="AllotmentRemittanceRejection" runat="server">
        <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />

        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="96%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <%-- <eluc:Title runat="server" ID="frmTitle" Text="Allotment Remittance Rejection"></eluc:Title>--%>

            <eluc:TabStrip ID="MenuRemittanceRejectionHeaderMain" runat="server" OnTabStripCommand="MenuRemittanceRejectionHeaderMain_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRemittanceRejection" Height="94%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvRemittanceRejection_ItemCommand" OnItemDataBound="gvRemittanceRejection_ItemDataBound" OnNeedDataSource="gvRemittanceRejection_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDREMITTANCEREJECTIONID">
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
                        <telerik:GridTemplateColumn HeaderText="File No.">
                            <HeaderStyle Width="8%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfileno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Name">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEENAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblEmployeeId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"Dataitem.FLDEMPLOYEEID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Vessel">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"Dataitem.FLDVESSELID") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Remittance No.">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemittanceId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRemittanceRejectionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCEREJECTIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRemittanceNumber" runat="server" Visible="TRUE" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMITTANCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Reason">
                            <HeaderStyle Width="15%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRejectionReason" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTIONREASON") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRejectionShortName" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREJECTIONSHORTNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <HeaderStyle Width="10%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRejectionRemarks" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>'></telerik:RadLabel>
                                <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" 
                                    ID="imgToolTip"></asp:ImageButton>
                                <eluc:ToolTip ID="ucToolTip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' TargetControlId="imgToolTip" />
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Rejected By">
                            <HeaderStyle Width="22%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREJECTEDBY")%>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREJECTEDBY").ToString() == "" ? "" : "<b>On</b>"%>
                                <%# DataBinder.Eval(Container, "DataItem.FLDREJECTIONDATE", "{0:dd/MMM/yyyy}")%>
                            </ItemTemplate>

                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="90px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <%--<img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                <asp:ImageButton ID="cmdRejection" runat="server" AlternateText="Delete" 
                                    CommandName="REJECTION" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>" ToolTip="Rejection Details" Style="visibility: visible" />
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Generate PMV" ImageUrl="<%$ PhoenixTheme:images/covering-letter.png%>"
                                    CommandName="GENERATEPMV" ID="imgPMVGenerate"
                                    ToolTip="Allotment Details" Style="visibility: hidden"></asp:ImageButton>
                                <%--  <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                        <asp:ImageButton runat="server" AlternateText="Send Earning entry to vessel" ImageUrl="<%$ PhoenixTheme:images/visa-issued.png %>"
                                            CommandName="EARNING" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEarning"
                                            ToolTip="Send Earning entry to vessel" Style="visibility: hidden"></asp:ImageButton>--%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

