<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSDMSMeasure.aspx.cs" Inherits="VesselPositionIMODCSDMSMeasure" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmVPRSLocation" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlVPRSLocation" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlVPRSLocation">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button ID="cmdHiddenPick" runat="server" Text="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="MenuProcedureDetailList" Title="Method to measure distance travelled" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr runat="server" visible="false">
                    <td width="10%">
                        <telerik:RadLabel runat="server" ID="lblreference" Text="Reference to Existing Procedure"></telerik:RadLabel>
                    </td>
                    <td width="90%">
                        <span id="spnPickListDocument">
                            <telerik:RadTextBox ID="txtDocumentName" runat="server" Width="350px" CssClass="input" AutoPostBack="true" OnDataBinding="txtDocumentName_OnTextChanged"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadTextBox ID="txtReferencetoExisting" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                    </td>
                </tr>
                <tr>
                    <td width="10%">
                        <telerik:RadLabel runat="server" ID="lblprocedure" Text="Reference to Existing Procedure"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadGrid ID="gvEUMRVFuelType" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" OnNeedDataSource="gvEUMRVFuelType_NeedDataSource"
                            OnItemCommand="gvEUMRVFuelType_RowCommand" OnItemDataBound="gvEUMRVFuelType_ItemDataBound"
                            ShowFooter="true" EnableViewState="false" EnableHeaderContextMenu="true" GroupingEnabled="false">
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
                                <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false" />

                                <Columns>
                                    <telerik:GridTemplateColumn HeaderText="Document">
                                        <ItemStyle Wrap="False" HorizontalAlign="left" Width="90%"></ItemStyle>
                                        <ItemTemplate>
                                            <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDMSSECTIONNAME") %>'></telerik:RadLabel>
                                            <telerik:RadLabel ID="lbluniqueid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSMETHODMEASUREOFFOCONSUMPTIONLINEID") %>' Visible="false"></telerik:RadLabel>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            <span id="spnPickListDocumentadd">
                                                <telerik:RadTextBox ID="txtDocumentNameadd" runat="server" Width="350px" CssClass="input" AutoPostBack="true" OnDataBinding="txtDocumentName_OnTextChanged"></telerik:RadTextBox>
                                                <asp:LinkButton runat="server" AlternateText=".." CommandName="Document" ID="btnShowDocumentsadd">
                                                    <span class="icon"><i class="fas fa-tasks"></i></span>
                                                </asp:LinkButton>

                                                <telerik:RadTextBox ID="txtDocumentIdadd" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                                            </span>
                                            <telerik:RadTextBox ID="txtReferencetoExistingadd" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                                                Width="70%" />
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                    <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="20%">
                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                        <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                                <span class="icon"><i class="fas fa-trash"></i></span>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                        <FooterStyle HorizontalAlign="Center" />
                                        <FooterTemplate>
                                            <asp:LinkButton runat="server" AlternateText="Add New" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                                <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                            </asp:LinkButton>
                                        </FooterTemplate>
                                    </telerik:GridTemplateColumn>
                                </Columns>
                            </MasterTableView>
                            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                            </ClientSettings>
                        </telerik:RadGrid>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                </tr>
                <tr runat="server">
                    <td>
                        <telerik:RadLabel runat="server" ID="lbleuprocedure" Text="Description"></telerik:RadLabel>
                    </td>
                    <td>
                        <%--<eluc:CustomEditor ID="txteuprocedure" runat="server" Width="100%" Height="450px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure" runat="server" Width="100%" Height="350px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>

