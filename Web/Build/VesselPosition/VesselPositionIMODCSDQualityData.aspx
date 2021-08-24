<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionIMODCSDQualityData.aspx.cs" Inherits="VesselPositionIMODCSDQualityData" MaintainScrollPositionOnPostback="true" %>

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
            <eluc:Status runat="server" ID="ucStatus" />
            <asp:Button ID="cmdHiddenPick" runat="server" Text="cmdHiddenPick" OnClick="cmdHiddenPick_Click" CssClass="hidden" />

            <eluc:TabStrip ID="MenuProcedureDetailList" Title="Data Quality" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>

            <eluc:TabStrip ID="TabProcedure" runat="server" OnTabStripCommand="TabProcedure_TabStripCommand"></eluc:TabStrip>

            <table width="100%">
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblFuelConsumption" Text="Fuel Consumption"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName1" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments1" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId1" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
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
                                            <span id="spnPickListDocumentadd1">
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting1" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                        <%--<eluc:CustomEditor ID="txteuprocedure1" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure1" runat="server" Width="100%" Height="200px" RenderMode="Lightweight">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblDistanceTravelled" Text="Distance Travelled"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument1" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName2" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments2" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId2" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadGrid ID="gvEUMRVFuelType2" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" OnItemCommand="gvEUMRVFuelType2_RowCommand" 
                            OnItemDataBound="gvEUMRVFuelType2_ItemDataBound" OnNeedDataSource="gvEUMRVFuelType2_NeedDataSource"
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
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblfactorhHeader" runat="server" Text="Document"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDMSSECTIONNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lbluniqueid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSMETHODMEASUREOFFOCONSUMPTIONLINEID") %>' Visible="false"></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListDocumentadd2">
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
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting2" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                        <%--<eluc:CustomEditor ID="txteuprocedure2" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure2" runat="server" Width="100%" Height="200px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblHoursUnderway" Text="Hours Underway"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument2" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName3" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments3" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId3" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadGrid ID="gvEUMRVFuelType3" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" 
                            OnItemCommand="gvEUMRVFuelType3_RowCommand" OnItemDataBound="gvEUMRVFuelType3_ItemDataBound" OnNeedDataSource="gvEUMRVFuelType3_NeedDataSource"
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
                                        <span id="spnPickListDocumentadd3">
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting3" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                        <%--<eluc:CustomEditor ID="txteuprocedure3" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure3" runat="server" Width="100%" Height="200px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                            </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblAdequecy" Text="Adequecy of the Data Collection Plan"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument3" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName4" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments4" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId4" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadGrid ID="gvEUMRVFuelType4" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" 
                             OnItemCommand="gvEUMRVFuelType4_RowCommand" OnItemDataBound="gvEUMRVFuelType4_ItemDataBound" OnNeedDataSource="gvEUMRVFuelType4_NeedDataSource"
                            ShowFooter="true" EnableViewState="false">
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
                                        <span id="spnPickListDocumentadd4">
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting4" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                        <%--<eluc:CustomEditor ID="txteuprocedure4" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                         <telerik:RadEditor ID="txteuprocedure4" runat="server" Width="100%" Height="200px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblInformationTechnology" Text="Information Technology"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument4" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName5" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments5" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId5" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadGrid ID="gvEUMRVFuelType5" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="false" ShowHeader="true" 
                            OnItemCommand="gvEUMRVFuelType5_RowCommand" OnItemDataBound="gvEUMRVFuelType5_ItemDataBound" OnNeedDataSource="gvEUMRVFuelType5_NeedDataSource"
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
                                        <span id="spnPickListDocumentadd5">
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting5" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                       <%-- <eluc:CustomEditor ID="txteuprocedure5" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure5" runat="server" Width="100%" Height="200px">
                            <Modules>
                                <telerik:EditorModule Name="RadEditorHtmlInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorNodeInspector" Enabled="false" Visible="false" />
                                <telerik:EditorModule Name="RadEditorDomInspector" Enabled="false" />
                                <telerik:EditorModule Name="RadEditorStatistics" Enabled="false" />
                                
                            </Modules>
                        </telerik:RadEditor>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel runat="server" ID="lblInternal" Text="Internal reviews and Validation of Data"></telerik:RadLabel>
                    </td>
                    <td>
                        <span id="spnPickListDocument5" runat="server" visible="false">
                            <telerik:RadTextBox ID="txtDocumentName6" runat="server" Width="350px" CssClass="input" AutoPostBack="true"></telerik:RadTextBox>
                            <asp:ImageButton ID="btnShowDocuments6" runat="server" ImageUrl="<%$ PhoenixTheme:images/picklist.png %>"
                                ImageAlign="AbsMiddle" Text=".." />
                            <telerik:RadTextBox ID="txtDocumentId6" runat="server" Width="0px" CssClass="hidden"></telerik:RadTextBox>
                        </span>
                        <telerik:RadGrid ID="gvEUMRVFuelType6" runat="server" AutoGenerateColumns="False"
                            Font-Size="11px" Width="100%" CellPadding="2" AllowSorting="true" ShowHeader="true" 
                            OnItemCommand="gvEUMRVFuelType6_RowCommand" OnItemDataBound="gvEUMRVFuelType6_ItemDataBound" OnNeedDataSource="gvEUMRVFuelType6_NeedDataSource"
                            ShowFooter="true" EnableViewState="false">
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
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblfactorhHeader" runat="server" Text="Document"></telerik:RadLabel>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <telerik:RadLabel ID="lblDocumentName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDMSSECTIONNAME") %>'></telerik:RadLabel>
                                        <telerik:RadLabel ID="lbluniqueid" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDIMODCSMETHODMEASUREOFFOCONSUMPTIONLINEID") %>' Visible="false"></telerik:RadLabel>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <span id="spnPickListDocumentadd6">
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
                                    <HeaderTemplate>
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                            Action
                                        </telerik:RadLabel>
                                    </HeaderTemplate>
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
                        <br />
                        <telerik:RadTextBox ID="txtReferencetoExisting6" runat="server" CssClass="input" Height="70px" TextMode="MultiLine" Visible="false"
                            Width="70%" />
                        <%--<eluc:CustomEditor ID="txteuprocedure6" runat="server" Width="100%" Height="150px"
                            Visible="true" PictureButton="false" TextOnly="true" DesgMode="true" HTMLMode="true"
                            PrevMode="false" ActiveMode="Design" AutoFocus="false" />--%>
                        <telerik:RadEditor ID="txteuprocedure6" runat="server" Width="100%" Height="200px">
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
