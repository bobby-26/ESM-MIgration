<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionRAProcessExtn.aspx.cs" Inherits="InspectionRAProcessExtn" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Quick" Src="~/UserControls/UserControlQuick.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Company" Src="~/UserControls/UserControlCompany.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Category" Src="~/UserControls/UserControlRACategoryExtn.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Risk Assessment Process</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script language="javascript" type="text/javascript">
            function TxtMaxLength(text, maxLength) {
                text.value = text.value.substring(0, maxLength);
            }

        </script>
        <style type="text/css">
            table.Hazard {
                border-collapse: collapse;
            }

                table.Hazard td, table.Hazard th {
                    border: 1px solid;
                    border-color: rgb(30, 57, 91);
                    padding: 5px;
                }
        </style>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmProcess" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" LoadingPanelID="RadAjaxLoadingPanel1" Height="100%">
            <eluc:TabStrip ID="MenuProcess" runat="server" OnTabStripCommand="MenuProcess_TabStripCommand" Title="Details"></eluc:TabStrip>
            <eluc:Status runat="server" ID="ucStatus" />
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <table runat="server" id="main" width="99.5%">

                <tr>
                    <td width="15%">
                        <telerik:RadLabel ID="lblRefNo" runat="server" Text="Ref Number"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <telerik:RadTextBox runat="server" Width="230px" CssClass="readonlytextbox" ID="txtRefNo" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblRevisionNo" runat="server" Text="Revision No"></telerik:RadLabel>
                        <telerik:RadTextBox runat="server" Width="60px" CssClass="readonlytextbox" ID="txtRevNO" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td width="15%">
                        <telerik:RadLabel ID="lblDayNight" runat="server" Text="Day/Night"></telerik:RadLabel>
                    </td>
                    <td width="35%">
                        <eluc:Quick ID="UCDayNight" runat="server" AppendDataBoundItems="true" QuickTypeCode="168" Width="360px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblPreparedBy" runat="server" Text="Prepared By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="230px" CssClass="readonlytextbox" ID="txtpreparedby" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblDate" runat="server" Text="On"></telerik:RadLabel>
                        <eluc:Date ID="ucCreatedDate" CssClass="readonlytextbox" runat="server" ReadOnly="true" Enabled="false" Width="100px" DatePicker="false" TimeProperty="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblVisibility" runat="server" Text="Visibility"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCVisibility" runat="server" AppendDataBoundItems="true" QuickTypeCode="171" Width="360px" />
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblApprovedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="240px" CssClass="readonlytextbox" ID="txtApprovedby" ReadOnly="true"></telerik:RadTextBox>
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblDate1" runat="server" Text="Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="ucApprovedDate" CssClass="readonlytextbox" runat="server"
                            ReadOnly="true" Enabled="false" />
                    </td>
                    <td colspan="2"></td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblIssuedBy" runat="server" Text="Approved By"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox runat="server" Width="230px" CssClass="readonlytextbox" ID="txtIssuedBy" ReadOnly="true"></telerik:RadTextBox>
                        <telerik:RadLabel ID="lblDate2" runat="server" Text="On"></telerik:RadLabel>
                        <eluc:Date ID="ucIssuedDate" CssClass="readonlytextbox" runat="server" ReadOnly="true" Width="100px" Enabled="false" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblSteering" runat="server" Text="Steering"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucSteering" runat="server" AppendDataBoundItems="true" QuickTypeCode="173" Width="360px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblProcess" runat="Server" Text="Process"></telerik:RadLabel>
                    </td>
                    <td>                        
                        <telerik:RadComboBox ID="ucCategory" runat="server" CssClass="dropdown_mandatory" OnTextChanged="ucCategory_TextChangedEvent" AppendDataBoundItems="true"
                            DataTextField="FLDNAME" DataValueField="FLDCATEGORYID" Width="360px" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="True" >
                        </telerik:RadComboBox>
                        <telerik:RadTextBox ID="txtActivity" runat="server" CssClass="readonlytextbox" Width="360px" ReadOnly="true" Visible="false"></telerik:RadTextBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblWX" runat="server" Text="WX"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCWX" runat="server" AppendDataBoundItems="true" QuickTypeCode="170" Width="360px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCategory" runat="server" Text="Activity"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick runat="server" ID="ucProcess" AppendDataBoundItems="true" CssClass="input_mandatory"
                            QuickTypeCode="92" Visible="false" />
                        <telerik:RadTextBox ID="txtProcess" runat="server" CssClass="input_mandatory" Width="360px"
                            Visible="false">
                        </telerik:RadTextBox>
                        <telerik:RadComboBox ID="ddlCategory" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true"
                            AutoPostBack="True" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" Width="360px" EmptyMessage="Type to select Activity" Filter="Contains" MarkFirstMatch="true">
                            <Items>
                                <telerik:RadComboBoxItem Text="--Select--" Value="DUMMY"></telerik:RadComboBoxItem>
                            </Items>
                        </telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblUMSPermitted" runat="server" Text="UMS Permitted"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="UCUMSPermitted" runat="server" AppendDataBoundItems="true" QuickTypeCode="172" Width="360px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblSupervision" runat="server" Text="Supervision Level"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlsupervisionlist" runat="server" AppendDataBoundItems="true" CssClass="dropdown_mandatory" Width="240px" Filter="Contains" MarkFirstMatch="true"></telerik:RadComboBox>
                        &nbsp;&nbsp
                        <eluc:Company ID="ucCompany" runat="server" CssClass="input_mandatory" AppendDataBoundItems="true" />
                        &nbsp;&nbsp
                                    <asp:LinkButton ID="lnkcomment" runat="server" ToolTip="Comments" Visible="false">
                                                    <span class="icon"><i class="fa fa-comments"></i></span>
                                    </asp:LinkButton>
                    </td>
                     <td>
                        <telerik:RadLabel ID="lblMachineryStatus" runat="server" Text="Machinery Status"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Quick ID="ucMachineryStatus" runat="server" AppendDataBoundItems="true" QuickTypeCode="174" Width="360px" />
                    </td>
                </tr>                
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone7" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock7" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblAdditionalControls" Text="Controls" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td align="center" width="32%" valign="top">
                                                <telerik:RadLabel ID="lblFormscheck" runat="server" Style="background-color: #c6e0b4; width: 99%;">Forms and CheckList</telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvForms" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvForms_ItemCommand" OnNeedDataSource="gvForms_NeedDataSource" OnItemDataBound="gvForms_ItemDataBound" AllowSorting="false" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMPOSTERID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <span id="spnPickListDocument">
                                                                        <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments" runat="server" ToolTip="Select Forms and Checklists">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action" HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="FORMDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="FORMADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="32%" valign="top">
                                                <telerik:RadLabel ID="lbldocumentprocedures" runat="server" Style="background-color: #c6e0b4; width: 99%;" Text="Procedures"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvprocedure" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvprocedure_ItemCommand" OnNeedDataSource="gvprocedure_NeedDataSource" OnItemDataBound="gvprocedure_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMPOSTERID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <span id="spnPickListDocument1">
                                                                        <telerik:RadTextBox ID="txtDocumentName1" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments1" runat="server" ToolTip="Select Procedures">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId1" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action" HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="PROCEDUREDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="PROCEDUREADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="32%" rowspan="2" valign="top">
                                                <table width="100%">
                                                    <tr>
                                                        <td colspan="2" width="100%" valign="top" align="center">
                                                            <telerik:RadLabel ID="lblPPE" runat="server" Style="background-color: #fbffe1; width: 99%;" Text="Recommended PPE"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td width="50%" valign="top" align="center">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPPE" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvPPE_NeedDataSource" OnItemDataBound="gvPPE_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>   
                                                            </td>
                                                        <td width="50%" valign="top" align="center">
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvPPE2" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvPPE_NeedDataSource" OnItemDataBound="gvPPE2_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                        </tr>
                                        <tr>
                                            <td align="center" valign="top">
                                                <telerik:RadLabel ID="lblworkpermit" runat="server" Style="background-color: #c6e0b4; width: 99%;" Text="Work Permit"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvworkpermit" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvworkpermit_ItemCommand" OnNeedDataSource="gvworkpermit_NeedDataSource" OnItemDataBound="gvworkpermit_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">

                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="10%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="80%">
                                                                <ItemStyle Wrap="true" HorizontalAlign="Left" Width="80%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <span id="spnPickListDocument5">
                                                                        <telerik:RadTextBox ID="txtDocumentName5" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments5" runat="server" ToolTip="Select Work Permit">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId5" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action" HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="WORKPERMITDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="WORKPERMITADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td></td>
                                            <td align="center" valign="top">
                                                <telerik:RadLabel ID="lblEPSS" runat="server" Text="EPSS" Style="background-color: #c6e0b4; width: 99%;"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvEPSS" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnItemCommand="gvEPSS_ItemCommand" OnNeedDataSource="gvEPSS_NeedDataSource" OnItemDataBound="gvEPSS_ItemDataBound" AllowSorting="true" ShowFooter="true" ShowHeader="false" EnableViewState="true">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>

                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="90%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="90%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblFormId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMID") %>'></asp:Label>
                                                                    <asp:Label ID="lbltype" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTYPE") %>'></asp:Label>
                                                                    <asp:LinkButton ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <span id="spnPickListDocument3">
                                                                        <telerik:RadTextBox ID="txtDocumentName3" runat="server" Width="90%" Enabled="False" Style="font-weight: bold">
                                                                        </telerik:RadTextBox>
                                                                        <asp:LinkButton ID="btnShowDocuments3" runat="server" ToolTip="Select EPSS">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                                                        </asp:LinkButton>
                                                                        <telerik:RadTextBox ID="txtDocumentId3" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                                                    </span>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn FooterStyle-HorizontalAlign="Left" HeaderText="Action" HeaderStyle-Width="10%">
                                                                <HeaderStyle HorizontalAlign="Left" VerticalAlign="Middle" Width="10%"></HeaderStyle>

                                                                <ItemStyle Wrap="false" HorizontalAlign="Center"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:LinkButton runat="server" AlternateText="Delete" CommandName="EPSSDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                                    </asp:LinkButton>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save"
                                                                        CommandName="EPSSADD" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                                                        ToolTip="Add New">
                                         <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                                                    </asp:LinkButton>
                                                                </FooterTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>

                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone6" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock6" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblhazards" Text="Hazards and Score Matrix" Font-Bold="true"></telerik:RadLabel>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <table width="99%">
                                        <tr>
                                            <td align="center" width="23%" valign="top">
                                                <telerik:RadLabel ID="lblhealth" runat="server" Style="background-color: #ffe699; width: 99%;" Text="Health and Safety Hazards"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvhazards" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvhazards_NeedDataSource" OnItemDataBound="gvhazards_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="false">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td align="center" width="23%" valign="top">
                                                <telerik:RadLabel ID="lblenvironmental1" runat="server" Style="background-color: #ffe699; width: 99%;" Text="Environmental Hazards"></telerik:RadLabel>
                                                <br />
                                                <telerik:RadGrid RenderMode="Lightweight" ID="gvenvironmental" runat="server" AutoGenerateColumns="False" AllowPaging="false" AllowCustomPaging="false"
                                                    Font-Size="10px" Width="100%" CellPadding="3" EnableHeaderContextMenu="true" GroupingEnabled="false" OnNeedDataSource="gvenvironmental_NeedDataSource" OnItemDataBound="gvenvironmental_ItemDataBound" AllowSorting="true" ShowFooter="false" ShowHeader="false" EnableViewState="false">
                                                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                                    <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                                        AutoGenerateColumns="false">
                                                        <NoRecordsTemplate>
                                                            <table width="99.9%" border="0">
                                                                <tr>
                                                                    <td align="center">
                                                                        <telerik:RadLabel ID="noRecordFound" runat="server" Text="NIL" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </NoRecordsTemplate>
                                                        <HeaderStyle Width="102px" />
                                                        <Columns>
                                                            <telerik:GridTemplateColumn HeaderText="Icon" HeaderStyle-Width="25%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgPhoto" runat="server" Height="30px"
                                                                        Width="40px" />
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                            <telerik:GridTemplateColumn HeaderText="Name" HeaderStyle-Width="75%">
                                                                <ItemStyle Wrap="False" HorizontalAlign="Left" Width="75%"></ItemStyle>
                                                                <ItemTemplate>

                                                                    <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                                                </ItemTemplate>
                                                            </telerik:GridTemplateColumn>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="true" UseClientSelectColumnOnly="true" />
                                                        <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                                    </ClientSettings>
                                                </telerik:RadGrid>
                                            </td>
                                            <td width="2%"></td>
                                            <td style="vertical-align: top" width="50%">
                                                <table class="Hazard">
                                                    <tr>
                                                        <td align="center" id="Td" width="24%"></td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblHealthSafety" runat="server">Health and Safety</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblEnviormental" runat="server">Environmental</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblEconomic" runat="server">Economic/Process Loss</telerik:RadLabel>
                                                        </td>
                                                        <td align="center" style="background-color: #ffe699;" width="19%">
                                                            <telerik:RadLabel ID="lblWorst" runat="server">Worst Case</telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="impact" runat="server">
                                                            <telerik:RadLabel ID="lblimpact" runat="server" Text="Impact"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impacthealth" runat="server">
                                                            <telerik:RadLabel ID="lblimpacthealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impactenv" runat="server">
                                                            <telerik:RadLabel ID="lblimpactenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impacteco" runat="server">
                                                            <telerik:RadLabel ID="lblimpacteco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="impactws" runat="server">
                                                            <telerik:RadLabel ID="lblimpactws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="POO" runat="server">
                                                            <telerik:RadLabel ID="lblPOO" runat="server" Text="POO"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOhealth" runat="server">
                                                            <telerik:RadLabel ID="lblPOOhealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOenv" runat="server">
                                                            <telerik:RadLabel ID="lblPOOenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOeco" runat="server">
                                                            <telerik:RadLabel ID="lblPOOeco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="POOws" runat="server">
                                                            <telerik:RadLabel ID="lblPOOws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="loh" runat="server">
                                                            <telerik:RadLabel ID="lblloh" runat="server" Text="LOH"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohhealth" runat="server">
                                                            <telerik:RadLabel ID="lbllohhealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohenv" runat="server">
                                                            <telerik:RadLabel ID="lbllohenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="loheco" runat="server">
                                                            <telerik:RadLabel ID="lblloheco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="lohws" runat="server">
                                                            <telerik:RadLabel ID="lbllohws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="tdControls" runat="server">
                                                            <telerik:RadLabel ID="lblControlstxt" runat="server" Text="Controls"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlshealth" runat="server">
                                                            <telerik:RadLabel ID="lblControlshealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlsenv" runat="server">
                                                            <telerik:RadLabel ID="lblControlsenv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlseco" runat="server">
                                                            <telerik:RadLabel ID="lblControlseco" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="Controlsws" runat="server">
                                                            <telerik:RadLabel ID="lblControlsws" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="level" runat="server">
                                                            <telerik:RadLabel ID="lblLevel" runat="server" Text="Residual Risk Level from JHA"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskhealth" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskHealth" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskenv" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskEnv" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskeco" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskEconomic" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="levelofriskworst" runat="server">
                                                            <telerik:RadLabel ID="lblLevelofRiskWorst" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="Td1" runat="server">
                                                            <telerik:RadLabel ID="lblcontrols" runat="server" Text="Additonal Controls due to Supervision / Checklist"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdhscontrols" runat="server">
                                                            <telerik:RadLabel ID="lblhscontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdencontrols" runat="server">
                                                            <telerik:RadLabel ID="lblencontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdecocontrols" runat="server">
                                                            <telerik:RadLabel ID="lbleccontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdwscontrols" runat="server">
                                                            <telerik:RadLabel ID="lblwscontrols" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td align="center" id="tdresrisk" runat="server">
                                                            <telerik:RadLabel ID="lblresrisk" runat="server" Text="Residual Risk"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdreshsrisk" runat="server">
                                                            <telerik:RadLabel ID="lblreshsrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdresenrisk" runat="server">
                                                            <telerik:RadLabel ID="lblresenrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdresecorisk" runat="server">
                                                            <telerik:RadLabel ID="lblresecorisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center" id="tdreswsrisk" runat="server">
                                                            <telerik:RadLabel ID="lblreswsrisk" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                    <tr valign="top">
                                                        <td align="center" id="Description" runat="server"></td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblHealthDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblEnvDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblEconomicDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                        <td align="center">
                                                            <telerik:RadLabel ID="lblWorstDescription" runat="server"></telerik:RadLabel>
                                                        </td>
                                                    </tr>
                                                </table>

                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr runat="server" visible="false">
                    <td>
                        <telerik:RadLabel ID="lblImportedJHA" runat="server" Text="Imported JHA"></telerik:RadLabel>
                    </td>
                    <td>
                        <br />
                        <eluc:Date ID="txtDate" Visible="false" CssClass="readonlytextbox" ReadOnly="false" runat="server" />
                        <asp:LinkButton ID="lnkImportJHA" runat="server" Text="Import JHA" ToolTip="Import JHA"><span><i class="fas fa-file-import"></i></span></asp:LinkButton>
                        <br />
                        <div id="dvJHA" runat="server" class="input" style="overflow: auto; width: 360px; height: 80px;">
                            <telerik:RadCheckBoxList ID="chkImportedJHAList" runat="server" Height="100%" OnSelectedIndexChanged="chkImportedJHAList_Changed"
                                Columns="1" Direction="Horizontal" RepeatLayout="Flow" AutoPostBack="true" Font-Bold="true">
                            </telerik:RadCheckBoxList>
                        </div>
                    </td>
                    <td colspan="2"></td>
                </tr>
                 <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone5" runat="server" FitDocks="true" Orientation="Horizontal" Width="99%" BorderStyle="None">
                            <telerik:RadDock Width="100%" RenderMode="Lightweight" ID="RadDock5" runat="server" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblUndesireable" Text="Undesirable Event / Worst Case Scenario" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkUndesireable" runat="server" ToolTip="Comments" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvevent" runat="server" AllowCustomPaging="false"
                                        Font-Size="11px" AllowPaging="false" AllowSorting="true" Width="100%" EnableViewState="false"
                                        CellPadding="3" OnNeedDataSource="gvevent_NeedDataSource" OnItemCommand="gvevent_ItemCommand" OnItemDataBound="gvevent_ItemDataBound"
                                        GroupingEnabled="false" ShowHeader="true" EnableHeaderContextMenu="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDWORSTCASEID">
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
                                            <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />
                                            <Columns>
                                                <telerik:GridTemplateColumn HeaderText="S.No." HeaderStyle-Width="5%">
                                                    <ItemStyle HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRownumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROWNUMBER") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Undesirable Event">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblevent" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEVENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Risks">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblRisks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKESCALATION") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Procedures">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <div id="divProcedures" runat="server" style="width: auto; border-width: 1px; border-style: solid; border: 0px solid #9f9fff">
                                                            <table id="tblProcedures" runat="server">
                                                            </table>
                                                        </div>
                                                        <%--<telerik:RadLabel ID="lblProcedures" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURES") %>'></telerik:RadLabel>--%>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Worst Case">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="60%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblWorstCaseid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASEID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblWorstCase" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORSTCASENAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" Visible="false" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="10%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" Width="100px" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Equipment" CommandName="EMAPPING" ID="cmdEquipment" ToolTip="Controls Mapping">
                                                    <span class="icon"><i class="fas fa-cogs"></i></span>
                                                        </asp:LinkButton>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="UDELETE" ID="cmdDelete" ToolTip="Delete">
                                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone1" runat="server" FitDocks="true" Orientation="Horizontal" Width="100%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock1" runat="server" Title="<b>Imported JHA</b>" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lbImportedJHA" Text="Imported JHA" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkImportedJHA" runat="server" ToolTip="Imported JHA" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuImportedJHA" runat="server" TabStrip="false" />
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvImportedJHA" runat="server" AllowCustomPaging="false"
                                        CellSpacing="0" EnableViewState="true" GridLines="None" Font-Size="11px" OnItemCommand="gvImportedJHA_ItemCommand"
                                        OnNeedDataSource="gvImportedJHA_NeedDataSource" OnItemDataBound="gvImportedJHA_ItemDataBound" ShowFooter="False" ShowHeader="true" Width="100%" EnableHeaderContextMenu="true" GroupingEnabled="false">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false" DataKeyNames="FLDJOBHAZARDID">
                                            <ColumnGroups>
                                                <telerik:GridColumnGroup Name="Controls" HeaderText="Controls" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Font-Size="X-Small"></telerik:GridColumnGroup>
                                            </ColumnGroups>
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
                                                <telerik:GridTemplateColumn HeaderText="Job" HeaderStyle-Width="8%" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%" Font-Size="X-Small"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblJobs" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOB") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Number" HeaderStyle-Width="7%" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%" Font-Size="X-Small"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbljhaid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBHAZARDID") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lbljhanumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>' Visible="false"></telerik:RadLabel>
                                                        <asp:LinkButton ID="lnkjhanumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNUMBER") %>' CommandName="JHA"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Aspects" HeaderStyle-Width="10%" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAspects" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDASPECTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Hazards/Risk" HeaderStyle-Width="10%" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblHazardrisk" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHAZARDRISK") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Operational Controls" HeaderStyle-Width="20%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lbloprational" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOPERATIONAL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Equipment" HeaderStyle-Width="7%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="7%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblEquipment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEQUIPMENT") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Procedures" HeaderStyle-Width="8%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblprocedures" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPROCEDURES") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Forms &  Checklists" HeaderStyle-Width="8%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblForms" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFORMSCHECKLISTS") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="PPE" HeaderStyle-Width="8%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="8%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblPPE" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPPELIST") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Supervision Level" HeaderStyle-Width="6%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="6%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblSupervision" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRISKLEVEL") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Conditions for Additional RA" HeaderStyle-Width="6%" ColumnGroupName="Controls" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="6%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblAdditional" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCONDITIONALRA") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Level of Residual Risk" HeaderStyle-Width="8%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="8%" Font-Size="X-Small" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <table Width="99%">
                                                            <tr>
                                                                <td width="45%">
                                                                    <telerik:RadLabel ID="lblhsheader" runat="server" Font-Bold="true" Text="H & S"></telerik:RadLabel>
                                                                </td>
                                                                <td width="55%">
                                                                    <telerik:RadLabel ID="lblHS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHSRR") %>' Width="99%" ></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblENVheader" runat="server" Font-Bold="true" Text="ENV"></telerik:RadLabel>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblENV" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDENVRR") %>' Width="99%"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                 <td>
                                                                    <telerik:RadLabel ID="lblECOheader" runat="server" Font-Bold="true" Text="ECO"></telerik:RadLabel>
                                                                 </td>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblECO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDECORR") %>' Width="99%"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblWCSheader" runat="server" Font-Bold="true" Text="WCS"></telerik:RadLabel>
                                                                </td>
                                                                <td>
                                                                    <telerik:RadLabel ID="lblWCS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWCRR") %>' Width="99%"></telerik:RadLabel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <telerik:RadLabel ID="lblmaxvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMAXVALUE") %>' Visible="false"></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblminvalue" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMINVALUE") %>' Visible="false"></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="6%" HeaderStyle-Font-Size="X-Small">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Center" Width="6%"></ItemStyle>
                                                    <FooterStyle HorizontalAlign="Center" />
                                                    <ItemTemplate>
                                                        <asp:LinkButton runat="server" AlternateText="Delete" CommandName="JHADELETE" ID="cmddelete" ToolTip="Delete">
                                                            <span class="icon"><i class="fas fa-trash"></i></span>
                                                        </asp:LinkButton>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <telerik:RadDockZone ID="RadDockZone2" runat="server" FitDocks="true" Orientation="Horizontal" Width="100%" BorderStyle="None">
                            <telerik:RadDock Width="99%" RenderMode="Lightweight" ID="RadDock2" runat="server" Title="<b>Team Composition</b>" EnableAnimation="true" EnableDrag="false"
                                EnableRoundedCorners="true" Resizable="true" CssClass="higherZIndex" Closed="false">
                                <Commands>
                                    <telerik:DockExpandCollapseCommand />
                                </Commands>
                                <TitlebarTemplate>
                                    <telerik:RadLabel runat="server" ID="lblTeamComposition" Text="Team Composition" Font-Bold="true"></telerik:RadLabel>
                                    &nbsp;&nbsp;
                                            <asp:LinkButton ID="lnkTeamComposition" runat="server" ToolTip="Team Composition" Visible="false">
                                                   <span class="icon"><i class="fa fa-comments"></i></span>
                                            </asp:LinkButton>
                                </TitlebarTemplate>
                                <ContentTemplate>
                                    <eluc:TabStrip ID="MenuTeamComposition" runat="server"></eluc:TabStrip>
                                    <telerik:RadGrid RenderMode="Lightweight" ID="gvTeamComposition" runat="server" AllowCustomPaging="false"
                                        CellSpacing="0" EnableViewState="true" GridLines="None" Font-Size="11px" OnItemDataBound="gvTeamComposition_ItemDataBound"
                                        OnNeedDataSource="gvTeamComposition_NeedDataSource" ShowFooter="False" ShowHeader="true" Width="100%">
                                        <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
                                        <MasterTableView EditMode="InPlace" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                                            AutoGenerateColumns="false">
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
                                                <telerik:GridTemplateColumn HeaderText="Deparment" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDeparment" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Function" HeaderStyle-Width="20%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="20%" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblFunction" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUNCTIONNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="Group Ranks" HeaderStyle-Width="30%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="30%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblDepartmentid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDEPARTMENTID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblfunctionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFUNCTIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lbllevelid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblcombositionid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRAACTIVITYTEAMCOMPOSITIONID") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblRankGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANKLIST") %>'></telerik:RadLabel>
                                                        <telerik:RadLabel ID="lblRankGroupList" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDGROUPRANK") %>' Visible="false"></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="STCW Level" HeaderStyle-Width="15%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="15%" VerticalAlign="Top"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblLevel" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDLEVELNAME") %>'></telerik:RadLabel>
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                                <telerik:GridTemplateColumn HeaderText="No.of People" HeaderStyle-Width="10%">
                                                    <ItemStyle Wrap="true" HorizontalAlign="Left" Width="10%"></ItemStyle>
                                                    <ItemTemplate>
                                                        <telerik:RadLabel ID="lblcount" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPERSONCOUNT") %>' />
                                                    </ItemTemplate>
                                                </telerik:GridTemplateColumn>
                                            </Columns>
                                        </MasterTableView>
                                        <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                            <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                            <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="" />
                                            <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                                        </ClientSettings>
                                    </telerik:RadGrid>
                                </ContentTemplate>
                            </telerik:RadDock>
                        </telerik:RadDockZone>
                    </td>
                </tr>
            </table>
            <br />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

