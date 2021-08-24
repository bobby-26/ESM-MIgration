<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewAcademicQualification.aspx.cs"
    Inherits="CrewAcademicQualification" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<!DOCTYPE html>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAcademic" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadFormDecorator RenderMode="Lightweight" ID="RadFormDecorator1" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />
        <eluc:TabStrip ID="MenuTab" runat="server" Title="Academics"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="91%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />

            <table width="100%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblEmployeeNumber" runat="server" Text="File No."></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                </tr>
            </table>
            <hr />
            <b>
                <telerik:RadLabel ID="lblAcademicDetails" runat="server" Text="Academic Details"></telerik:RadLabel>
            </b>
            <eluc:TabStrip ID="MenuCrewAcademic" runat="server" OnTabStripCommand="CrewAcademicMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewAcademics" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewAcademics_NeedDataSource" OnItemDataBound="gvCrewAcademics_ItemDataBound" Height="31%"
                OnItemCommand="gvCrewAcademics_ItemCommand" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                    ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDACADEMICID"
                    TableLayout="Fixed">
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
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Qualification" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAcademicsId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDACADEMICID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblAcademicsname" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>' CommandName="EDIT"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Place of Institution" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPlaceofInstitution" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Percentage" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="Lblgrade" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGRADE")  %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Pass Date" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFPASS","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="120px">
                            <ItemStyle Width="40px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            &nbsp;<b><telerik:RadLabel ID="lblPreSeaTraining" runat="server" Text="PreSea Training"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="CrewPreSeaCourse" runat="server" OnTabStripCommand="CrewPreSeaCourseMenu_TabStripCommand"></eluc:TabStrip>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvPreSeaCourse" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvPreSeaCourse_NeedDataSource" OnItemDataBound="gvPreSeaCourse_ItemDataBound" Height="31%"
                OnItemCommand="gvPreSeaCourse_ItemCommand" EnableViewState="false">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true"
                    ShowHeadersWhenNoRecords="true" AllowNaturalSort="false" AutoGenerateColumns="false" DataKeyNames="FLDPRESEACOURSEID"
                    TableLayout="Fixed">
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
                    <CommandItemSettings ShowAddNewRecordButton="false" ShowRefreshButton="false" />
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Qualification" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPreseacourseId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPRESEACOURSEID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCourseName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDQUALIFICATION") %>' CommandName="EDIT"></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Institution" HeaderStyle-Width="180px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblInstitution" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSTITUTIONNAME")%>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Length>20 ? DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString().Substring(0, 20) + "..." : DataBinder.Eval(Container, "DataItem.FLDREMARKS").ToString() %>'></telerik:RadLabel>
                                <eluc:ToolTip ID="ucToolTipRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    Width="220px" />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Percentage" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPercentage" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERCENTAGE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Pass Date" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblPassDate" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDPASSDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action" HeaderStyle-Width="120px">
                            <ItemStyle Width="40px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                             <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
                                             <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <br />
            &nbsp;<b><telerik:RadLabel ID="lblAwardandCertificate" runat="server" Text="Award and Certificate"></telerik:RadLabel>
            </b>
            <br />
            <eluc:TabStrip ID="CrewAwardandCertificate" runat="server" OnTabStripCommand="AwardandCertificateMenu_TabStripCommand"></eluc:TabStrip>

            <telerik:RadGrid RenderMode="Lightweight" ID="gvAwardAndCertificate" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true" Height="35%"
                CellSpacing="0" GridLines="None" OnNeedDataSource="gvAwardAndCertificate_NeedDataSource" OnItemDataBound="gvAwardAndCertificate_ItemDataBound"
                OnItemCommand="gvAwardAndCertificate_ItemCommand" EnableViewState="false" ShowFooter="true">
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
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblSlNo" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Award/Certificate">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblAwardId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblCertificate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCERTIFICATENAME") %>' CommandName="EDIT"></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblAwardIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAWARDID") %>'></telerik:RadLabel>
                                <eluc:Quick ID="ddlCertificateEdit" runat="server" CssClass="input_mandatory" QuickTypeCode="109" Width="100%"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Quick ID="ddlCertificateAdd" runat="server" QuickTypeCode="109" CssClass="input_mandatory" Width="100%"
                                    QuickList='<%#PhoenixRegistersQuick.ListQuick(1,109)%>' AppendDataBoundItems="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Issue Date" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblIssueDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDISSUEDATE", "{0:dd/MMM/yyyy}")%>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDATE") %>' />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" Width="100%" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Remarks">
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblRemarks" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtRemarksEdit" runat="server" CssClass="gridinput_mandatory" Width="100%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'
                                    MaxLength="200">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtRemarksAdd" runat="server" CssClass="gridinput_mandatory" Width="100%" MaxLength="200"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                            <ItemStyle Width="40px" Wrap="false" />
                            <ItemTemplate>
                                <asp:LinkButton runat="server" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                            <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                           <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>

                                <asp:LinkButton runat="server" CommandName="Attachment" ID="cmdAtt" ToolTip="Attachment">
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
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" CssClass="RadGrid_Default rgPagerTextBox" PagerTextFormat="{4}<strong>{5}</strong> Records Found" AlwaysVisible="true"
                        PageSizeLabelText="Records per page:" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true"
                    ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
            <eluc:Status runat="server" ID="ucStatus" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
