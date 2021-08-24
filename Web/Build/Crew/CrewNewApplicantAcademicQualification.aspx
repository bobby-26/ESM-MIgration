<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantAcademicQualification.aspx.cs"
    Inherits="CrewNewApplicantAcademicQualification" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlDate" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlQualificaiton" Src="~/UserControls/UserControlQualification.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewNewApplicantAcademic" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:TabStrip ID="CrewAcademic" Title="Licence Document" runat="server"></eluc:TabStrip>

            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <div>
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
                        <td>
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <%-- <td>
                            <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="5">
                            <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                        </td>--%>
                    </tr>
                </table>
            </div>
            <hr />
            <br />
            <b>
                <telerik:RadLabel ID="lblAcademicDetails" runat="server" Text="Academic Details"></telerik:RadLabel>
            </b>

            <eluc:TabStrip ID="MenuCrewAcademic" runat="server" OnTabStripCommand="CrewAcademicMenu_TabStripCommand"></eluc:TabStrip>

            <%-- <asp:GridView ID="gvCrewAcademics" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                EnableViewState="false" Width="100%" CellPadding="3" OnRowDataBound="gvCrewAcademics_RowDataBound"
                OnRowDeleting="gvCrewAcademics_RowDeleting" OnRowEditing="gvCrewAcademics_RowEditing"
                OnRowCommand="gvCrewAcademics_RowCommand" Style="margin-bottom: 0px">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewAcademics" runat="server" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewAcademics_NeedDataSource" OnItemDataBound="gvCrewAcademics_ItemDataBound" Height=""
                OnItemCommand="gvCrewAcademics_ItemCommand"
                GroupingEnabled="false" EnableHeaderContextMenu="true"
                EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                    ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDACADEMICID"
                    TableLayout="Fixed">
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
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>

                        <telerik:GridTemplateColumn HeaderText="Qualification">
                            <HeaderStyle Width="150px" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAcademicsId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <asp:LinkButton ID="lblAcademicsname" runat="server" CommandArgument='<%#Container.DataSetIndex%>'
                                    Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>' CommandName="EDIT"></asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Institution">
                            <HeaderStyle Width="150px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceofInstitution" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="From Date">
                            <HeaderStyle Width="100px" />
                            <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                            <HeaderTemplate>
                                <telerik:RadLabel ID="lblPercentageHeader" runat="server" Text="Percentage/Grade"></telerik:RadLabel>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="Lblgrade" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pass Date">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <HeaderStyle Width="100px" />

                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFPASS","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Action">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            &nbsp;<b><telerik:RadLabel ID="lblPreSeaTraining" runat="server" Text="PreSea Training"></telerik:RadLabel>
            </b>
            <br />

            <eluc:TabStrip ID="CrewPreSeaCourse" runat="server" OnTabStripCommand="CrewPreSeaCourseMenu_TabStripCommand"></eluc:TabStrip>

            <div id="divCourse" style="position: relative; z-index: +1">
                <telerik:RadGrid RenderMode="Lightweight" ID="gvPreSeaCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None" OnNeedDataSource="gvPreSeaCourse_NeedDataSource" OnItemDataBound="gvPreSeaCourse_ItemDataBound" Height=""
                    OnItemCommand="gvPreSeaCourse_ItemCommand"
                    GroupingEnabled="false" EnableHeaderContextMenu="true"
                    EnableViewState="false">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                        ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDPRESEACOURSEID"
                        TableLayout="Fixed">
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
                        <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                        <Columns>

                            <telerik:GridTemplateColumn HeaderText="Qualification">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPreseacourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEACOURSEID") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                    <asp:LinkButton ID="lblCourseName" runat="server" CommandArgument='<%#Container.DataSetIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>' CommandName="EDIT"></asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Institution">
                                <HeaderStyle Width="150px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME")%>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Remarks">
                                <HeaderStyle Width="150px" />
                                <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                    <eluc:ToolTip ID="ucToolTipRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                        Width="220px" />
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Percentage">
                                <HeaderStyle Width="100px" />
                                <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>

                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Pass Date">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <telerik:RadLabel ID="lblPassDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="120px">
                                <ItemStyle Width="40px" Wrap="false" />
                                <ItemTemplate>
                                    <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                    </asp:LinkButton>

                                    <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                        ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>
            </div>

            <div id="divAwards" runat="server">
                <br />
                &nbsp;<b><telerik:RadLabel ID="lblAwardandCertificate" runat="server" Text="Award and Certificate"></telerik:RadLabel>
                </b>
                <br />

                <eluc:TabStrip ID="CrewAwardandCertificate" runat="server" OnTabStripCommand="AwardandCertificateMenu_TabStripCommand"></eluc:TabStrip>

                <div id="div2" style="position: relative; z-index: 1">

                    <telerik:RadGrid RenderMode="Lightweight" ID="gvAwardAndCertificate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height=""
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvAwardAndCertificate_NeedDataSource" OnItemDataBound="gvAwardAndCertificate_ItemDataBound"
                        OnItemCommand="gvAwardAndCertificate_ItemCommand"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        EnableViewState="false" ShowFooter="true">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <GroupingSettings ShowUnGroupButton="true"></GroupingSettings>
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

                                <telerik:GridTemplateColumn HeaderText="Sl no">
                                    <ItemStyle Wrap="False" HorizontalAlign="Right"></ItemStyle>
                                    <HeaderStyle Width="30px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Award/Certificate">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblAwardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                        <asp:LinkButton ID="lblCertificate" runat="server" CommandArgument='<%#Container.DataSetIndex%>'
                                            Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' CommandName="EDIT"></asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Quick ID="ddlCertificateEdit" runat="server" CssClass="input_mandatory" QuickTypeCode="109"
                                            QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Quick ID="ddlCertificateAdd" runat="server" QuickTypeCode="109" CssClass="input_mandatory"
                                            QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issue Date">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDISSUEDATE", "{0:dd/MMM/yyyy}")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadLabel ID="lblAwardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                        <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container, "DataItem.FLDREMARKS")%>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="gridinput_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                            MaxLength="200">
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput_mandatory" MaxLength="200"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="120px">
                                    <ItemStyle Width="40px" Wrap="false" />
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdXEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdXDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash-alt"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdXAtt" ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" CommandName="Update" ID="cmdUpdate" ToolTip="Update">
                                            <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                            <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save" ID="cmdAdd" CommandName="Add" ToolTip="Add New" Width="20px" Height="20px">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                            ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>

            </div>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
