<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionSIPFuelSystemOilModification.aspx.cs" Inherits="VesselPositionSIPFuelSystemOilModification" MaintainScrollPositionOnPostback="true" EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="CustomEditor" Src="~/UserControls/UserControlEditor.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Tooltip" Src="~/UserControls/UserControlToolTip.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Fuel oil system modifications</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
            <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmSIPTanksConfuguration" runat="server">
        <telerik:RadScriptManager ID="ToolkitScriptManager1" runat="server">
        </telerik:RadScriptManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <telerik:RadFormDecorator ID="RadformDecortor" DecorationZoneID="pnlSIPTanksConfuguration" runat="server" DecoratedControls="All" EnableRoundedCorners="true" />

        <telerik:RadAjaxPanel runat="server" ID="pnlSIPTanksConfuguration">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <asp:Button runat="server" ID="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />
            <eluc:Status runat="server" ID="ucStatus" />

            <eluc:TabStrip ID="TabFuelSystemMofify" Title="Fuel oil system modifications" runat="server" OnTabStripCommand="TabFuelSystemMofify_TabStripCommand" TabStrip="true" />

            <eluc:TabStrip ID="MenuFuelSystemMofify" runat="server" OnTabStripCommand="MenuFuelSystemMofify_TabStripCommand" />

            <table id="tblSearch" width="100%" style="display: none;">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblVesselName" runat="server" Text="Vessel Name"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Vessel ID="UcVessel" runat="server" CssClass="dropdown_mandatory" AutoPostBack="true" SyncActiveVesselsOnly="True" AssignedVessels="true"
                            VesselsOnly="true" AppendDataBoundItems="true" />
                    </td>
                </tr>
            </table>

            <telerik:RadLabel ID="lblmanufactTitle" runat="server"><b>Schedule for meeting with manufacturer:</b></telerik:RadLabel>


            <telerik:RadGrid ID="gvMeetingManufact" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="MeetingManufact_RowCommand" OnItemDataBound="MeetingManufact_ItemDataBound"
                AllowSorting="true" OnNeedDataSource="gvMeetingManufact_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnUpdateCommand="MeetingManufactRowUpdating" OnSortCommand="MeetingManufact_Sorting" ShowFooter="true"
                ShowHeader="true" EnableViewState="false">
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
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />


                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="14%" AllowFiltering="true" SortExpression="FLDDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATE") %>'
                                    CssClass="input" DatePicker="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateAdd" runat="server" CssClass="input" DatePicker="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Manufacturer" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFOSystemModifyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPFOSYSTEMMODIFICATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblManufacturer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFOSystemModifyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPFOSYSTEMMODIFICATIONID") %>'></telerik:RadLabel>
                                <telerik:RadTextBox ID="txtFOSystemModifyEdit" runat="server" Width="98%" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTCLASS") %>'
                                    CssClass="gridinput">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtFOSystemModifyAdd" runat="server" CssClass="gridinput" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Details" HeaderStyle-Width="54%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'
                                    CssClass="gridinput" TextMode="MultiLine" Resize="Both" Rows="4" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDetailsAdd" runat="server" CssClass="gridinput" TextMode="MultiLine" Resize="Both" Rows="4" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" ></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />

            <telerik:RadLabel ID="lblMeetingWithClass" runat="server"><b>Schedule for meeting with class:</b></telerik:RadLabel>



            <telerik:RadGrid ID="gvMeetingClass" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                Width="100%" CellPadding="3" OnItemCommand="gvMeetingClass_RowCommand" OnItemDataBound="gvMeetingClass_ItemDataBound"
                AllowSorting="true" OnRowEditing="gvMeetingClass_RowEditing" OnNeedDataSource="gvMeetingClass_NeedDataSource"
                AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" GroupingEnabled="false"
                OnUpdateCommand="gvMeetingClassRowUpdating" OnSortCommand="gvMeetingClass_Sorting" ShowFooter="true"
                ShowHeader="true" EnableViewState="false">
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
                    <CommandItemSettings ShowRefreshButton="true" ShowPrintButton="true" ShowExportToExcelButton="true" ShowAddNewRecordButton="true" ShowExportToPdfButton="false" />

                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Date" HeaderStyle-Width="14%" AllowFiltering="true" SortExpression="FLDDATE">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDate" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATE") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <eluc:Date ID="ucDateEdit" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDATE") %>'
                                    CssClass="input" DatePicker="true" />
                            </EditItemTemplate>
                            <FooterTemplate>
                                <eluc:Date ID="ucDateAdd" runat="server" CssClass="input" DatePicker="true" />
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Class" HeaderStyle-Width="25%">
                            <ItemStyle Wrap="False" HorizontalAlign="Left" Width="25%"></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left" Width="25%" />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblFOSystemModifyId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPFOSYSTEMMODIFICATIONID") %>'></telerik:RadLabel>
                                <telerik:RadLabel ID="lblManufacturer" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDMANUFACTCLASS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadLabel ID="lblFOSystemModifyEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIPFOSYSTEMMODIFICATIONID") %>'></telerik:RadLabel>
                                <span id="spnPickListFOSystemModifyEdit">
                                    <telerik:RadTextBox ID="txtFOSystemModifyCodeEdit" runat="server" Width="70px" CssClass="gridinput" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCLASSCODE") %>'></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtFOSystemModifyEdit" runat="server" Width="170px" CssClass="gridinput" Text='<%#DataBinder.Eval(Container,"DataItem.FLDMANUFACTCLASS") %>'
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" AlternateText=".." ID="btnFOSystemModifyEdit" ToolTip="Class">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtFOSystemModifyIdEdit" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDCALASSID") %>'
                                        MaxLength="20" CssClass="input" Width="10px">
                                    </telerik:RadTextBox>
                                </span>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <span id="spnPickListFOSystemModifyAdd">
                                    <telerik:RadTextBox ID="txtFOSystemModifyCodeAdd" runat="server" Width="70px" CssClass="gridinput"></telerik:RadTextBox>
                                    <telerik:RadTextBox ID="txtFOSystemModifyAdd" runat="server" Width="170px" CssClass="gridinput"
                                        Enabled="False">
                                    </telerik:RadTextBox>
                                    <asp:LinkButton runat="server" AlternateText=".." ID="btnFOSystemModifyAdd" ToolTip="Class">
                                        <span class="icon"><i class="fas fa-tasks"></i></span>
                                    </asp:LinkButton>
                                    <telerik:RadTextBox ID="txtFOSystemModifyIdAdd" runat="server"
                                        MaxLength="20" CssClass="input" Width="0px">
                                    </telerik:RadTextBox>
                                </span>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Details" HeaderStyle-Width="54%">
                            <ItemStyle Wrap="True" HorizontalAlign="Left" ></ItemStyle>
                            <FooterStyle Wrap="false" HorizontalAlign="Left"  />
                            <ItemTemplate>
                                <telerik:RadLabel ID="lblDetails" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'></telerik:RadLabel>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <telerik:RadTextBox ID="txtDetailsEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDETAILS") %>'
                                    CssClass="gridinput" TextMode="MultiLine" Resize="Both" Rows="4" Width="98%">
                                </telerik:RadTextBox>
                            </EditItemTemplate>
                            <FooterTemplate>
                                <telerik:RadTextBox ID="txtDetailsAdd" runat="server" CssClass="gridinput" TextMode="MultiLine" Resize="Both" Rows="4" Width="98%"></telerik:RadTextBox>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>

                        <telerik:GridTemplateColumn HeaderText="Action" HeaderStyle-Width="7%">
                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                            <ItemStyle Wrap="False" HorizontalAlign="Center" Width="8%"></ItemStyle>
                            <ItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Attachment" CommandName="ATTACHMENT" ID="cmdAtt" ToolTip="Attachment">
                                    <span class="icon"><i class="fas fa-paperclip"></i></span>
                                </asp:LinkButton>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:LinkButton runat="server" AlternateText="Save" CommandName="UPDATE" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                                </asp:LinkButton>
                                <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                                </asp:LinkButton>
                            </EditItemTemplate>
                            <FooterStyle HorizontalAlign="Center" />
                            <FooterTemplate>
                                <asp:LinkButton runat="server" AlternateText="Add" CommandName="Add" ID="cmdAdd" ToolTip="Add">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                                </asp:LinkButton>
                            </FooterTemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass="RadGrid_Default rgPagerTextBox" AlwaysVisible="true" />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>

            <br />
            <table width="100%">
                <tr>
                    <td style="width: 40%;" colspan="2">
                        <telerik:RadLabel ID="lblFuelMofifyreq" runat="server" Text="Are structural modifications (installation of fuel oil systems/tankage) required?"></telerik:RadLabel>
                    </td>
                    <td style="width: 60%; align-content: flex-start;" colspan="4">
                        <telerik:RadRadioButtonList ID="rdFuelMofifyreqyn" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdFuelMofifyreqyn_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="NA" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadCheckBox ID="chkFuelMofifyreqyn" runat="server" AutoPostBack="true" OnCheckedChanged="chkFuelMofifyreqyn_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr id="trdescriptionHead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal1" runat="server" Text="<b>Description of modifications:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr id="trOfficeDiscription" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficeDescription" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both"
                            Width="98%" />
                    </td>
                </tr>
                <tr id="trHeader" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblHeader" runat="server" Text="<b>Provide details about modifications, time schedules and yard bookings:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr align="left" id="trDate" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblType" runat="server" Text="Fuel Oil Storage System"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlType" runat="server" CssClass="input" Width="180px" Visible="false"></telerik:RadComboBox>
                        <telerik:RadCheckBox ID="chkfuelstorage" runat="server" AutoPostBack="true" OnCheckedChanged="chkfuelstorage_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblstartDate" runat="server" Text="Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcstartDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblendDate" runat="server" Text="End"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcEndDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trDiscription" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtDiscription" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>

                <tr align="left" id="trfueltransfer" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblfueltransfer" runat="server" Text="Fuel Transfer/Filtration/Delivery system"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkfueltransfer" runat="server" AutoPostBack="true" OnCheckedChanged="chkfueltransfer_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfueltransferstart" runat="server" Text="Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="Ucfueltransferstart" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblfueltransferend" runat="server" Text="End"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="Ucfueltransferend" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trfueltransferdesc" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtfueltransfer" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>
                <tr align="left" id="trCombustion" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblCombustion" runat="server" Text="Combustion Equipment"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadCheckBox ID="chkCombustion" runat="server" AutoPostBack="true" OnCheckedChanged="chkCombustion_CheckedChanged" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCombustionstart" runat="server" Text="Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcCombustionstart" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCombustionend" runat="server" Text="End"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcCombustionend" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr id="trCombustiondesc" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtCombustion" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>
                <tr id="trmodfiyDocument" runat="server">
                    <td colspan="4">
                        <telerik:RadLabel ID="lblmodification" runat="server" Text="<b>Upload relevant documents:</b>"></telerik:RadLabel>
                        &nbsp;&nbsp;&nbsp;
                        <asp:LinkButton ID="linkModification" runat="server" Text="<b>Documents</b>" OnClick="linkModification_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr id="tryardplace" runat="server" style="display: none;">
                    <td>
                        <telerik:RadLabel ID="lblregion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlregion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlregion_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlCountry" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblYard" runat="server" Text="Yard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtYard" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <telerik:RadLabel ID="lblscriberwillinstallyn" runat="server" Text="Are scrubbers going to be installed?"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadRadioButtonList ID="rdscriberwillinstallyn" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdscriberwillinstallyn_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="NA" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                        <telerik:RadCheckBox ID="chkscriberwillinstallyn" runat="server" AutoPostBack="true" OnCheckedChanged="chkscriberwillinstallyn_CheckedChanged" Visible="false" />
                    </td>
                </tr>
                <tr id="trscrbrHeader" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblScrubberHeader" runat="server" Text="<b>Provide details about scrubber installation:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr align="left" id="trScruberDate" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblScruberstartDate" runat="server" Text="Start"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Date ID="UcScruberstartDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblScruberendDate" runat="server" Text="End"></telerik:RadLabel>
                    </td>
                    <td colspan="3">
                        <eluc:Date ID="UcScruberEndDate" runat="server" CssClass="input" DatePicker="true" />
                    </td>
                </tr>
                <tr align="left" id="trScruberyardplace" runat="server">
                    <td>
                        <telerik:RadLabel ID="lblScruberregion" runat="server" Text="Region"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlScruberregion" runat="server" Width="180px" CssClass="input" AutoPostBack="true" OnSelectedIndexChanged="ddlScruberregion_SelectedIndexChanged"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblScruberCountry" runat="server" Text="Country"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadComboBox ID="ddlScruberCountry" runat="server" Width="180px" CssClass="input"></telerik:RadComboBox>
                    </td>
                    <td>
                        <telerik:RadLabel ID="lblScruberYard" runat="server" Text="Yard"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtScruberYard" runat="server" Width="180px" CssClass="input"></telerik:RadTextBox></td>
                </tr>
                <tr id="trscrbrdescriptionHead" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal2" runat="server" Text="<b>Description of installation:</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr id="trScruberOfficeDiscription" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtScruberOfficeDiscription" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr id="trScruberDiscription" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtscrberDiscription" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>
                <tr id="tr1" runat="server">
                    <td colspan="2">
                        <telerik:RadLabel ID="lbldedicatedFOSample" runat="server" Text="Has a dedicated FO sampling point been designated?"></telerik:RadLabel>
                    </td>
                    <td colspan="4">
                        <telerik:RadRadioButtonList ID="rddedicatedFOSample" runat="server" Direction="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rddedicatedFOSample_SelectedIndexChanged">
                            <Items>
                                <telerik:ButtonListItem Value="0" Text="Yes"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="1" Text="No"></telerik:ButtonListItem>
                                <telerik:ButtonListItem Value="2" Text="NA" Selected="True"></telerik:ButtonListItem>
                            </Items>
                        </telerik:RadRadioButtonList>
                    </td>
                </tr>
                <tr id="tr3" runat="server">
                    <td colspan="6">
                        <telerik:RadLabel ID="Literal3" runat="server" Text="<b>Description</b>"></telerik:RadLabel>
                    </td>
                </tr>
                <tr id="trOfficededicatedFOSample" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtOfficededicatedFOSample" runat="server" CssClass="readonlytextbox" Enabled="false" Height="70px" TextMode="MultiLine" Resize="Both" Rows="3"
                            Width="98%" />
                    </td>
                </tr>
                <tr id="trdedicatedFOSample" runat="server">
                    <td colspan="6">
                        <telerik:RadTextBox ID="txtdedicatedFOSample" runat="server" Width="98%" CssClass="input" TextMode="MultiLine" Resize="Both" Rows="3"></telerik:RadTextBox></td>
                </tr>
            </table>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
