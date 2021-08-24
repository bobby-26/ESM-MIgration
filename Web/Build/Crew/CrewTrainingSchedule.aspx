<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTrainingSchedule.aspx.cs" Inherits="CrewTrainingSchedule" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Schedule Add</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="90%">
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lbCourseCode" runat="server" Text="Course Code"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtCourseCode" CssClass="readonlytextbox" Visible="false"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCourseName" CssClass="readonlytextbox" runat="server" Width="300px"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtCourseId" runat="server" Visible="false"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListInstitute">
                            <telerik:RadTextBox ID="txtInstituteId" runat="server" CssClass="hidden" Width="0"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input_mandatory" Enabled="false"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks"></i></span>
                            </asp:LinkButton>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>
                    </td>
                    <td>&nbsp;<eluc:Number runat="server" ID="txtDuration" CssClass="input_mandatory" MaxLength="3" IsInteger="true"></eluc:Number>
                        <telerik:RadLabel ID="lbldays" runat="server" Text="Day(s)"></telerik:RadLabel>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblParticipant" runat="server" Text="Participants"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ID="lblMin" runat="server" Text="Min"></telerik:RadLabel>
                                </td>
                                <td></td>
                                <td align="center">
                                    <telerik:RadLabel ID="lblMax" runat="server" Text="Max"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <eluc:Number ID="txtMinParticipant" runat="server" CssClass="input"
                                        MaxLength="3" IsInteger="true" />
                                </td>
                                <td>-
                                </td>
                                <td>
                                    <eluc:Number ID="txtMaxParticipant" runat="server" CssClass="input"
                                        MaxLength="3" IsInteger="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <br />        
            <telerik:RadLabel ID="lblGridTitle" runat="server" Font-Bold="true" Text="Recommended Courses"></telerik:RadLabel>
            <eluc:TabStrip ID="MenuGrid" runat="server" OnTabStripCommand="MenuGrid_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnItemCommand="gvCourse_ItemCommand" OnNeedDataSource="gvCourse_NeedDataSource" Height="65%"
                OnItemDataBound="gvCourse_ItemDataBound" EnableViewState="false" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
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
                        <telerik:GridTemplateColumn HeaderText="S.No" HeaderStyle-Width="30px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSNo" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course Code" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDABBREVIATION") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course" HeaderStyle-Width="200px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblCourseName" runat="server"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSE") %>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblDocumentId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Recommended" HeaderStyle-Width="100px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton ID="lblCount" runat="server" Text='<%# (General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDCOUNT").ToString())<0)?
                                            -(General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDCOUNT").ToString())):
                                            General.GetNullableInteger(DataBinder.Eval(Container,"DataItem.FLDCOUNT").ToString()) %>'
                                    OnClick="lblCount_Click"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Enrolled">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblseafarersEnrolled" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENROLLED") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Expiry Date" Visible="false">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblExpiry" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action" Visible="false">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Batch List" Visible="false"
                                    CommandName="LIST" CommandArgument="<%# Container.DataSetIndex %>" ID="ImgBatch"
                                    ToolTip="Batch List" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-file-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="false"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
