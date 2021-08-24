<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkGearNeededItem.aspx.cs"
    Inherits="CrewWorkGearNeededItem" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="WorkingGearItemType" Src="~/UserControls/UserControlWorkingGearItemType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlSize.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MultiAddr" Src="~/UserControls/UserControlMultiColumnAddress.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="MaskNumber" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVesselCommon.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Working Gear Needed Item</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="worknggearmain" runat="server" OnTabStripCommand="worknggearmain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="CrewWorkGearNeededItemRequest" runat="server" OnTabStripCommand="CrewWorkGearNeededItemRequest_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" Visible="false" />
            <%-- <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblReferenceNo" runat="server" Text="Reference No"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRefNo" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>

                    <td>
                        <telerik:RadLabel ID="lblRequestDate" runat="server" Text="Request date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtRequestDate" runat="server" CssClass="input_mandatory" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblZone" runat="server" Text="Zone"></telerik:RadLabel>
                    </td>

                    <td>
                        <eluc:Zone ID="ucZone" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" />
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblPayable" runat="server" Text="Payable By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlPayby" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="60%"
                            Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="1" Text="Office" />
                                <telerik:RadComboBoxItem Value="2" Text="Seafarer" />
                                <%--  <telerik:RadComboBoxItem Value="3" Text="Vendor" />--%>
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblBudgetCode" runat="server" Text="Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtBudgetCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"
                            Width="20%">
                        </telerik:RadTextBox>
                        <telerik:RadComboBox ID="ddlBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="50%"
                            AppendDataBoundItems="true" AutoPostBack="true" OnTextChanged="OnBudgetChange" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>

                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ucVessel" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="60%"
                            AutoPostBack="true" OnTextChanged="VesselChanged" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>

                    <td>
                        <telerik:RadLabel ID="lblVesselAccount" runat="server" Text="Vessel Account"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlAccountDetails" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true"
                            OnDataBound="ddlAccountDetails_DataBound" DataTextField="FLDVESSELACCOUNTNAME" OnTextChanged="ddlAccountDetails_TextChanged"
                            DataValueField="FLDACCOUNTID" Width="60%" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                        </telerik:RadComboBox>

                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOwnerBudgetCode" runat="server" Text="Owner Budget Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlOwnerBudgetCode" runat="server" CssClass="dropdown_mandatory" Width="52%"
                            AppendDataBoundItems="false" AutoPostBack="true" Filter="Contains" MarkFirstMatch="true" EnableLoadOnDemand="True">
                            <Items>
                                <telerik:RadComboBoxItem Value="" Text="--Select--" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtid" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td></td>
                    <td></td>
                </tr>
            </table>
            <br />
            <telerik:RadLabel ID="lblAlert" runat="server" Style="color: Red; font-weight: bold;" Visible="false"></telerik:RadLabel>
            <br />
            <eluc:TabStrip ID="Menuitems" runat="server" OnTabStripCommand="Menuitems_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvRegistersworkinggearitem" runat="server" EnableViewState="false" Height="60%"
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvRegistersworkinggearitem_NeedDataSource" EnableHeaderContextMenu="true" OnItemDataBound="gvRegistersworkinggearitem_ItemDataBound"
                OnItemCommand="gvRegistersworkinggearitem_ItemCommand" OnUpdateCommand="gvRegistersworkinggearitem_UpdateCommand" ShowFooter="false" OnDeleteCommand="gvRegistersworkinggearitem_DeleteCommand"
                OnSortCommand="gvRegistersworkinggearitem_SortCommand" AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <HeaderStyle Width="102px" />
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Working Gear Item" AllowSorting="false" ShowSortIcon="true">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblItemtypeid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMTYPEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblitemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblneededid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWWGNEEDEDITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lbllineitemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWWGNEEDEDLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGearitemiditem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lbllineitemidedit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWWGNEEDEDLINEITEMID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblGearitemidEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Requested Quantity" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblGearitemQuantityitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblGearitemQuantityedite" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUANTITY") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Order Quantity" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOrderQuantityItem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblOrderQuantityedit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>'></telerik:RadLabel>
                                <eluc:Number ID="txtOrderQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERQUANTITY") %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued Quantity" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrRGearitemissueitem" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Received Quantity" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblrRGearitemQuantityitemreceived" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="txtRQuantityEdit" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Stock Price" AllowSorting="false" ShowSortIcon="true">
                            <HeaderStyle HorizontalAlign="Center" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStockPrice" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTOCKPRICE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" AllowSorting="true" SortExpression="">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="cmdEdit" ToolTip="Edit" CommandName="EDIT" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="cmdDelete" CommandName="DELETE" ToolTip="Delete" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" ID="imgIssue" ToolTip="Issue" CommandName="ISSUE" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-check-circle-ei"></i></span>
                                </asp:LinkButton>
                                <%--   <asp:LinkButton runat="server" AlternateText="Approve" CommandName="APPROVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdApprove" ToolTip="Approve">
                                    <span class="icon"><i class="fas fa-award"></i></span>
                                </asp:LinkButton>--%>
                                <asp:LinkButton runat="server" ID="ImageCancel" Visible="false" CommandName="CANCELREQ" ToolTip="Cancel Issue" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ID="cmdSave" CommandName="Update" ToolTip="Save" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ID="cmdCancel" CommandName="Cancel" ToolTip="Cancel" Width="20px" Height="20px">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="Status1" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
