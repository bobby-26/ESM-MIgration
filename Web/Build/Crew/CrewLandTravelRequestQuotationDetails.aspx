<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLandTravelRequestQuotationDetails.aspx.cs" Inherits="CrewLandTravelRequestQuotationDetails" %>
<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PhoneNumber" Src="../UserControls/UserControlPhoneNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Land Travel Request</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server"> 
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <div style="margin: 0 auto; width: 1024px; text-align: left;">
        <form id="frmLandTravelRequestGeneral" runat="server">
            <telerik:RadScriptManager runat="server" ID="RadScriptManager1" EnableScriptCombine="false"></telerik:RadScriptManager>
            <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
            <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
                <div style="height: 60px;" class="RadToolBar RadToolBar_Horizontal RadToolBar_Windows7 RadToolBar_Windows7_Horizontal">
                    <div style="position: absolute; top: 15px;">
                        <img id="Img1" runat="server" style="vertical-align: middle" src="<%$ PhoenixTheme:images/esmlogo4_small.png %>"
                            alt="Phoenix" onclick="parent.hideMenu();" />
                        <span class="title" style="color: black">
                            <%=Application["softwarename"].ToString() %></span>
                        <telerik:RadLabel runat="server" ID="lblDatabase" ForeColor="Red" Font-Size="Large" Visible="false"
                            Text="Testing on ">
                        </telerik:RadLabel>
                        <br />
                    </div>
                </div>
            </telerik:RadCodeBlock>
            <eluc:TabStrip ID="MenuRequest" runat="server" OnTabStripCommand="MenuRequest_TabStripCommand" Title="Land Travel Request"></eluc:TabStrip>
            <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server"></eluc:Status>
                <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

                <table width="100%" cellpadding="1" cellspacing="2">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblRequisitionNo" runat="server" Text="Requisition No"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtReqNo" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCity" runat="server" Text="City"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtCity" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFromPlace" runat="server" Text="From Place"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtFromPlace" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblToPlace" runat="server" Text="To Place"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtToPlace" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTravelDate" runat="server" Text="Travel Date"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTravelDate" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTime" runat="server" Text="Time"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadMaskedTextBox runat="server" ID="txtFromTime"  ReadOnly="true" Enabled ="false" Width="50px" Mask="##:##"></telerik:RadMaskedTextBox>
                            <telerik:RadLabel ID="lblTo" runat="server" Text="to"></telerik:RadLabel>
                            <telerik:RadMaskedTextBox runat="server" ID="txtToTime" ReadOnly="true" Enabled ="false" Width="50px" Mask="##:##"></telerik:RadMaskedTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblTypeofTransport" runat="server" Text="Type of Transport"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTypeofTransport" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblTypeofDuty" runat="server" Text="Type Of Duty"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtTypeofDuty" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblPackageType" runat="server" Text="Package Type"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtType" runat="server" Enabled ="false" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td></td>
                        <td></td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblNoOfPassengers" runat="server" Text="No. Of Passengers"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Number ID="txtNoofPassengers" runat="server" Enabled ="false" ReadOnly="true" />
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMobileNumber" runat="server" Text="Contact No"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:PhoneNumber ID="txtMobileNumber" IsMobileNumber="true" runat="server" Enabled ="false" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblOtherInfo" runat="server" Text="Any other information"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtOtherInfo" runat="server" Enabled ="false" ReadOnly="true" TextMode="MultiLine"
                                Width="200px" Rows="4">
                            </telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblCurrency" runat="server" Text="Currency"></telerik:RadLabel>
                            <br />
                            <br />
                            <telerik:RadLabel ID="lblqAmount" runat="server" Text="Amount"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Currency ID="ddlCurrencyCode" CurrencyList='<%# PhoenixRegistersCurrency.ListCurrency(1)%>'
                                runat="server" AppendDataBoundItems="true" />
                            <br />
                            <br />
                            <eluc:Number ID="txtAmount" runat="server"  DecimalPlace="2" IsPositive="true" Width="50%"/>
                        </td>
                    </tr>
                </table>
                <br />
                <b>
                    <telerik:RadLabel ID="lblRequisitionPassengerDetails" runat="server" Text="Requisition Passenger Details"></telerik:RadLabel>
                </b>
                <div id="divReqDetail" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvPassengers" runat="server" EnableViewState="false"
                        AllowCustomPaging="true" AllowSorting="true" AllowPaging="false" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                        OnNeedDataSource="gvPassengers_NeedDataSource" EnableHeaderContextMenu="true" ShowFooter="false" AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText="Passenger Name" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblName" Width="150px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblPassengerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSENGERID") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Designation" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDesignation" Width="80px" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESIGNATIONNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Reason" AllowSorting="false" ShowSortIcon="true">
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblReason" runat="server" Width="80px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREASON") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
            </telerik:RadAjaxPanel>
        </form>
    </div>
</body>
</html>
