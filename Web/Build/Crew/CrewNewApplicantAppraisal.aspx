<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantAppraisal.aspx.cs"
    Inherits="CrewNewApplicantAppraisal" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlCommonVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Occassion" Src="~/UserControls/UserControlOccassionForReport.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Appraisal</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
function Resize() {
setTimeout(function () {
TelerikGridResize($find("<%= gvAQ.ClientID %>"));
}, 200);
}
window.onresize = window.onload = Resize;

function pageLoad(sender, eventArgs) {
Resize();
fade('statusmessage');
}
</script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1"></telerik:RadScriptManager>
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTitle" runat="server" Title="Appraisal"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="88%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>         
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeFirstName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeMiddleName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtEmployeeLastName" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="5">
                        <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="color: Blue" colspan="6">
                        <telerik:RadLabel ID="lblNote" Text="<b> Notes:</b> The attachment option to be used only for below reasons  <br/>" runat="server"></telerik:RadLabel>
                    </td>
                    </tr>
                <tr>                   
                    <td style="color: Blue" colspan="6">
                        <telerik:RadLabel ID="Literal1" Text=" 1. The seafarer was not able to put his comments onboard and the attached document has the comments.</br> 2. The whole apprasial was done manually and the attached document is the apprasial complete in all aspects."
                            runat="server">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>
            <eluc:TabStrip ID="CrewAppraisal" runat="server" OnTabStripCommand="CrewAppraisal_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server" EnableViewState="false" 
                AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" CellSpacing="0" GridLines="None" GroupingEnabled="false"
                OnNeedDataSource="gvAQ_NeedDataSource" EnableHeaderContextMenu="true" ShowFooter="false" AutoGenerateColumns="false"
                OnItemCommand="gvAQ_ItemCommand" OnItemDataBound="gvAQ_ItemDataBound"  >
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="200px"></HeaderStyle>
                            <HeaderTemplate>
                                <asp:LinkButton ID="lnkvessel" runat="server" CommandName="Sort" CommandArgument="FLDVESSELID">Vessel Name&nbsp;</asp:LinkButton>
                                <%--    <img id="FLDVESSELID" runat="server" visible="false" />--%>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                <asp:LinkButton ID="lbtnvesselname" runat="server" 
                                    Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandName="SELECT"></asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAppraisalIdEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                <eluc:Vessel ID="ddlVesselEdit" runat="server" AppendDataBoundItems="true" VesselsOnly="true" CssClass="input_mandatory"
                                    EntityType="VSL" ActiveVessels="true" SelectedVessel='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtFromDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="To Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblTodate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtToDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE","{0:dd/MMM/yyyy}")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Appraisal Date">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAppraisaldate" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="txtAppraisalDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE","{0:dd/MMM/yyyy}")%>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Occasion For Report">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblOccassion" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDOCCASSIONFORREPORT")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Occassion ID="ddlOccassionForReportedit" runat="server" CssClass="dropdown_mandatory"
                                    AppendDataBoundItems="true" SelectedOccassion='<%# DataBinder.Eval(Container, "DataItem.FLDOCCASIONID").ToString() %>'
                                    OccassionList='<%# PhoenixRegistersMiscellaneousAppraisalOccasion.ListMiscellaneousAppraisalOccasion(
                                            General.GetNullableInteger(ViewState["Rcategory"].ToString()) == null ? 0 : int.Parse(ViewState["Rcategory"].ToString())) %>'
                                    Category='<%# ViewState["Rcategory"].ToString() %>' />
                            </EditItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn>
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action                                           
                                </telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit"
                                    CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                    ToolTip="Edit" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete"
                                    CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                    ToolTip="Delete" Width="20PX" Height="20PX">
                                <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="Attachment" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdAtt"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                               <span class="icon"><i class="fas fa-paperclip"></i></span>
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass=" RadGrid_Default rgPagerTextBox" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
