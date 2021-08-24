<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaChecking.aspx.cs"
    Inherits="Accounts_AccountsSoaChecking" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesselaccount" Src="~/UserControls/UserControlMultipleColumnVesselAccountCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Statement of Accounts</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>


    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmOwnersAccounts" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="false"></eluc:TabStrip>

            <br />
                <table width="50%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel Account"></telerik:RadLabel>
                        </td>
                        <td style="white-space: nowrap" rowspan="3">
                            <telerik:RadTextBox runat="server" ID="txtAccountSearch" MaxLength="50"
                                Width="240px">
                            </telerik:RadTextBox>&nbsp;
                        <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                            <div runat="server" id="Div2" class="input_mandatory" style="overflow: auto; width: 240px; height: 75px">
                                <asp:RadioButtonList ID="rblAccount" runat="server" Height="100%" RepeatColumns="1"
                                    RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="AccountSelection">
                                </asp:RadioButtonList>
                            </div>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblyear" runat="server" Text="Year"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Year ID="ucYear" runat="server" AppendDataBoundItems="true"
                                QuickTypeCode="55" Width="65px" />
                        </td>
                    </tr>
                    <tr>

                        <td></td>
                        <td>
                            <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true"
                                HardTypeCode="55" SortByShortName="true" Width="65px" />
                        </td>
                    </tr>

                    <tr>
                        <td></td>
                        <td>
                            <telerik:RadLabel ID="Literal1" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadComboBox ID="ddlstatus" runat="server" Filter="Contains" EmptyMessage="Type to select">
                                <Items>
                                    <telerik:RadComboBoxItem Value="0" Text="--select--"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="1" Text="Pending"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="2" Text="1st Level Checked"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="3" Text="2nd level checked"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="4" Text="Published"></telerik:RadComboBoxItem>
                                    <telerik:RadComboBoxItem Value="5" Text="Cancelled"></telerik:RadComboBoxItem>
                                </Items>
                            </telerik:RadComboBox>
                        </td>
                    </tr>
                    <tr>
                        <%--                     <td>
                        <telerik:RadLabel ID="lblVessel" runat="server" Text="Vessel"></telerik:RadLabel>
                    </td>
                    
                    <td style="white-space: nowrap" rowspan="2">
                        <telerik:RadTextBox runat="server" ID="txtAccountSearch"  MaxLength="50"
                            Width="240px"></telerik:RadTextBox>&nbsp;
                        <asp:ImageButton runat="server" ImageUrl="<%$ PhoenixTheme:images/search.png %>"
                            ID="cmdAccountSearch" OnClick="cmdSearchAccount_Click" ToolTip="Search" />
                        <div runat="server" id="Div2" class="input_mandatory" style="overflow: auto; width: 240px;
                            height: 75px">
                            <asp:RadioButtonList ID="rblAccount" runat="server" Height="100%" RepeatColumns="1"
                                RepeatDirection="Horizontal" RepeatLayout="Flow" AutoPostBack="True" OnSelectedIndexChanged="AccountSelection">
                            </asp:RadioButtonList>
                        </div>
                    </td>--%>
                    </tr>
                </table>
            <br />

            <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" runat="server" Height="68%" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvOwnersAccount_NeedDataSource"
                OnItemCommand="gvOwnersAccount_ItemCommand"
                OnItemDataBound="gvOwnersAccount_ItemDataBound"
                OnItemCreated="gvOwnersAccount_ItemCreated"
                GroupingEnabled="false" EnableHeaderContextMenu="true"

                AutoGenerateColumns="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
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
                        <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="30px">
                           
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Statement Reference" HeaderStyle-Width="150px">
                            
                            <itemtemplate>
                                    <asp:LinkButton ID="lnkStatementReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                        CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                    <telerik:RadLabel ID="lblStatementReference" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                                    <%-- <telerik:RadLabel ID="lblVesselId" Visible="false" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></telerik:RadLabel>--%>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px">
                           
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblDebitReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="100px">
                        
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblAccountId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'
                                        Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblAccountCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" HeaderStyle-Width="150px">
                          
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblAccountCodeDescription" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Billing Party" HeaderStyle-Width="100px">
                           
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblBillingPary" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-Width="50px">
                        
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="50px">
                            
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblMonthNumber" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="100px">
                            
                            <itemtemplate>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSOASTATUSNAME") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lbldtkey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                        Visible="false"></telerik:RadLabel>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="250px">
                            <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                           
                            <itemstyle wrap="False" horizontalalign="Center" ></itemstyle>
                            <itemtemplate>
                                    <img id="Img9" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" Visible="false" AlternateText="Reset" ImageUrl="<%$ PhoenixTheme:images/in-progress.png %>"
                                        CommandName="RESET" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdReset"
                                        ToolTip="Reset"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="1st Check" ImageUrl="<%$ PhoenixTheme:images/select.png %>"
                                        CommandName="1STCHECK" CommandArgument='<%# Container.DataSetIndex %>' ID="img1stCheck"
                                        ToolTip="1st Level Checked"></asp:ImageButton>
                                    <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="2nd Check" ImageUrl="<%$ PhoenixTheme:images/approved.png %>"
                                        CommandName="2NDCHECK" CommandArgument='<%# Container.DataSetIndex %>' ID="img2ndCheck"
                                        ToolTip="2nd Level Checked"></asp:ImageButton>
                                    <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Published" ImageUrl="<%$ PhoenixTheme:images/visa-issued.png %>"
                                        CommandName="PUBLISHED" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPublished"
                                        ToolTip="Publish" Visible="false"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="UnPublished" ImageUrl="<%$ PhoenixTheme:images/ticket_cancel.png %>"
                                        CommandName="UNPUBLISHED" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUnPublished"
                                        ToolTip="UnPublish" Visible="false"></asp:ImageButton>
                                    <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Untag All Vouchers" ImageUrl="<%$ PhoenixTheme:images/cancel.png %>"
                                        CommandName="UNTAGVOUCHERS" CommandArgument='<%# Container.DataSetIndex %>'
                                        ID="cmdUntagVouchers" ToolTip="Untag All Vouchers"></asp:ImageButton>
                                    <%--                                <img id="Img6" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />--%>
                                    <%--                                <asp:ImageButton runat="server" AlternateText="Generate Pdf" ImageUrl="<%$ PhoenixTheme:images/pdf.png %>"
                                    CommandName="GENERATEPDF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdPdf"
                                    ToolTip="Generate Pdf"></asp:ImageButton>--%>
                                    <asp:ImageButton runat="server" AlternateText="Generate Pdf" ImageUrl="<%$ PhoenixTheme:images/pdf.png%>"
                                        CommandName="GENERATEPDF" CommandArgument="<%# Container.DataSetIndex %>"
                                        ID="imgGeneratePdf" ToolTip="Generate Pdf"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                        ToolTip="Owner Budget Code Attachment"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Attachment1" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                        CommandName="ATTACHMENTESM" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachmentESM"
                                        ToolTip="Budget Code Attachment"></asp:ImageButton>
                                    <asp:ImageButton runat="server" AlternateText="Report History" ImageUrl="<%$ PhoenixTheme:images/te_notes.png %>"
                                        CommandName="INFO" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdMoreInfo"
                                        ToolTip="Report History"></asp:ImageButton>
                                    <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </itemtemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
    </form>
</body>
</html>
