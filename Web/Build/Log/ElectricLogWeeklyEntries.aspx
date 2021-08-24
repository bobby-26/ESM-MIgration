<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogWeeklyEntries.aspx.cs" Inherits="Log_ElectricLogWeeklyEntries" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Mask" Src="~/UserControls/UserControlMaskedTextBox.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Weekly Entry</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
            function NumberFieldKeyPress(obj, arg) {
                if (arg.get_keyCode() == 45) {
                    alert("This field only takes positive number.");
                    arg.set_cancel(true);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .bold {
            font-weight: bold;
        }

        .border-table th {
            /*font-weight:bold;
                text-align:center;*/
        }

        .border-table tr td {
            /*border-left: 1px solid black;
                border-top: 1px solid black;
                border-bottom: 1px solid black;
                border-right: 1px solid black;*/
        }

        .float-right {
            float: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <table>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblOperationDate" runat="server" Text="Weekly Entries"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_TextChangedEvent" AutoPostBack="true">
                            <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                        </telerik:RadDatePicker>
                    </td>
                </tr>
                <tr runat="server" id="inchargeRow">
                    <td>
                        <telerik:RadLabel ID="lblOfficerInchargeSign" runat="server" Text="Officer Incharge Sign"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                        <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                        <asp:LinkButton runat="server" AlternateText="Incharge Sign" 
                            CommandName="INCHARGEAUTOSIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                            ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <br />

            <table>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvSludgeTank" runat="server" AllowCustomPaging="true" AllowPaging="true" Height="500px"
                            Width="500px"
                            ShowGroupPanel="false" CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvSludgeTank_ItemDataBound"
                            OnItemCommand="gvSludgeTank_ItemCommand"
                            OnNeedDataSource="gvSludgeTank_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage"
                                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Sludge Tank as per IOPP item 3.1">
                                        <HeaderStyle Width="118px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Max Cap, m3">
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblCapacity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="ROB, m3">
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadNumericTextBox ID="txtSludgeRob" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" OnTextChanged="txtSludgeRob_TextChangedEvent" AutoPostBack="true">
                                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                            </telerik:RadNumericTextBox>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWeeklyEntrySludge" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="500px"
                            CellSpacing="0" GridLines="None"
                            OnNeedDataSource="gvWeeklyEntrySludge_NeedDataSource"
                            OnItemDataBound="gvWeeklyEntrySludge_ItemDataBound"
                            OnItemCommand="gvWeeklyEntrySludge_ItemCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <Columns>

                                    <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true">
                                        <HeaderStyle Width="50px" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblDate" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                            </table>
                                            
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true">
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCode" runat="server" Text="C"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Item No" AllowSorting="true" ShowSortIcon="true">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo1" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo2" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo3" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Record of operation / Signature of officer in charge">
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>

                                            <table>
                                                <tr>
                                                    <td>
                                                        Name of tank :
                                                        <telerik:RadLabel ID="lblTankName" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    Capacity
                                                        <telerik:RadLabel ID="lblCapacity" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                                                        <span class="bold">m3</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    Retained
                                                        <telerik:RadLabel ID="lblBeforeROB" runat="server" Visible="false" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblROB" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:RadLabel>
                                                        <span class="bold">m3</span>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblOfficerInchargeSign" runat="server" Text="Signed: (Officer in Charge, Name & Rank)"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblInchargeSigndate" Style="float: right" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCheifOfficerInchargeSign" runat="server" Text="Signed: (Chief Engineer, Name)"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCheifOfficerSigndate" Style="float: right;" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>


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
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvBilgeTank" runat="server" AllowCustomPaging="true" AllowPaging="true" Height="500px"
                            Width="500px"
                            ShowGroupPanel="false" CellSpacing="0" GridLines="None"
                            OnItemDataBound="gvBilgeTank_ItemDataBound"
                            OnItemCommand="gvBilgeTank_ItemCommand"
                            OnNeedDataSource="gvBilgeTank_NeedDataSource">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace"
                                InsertItemPageIndexAction="ShowItemOnCurrentPage"
                                HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Bilge Tank as per IOPP item 3.3">
                                        <HeaderStyle Width="118px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lblTankName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>' ToolTip='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Max Cap, m3">
                                        <HeaderStyle Width="50px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblIOPPName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="ROB, m3">
                                        <HeaderStyle Width="100px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <%--<eluc:Number ID="txtBilgeRob" runat="server" Text='' MaxLength="3" IsInteger="true" IsPositive="true" DecimalPlace="0" AutoPostBack="true" OnTextChangedEvent="txtBilgeRob_TextChangedEvent" />--%>
                                            <telerik:RadNumericTextBox ID="txtBilgeRob" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" runat="server"  MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" OnTextChanged="txtBilgeRob_TextChangedEvent" AutoPostBack="true">
                                                <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                            </telerik:RadNumericTextBox>
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
                            <ClientSettings EnableRowHoverStyle="true" AllowDragToGroup="false" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                    <td>
                        <telerik:RadGrid RenderMode="Lightweight" ID="gvWeeklyEntryBilge" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="500px"
                            CellSpacing="0" GridLines="None"
                            OnNeedDataSource="gvWeeklyEntryBilge_NeedDataSource"
                            OnItemDataBound="gvWeeklyEntryBilge_ItemDataBound"
                            OnItemCommand="gvWeeklyEntryBilge_ItemCommand">
                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                AutoGenerateColumns="false">
                                <HeaderStyle Width="102px" />
                                <Columns>

                                    <telerik:GridTemplateColumn HeaderText="Date" AllowSorting="true">
                                        <HeaderStyle Width="50px" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblDate" runat="server"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblLocationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTANKID") %>'></telerik:RadLabel>
                                                    </td>
                                                    <tr>
                                                        <td>&nbsp</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp</td>
                                                    </tr>
                                                    <tr>
                                                        <td>&nbsp</td>
                                                    </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Code" AllowSorting="true">
                                        <HeaderStyle Width="30px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                     <telerik:RadLabel ID="lblCode" runat="server" Text="I"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>

                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Item No" AllowSorting="true" ShowSortIcon="true">
                                        <HeaderStyle Width="30px" />
                                        <ItemTemplate>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo1" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo2" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblItemNo3" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                                <tr>
                                                    <td>&nbsp</td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </telerik:GridTemplateColumn>

                                    <telerik:GridTemplateColumn HeaderText="Record of operation / Signature of officer in charge">
                                        <HeaderStyle Width="200px" />
                                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                        <ItemTemplate>

                                            <table>
                                                <tr>
                                                    <td><telerik:RadLabel  runat="server" ID="lblRecord" Text="Weekly Inventory of Bilge Water Tanks(Listed under item 3.3)"></telerik:RadLabel>  </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblTankName" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIOPPNAME") %>'></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        Capacity 
                                                        <telerik:RadLabel ID="lblCapacity" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCAPACITY") %>'></telerik:RadLabel>
                                                        <span class="bold">m3</span>,

                                                        Retained
                                                        <telerik:RadLabel ID="lblBeforeROB" runat="server" Visible="false" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblROB" runat="server" CssClass="bold" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:RadLabel>
                                                        <span class="bold">m3</span>
                                                    </td>
                                                </tr>
                                                <%--<tr>
                                                    <td>
                                                        
                                                    </td>
                                                </tr>--%>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblOfficerInchargeSign" runat="server" Text="Signed: (Officer in Charge, Name & Rank)"></telerik:RadLabel>
                                                        <div class="float-right">
                                                            <telerik:RadLabel ID="lblInchargeSigndate" Style="float: right" runat="server"></telerik:RadLabel>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <telerik:RadLabel ID="lblCheifOfficerInchargeSign" runat="server" Text="Signed: (Chief Engineer, Name)"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblCheifOfficerSigndate" Style="float: right" runat="server"></telerik:RadLabel>
                                                    </td>
                                                </tr>
                                            </table>
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
                                <%--          <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />--%>
                            </MasterTableView>

                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="460px" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
            </table>
                            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
