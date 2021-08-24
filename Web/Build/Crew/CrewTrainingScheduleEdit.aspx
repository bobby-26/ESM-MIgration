<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewTrainingScheduleEdit.aspx.cs" Inherits="Crew_CrewTrainingScheduleEdit" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Training Schedule Edit</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager2" />
        <telerik:RadSkinManager ID="RadSkinManager2" runat="server" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <eluc:TabStrip ID="MenuTrainingSchedule" runat="server" OnTabStripCommand="MenuTrainingSchedule_TabStripCommand" Title="Training Schedule Edit"></eluc:TabStrip>
        <br />
        <table cellpadding="1" cellspacing="1" width="100%">
            <tr>
                <td>
                    <telerik:RadLabel ID="lbCourseCode" runat="server" Text="Course Code"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtCourseCode" CssClass="readonlytextbox" Visible="false"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCourseName" runat="server" Width="300px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    <telerik:RadTextBox ID="txtCourseId" runat="server" Visible="false" CssClass="hidden" Width="0"></telerik:RadTextBox>
                    <asp:LinkButton runat="server" AlternateText="Add Course Plan" CommandName="EDIT" ID="imgAddPlan"
                        ToolTip="Add Course Plan" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-calendar-alt"></i></span>
                    </asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                </td>
                <td>
                    <span id="spnPickListInstitute">
                        <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0" CssClass="hidden"></telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input_mandatory"></telerik:RadTextBox>
                        <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                        </asp:LinkButton>
                        <%--  <img runat="server" id="btnShowInstitute" style="cursor: pointer; vertical-align: top"
                            src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListInstitute', 'codehelp1', '', '../Common/CommonPickListInistituteList.aspx', true); " />--%>
                        <img id="Img2" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                        <asp:LinkButton runat="server" AlternateText="Add Institute Plan" CommandName="EDIT" ID="imgInstitutecal" ToolTip="Add Institute Plan" Width="20PX" Height="20PX">                            
                                <span class="icon"><i class="fas fa-calendar-alt"></i></span>
                        </asp:LinkButton>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblDuration" runat="server" Text="Duration"></telerik:RadLabel>
                </td>
                <td>
                    <eluc:Number runat="server" ID="txtDuration" CssClass="input_mandatory" MaxLength="3" IsInteger="true"></eluc:Number>
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
                                <eluc:Number ID="txtMinParticipant" runat="server" CssClass="input" MaxLength="3"
                                    IsInteger="true" />
                            </td>
                            <td>-
                            </td>
                            <td>
                                <eluc:Number ID="txtMaxParticipant" runat="server" CssClass="input" MaxLength="3"
                                    IsInteger="true" />
                                <telerik:RadLabel ID="lblcourseInstituteId" runat="server" Visible="true" Width="0"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <br />
        <b>
            <telerik:RadLabel ID="lblbatchFacultyDetails" runat="server" Text="Course Faculty"></telerik:RadLabel>
        </b>
        <eluc:TabStrip ID="MenuFaculty" runat="server"></eluc:TabStrip>
        <telerik:RadGrid RenderMode="Lightweight" ID="gvFaculty" runat="server" AllowCustomPaging="true" AllowSorting="true"
            CellSpacing="0" GridLines="None" OnItemCommand="gvFaculty_ItemCommand" OnNeedDataSource="gvFaculty_NeedDataSource" OnItemDataBound="gvFaculty_ItemDataBound"
            EnableViewState="true"  GroupingEnabled="false" EnableHeaderContextMenu="true">
            <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDINSTITUTEFACULTYID">
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
                    <telerik:GridTemplateColumn HeaderText="Name">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblCourseContactId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSECONTACTID")%>' Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFaculty" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYNAME")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Initial" HeaderStyle-Width="110px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFacultyInitial" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINITIAL")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Designation" HeaderStyle-Width="100px">
                        <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblFacultyRole" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROLE")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>

                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Plan"
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Plan" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-calendar-alt"></i></span>
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

        <eluc:Status ID="ucStatus" runat="server" />
    </form>
</body>
</html>
