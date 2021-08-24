<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAGeneration.aspx.cs" EnableEventValidation="false"
    Inherits="AccountsSOAGeneration" %>

<%@ Import Namespace="SouthNests.Phoenix.Accounts" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Month" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Year" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vesselaccount" Src="~/UserControls/UserControlMultipleColumnVesselAccountCode.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Country</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

<script type="text/javascript">
           function Resize() {

                   TelerikGridResize($find("<%= gvDebitReference.ClientID %>"));
           }
            window.onresize = window.onload = Resize;

           function pageLoad(sender, eventArgs) {
               Resize();
               fade('statusmessage');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmErmDebitReference" runat="server" submitdisabledcontrols="true">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server" Height="90%">

            <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status runat="server" ID="ucStatus" />

            <%--  <eluc:TabStrip ID="MenuFormMain" runat="server" OnTabStripCommand="MenuFormMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>--%>
            <table width="100%;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblyear" runat="server" Text="Year"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Year ID="ucYear" runat="server" AppendDataBoundItems="true"
                            QuickTypeCode="55" Width="65px" />
                    </td>

                    <td style="width: 200px"></td>
                    <td>
                        <telerik:RadLabel ID="lblStatusMain" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadRadioButtonList ID="rblStatus" runat="server" AutoPostBack="true" Direction="Horizontal">
                            <Items>
                                <telerik:ButtonListItem Value="Pending" Selected="True" Text="Pending" />
                                <telerik:ButtonListItem Value="1st Level Checked" Text="1st Level Checked" />
                                <telerik:ButtonListItem Value="2nd Level Checked" Text="2nd Level Checked" />
                                <telerik:ButtonListItem Value="Published" Text="Published" />
                                <telerik:ButtonListItem Value="1" Text="All" />
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>

                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblmonth" runat="server" Text="Month"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" Width="100px"
                            HardTypeCode="55" SortByShortName="true" />
                    </td>
                    <td style="width: 200px"></td>
                </tr>

                <tr>
                    <td>
                        <telerik:RadLabel ID="Literal2" runat="server" Text="Report Type"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlReportType" runat="server" Filter="Contains" EmptyMessage="Type to select" Width="40%">
                            <Items>
                                <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td style="width: 200px"></td>
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
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="Literal1" runat="server" Text="Principal"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListMaker">
                            <telerik:RadTextBox ID="txtVendorCode" runat="server" ReadOnly="false"
                                Width="90px">
                            </telerik:RadTextBox>
                            <asp:ImageButton ID="ImgSupplierPickList" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                Style="cursor: pointer; vertical-align: middle; padding-bottom: 3px;" />
                            <telerik:RadTextBox ID="txtVenderName" runat="server" ReadOnly="false"
                                Width="180px">
                            </telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtVendorId" runat="server" Width="10px"></telerik:RadTextBox>
                        </span>

                    </td>

                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblDebitNoteReference" runat="server" Text="Debit Note Reference"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtDebitRefernce" runat="server"></telerik:RadTextBox>
                    </td>

                </tr>
            </table>
            <br />
            <eluc:TabStrip ID="MenuDebitReference" runat="server" OnTabStripCommand="MenuDebitReference_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvDebitReference" runat="server" Height="" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" EnableViewState="true" OnNeedDataSource="gvDebitReference_NeedDataSource"
                GroupingEnabled="false" EnableHeaderContextMenu="true" 
                OnItemCommand="gvDebitReference_ItemCommand"
                OnItemDataBound="gvDebitReference_ItemDataBound"
                AutoGenerateColumns="false" ShowFooter="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDDEBINOTEREFERENCEID">
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

                        <telerik:GridTemplateColumn HeaderText="Account" HeaderStyle-Width="11%">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lnkAccountCode" runat="server" CommandName="SELECTROW" CommandArgument='<%# Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblAccountId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDebitReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblDebitReferenceId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBINOTEREFERENCEID") %>'
                                    Visible="false">
                                </telerik:RadLabel>
                                <span id="spnPickListCreditAccount">
                                    <telerik:RadTextBox ID="txtAccountCodeEdit" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Width="60px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODE")  %>'>
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtCreditAccountDescriptionEdit" runat="server" CssClass="hidden"
                                        Enabled="false" MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCreditAccountIdEdit" runat="server" CssClass="hidden" MaxLength="20"
                                        Width="0px">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListCreditAccount">
                                    <telerik:RadTextBox ID="txtAccountCode" runat="server" CssClass="input_mandatory" Enabled="false"
                                        MaxLength="20" Width="60px">
                                    </telerik:RadTextBox>
                                    <asp:ImageButton ID="imgShowAccount" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                        ImageAlign="AbsMiddle" Text=".." CommandName="picklist" CommandArgument="<%# Container.DataSetIndex %>" />
                                    <telerik:RadTextBox ID="txtCreditAccountDescription" runat="server" CssClass="hidden" Enabled="false"
                                        MaxLength="50" Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtCreditAccountId" runat="server" CssClass="hidden" MaxLength="20"
                                        Width="0px">
                                    </telerik:RadTextBox>
                                    <telerik:RadLabel ID="lblOwnerId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Account Description" HeaderStyle-Width="10%">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAccountCodeDescriptin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCOUNTCODEDESCRIPTION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Type" HeaderStyle-Width="100px">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadComboBox ID="ddlTypeEdit" runat="server" CssClass="input_mandatory" Width="100%" Filter="Contains" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                        <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                        <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                        <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                    </Items>
                                </telerik:RadComboBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadComboBox ID="ddlType" Width="100%" runat="server" CssClass="input_mandatory" Filter="Contains" EmptyMessage="Type to select">
                                    <Items>
                                        <telerik:RadComboBoxItem Value="Dummy" Text="--Select--" />
                                        <telerik:RadComboBoxItem Value="Monthly Report" Text="Monthly Report" />
                                        <telerik:RadComboBoxItem Value="Non-Budgeted Report" Text="Non-Budgeted Report" />
                                        <telerik:RadComboBoxItem Value="Predelivery Report" Text="Predelivery Report" />
                                        <telerik:RadComboBoxItem Value="Dry Docking Report" Text="Dry Docking Report" />
                                    </Items>
                                </telerik:RadComboBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Month" HeaderStyle-Width="9%">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblMonth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Month ID="ucMonthEdit" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" HardList='<%# PhoenixRegistersHard.ListHard(1, 55) %>'
                                    HardTypeCode="55" Width="100%" SortByShortName="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Month ID="ucMonth" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory"
                                    HardTypeCode="55" Width="100%" SortByShortName="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Year" HeaderStyle-Width="9%">

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblYear" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Year ID="ucYearEdit" runat="server" CssClass="input_mandatory" QuickList='<%# PhoenixRegistersQuick.ListQuick(1, 55) %>' AppendDataBoundItems="true"
                                    QuickTypeCode="55" Width="100%" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Year ID="ucYear" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true"
                                    QuickTypeCode="55" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Debit Note Reference" HeaderStyle-Width="11%">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkStatementReference" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'
                                    CommandName="Select" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblDebitNoteRef" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDebitReferenceEdit" runat="server" CssClass="input_mandatory"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEBITNOTEREFERENCE") %>' Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox Width="100%" ID="txtDebitNoteRef" runat="server" CssClass="input_mandatory"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Status" HeaderStyle-Width="10%">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkSOASatatus" runat="server" CommandName="STATUS" CommandArgument='<%# Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS")  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="With Queries" HeaderStyle-Width="9%">

                            <ItemTemplate>
                                <asp:LinkButton ID="lnkQueriesYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISQUERY") %>'
                                    CommandName="QUERY" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblQueriesYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISQUERY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblOwnerID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOWNERID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="URL" HeaderStyle-Width="11%">

                            <ItemTemplate>
                                <asp:LinkButton ID="lblURLlink" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDURL").ToString().Substring(0, 30)+ "..." : DataBinder.Eval(Container, "DataItem.FLDURL").ToString() %>'
                                    CommandName="SOAURL" CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                <telerik:RadLabel ID="lblURL" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL").ToString().Length>30 ? DataBinder.Eval(Container, "DataItem.FLDURL").ToString().Substring(0, 30)+ "..." : DataBinder.Eval(Container, "DataItem.FLDURL").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTiplblURL" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' />
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtURLEdit" runat="server" TextMode="MultiLine" Rows="1"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDURL") %>' Width="100%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtURL" runat="server" TextMode="MultiLine" Rows="1" Width="100%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                            <ItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit"></asp:ImageButton>
                                <img id="Img8" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="DebitNote Reference Update" ImageUrl="<%$ PhoenixTheme:images/bulk_save.png %>"
                                    CommandName="BULKSAVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdBulkSave"
                                    ToolTip="DebitNote Reference Update"></asp:ImageButton>
                                <img id="Img7" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
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
                                <asp:ImageButton runat="server" AlternateText="Generate Pdf" ImageUrl="<%$ PhoenixTheme:images/pdf.png%>"
                                    CommandName="GENERATEPDF" CommandArgument="<%# Container.DataSetIndex %>"
                                    ID="imgGeneratePdf" ToolTip="Generate Pdf"></asp:ImageButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                    CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                    ToolTip="Attachment"></asp:ImageButton>
                                <img id="Img5" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="DELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                    ToolTip="Delete"></asp:ImageButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                    CommandName="Save" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save"></asp:ImageButton>
                                <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel"></asp:ImageButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/te_check.png %>"
                                    CommandName="Add" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAdd"
                                    ToolTip="Add New"></asp:ImageButton>
                            </FooterTemplate>
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
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
