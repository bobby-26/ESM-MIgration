<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSoaGenerationLineItems.aspx.cs"
    Inherits="AccountsSoaGenerationLineItems" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
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

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <eluc:TabStrip ID="MenuGeneral" runat="server" OnTabStripCommand="MenuGeneral_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuGenralSub" runat="server" OnTabStripCommand="MenuGenralSub_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuSOALineItems" runat="server" OnTabStripCommand="MenuSOALineItems_TabStripCommand"></eluc:TabStrip>


            <eluc:TabStrip ID="MenuVoucherLI" runat="server" OnTabStripCommand="MenuVoucherLI_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkNumber" runat="server" Font-Size="Medium" ForeColor="Blue" Visible="false"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td style="vertical-align: top; min-height: 750px;">
                        <table width="100%" style="min-height: 750px;">
                            <tr>
                                <td style="width: 100%; min-height: 750px;" valign="top" align="left"; >
                 <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator2" runat="server" DecorationZoneID="gvOwnersAccount" DecoratedControls="All" EnableRoundedCorners="true" />
                <telerik:RadGrid RenderMode="Lightweight" ID="gvOwnersAccount" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvOwnersAccount_NeedDataSource" AllowMultiRowSelection="true" FilterType="CheckList"
                    EnableViewState="false" EnableHeaderContextMenu="true" Width="100%" GroupingEnabled="false" OnSelectedIndexChanging="gvOwnersAccount_SelectedIndexChanging"
                    OnItemDataBound="gvOwnersAccount_ItemDataBound" OnItemCommand="gvOwnersAccount_ItemCommand"
                    ShowFooter="false" ShowHeader="true" >
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDVOUCHERDETAILID">

                                  <%--  <div id="divGrid" style="position: relative; z-index: 1; width: 100%; height: 550px; top: 10px;">
                                        <asp:GridView ID="gvOwnersAccount" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                            CellPadding="3" AllowSorting="true" ShowHeader="true" EnableViewState="false"
                                            OnRowDataBound="gvOwnersAccount_RowDataBound" OnRowCommand="gvOwnersAccount_RowCommand"
                                            OnRowCreated="gvOwnersAccount_RowCreated" OnRowEditing="gvOwnersAccount_RowEditing"
                                            OnSelectedIndexChanging="gvOwnersAccount_SelectedIndexChanging"
                                            OnRowUpdating="gvOwnersAccount_RowUpdating" DataKeyNames="FLDVOUCHERDETAILID"
                                            OnRowCancelingEdit="gvOwnersAccount_RowCancelingEdit">
                                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                                            <RowStyle Height="10px" />--%>
                                            <Columns>
                                                <telerik:GridTemplateColumn>   
                                                    <HeaderStyle Width="30px" />                                                 
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
                                                     <HeaderStyle Width="70px" />      
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVoucherDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDATE") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Voucher Number">
                                                    <HeaderStyle Width="99px" />    
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblVoucherType" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISPHOENIXVOUCHER") %>'
                                                            Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVoucherDetailId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                                            CssClass="gridinput"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'
                                                            Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVoucherDtKey" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDTKEY") %>'
                                                            Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVoucherLineItemId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERLINEITEMID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblVoucherId" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERDETAILID") %>'
                                                            Visible="false"></telerik:RadLabel>
                                                        <asp:LinkButton ID="lblVoucherRow" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVOUCHERROW") %>'
                                                            CommandArgument="<%# Container.DataSetIndex %>" CommandName="Select"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Description">
                                                     <HeaderStyle Width="160px" />    
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblParticulars" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLONGDESCRIPTION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Amount">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                     <HeaderStyle Width="61px" />    
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAmount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNT","{0:n}") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Included">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                   <HeaderStyle Width="64px" />    
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblIncluded" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINCULDEINOWNERREPORTYN") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                    <EditItemTemplate>
                                                        <asp:CheckBox ID="chkIncludeEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDINCULDEINOWNERREPORT").ToString().Equals("1"))?true:false %>' />
                                                    </EditItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action">
                                                    <HeaderStyle HorizontalAlign="Center"  Width="89px"  VerticalAlign="Middle"></HeaderStyle>
                                                                                                      <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                                                    <ItemTemplate>
                                                        <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                                            ToolTip="Edit"></asp:ImageButton>
                                                        <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                        <asp:ImageButton runat="server" AlternateText="UnTag" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                                            CommandName="UNTAG" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdUntag"
                                                            ToolTip="UnTag"></asp:ImageButton>
                                                        <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAttachment"
                                                            ToolTip="Attachment" Visible="false"></asp:ImageButton>
                                                        <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdNoAttachment"
                                                            Visible="false" ToolTip="No Attachment"></asp:ImageButton>
                                                        <asp:ImageButton runat="server" AlternateText="Details" ImageUrl="<%$ PhoenixTheme:images/anyfile.png %>"
                                                            CommandName="DETAILS" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDetails"
                                                            Visible="false" ToolTip="Details"></asp:ImageButton>
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
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="400px"  />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
                                   
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 50%; vertical-align: top; min-height: 750px; border: 1px solid #CCC;">
                        <div>
                            <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 750px; width: 100%"
                                frameborder="0"></iframe>
                            <asp:HiddenField ID="hdnScroll" runat="server" />
                        </div>
                    </td>
                </tr>
            </table>
    </form>
</body>
</html>
