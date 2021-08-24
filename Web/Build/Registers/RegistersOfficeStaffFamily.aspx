<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RegistersOfficeStaffFamily.aspx.cs"
    Inherits="RegistersOfficeStaffFamily" %>

<!DOCTYPE html>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>City</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRegistersCity" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuOfficestaff" runat="server" OnTabStripCommand="MenuOfficestaff_TabStripCommand" TabStrip="True"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Family"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="85%">
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table width="100%">
                <tr>
                    <td>File No.</td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeNo" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>
                    <td>User Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtUsername" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Salutation</td>
                    <td>
                        <telerik:RadTextBox ID="txtSalutation" runat="server" ReadOnly="true" Text="" Width="98%" CssClass="readonlytextbox"></telerik:RadTextBox></td>

                </tr>
                <tr>
                    <td>First Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtfirstname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>Middle Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtmiddlename" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                    <td>last Name</td>
                    <td>
                        <telerik:RadTextBox ID="txtLastname" runat="server" ReadOnly="true" Text="" CssClass="readonlytextbox" Width="98%"></telerik:RadTextBox></td>
                </tr>
            </table>
            <br />
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewComponentTrack" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewComponentTrack_NeedDataSource" EnableViewState="false" ShowFooter="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDPERSONALINFOSN">
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
                        <telerik:GridTemplateColumn HeaderText="Salutation">
                            <ItemStyle Wrap="false" />
                            <ItemTemplate>
                                <%# DataBinder.Eval(Container, "DataItem.FLDSALUTATION")%>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDNAME") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Relation">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDRELATIONSHIP") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="D.O.B">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Passport No.">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued On">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issued Place">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDDATEOFISSUE") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry On">
                            <ItemTemplate>
                                <%#DataBinder.Eval(Container,"DataItem.FLDDATEOFEXPIRY") %>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" SaveScrollPosition="true" UseStaticHeaders="true" EnableNextPrevFrozenColumns="true" FrozenColumnsCount="2" EnableColumnClientFreeze="true"
                        ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
