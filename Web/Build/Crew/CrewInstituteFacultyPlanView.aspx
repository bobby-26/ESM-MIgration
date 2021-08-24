<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewInstituteFacultyPlanView.aspx.cs" Inherits="Crew_CrewInstituteFacultyPlanView" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CldYear" Src="~/UserControls/UserControlCalenderYear.ascx" %>
<%@ Register TagPrefix="eluc" TagName="cldMonth" Src="~/UserControls/UserControlCalenderMonths.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Faculty Plan view</title>
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
        <eluc:TabStrip ID="MenuTitle" runat="server" OnTabStripCommand="MenuTitle_TabStripCommand"
            TabStrip="true"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:TabStrip ID="MenuFacultyPlanner" runat="server" TabStrip="false"></eluc:TabStrip>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table cellpadding="1" cellspacing="1" width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblInstitute" runat="server" Text="Institute"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListInstitute">
                            <telerik:RadTextBox ID="txtInstituteId" runat="server" Width="0"></telerik:RadTextBox>
                            <telerik:RadTextBox ID="txtInstituteName" runat="server" Width="350px" CssClass="input_mandatory" Enabled="false"></telerik:RadTextBox>
                             <asp:LinkButton runat="server" Style="cursor: pointer; vertical-align: top" ID="btnShowInstitute" ToolTip="Select Institute">
                                <span class="icon"><i class="fas fa-tasks-picklist"></i></span>
                            </asp:LinkButton>
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
                        <telerik:RadLabel ID="lblToviewtheGuidelinesplacethemouseonthe" runat="server" Text="To view the Guidelines, place the mouse on the"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadToolTip RenderMode="Lightweight" ID="btnTooltipHelp" runat="server" Height="100%" TargetControlID="btnHelp" HideDelay="10000">
                        </telerik:RadToolTip>
                        <asp:LinkButton runat="server" AlternateText="Guide" ID="btnHelp" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-info-circle"></i></span>
                        </asp:LinkButton>
                    </td>
                </tr>
            </table>
            <b>
                <telerik:RadLabel ID="lblTitle" runat="server" Text="Faculty"></telerik:RadLabel>
            </b>
            <div id="divGrid" style="position: relative; z-index: 0" runat="server">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvfaculty" runat="server" AllowCustomPaging="true" AllowSorting="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvfaculty_NeedDataSource" OnItemDataBound="gvfaculty_ItemDataBound"
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
                            <telerik:GridTemplateColumn HeaderText="Faculty" HeaderStyle-Width="65px">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFcaulty" runat="server" Text='<%# ((DataRowView)Container.DataItem)["FLDFACULTYNAME"].ToString() %>'></telerik:RadLabel>

                                    <telerik:RadLabel ID="lblFacultyId" runat="server" Visible="false"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTEFACULTYID") %>'>
                                    </telerik:RadLabel>
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

            </div>


            <asp:HiddenField ID="hdnCalendarId" runat="server" />
            <asp:HiddenField ID="hdnDate" runat="server" />
            <br />
            <b>
                <telerik:RadLabel ID="lblFacutyPlan" runat="server" Text="Course Plan"></telerik:RadLabel>
            </b>
            <br />

            <eluc:TabStrip ID="MenuFacultyCourse" runat="server" OnTabStripCommand="MenuFacultyCourse_TabStripCommand"></eluc:TabStrip>

            <div id="div2" runat="server" style="position: relative; z-index: 0">

                <telerik:RadGrid RenderMode="Lightweight" ID="gvCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="false"
                    CellSpacing="0" GridLines="None" OnItemCommand="gvCourse_ItemCommand" OnNeedDataSource="gvCourse_NeedDataSource"
                    OnItemDataBound="gvCourse_ItemDataBound" EnableViewState="false" ShowFooter="false">
                    <SortingSettings EnableSkinSortStyles="true"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" TableLayout="Fixed" DataKeyNames="FLDFACULTYCOURSEID,FLDCOURSEID">
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
                                    <telerik:RadLabel ID="lblFacultyCalendarId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYCALENDARID")%>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcourseCalendarId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSECALENDARID")%>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseCode" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDABBREVIATION")%>'></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblFacultyCalendarIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYCALENDARID")%>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblcourseCalendarIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSECALENDARID")%>' Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseCodeEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDABBREVIATION")%>'></telerik:RadLabel>
                                </EditItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Name">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <asp:LinkButton ID="lblCourse" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>'
                                        CommandName="SELECT"></asp:LinkButton>
                                    <telerik:RadLabel ID="lblCourseId" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSEID")%>' Visible="false"></telerik:RadLabel>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <telerik:RadLabel ID="lblCourseEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSE")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCourseIdEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDCOURSEID")%>' Visible="false"></telerik:RadLabel>
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
                                    <%--    <asp:TextBox ID="txtstarttime" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMEFROM","{0: hh:mm tt}") %>'
                                        CssClass="input" runat="server">
                                    </asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtStartTimeMaskEdit" runat="server" TargetControlID="txtstarttime"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />--%>
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
                                    <%-- <asp:TextBox ID="txtEndTime" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTIMETO","{0: hh:mm tt}") %>' runat="server"
                                        CssClass="input">
                                    </asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtEndTimeMaskEdit" runat="server" TargetControlID="txtEndTime"
                                        Mask="99:99" MaskType="Time" AcceptAMPM="true" ClearMaskOnLostFocus="false" ClearTextOnInvalid="false" />--%>
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
                                    <asp:LinkButton runat="server" AlternateText="Plan"
                                        CommandName="Plan" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCal"
                                        ToolTip="Plan" Width="20PX" Height="20PX" Visible="false">
                                <span class="icon"><i class="fas fa-calendar-plus"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Edit"
                                        CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                        ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <asp:LinkButton runat="server" AlternateText="Course Plan"
                                        CommandName="cmdCourse" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCourse"
                                        ToolTip="Course Plan" Width="20PX" Height="20PX">
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
                            <telerik:GridTemplateColumn HeaderText="Faculty">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblFaculty" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDFACULTYNAME")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Initial">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInitial" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDINITIAL")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Role">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRole" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDROLE")%>'></telerik:RadLabel>
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
            </div>
            <eluc:Status ID="ucStatus" runat="server" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
