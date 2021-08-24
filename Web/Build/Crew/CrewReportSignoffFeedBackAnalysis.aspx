<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewReportSignoffFeedBackAnalysis.aspx.cs"
    Inherits="Crew_CrewReportSignoffFeedBackAnalysis" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign off Feedback Report</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
         <script type="text/javascript">
            function Resize() {
                setTimeout(function () {
                    TelerikGridResize($find("<%= gvFeedBackQst.ClientID %>"));
                }, 200);
            }
            window.onresize = window.onload = Resize;

            function pageLoad(sender, eventArgs) {
                Resize();
            }
            </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSignoffFBQuestion" runat="server">
    <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status ID="ucStatus" runat="server" Text="" Visible="false" />
                
                        <eluc:TabStrip ID="CrewFeedBackMain" runat="server"></eluc:TabStrip>
                
            <table width="100%">
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblName" runat="server" Text="First Name"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblRank" runat="server" Text="Rank"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:radlabel ID="lblJoinedVessel" runat="server" Text="Vessel"></telerik:radlabel>
                        </td>
                        <td>
                           <telerik:RadTextBox runat="server" ID="txtVessel" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:radlabel ID="lblJoinedDate" runat="server" Text="Sign On Date"></telerik:radlabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtSignonDate" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
              
                        <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                              <telerik:RadGrid ID="gvFeedBackQst" runat="server" AutoGenerateColumns="False" Font-Size="11px"   OnItemDataBound="gvFeedBackQst_ItemDataBound"
                CellPadding="3" RowStyle-Wrap="false" ShowHeader="true" EnableViewState="false" CellSpacing="0" GridLines="None" GroupingEnabled="false" EnableHeaderContextMenu="true"
                OnNeedDataSource="gvFeedBackQst_NeedDataSource" RenderMode="Lightweight" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" TableLayout="Fixed">
                    <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel ForeColor="Black"   ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                    <Columns>

                           <telerik:GridTemplateColumn HeaderText="S.No"     HeaderStyle-Width="5%" >
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblsno" runat="server" ></telerik:RadLabel>
                                </ItemTemplate>
                                <ItemStyle Width="40px" HorizontalAlign="Center" />
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Feedback Questions" HeaderStyle-Width="30%" >

                                <ItemTemplate>
                                    <telerik:radlabel ID="lblQuestionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONID")%>'></telerik:radlabel>
                                    <telerik:radlabel ID="lblQuestionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDQUESTIONNAME")%>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Answer"  HeaderStyle-Width="30%"  >
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblOptionId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOPTIONID")%>'></telerik:radlabel>
                                    <telerik:radlabel ID="lblOptionName" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOPTIONNAME")%>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                           <telerik:GridTemplateColumn HeaderText="Comments"  HeaderStyle-Width="35%"  >
                                <ItemTemplate>
                                    <telerik:radlabel ID="lblquestioncomments" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCOMMENTS")%>'></telerik:radlabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                         </Columns>
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

        </telerik:RadAjaxPanel>
    </form>
</body>
</html>