<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAppraisalPendingSeafarercomments.aspx.cs"
    Inherits="Crew_CrewAppraisalPendingSeafarercomments" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Pending Seafarer Comments</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" Visible="false" />
                <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />

                <eluc:TabStrip ID="MenuCrewAppraisal" runat="server" OnTabStripCommand="MenuCrewAppraisal_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <%-- <asp:GridView ID="gvAQ" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowCommand="gvAQ_RowCommand"
                        OnRowDataBound="gvAQ_RowDataBound" AllowSorting="true" OnSorting="gvAQ_Sorting"
                        EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None" EnableViewState="false" OnNeedDataSource="gvAQ_NeedDataSource"
                        OnItemCommand="gvAQ_ItemCommand"
                        OnItemDataBound="gvAQ_ItemDataBound"
                        OnSortCommand="gvAQ_SortCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                     
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <NoRecordsTemplate>
                                <table width="100%" border="0">
                                    <tr>
                                        <td align="center">
                                            <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                        </td>
                                    </tr>
                                </table>
                            </NoRecordsTemplate>
                            <HeaderStyle Width="102px" />
                            <Columns>
                                <telerik:GridTemplateColumn HeaderText=" ">
                                    <headerstyle width="50px" />
                                    <itemtemplate>
                                    <asp:CheckBox ID="ckbselect" runat="server" Checked="false"></asp:CheckBox>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <headerstyle horizontalalign="left" verticalalign="Middle" width="150px"></headerstyle>
                                   
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </itemtemplate>
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblemployeeid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblactiveyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDACTIVEYN")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblempname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lbtnvesselname" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                        Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandName="SELECT"></asp:LinkButton>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Name">
                                    <headerstyle width="150px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblempnameitem" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From">
                                    <headerstyle width="100px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To">
                                    <headerstyle width="100px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblTodate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Appraisal Date">
                                    <headerstyle width="100px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblAppraisaldate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Occassion">
                                    <headerstyle width="150px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblOccassion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOCCASSIONFORREPORT")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <headerstyle width="75px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblRankid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Promotion Y/N">
                                    <headerstyle wrap="true" width="75px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblIsRecommendPromo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Fit Y/N">
                                    <headerstyle wrap="true" width="75px" />
                                    <itemtemplate>
                                    <%#DataBinder.Eval(Container, "DataItem.FLDRECOMMENDEDSTATUSNAME")%>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Sent Email Count">
                                    <headerstyle width="50px" />
                                    <itemtemplate>
                                    <telerik:RadLabel ID="lblsentmailcount" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDMAILCOUNT")%>'></telerik:RadLabel>
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <headerstyle width="50px" />
                                 
                                    <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                    <itemtemplate>
                                   
                                    <asp:LinkButton runat="server" AlternateText="sent mail" 
                                        CommandName="SENDMAILTOCREW" CommandArgument='<%# Container.DataSetIndex %>'
                                        ID="cmdsendmail" Visible="false" ToolTip="Send mail to seafarer">
                                        <span class="icon"><i class="fas fa-envelope"></i></span>
                                    </asp:LinkButton>
                                    
                                </itemtemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" FrozenColumnsCount="4" ScrollHeight="415px" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                  
                </div>
            </div>
          </telerik:RadAjaxPanel>
    </form>
</body>
</html>
