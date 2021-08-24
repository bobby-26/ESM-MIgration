<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAppraisal.aspx.cs" Inherits="CrewOffshoreAppraisal" %>

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
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Appraisal</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>


</head>
<body>
    <form id="frmAppraisalQuestion" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <div class="navigation" id="Div1" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="CrewAppraisalMain" runat="server" OnTabStripCommand="CrewAppraisalMain_TabStripCommand"></eluc:TabStrip>

            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:ConfirmMessage ID="ucConfirm" runat="server" Text="" Visible="false" />
            <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            </div>
            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" Width="130px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" Width="130px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" Width="130px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="Employee Number"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" Width="130px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <telerik:RadTextBox runat="server" ID="txtRank" Width="130px" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td style="color: Blue" colspan="6">
                        <telerik:RadLabel ID="Literal1" Text="<b> Notes : </b>The attachment option to be used only for below reasons  <br/> 1. The seafarer was not able to put his comments onboard and the attached document has the comments.</br> 2. The whole appraisal was done manually and the attached, documents the appraisal complete in all aspects."
                            runat="server">
                        </telerik:RadLabel>
                    </td>
                </tr>
            </table>

            <eluc:TabStrip ID="MenuCrewAppraisal" runat="server" OnTabStripCommand="MenuCrewAppraisal_TabStripCommand"></eluc:TabStrip>

            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%--     <asp:GridView ID="gvAQ" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowCommand="gvAQ_RowCommand"
                        OnRowDataBound="gvAQ_RowDataBound" OnRowEditing="gvAQ_RowEditing" OnRowCancelingEdit="gvAQ_RowCancelingEdit"
                        OnRowUpdating="gvAQ_RowUpdating" AllowSorting="true" OnSorting="gvAQ_Sorting"
                        OnRowDeleting="gvAQ_RowDeleting" EnableViewState="false">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />--%>
                <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAQ" runat="server" Height="500px" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                        CellSpacing="0" GridLines="None"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        OnNeedDataSource="gvAQ_NeedDataSource" EnableViewState="false"
                        OnSortCommand="gvAQ_SortCommand"
                        OnItemCommand="gvAQ_ItemCommand"
                        OnItemDataBound="gvAQ_ItemDataBound"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed">
                            <HeaderStyle Width="102px" />
                            <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
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
                                <telerik:GridTemplateColumn HeaderText="Vessel">
                                    <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="150px"></HeaderStyle>
                              
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisalId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblemployeeid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDEMPLOYEEID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblvesselid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblactiveyn" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDACTIVEYN")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblrankid" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKID")%>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblempname" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lbtnvesselname" runat="server" CommandArgument='<%# Container.DataSetIndex %>'
                                            Text='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>' CommandName="SELECT"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisalIdEdit" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDCREWAPPRAISALID")%>'></telerik:RadLabel>
                                        <eluc:Vessel ID="ddlVesselEdit" runat="server" AppendDataBoundItems="true" VesselsOnly="true"
                                            CssClass="input_mandatory" VesselList="<%#PhoenixRegistersVessel.ListVessel() %>"
                                            SelectedVessel='<%#DataBinder.Eval(Container, "DataItem.FLDVESSELID")%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="From">

                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFromdate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDFROMDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtFromDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDFROMDATE","{0:dd/MMM/yyyy}")%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblTodate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDTODATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtToDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDTODATE","{0:dd/MMM/yyyy}")%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Appraisal Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAppraisaldate" runat="server" Text='<%# SouthNests.Phoenix.Framework.General.GetDateTimeToString(DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE", "{0:dd/MMM/yyyy}"))%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date ID="txtAppraisalDateEdit" runat="server" CssClass="input_mandatory" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALDATE","{0:dd/MMM/yyyy}")%>' />
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Occasion">
                                    <HeaderStyle Width="200px" />
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
                                <telerik:GridTemplateColumn HeaderText="Rank">
                                    <HeaderStyle Width="75px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRank" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>'></telerik:RadLabel>
                                        <%--<telerik:RadLabel ID="lblrankCode" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKCODE")%>'></telerik:RadLabel>--%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblRankEdit" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="To Be Promoted">
                                    <HeaderStyle Wrap="true" Width="5%" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIsRecommendPromo" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblIsRecommendPromoEdit" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDPROMOTIONYESNO")%>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblStatus" runat="server" Text='<%#DataBinder.Eval(Container, "DataItem.FLDAPPRAISALSTATUS")%>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Fit Y/N">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <%#DataBinder.Eval(Container, "DataItem.FLDRECOMMENDEDSTATUSNAME")%>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></HeaderStyle>
                                   
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                            ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                            ToolTip="Delete">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                        <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="Attachment" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment"></asp:ImageButton>

                                        <asp:LinkButton runat="server" AlternateText="Email"
                                            CommandName="SENDMAIL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEmail"
                                            Visible="false" ToolTip="Send mail to vessel">
                                            
                                             <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Email"
                                            CommandName="DEBRIEF" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDeBrief"
                                            Visible="false" ToolTip="Debrief the Seafarer">
                                            <span class="icon"><i class="fas fa-list-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="sent mail"
                                            CommandName="SENDMAILTOCREW" CommandArgument='<%# Container.DataSetIndex %>'
                                            ID="cmdsendmail" Visible="false" ToolTip="Send mail to seafarer">
                                             <span class="icon"><i class="fas fa-envelope"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                            ToolTip="Save">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>
                                        <img id="Img1" alt="" runat="server" src="<%$ PhoenixTheme:images/spacer.png%>" width="3" />
                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                            ToolTip="Cancel">
                                            <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
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
                </telerik:RadAjaxPanel>

            </div>
        </div>

    </form>
</body>
</html>
