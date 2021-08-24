<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaCheckingLineItems.aspx.cs"
    Inherits="Accounts_AccountsSoaCheckingLineItems" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Vessel List</title>
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
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuSOALineItems" runat="server" OnTabStripCommand="MenuSOALineItems_TabStripCommand"></eluc:TabStrip>

            <div runat="server" id="divFind">

                <eluc:TabStrip ID="MenuVoucherLI" runat="server" OnTabStripCommand="MenuVoucherLI_TabStripCommand"></eluc:TabStrip>

                <table width="100%">
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkNumber" runat="server" Font-Size="Medium" ForeColor="Blue" Visible="false"></asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 50%; vertical-align: top; min-height: 750px;">
                            <table width="100%">
                                <tr>
                                    <td style="width: 50%;" valign="top" align="left">

                                        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvOwnersAccount" DecoratedControls="All" EnableRoundedCorners="true" />
                                        <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                                            CellSpacing="0" GridLines="None" OnNeedDataSource="gvOwnersAccount_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                                            EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvOwnersAccount_SelectedIndexChanging"
                                            OnItemDataBound="gvOwnersAccount_ItemDataBound" OnItemCommand="gvOwnersAccount_ItemCommand"
                                            ShowFooter="false" ShowHeader="true">
                                            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                AutoGenerateColumns="false" DataKeyNames="FLDVOUCHERDETAILID">


                                                <Columns>
                                                    <telerik:GridTemplateColumn>

                                                        <ItemTemplate>
                                                            <asp:ImageButton runat="server" AlternateText="Queries" ImageUrl="<%$ PhoenixTheme:images/audit_complete.png %>"
                                                                ID="cmdQueries" ToolTip="Queries" Enabled="false" Visible="false"></asp:ImageButton>
                                                            <img id="Img4" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                                                width="3" />
                                                            <telerik:RadLabel ID="lblVerified" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVERFIED") %>'></telerik:RadLabel>
                                                            <asp:ImageButton ID="imbVeified" runat="server" AlternateText="VERIFIED" CommandArgument="<%# Container.DataSetIndex %>"
                                                                CommandName="VERIFIED" ImageUrl="<%$ PhoenixTheme:images/deficiency-action.png%>"
                                                                Enabled="false" Visible="false" />
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Voucher Date">

                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Voucher Number">

                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblVoucherType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPHOENIXVOUCHER") %>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblVoucherDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                                                CssClass="gridinput">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblVoucherDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDTKEY") %>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblVoucherLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>
                                                            <telerik:RadLabel ID="lblVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                                                Visible="false">
                                                            </telerik:RadLabel>
                                                            <asp:LinkButton ID="lblVoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>'
                                                                CommandArgument="<%# Container.DataSetIndex %>" CommandName="Select"></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Description">

                                                        <ItemStyle Wrap="true"></ItemStyle>

                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn>
                                                        <HeaderStyle Width="10%" />
                                                        <ItemStyle HorizontalAlign="Right" />
                                                        <HeaderTemplate>
                                                            <telerik:RadLabel ID="Label1" runat="server" Text="Amount"></telerik:RadLabel>
                                                            &nbsp;(<%=vesselcurrency%>)                                                               
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n}") %>'></telerik:RadLabel>
                                                        </ItemTemplate>
                                                    </telerik:GridTemplateColumn>
                                                    <telerik:GridTemplateColumn HeaderText="Action">

                                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>


                                                        <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                                        <ItemTemplate>
                                                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                                ToolTip="Edit"></asp:ImageButton>
                                                            <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                            <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                                CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                                                ToolTip="Attachment" Visible="false"></asp:ImageButton>
                                                            <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                                                CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                                                Visible="false" ToolTip="No Attachment"></asp:ImageButton>
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
                                                <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                                                    PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                                            </MasterTableView>
                                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                                                EnablePostBackOnRowClick="true" ColumnsReorderMethod="Reorder">
                                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                                <Scrolling AllowScroll="true" UseStaticHeaders="true" FrozenColumnsCount="2" />
                                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                            </ClientSettings>
                                        </telerik:RadGrid>
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="width: 50%; vertical-align: top; min-height: 750px; border: 1px solid #CCC;">
                            <div>
                                <iframe runat="server" id="ifMoreInfo" scrolling="No" style="min-height: 750px; width: 100%"
                                    frameborder="0"></iframe>
                                <asp:HiddenField ID="hdnScroll" runat="server" />
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

        </div>
    </form>
</body>
</html>
