<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseQuotationNotSubmittedWithInSevenDays.aspx.cs" Inherits="PurchaseQuotationNotSubmittedWithInSevenDays" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>



<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase Quotation Not Submitted</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />

            <table id="tblzone" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblvessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVesselSearch" runat="server" OnTextChangedEvent="ucVessel_TextChangedEvent" AutoPostBack="true" VesselsOnly="true" Width="60%" AppendDataBoundItems="true" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfromdate" runat="server" Text="From Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucfromdatesearch" runat="server" />

                    </td>
                    <td>
                        <telerik:RadLabel ID="lbltodate" runat="server" Text="To Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="uctodatesearch" runat="server" />

                    </td>
                </tr>
                <tr>
                      <td>
                        <telerik:RadLabel ID="lblstock" runat="server" Text="Stock Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox RenderMode="Lightweight" Width="60%" runat="server" ID="ddlStockType" OnTextChanged="ddlStockType_TextChanged" AutoPostBack="true" EnableLoadOnDemand="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="Dummy" />
                                <telerik:RadComboBoxItem Text="Spares" Value="SPARE" />
                                <telerik:RadComboBoxItem Text="Stores" Value="STORE" />
                                <telerik:RadComboBoxItem Text="Service" Value="SERVICE" />
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td >
                        <telerik:RadLabel ID="lblnoofdays" runat="server" Text="No Of Days"></telerik:RadLabel>
                    </td>
                    <td >
                     <eluc:Number ID="ucnoofdays" runat="server"  Width="50%" ReadOnly="true"/>
                            
                    </td>
                    <td >
                        <telerik:RadLabel ID="lblzone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>
                    <td >
                     
                      <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" OnSelectedChangedEvent="ucZone_Changed" Width="50%" />
                    </td>
               </tr>
            </table>

            <eluc:TabStrip ID="MenuPurchasequotationnotsub" runat="server" OnTabStripCommand="Purchasequotationnotsub_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPurchasequotationnotsub" Height="86%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvPurchasequotationnotsub_ItemCommand" OnItemDataBound="gvPurchasequotationnotsub_ItemDataBound" OnNeedDataSource="gvPurchasequotationnotsub_NeedDataSource"
                ShowFooter="false" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false">
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
                        <telerik:GridTemplateColumn HeaderText="Priority">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblpriority" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRIORITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqn Ref">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrefno" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqn Main Status">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmainstatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAINSTATUS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Reqn Date">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblreqdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Pr Approval Date">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapprovaldate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRAPPROVALDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pr Approval">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblapproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRAPPROVAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="First RFQ Sent Date">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrfqsentdate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRFQSENTDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel Name">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblvesselname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Eqp Name">
                            <HeaderStyle Width="130px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbleqpname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQPNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Counter">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcounter" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Not Submit For Supt Approval More Than 7 Days">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSuptApproval7" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNOTSUBMITFORDAYS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Incomplete Info For Sourcing">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblincompleteinfo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCOMPLETEINFO") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Chase Vendor For Quotation">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblchasevendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCHASEVENDORQTN") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Invite More Vendor">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblinvitemorevendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVITEVENDOR") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Enter Quotation And Submit">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblqtnandsubmit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQTNANDSUBMIT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Submit For Approval">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblsumbitforapproval" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBMITFORAPPROVAL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tech Supt to assist & Follow Up">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltechsuptoassist" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHSUPTOASSIST") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Include Freight Charges">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblfreightcharges" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFREIGHTCHARGES") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Clarification With Vessel">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblclarificationvessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCLARIFICATIONVESSEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tech Supt To Cancel">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltechsuptocancle" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHSUPTOCANCEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purchaser">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPurchaser" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURCHASER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Tech Supt">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lbltechsupt" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTECHSUPT") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Findings/Action/Comments">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFindings" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFINDACTIONCOMMENTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Maker">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmaker" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAKER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Model">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblmodel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMODEL") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="PR Comments">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblprcomments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRCOMMENTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Comments">
                            <HeaderStyle Width="102px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblcomments" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMMENTS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
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
