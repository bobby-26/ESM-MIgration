<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersSignoffFeedBackQuestionOptions.aspx.cs"
    Inherits="Registers_RegistersSignoffFeedBackQuestionOptions" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Off FeedBack Question Options</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
      <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
          <script type="text/javascript">
            $(document).ready(function () {
                var browserHeight = $telerik.$(window).height();
                $("#gvFBQOptions").height(browserHeight - 40);
            });
        </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmRegistersFBQOptions" runat="server" submitdisabledcontrols="true">
    <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />      
        <telerik:RadAjaxPanel ID="RadAjaxPanel" runat="server" Height="93%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>          
                <table>
                    <tr>
                        <td style="width: 50px;">
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblQuestionHeader" runat="server" Text="Question"></telerik:RadLabel>
                        </td>
                        <td style="width: 20px;">
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtQuestion" runat="server" Width="500px" CssClass="readonlytextbox"
                                ReadOnly="true" TextMode="MultiLine" Height="30px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>               
                    <eluc:TabStrip ID="MenuRegistersFBQOptions" runat="server" OnTabStripCommand="MenuRegistersFBQOptions_TabStripCommand">
                    </eluc:TabStrip>               
                <telerik:RadGrid RenderMode="Lightweight" ID="gvFBQOptions" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvFBQOptions_ItemCommand" OnNeedDataSource="gvFBQOptions_NeedDataSource" Height="88%"
                OnItemDataBound="gvFBQOptions_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true" ShowFooter="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" ItemStyle-Wrap="false" ItemStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderStyle-HorizontalAlign="Center">
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
                            <%--<asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />--%>
                            <telerik:GridTemplateColumn HeaderText="Order No" HeaderStyle-Width="100px">
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                <FooterStyle HorizontalAlign="Center" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblOrderNoHeader" runat="server" Text="Order No"></telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOrderNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtOrderNoEdit" runat="server" CssClass="input_mandatory" IsPositive="true"
                                        IsInteger="true" Width="30px" MaxLength="3" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERNO") %>' />
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <eluc:Number ID="txtOrderNoAdd" runat="server" CssClass="input_mandatory" IsPositive="true"
                                        IsInteger="true" Width="30px" MaxLength="3" />
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Option" HeaderStyle-Width="300px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:LinkButton ID="lblHeader" runat="server" CommandName="Sort" CommandArgument="FLDOPTIONNAME">
                                        Option&nbsp;</asp:LinkButton>
                                   <%-- <img id="FLDOPTIONNAME" runat="server" visible="false" />--%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblOptionId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lnkOptionName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblOptionIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONID") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtOptionEdit" runat="server" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPTIONNAME") %>' Width="400px"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtOptionAdd" runat="server" CssClass="input_mandatory" ToolTip="Add Option" Width="400px"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="70px"> 
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                    </telerik:RadLabel>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center"></ItemStyle>
                                 <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Save" Width="20PX" Height="20PX"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" ToolTip="Cancel" Width="20PX" Height="20PX"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" ToolTip="Add New" Width="20PX" Height="20PX"
                                    CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>          
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
               <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
