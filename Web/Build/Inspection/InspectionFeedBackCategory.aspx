<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionFeedBackCategory.aspx.cs" Inherits="InspectionFeedBackCategory" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>FeedBack Category</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvFeedBackCategory").height(browserHeight - 40);
            });

        </script>
        <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFeedBackCategory.ClientID %>"));
               }, 200);
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
    <form id="frmFeedBackCategory" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager" runat="server"></telerik:RadSkinManager>
        <telerik:RadWindowManager runat="server" ID="RadWindowManager1" Localization-OK="Yes" Localization-Cancel="No" Width="99.9%"></telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Title runat="server" ID="ucTitle" Text="Feedback Category" Visible="false"></eluc:Title>
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFeedBackCategoryName" runat="server" Text="Feedback Category"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFeedBackCategoryName" Width="200px" CssClass="input" MaxLength="400"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="MenuInspectionFeedBackCategory" runat="server" OnTabStripCommand="InspectionFeedBackCategory_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFeedBackCategory" runat="server" AutoGenerateColumns="False" Font-Size="11px" AllowPaging="true" AllowCustomPaging="true"
                AllowSorting="true" Width="100%" CellPadding="3" ShowFooter="true" OnItemCommand="gvFeedBackCategory_ItemCommand" EnableHeaderContextMenu="true" GroupingEnabled="false"
                ShowHeader="true" EnableViewState="true" OnNeedDataSource="gvFeedBackCategory_NeedDataSource"
                OnItemDataBound="gvFeedBackCategory_ItemDataBound">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDFEEDBACKCATEGORYID">
                    <NoRecordsTemplate>
                        <table width="99.9%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="10%" HeaderStyle-HorizontalAlign="Center" AllowSorting="true" ShowSortIcon="true" SortExpression="FLDSERIALNUMBER">
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSerailNumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'
                                    Width="50px">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Number ID="txtSerialNumberEdit" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" Width="45px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSERIALNUMBER") %>'></eluc:Number>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Number ID="txtSerialNumberAdd" runat="server" CssClass="input"
                                    MaxLength="3" IsPositive="true" Width="45px"></eluc:Number>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Short Code" HeaderStyle-Width="20%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedbackShortcodeItem" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCODE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFeedbackShortCodeEdit" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKCODE") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFeedbackShortCodeAdd" runat="server" MaxLength="400" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Feedback Category" HeaderStyle-Width="40%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFeedbackCategoryName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCATEGORYNAME") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFeedbackCategoryId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKCATEGORYID") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtFeedbackCategoryNameEdit" runat="server" MaxLength="400" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFEEDBACKCATEGORYNAME") %>'
                                    CssClass="gridinput_mandatory" Width="98%">
                                </telerik:RadTextBox>
                                <telerik:RadLabel ID="lblFeedbackCategoryIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFEEDBACKCATEGORYID") %>'></telerik:RadLabel>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFeedbackCategoryNameAdd" runat="server" MaxLength="400" CssClass="gridinput_mandatory" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action Required" HeaderStyle-Width="75px" AllowSorting="true" SortExpression="FLDEUCOUNTRYYN">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblActionreq" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED").ToString().Equals("1"))?"Yes":"No" %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadCheckBox ID="chkActionreqEdit" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDACTIONREQUIRED").ToString().Equals("1"))?true:false %>' Width="100%"></telerik:RadCheckBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadCheckBox ID="chkActionreqNAdd" runat="server" Width="100%" MaxLength="6">
                                </telerik:RadCheckBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="20%"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="20%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                        <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel" ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" Width="50px" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="ADD" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
