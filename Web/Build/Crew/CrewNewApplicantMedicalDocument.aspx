<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewNewApplicantMedicalDocument.aspx.cs"
    Inherits="CrewNewApplicantMedicalDocument" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.CrewManagement" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Documents" Src="~/UserControls/UserControlDocuments.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Flag" Src="~/UserControls/UserControlFlag.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Country" Src="~/UserControls/UserControlCountry.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vaccination" Src="~/UserControls/UserControlDocumentVaccination.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>New Applicant Medical Documents</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewMedical" runat="server" submitdisabledcontrols="true">

        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <telerik:RadAjaxPanel ID="panel1" runat="server">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <div id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%; position: absolute;">

                <eluc:TabStrip ID="CrewMedical" Title="Medical Document" runat="server"></eluc:TabStrip>

                <div>
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
                            <td>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <%--  <td>
                                <telerik:RadLabel ID="lblAppliedRank" runat="server" Text="Applied Rank"></telerik:RadLabel>
                            </td>
                            <td colspan="5">
                                <telerik:RadTextBox ID="txtRank" runat="server" ReadOnly="true" CssClass="readonlytextbox"></telerik:RadTextBox>
                            </td>--%>
                        </tr>
                    </table>
                </div>
                <hr />
                <b>Medical</b>
                <br />

                <eluc:TabStrip ID="MenuCrewMedical" runat="server" OnTabStripCommand="CrewMedical_TabStripCommand"></eluc:TabStrip>

                <div id="divGrid" style="position: relative; z-index: +3">
                    <%-- <asp:GridView ID="gvCrewMedical" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                    Width="100%" CellPadding="3" OnRowCommand="gvCrewMedical_RowCommand" OnRowDataBound="gvCrewMedical_RowDataBound"
                    OnRowDeleting="gvCrewMedical_RowDeleting" OnRowCancelingEdit="gvCrewMedical_RowCancelingEdit"
                    OnSelectedIndexChanging="gvCrewMedical_SelectedIndexChanging" OnRowEditing="gvCrewMedical_RowEditing"
                    ShowFooter="false" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvCrewMedical" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvCrewMedical_NeedDataSource"
                        OnItemDataBound="gvCrewMedical_ItemDataBound"
                        OnItemCommand="gvCrewMedical_ItemCommand"
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                <telerik:GridTemplateColumn ItemStyle-Width="15px" HeaderText="Status">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />

                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Medical">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblMedicalId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWFLAGMEDICALID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMedicalName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Place of Issue">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issue Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expiry">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>

                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Flag">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOUNTRYNAME") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle Width="100px" HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="MEDICALDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="MEDICALATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Archive"
                                            CommandName="MEDICALARCHIVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive"
                                            ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <br />
                <b>Medical Test</b>
                <%--<hr />--%>
                <br />

                <eluc:TabStrip ID="CrewMedicalTest" runat="server" OnTabStripCommand="CrewMedicalTest_TabStripCommand"></eluc:TabStrip>

                <div id="divPage1" style="position: relative; z-index: +2">
                    <%-- <asp:GridView ID="gvMedicalTest" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                    Width="100%" CellPadding="3" OnRowCommand="gvMedicalTest_RowCommand" OnRowDataBound="gvMedicalTest_RowDataBound"
                    OnRowDeleting="gvMedicalTest_RowDeleting" OnRowCancelingEdit="gvMedicalTest_RowCancelingEdit"
                    OnSelectedIndexChanging="gvMedicalTest_SelectedIndexChanging" OnRowEditing="gvMedicalTest_RowEditing"
                    OnRowUpdating="gvMedicalTest_RowUpdating" ShowFooter="true" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvMedicalTest" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvMedicalTest_NeedDataSource"
                        OnItemCommand="gvMedicalTest_ItemCommand"
                        OnItemDataBound="gvMedicalTest_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                <telerik:GridTemplateColumn ItemStyle-Width="15px" HeaderText="Status">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Medical Test">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblMedicalTestId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblMedicalTestName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblMedicalTestIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWMEDICALTESTID") %>'></telerik:RadLabel>
                                        <eluc:Documents runat="server" ID="ucMedicalTestEdit" CssClass="dropdown_mandatory"
                                            AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                            DocumentType="MEDICAL" SelectedDocument='<%# DataBinder.Eval(Container,"DataItem.FLDMEDICALTESTID") %>' Width="100%" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Documents runat="server" ID="ucMedicalTestAdd" CssClass="dropdown_mandatory"
                                            AppendDataBoundItems="true" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentMedical() %>'
                                            DocumentType="MEDICAL" Width="100%" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Place of Issue">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtPlaceOfIssueEdit" Width="100%" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtPlaceOfIssueAdd" Width="100%" runat="server" CssClass="input" MaxLength="200"
                                            ToolTip="Enter Place of issue">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issue Date">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date runat="server" ID="txtIssueDateEdit" Width="100%" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date runat="server" ID="txtIssueDateAdd" Width="100%" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expiry">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date runat="server" ID="txtExpiryDateEdit" Width="100%" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date runat="server" ID="txtExpiryDateAdd" Width="100%" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Status">
                                    <HeaderStyle Width="80px" />
                                    <ItemTemplate>
                                        <%# DataBinder.Eval(Container,"DataItem.FLDSTATUSNAME") %>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Hard runat="server" ID="ddlStatusEdit" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="100%"
                                            HardTypeCode="105" AutoPostBack="true" HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105) %>'
                                            SelectedHard='<%# DataBinder.Eval(Container,"DataItem.FLDSTATUS") %>' OnTextChangedEvent="Status_OnTextChangedEvent" />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Hard runat="server" ID="ddlStatusAdd" CssClass="dropdown_mandatory" AppendDataBoundItems="true" Width="100%"
                                            HardTypeCode="105" AutoPostBack="true" HardList='<%# PhoenixRegistersHard.ListHard(SouthNests.Phoenix.Framework.PhoenixSecurityContext.CurrentSecurityContext.UserCode, 105) %>'
                                            OnTextChangedEvent="Status_OnTextChangedEvent" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Remarks">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="220px"></ItemStyle>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox ID="txtRemarksEdit" runat="server" Width="100%" CssClass="gridinput" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS")%>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox ID="txtRemarksAdd" runat="server" Width="100%" CssClass="gridinput" MaxLength="200" ToolTip="Enter remarks"></telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Action">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="100px"></HeaderStyle>

                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Edit"
                                            CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                            ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="MEDICALTESTDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                     <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="MEDICALTESTATTCHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Archive"
                                            CommandName="MEDICALTESTARCHIVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive"
                                            ToolTip="Archive">
                                    <span class="icon"><i class="fas fa-archive"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>

                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="MEDICALTESTADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <br />
                <b>Vaccination</b>
                <%--<hr />--%>
                <br />

                <eluc:TabStrip ID="CrewVaccination" runat="server" OnTabStripCommand="CrewVaccination_TabStripCommand"></eluc:TabStrip>

                <div id="divPage2" style="position: relative; z-index: +1">
                    <%--  <asp:GridView ID="gvVaccination" runat="server" AutoGenerateColumns="False" Font-Size="11px" GridLines="None"
                    Width="100%" CellPadding="3" OnRowCommand="gvVaccination_RowCommand" OnRowDataBound="gvVaccination_RowDataBound"
                    OnRowDeleting="gvVaccination_RowDeleting" OnRowCancelingEdit="gvVaccination_RowCancelingEdit"
                    OnSelectedIndexChanging="gvVaccination_SelectedIndexChanging" OnRowEditing="gvVaccination_RowEditing"
                    OnRowUpdating="gvVaccination_RowUpdating" ShowFooter="true" ShowHeader="true"
                    EnableViewState="false">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                    <RowStyle Height="10px" />--%>
                    <telerik:RadGrid RenderMode="Lightweight" ID="gvVaccination" runat="server" Height="" AllowCustomPaging="false" AllowSorting="true" AllowPaging="false"
                        CellSpacing="0" GridLines="None" OnNeedDataSource="gvVaccination_NeedDataSource"
                        OnItemCommand="gvVaccination_ItemCommand"
                        OnItemDataBound="gvVaccination_ItemDataBound"
                        GroupingEnabled="false" EnableHeaderContextMenu="true"
                        AutoGenerateColumns="false">
                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                        <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                            AutoGenerateColumns="false" TableLayout="Fixed" ShowFooter="true">
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
                                <telerik:GridButtonColumn Text="DoubleClick" CommandName="SELECT" Visible="false" />
                                <telerik:GridTemplateColumn ItemStyle-Width="15px" HeaderText="Status">
                                    <HeaderStyle Width="50px" />
                                    <ItemTemplate>
                                        <asp:Image ID="imgFlag" runat="server" Visible="false" />
                                    </ItemTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Vaccination">
                                    <HeaderStyle Width="150px" />
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClick" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblCrewVaccinationId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWVACCINATIONID") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblIsAtt" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISATTACHMENT") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lblVaccinationName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton ID="lnkDoubleClickEdit" runat="server" Visible="false" CommandName="Edit"
                                            CommandArgument='<%# Container.DataSetIndex %>'></asp:LinkButton>
                                        <telerik:RadLabel ID="lblCrewVaccinationIdEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCREWVACCINATIONID") %>'></telerik:RadLabel>
                                        <asp:HiddenField ID="hdnVaccinationIdEdit" runat="server" Value='<%# DataBinder.Eval(Container,"DataItem.FLDVACCINATIONID") %>'></asp:HiddenField>
                                        <%-- <eluc:vaccination runat="server" id="ucVaccinationEdit" appenddatabounditems="true"
                                        cssclass="dropdown_mandatory" documentlist='<%# PhoenixRegistersDocumentMedical.ListDocumentVaccination() %>'
                                        selecteddocument='<%# DataBinder.Eval(Container,"DataItem.FLDVACCINATIONID") %>' />--%>
                                        <telerik:RadLabel ID="lblVaccinationNameEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFMEDICAL") %>'></telerik:RadLabel>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Vaccination runat="server" ID="ucVaccinationAdd" AppendDataBoundItems="true"
                                            CssClass="dropdown_mandatory" DocumentList='<%# PhoenixRegistersDocumentMedical.ListDocumentVaccination() %>'
                                            AutoPostBack="true" OnTextChangedEvent="ucVaccination_TextChangedEvent" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Place of Issue">
                                    <HeaderStyle Width="150px" />
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblPlaceOfIssue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <telerik:RadTextBox Width="100%" ID="txtPlaceOfIssueEdit" runat="server" CssClass="input" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPLACEOFISSUE")%>'>
                                        </telerik:RadTextBox>
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <telerik:RadTextBox Width="100%" ID="txtPlaceOfIssueAdd" runat="server" CssClass="input" MaxLength="200"
                                            ToolTip="Enter Place of issue">
                                        </telerik:RadTextBox>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Issued">
                                    <HeaderStyle Width="100px" />
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblIssueDate" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date Width="100%" runat="server" ID="txtIssueDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDISSUEDDATE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date Width="100%" runat="server" ID="txtIssueDateAdd" CssClass="input_mandatory" />
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                                <telerik:GridTemplateColumn HeaderText="Expiry">
                                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                    <HeaderStyle Width="100px" />

                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblExpiryDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE","{0:dd/MMM/yyyy}") %>'></telerik:RadLabel>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <eluc:Date Width="100%" runat="server" ID="txtExpiryDateEdit" CssClass="input_mandatory" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEXPIRYDATE") %>' />
                                    </EditItemTemplate>
                                    <FooterTemplate>
                                        <eluc:Date Width="100%" runat="server" ID="txtExpiryDateAdd" CssClass="input_mandatory" />
                                    </FooterTemplate>
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
                                            ToolTip="Edit">
                                     <span class="icon"><i class="fas fa-edit"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Delete"
                                            CommandName="VACCINATIONDELETE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdDelete"
                                            ToolTip="Delete">
                                    <span class="icon"><i class="fa fa-trash"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Attachment"
                                            CommandName="VACCINATIONATTACHMENT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAtt"
                                            ToolTip="Attachment">
                                     <span class="icon"><i class="fas fa-paperclip"></i></span>
                                        </asp:LinkButton>
                                        <asp:LinkButton runat="server" AlternateText="Archive"
                                            CommandName="VACCINATIONARCHIVE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdArchive"
                                            ToolTip="Archive">
                                     <span class="icon"><i class="fas fa-archive"></i></span>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                    <EditItemTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="UPDATE" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                            ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                        </asp:LinkButton>

                                        <asp:LinkButton runat="server" AlternateText="Cancel"
                                            CommandName="CANCEL" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                            ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                        </asp:LinkButton>
                                    </EditItemTemplate>
                                    <FooterStyle HorizontalAlign="Center" />
                                    <FooterTemplate>
                                        <asp:LinkButton runat="server" AlternateText="Save"
                                            CommandName="VACCINATIONADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                            ToolTip="Add New">
                                     <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                        </asp:LinkButton>
                                    </FooterTemplate>
                                </telerik:GridTemplateColumn>
                            </Columns>
                            <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="false" CssClass="RadGrid_Default rgPagerTextBox" />
                        </MasterTableView>
                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                        </ClientSettings>
                    </telerik:RadGrid>
                </div>
                <table cellpadding="1" cellspacing="1">
                    <tr>
                        <td>
                            <img id="Img1" src="<%$ PhoenixTheme:images/red.png%>" runat="server" />
                        </td>
                        <td>* Documents Expired
                        </td>
                        <td>
                            <img id="Img2" src="<%$ PhoenixTheme:images/yellow.png%>" runat="server" />
                        </td>
                        <td>* Documents Expiring in 120 Days
                        </td>
                    </tr>
                </table>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
