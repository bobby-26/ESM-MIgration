    
<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreQuick.aspx.cs" Inherits="CrewOffshoreQuick" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="QuickType" Src="~/UserControls/UserControlQuickType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>PreSea Quick</title><telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
    </head>
<body>
    <form id="frmOffshoreQuick" runat="server" autocomplete="off">
    <telerik:RadScriptManager ID="radscript1" runat="server">
    </telerik:RadScriptManager>
    <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <eluc:Title runat="server" ID="ucTitle" Text="Other Registers" Visible="false"></eluc:Title>
                    <table id="tblConfigureQuick" >
                        <tr>
                            <td width="20%">
                                <telerik:RadLabel ID="lblRegister" runat="server" Text="Register"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:QuickType ID="ucQuickType" runat="server" AppendDataBoundItems="false" AutoPostBack="true"
                                     OnTextChangedEvent="cmdSearch_Click" />
                            </td>
                        </tr>
                    </table>
                                
                    <eluc:TabStrip ID="MenuOffshoreQuick" runat="server" OnTabStripCommand="OffshoreQuick_TabStripCommand">
                    </eluc:TabStrip>
              
                    <telerik:RadGrid ID="gvQuick" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnItemCommand="gvQuick_ItemCommand" OnItemDataBound="gvQuick_ItemDataBound"
                        AllowSorting="true" OnSorting="gvQuick_Sorting" ShowFooter="true"
                        ShowHeader="true" EnableViewState="false"
                        AllowPaging="true" AllowCustomPaging="true" OnNeedDataSource="gvQuick_NeedDataSource"
                         RenderMode="Lightweight"  GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true" >

                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage"
                    HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed"  >
                    <NoRecordsTemplate>
                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger"
                            Font-Bold="true">
                        </telerik:RadLabel>
                    </NoRecordsTemplate>
                    <HeaderStyle Width="102px" />

                        <Columns>
                            
                            <telerik:GridTemplateColumn FooterText="New Short"  HeaderText="Code"   AllowSorting="true" SortExpression="FLDSHORTNAME">
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="35%" />
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                <HeaderTemplate>
                                    <telerik:RadLabel ID="lblQuickCodeHeader" Visible="true" runat="server">
                                        <asp:ImageButton runat="server" ID="cmdAbbreviation" OnClick="cmdSearch_Click" CommandName="FLDSHORTNAME"
                                            ImageUrl="<%$ PhoenixTheme:images/spacer.png %>" />
                                    </telerik:RadLabel>
                                   
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkShortName" runat="server" CommandName="EDIT" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadTextBox ID="txtShortNameEdit" runat="server" Width="100%" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSHORTNAME") %>'></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtShortNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="10" Width="100%"
                                        ToolTip="Enter Quick Name"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn FooterText="New Quick"  HeaderText="Name"   AllowSorting="true" SortExpression="FLDQUICKNAME"   >
                                <ItemStyle Wrap="False" HorizontalAlign="Left" ></ItemStyle>
                                <HeaderStyle Wrap="true" HorizontalAlign="Left" Width="55%"/>
                                <FooterStyle Wrap="true" HorizontalAlign="Left" />
                                
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblQuickTypeCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKTYPECODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuickCode" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKCODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuickName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblQuickTypeCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKTYPECODE") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblQuickCodeEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKCODE") %>'></telerik:RadLabel>
                                    <telerik:RadTextBox ID="txtQuickNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUICKNAME") %>' Width="100%"
                                        CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                                </EditItemTemplate>
                                <FooterTemplate>
                                    <telerik:RadTextBox ID="txtQuickNameAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200" Width="100%"
                                        ToolTip="Enter Quick Name"></telerik:RadTextBox>
                                </FooterTemplate>
                            </telerik:GridTemplateColumn>
                            
                           <telerik:GridTemplateColumn HeaderText="Action" >
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit"
                                    ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete"
                                    ToolTip="Delete">
                                         <span class="icon"><i class="fa fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave"
                                    ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                    width="3" />
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="CANCEL" ID="cmdCancel"
                                    ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" ToolTip="Add New" CommandName="Add"
                                    ID="cmdAdd">
                                        <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                       </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true"
                    AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4"
                        ScrollHeight="415px" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
     
    </telerik:RadAjaxPanel>
    </form>
</body>
</html>