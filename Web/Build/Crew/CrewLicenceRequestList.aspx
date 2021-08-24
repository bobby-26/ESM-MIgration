<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceRequestList.aspx.cs"
    Inherits="CrewLicenceRequestList" %>

<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CommonToolTip" Src="~/UserControls/UserControlCommonToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Licence List</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmLicReq" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="95%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:TabStrip ID="MenuLicenceList" runat="server" OnTabStripCommand="MenuLicenceList_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvLicReq" runat="server" Height="95%" EnableViewState="false"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvLicReq_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvLicReq_ItemDataBound"
                OnItemCommand="gvLicReq_ItemCommand" OnUpdateCommand="gvLicReq_UpdateCommand" ShowFooter="false"
                OnSortCommand="gvLicReq_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" DataKeyNames="FLDREQUESTID" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
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
                    <NestedViewSettings>
                        <ParentTableRelation>
                            <telerik:GridRelationFields MasterKeyField="FLDREQUESTID" DetailKeyField="FLDREQUESTID" />
                        </ParentTableRelation>
                    </NestedViewSettings>
                    <NestedViewTemplate>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="RadLabel1" runat="server" Font-Bold="true" Text="Consulate:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblConsulate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONSULATE") %>'></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel2" runat="server" Font-Bold="true" Text="Payment:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel3" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTTYPE") %>'></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel4" runat="server" Font-Bold="true" Text="Invoice/Voucher No.:"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="RadLabel5" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTNUMBER") %>'></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NestedViewTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No." AllowSorting="false" ShowSortIcon="true"  Visible="false">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Request No." AllowSorting="true" SortExpression="FLDREQUISITIONNUMBER" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lblReqNo" runat="server" CommandName="LICENCEDETAIL" CommandArgument="<%# Container.DataSetIndex %>"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUISITIONNUMBER") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEmployee" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME")%>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="File No." AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFileNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFILENO")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Flag" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRequestId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDtkey" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDDTKEY")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFlagId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGID") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblFlag" runat="server" CommandName="SELECT" CommandArgument="<%# Container.DataSetIndex %>"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFLAGNAME") %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Vessel" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblVessel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="7%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReqstatid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQSTATUSID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestedYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTSENTSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblRequestEditId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTID")%>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRequestYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQUESTSENTYN")%>'></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkRequestedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTSENTYN").ToString() == "1" ? true : false%>'
                                    Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDREQUESTSENTYN").ToString() == "1" ? false : true%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblReceivedYN" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblReceivedYN" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN")%>'></telerik:RadLabel>
                                <telerik:RadCheckBox ID="chkReceivedEdit" runat="server" Checked='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN").ToString() == "1" ? true : false%>'
                                    Enabled='<%# DataBinder.Eval(Container, "DataItem.FLDRECEIVEDYN").ToString() == "1" ? false : true%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Amount" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDAMOUNT")%>'></telerik:RadLabel>                               
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Currency" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="6%">
                            <ItemTemplate>                             
                                <telerik:RadLabel ID="lblCurrencyCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCURRENCYCODE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" AllowSorting="false" ShowSortIcon="true" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDREQSTATUS")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="13%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdLicenceDetail" CommandName="LICENCEDETAIL" ToolTip="Licence Detail" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-tasks-ld"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdGeneratePO" ToolTip="Make Payment" CommandName="MAKEPAYMENT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-coins-Payment"></i></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdMoreInfo" CommandName="MOREINFO" ToolTip="More Info" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-info-circle"></i></span>
                                </asp:LinkButton>
                                <eluc:CommonToolTip ID="ucCommonToolTip" runat="server" Screen='<%# "Crew/CrewLicenceRequestMoreInfoList.aspx?RequestId=" + DataBinder.Eval(Container,"DataItem.FLDREQUESTID").ToString()%>' />
                                <asp:LinkButton runat="server" ID="cmdCancel" CommandName="CANCELREQUEST" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdUpdateCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true"  AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
