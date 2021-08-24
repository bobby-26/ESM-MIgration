<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsAccessList.aspx.cs"
    Inherits="OptionsAccessList" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Department" Src="~/UserControls/UserControlDepartment.ascx" %>


<!DOCTYPE html >
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock></head>
<body>
     <form id="form1" runat="server">
     <telerik:RadFormDecorator ID="RadFormDecorator1" runat="server" DecorationZoneID="frmRegistersRank" DecoratedControls="All" />
   
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1">
        </telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="99%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
               
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAccessList" Height="98%" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvAccessList_ItemCommand" OnItemDataBound="gvAccessList_ItemDataBound" OnNeedDataSource="gvAccessList_NeedDataSource"
                ShowFooter="True" ShowHeader="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
                <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" >
                    <NoRecordsTemplate>
                        <table runat="server" width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                      <Columns>
                        <telerik:GridTemplateColumn HeaderText="Name" >
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblAccessID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSID") %>'></Telerik:RadLabel>
                                    <asp:LinkButton ID="lnkAccessName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSNAME") %>' CommandName="ACCESSLIST" ></asp:LinkButton>
                            </ItemTemplate>
                             <EditItemTemplate>
                                    <Telerik:RadLabel ID="lblAccessID" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSID") %>'></Telerik:RadLabel>
                                    <Telerik:RadTextBox ID="txtNameEdit" runat="server" CssClass="gridinput_mandatory" Width="280px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSNAME") %>'></Telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                 <Telerik:RadTextBox ID="txtNameAdd" runat="server" CssClass="gridinput_mandatory" Width="280px" MaxLength="200"></Telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Purpose">
                            <HeaderStyle Width="30%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                 <Telerik:RadLabel ID="lblPurpose" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <Telerik:RadTextBox ID="txtPurposeEdit" runat="server" CssClass="gridinput_mandatory" Width="280px" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPURPOSE") %>'></Telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <Telerik:RadTextBox ID="txtPurposeAdd" runat="server" Width="280px" CssClass="gridinput_mandatory"></Telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                         <telerik:GridTemplateColumn HeaderText="Department">
                            <HeaderStyle Width="20%" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <Telerik:RadLabel ID="lblDepartment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></Telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Department runat="server" CssClass="dropdown_mandatory" id="ucDepartmentEdit" Width="200px"  DepartmentList='<%# PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null)%>' SelectedDepartment='<%# DataBinder.Eval(Container,"DataItem.FLDACCESSTYPE") %>'></eluc:Department>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Department runat="server" id="ucDepartmentAdd" CssClass="gridinput_mandatory" Width="200px" DepartmentList='<%# PhoenixRegistersDepartment.Listdepartment(PhoenixSecurityContext.CurrentSecurityContext.UserCode, null)%>'></eluc:Department>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="100px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" ToolTip="Edit" Width="20PX" Height="20PX"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                              <%--  <asp:LinkButton runat="server" AlternateText="Delete" ToolTip="Delete" Width="20PX" Height="20PX"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>--%>
                                 <asp:LinkButton runat="server" AlternateText="Copy" ToolTip="Copy" Width="20PX" Height="20PX"
                                    CommandName="COPY" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCopy">
                                <span class="icon"><i class="fa-imagefile"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

