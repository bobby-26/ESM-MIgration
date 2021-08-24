<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteCoursePlanView.aspx.cs" Inherits="Crew_CrewInstituteCoursePlanView" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Course Plan view</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuCoursePlanner" runat="server" OnTabStripCommand="MenuCoursePlanner_TabStripCommand"
            TabStrip="false"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Width="0" />

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListInstitute">
                            <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input_mandatory" AutoPostBack="false"></telerik:RadTextBox>
                            <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                            </asp:LinkButton>
                            <%--    <img runat="server" id="btnShowInstitute" style="cursor: pointer; vertical-align: top" alt=""
                                src="<%$ PhoenixTheme:images/picklist.png %>" onclick="return showPickList('spnPickListInstitute', 'codehelp1', '', '../Common/CommonPickListInistituteList.aspx', true); " />--%>
                        </span>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPlanBetween" runat="server" Text="Plan Between"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="txtPlanFrom" runat="server" CssClass="input_mandatory" />
                        <telerik:RadLabel ID="txtdash" runat="server" Text="-"></telerik:RadLabel>
                        <eluc:Date ID="txtPlanTo" runat="server" CssClass="input_mandatory" />
                    </td>
                </tr>
            </table>
            <br />
            <table style="color: Blue">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="Place the mouse pointer in this icon"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="to view Guidelines"></telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <b>
                <telerik:RadLabel ID="lblTitle" runat="server" Text="Course"></telerik:RadLabel>
            </b>
            <%--  <div id="divGrid" style="position: relative; z-index: 0" runat="server">--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvcourseView" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvcourseView_NeedDataSource" OnItemDataBound="gvcourseView_ItemDataBound"
                EnableViewState="false" EnableHeaderContextMenu="true">
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
                        <telerik:GridTemplateColumn HeaderText="Course" HeaderStyle-Width="110px">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseName" Text='<%# ((DataRowView)Container.DataItem)["FLDCOURSE"].ToString() %>' runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcourseId" runat="server" Visible="false"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOURSEID") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <asp:HiddenField ID="hdnCalendarId" runat="server" />
            <asp:HiddenField ID="hdnDate" runat="server" />
            <br />
            <b>
                <telerik:RadLabel ID="lblFacutyPlan" runat="server" Text="Faculty"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuFaculty" runat="server" OnTabStripCommand="MenuFacultyAdd_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvFaculty" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnItemCommand="gvFaculty_ItemCommand" OnNeedDataSource="gvFaculty_NeedDataSource"
                OnItemDataBound="gvFaculty_ItemDataBound" EnableViewState="false" ShowFooter="false">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDFACULTYCOURSEID,FLDFACULTYID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNo" runat="server" Width="40px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Name">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFacultyCalendarIdInsert"
                                    runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYCALENDARID")%>' Visible="false">
                                </telerik:RadLabel>
                                <asp:LinkButton ID="lblFaculty" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYNAME")%>'
                                    CommandName="SELECT"></asp:LinkButton>
                                <telerik:RadLabel ID="lblFacultyId" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYID")%>' Visible="false">
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFacultyEditId" runat="server"
                                    Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYID")%>' Visible="false">
                                </telerik:RadLabel>
                                <telerik:RadLabel ID="lblFacultyCalendarId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYCALENDARID")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblcourseCalendarId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSECALENDARID")%>' Visible="false"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblFacultyEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYNAME") %>' runat="server"></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Initial">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFacultyInitial" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINITIAL")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFacultyInitialEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINITIAL") %>' runat="server"></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Designation">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFacultyRole" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROLE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFacultyRoleEdit" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROLE") %>' runat="server"></telerik:RadLabel>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Start">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblStartTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEFROM","{0: hh:mm tt}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadMaskedTextBox runat="server" ID="txtstarttime" CssClass="input_mandatory" Width="70px" Mask="##:## LL"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEFROM","{0: hh:mm tt}") %>'>
                                </telerik:RadMaskedTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Finish">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblendTime" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMETO","{0: hh:mm tt}") %>'>
                                </telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadMaskedTextBox runat="server" ID="txtEndTime" CssClass="input_mandatory" Width="70px" Mask="##:## LL"
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMETO","{0: hh:mm tt}") %>'>
                                </telerik:RadMaskedTextBox>

                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Class No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRoomNo" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCLASSROOM")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRoomNo" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCLASSROOM") %>'
                                    runat="server" CssClass="input">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="110px">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Course Plan"
                                    CommandName="cmdFaculty" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdFaculty"
                                    ToolTip="Faculty Plan" Width="20PX" Height="20PX">
                                <span class="icon"><i class="far fa-calendar-alt"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save"
                                    CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                    ToolTip="Save" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel"
                                    CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                    ToolTip="Cancel" Width="20PX" Height="20PX">
                                 <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <b>
                <telerik:RadLabel ID="lblFacultyCourse" runat="server" Text="Faculty - Course Plan"></telerik:RadLabel>
            </b>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvFacultyCourse" runat="server" AllowCustomPaging="true" AllowSorting="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvFacultyCourse_NeedDataSource"
                OnItemDataBound="gvFacultyCourse_ItemDataBound" EnableViewState="true" GroupingEnabled="false" EnableHeaderContextMenu="true">
                <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDFACULTYCOURSEID">
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
                        <telerik:GridTemplateColumn HeaderText="S.No">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRowNo" runat="server" Width="40px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course Code">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourseCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDABBREVIATION")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Course">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" AlwaysVisible="true" PagerTextFormat="{4}<strong>{5}</strong> Records Found"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
