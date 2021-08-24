<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockJob.aspx.cs" Inherits="DryDockJob" EnableEventValidation="true" ValidateRequest="true" %>

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ConfirmMessage" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Unit" Src="~/UserControls/UserControlUnit.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Responsibilty" Src="~/UserControls/UserControlDiscipline.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Untitled Page</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmRepairJob" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

        <div style="font-weight: 600; font-size: 12px;" runat="server">
            <eluc:TabStrip ID="MenuHeader" runat="server" OnTabStripCommand="MenuHeader_TabStripCommand"
                TabStrip="true"></eluc:TabStrip>
        </div>
        <div style="font-weight: 600; font-size: 12px;" runat="server">

            <eluc:TabStrip ID="MenuRepairJobSpecification" runat="server" OnTabStripCommand="RepairJobSpecification_TabStripCommand"></eluc:TabStrip>

        </div>
        <table width="100%" cellpadding="2" cellspacing="3">
            <tr>
                <td>
                    <telerik:RadLabel ID="lblJobType" runat="server" Text="Job Type"></telerik:RadLabel>
                </td>
                <td>
                    <telerik:RadDropDownList DefaultMessage="--Select--" ID="ddlJobType" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                    </telerik:RadDropDownList>
                </td>
                <td rowspan="13">
                    <b>
                        <telerik:RadLabel ID="lblInclude" runat="server" Text="Include"></telerik:RadLabel>
                    </b>
                    <telerik:RadListBox CheckBoxes="true" ID="cblInclude" runat="server" RepeatDirection="Vertical">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblNumber" runat="server" Text="Number"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtNumber" CssClass="input_mandatory"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblserverStatus" runat="server" Text="Status"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadLabel runat="server" ID="lblStatus" CssClass="input" MaxLength="10"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblTitle" runat="server" Text="Title"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox runat="server" ID="txtTitle" CssClass="input_mandatory" MaxLength="100"
                        Width="360px">
                    </telerik:RadTextBox>
                </td>
            </tr>
            <tr valign="top">
                <td>
                    <telerik:RadLabel ID="lblJobDescription" runat="server" Text="Job Description"></telerik:RadLabel>

                </td>
                <td colspan="3">
                    <telerik:RadTextBox ID="txtJobDescription" runat="server" CssClass="input_mandatory" Width="60%"
                        TextMode="MultiLine" Rows="6">
                    </telerik:RadTextBox>
                    <telerik:RadLabel ID="lblTempJobDescription" runat="server" Visible="false"></telerik:RadLabel>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblComponent" runat="server" Text="Component"></telerik:RadLabel>

                </td>
                <td>
                    <span id="spnPickListComponent">
                        <telerik:RadTextBox ID="txtComponentCode" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Width="100px">
                        </telerik:RadTextBox>
                        <telerik:RadTextBox ID="txtComponentName" runat="server" CssClass="input_mandatory" MaxLength="20"
                            Width="210px">
                        </telerik:RadTextBox>
                        <asp:ImageButton ID="imgComponent" runat="server"
                            ImageUrl="<%$ PhoenixTheme:images/picklist.png %>" Style="cursor: pointer; vertical-align: top" />
                        <telerik:RadTextBox ID="txtComponentId" runat="server" CssClass="input" Width="10px"></telerik:RadTextBox>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblSpecification" runat="server" Text="Specification"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtTechnicalSpec" runat="server" CssClass="input" Width="360px" TextMode="MultiLine" Rows="5"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblLocation" runat="server" Text="Location"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadTextBox ID="txtLocation" runat="server" CssClass="input" TextMode="MultiLine" Rows="2" Width="360px"></telerik:RadTextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblResponsibilty" runat="server" Text="Responsibilty"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadListBox CheckBoxes="true" ID="cblResponsibilty" runat="server" RepeatDirection="Horizontal">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td>
                    <telerik:RadLabel ID="lblEstimatedDurationHrs" runat="server" Text="Estimated Duration(Hrs)"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Number runat="server" ID="txtDuration" CssClass="input" Width="60px" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblPlannedStartDate" runat="server" Text="Planned Start Date"></telerik:RadLabel>

                </td>
                <td>
                    <eluc:Date ID="ucStartDate" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblCostClassification" runat="server" Text="Cost Classification"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadDropDownList DefaultMessage="--Select--" ID="ddlCostClassification" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true">
                    </telerik:RadDropDownList>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblWorktobesurveyedby" runat="server" Text="Work to be surveyed by"></telerik:RadLabel>

                </td>
                <td>
                    <telerik:RadListBox CheckBoxes="true" ID="cblWorkSurvey" runat="server" RepeatDirection="Horizontal">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblMaterial" runat="server" Text="Material"></telerik:RadLabel>

                </td>
                <td colspan="2">
                    <telerik:RadListBox CheckBoxes="true" ID="cblMaterial" runat="server" RepeatDirection="Horizontal">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadLabel ID="lblEnclosed" runat="server" Text="Enclosed"></telerik:RadLabel>

                </td>
                <td colspan="2">
                    <telerik:RadListBox CheckBoxes="true" ID="cblEnclosed" runat="server" RepeatDirection="Horizontal">
                    </telerik:RadListBox>
                </td>
            </tr>
            <tr runat="server" visible="false">
                <td>
                    <telerik:RadLabel ID="lblEstimatedDays" runat="server" Text="Estimated Days"></telerik:RadLabel>

                </td>
                <td colspan="2">
                    <telerik:RadRadioButtonList ID="rblEstimatedDays" runat="server" RepeatDirection="Horizontal">
                    </telerik:RadRadioButtonList>
                </td>
            </tr>
        </table>
        <hr />
        <div style="position: relative;">
            <eluc:TabStrip ID="MenuDryDockRepairJobLineItem" runat="server" OnTabStripCommand="DryDockRepairJobLineItem_TabStripCommand"></eluc:TabStrip>
        </div>
        <div id="divGrid" style="position: relative; z-index: +1;">
            <%--<asp:GridView ID="gvRepairJobLineItem" runat="server" AutoGenerateColumns="False"
                Font-Size="11px" Width="100%" CellPadding="3" OnRowCommand="gvRepairJobLineItem_RowCommand"
                OnRowDataBound="gvRepairJobLineItem_ItemDataBound" OnRowCancelingEdit="gvRepairJobLineItem_RowCancelingEdit"
                OnRowDeleting="gvRepairJobLineItem_RowDeleting" OnRowEditing="gvRepairJobLineItem_RowEditing"
                ShowFooter="true" ShowHeader="true" EnableViewState="false" OnSorting="gvRepairJobLineItem_Sorting"
                AllowSorting="true" OnRowCreated="gvRepairJobLineItem_RowCreated" OnSelectedIndexChanging="gvRepairJobLineItem_SelectedIndexChanging"
                OnRowUpdating="gvRepairJobLineItem_RowUpdating">
                <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                <SelectedRowStyle CssClass="datagrid_selectedstyle" />
                <RowStyle Height="10px" />--%>
            <telerik:RadGrid RenderMode="Lightweight" ID="gvRepairJobLineItem" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                CellSpacing="0" GridLines="None"
               
                OnNeedDataSource="gvRepairJobLineItem_NeedDataSource"
                OnItemCommand="gvRepairJobLineItem_ItemCommand"
                OnItemDataBound="gvRepairJobLineItem_ItemDataBound1"
                OnUpdateCommand="gvRepairJobLineItem_UpdateCommand">
                <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                <MasterTableView ShowFooter="true" EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                    AutoGenerateColumns="false" DataKeyNames="FLDJOBDETAILID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                   <NoRecordsTemplate>
                        <table width="100%" border="0">
                            <tr>
                                <td align="center">
                                    <telerik:RadLabel  ForeColor="Black"  ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                                </td>
                            </tr>
                        </table>
                    </NoRecordsTemplate>
                  
                    <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                    <Columns>
                        <telerik:GridTemplateColumn HeaderText="Job Detail">
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                           
                            <itemtemplate>
                            <telerik:RadLabel ID="lbljobdetailid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDTKey" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDTKEY") %>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblDetail" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'></telerik:RadLabel>
                        </itemtemplate>
                            <edititemtemplate>
                            <telerik:RadLabel ID="lbljobdetailidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDJOBDETAILID") %>'></telerik:RadLabel>
                            <telerik:RadTextBox ID="txtDetailEdit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESCRIPTION") %>'
                                CssClass="gridinput_mandatory" ToolTip="Job Detail"></telerik:RadTextBox>
                        </edititemtemplate>
                            <footertemplate>
                            <telerik:RadTextBox ID="txtDetailAdd" runat="server" CssClass="gridinput_mandatory"
                                Width="90%" ToolTip="Job Detail"></telerik:RadTextBox>
                        </footertemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText="Unit">
                            <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                           
                            <itemtemplate>
                            <telerik:RadLabel ID="lblUnit" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUNITNAME") %>'></telerik:RadLabel>
                        </itemtemplate>
                            <edititemtemplate>
                            <eluc:Unit ID="ucUnitEdit" CssClass="input" runat="server" AppendDataBoundItems="true"
                                UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' SelectedUnit='<%# DataBinder.Eval(Container,"DataItem.FLDUNIT") %>' />
                        </edititemtemplate>
                            <footertemplate>
                            <eluc:Unit ID="ucUnitAdd" CssClass="input" runat="server" AppendDataBoundItems="true"
                                UnitList='<%# PhoenixRegistersUnit.ListUnit()%>' />
                        </footertemplate>
                        </telerik:GridTemplateColumn>
                        <telerik:GridTemplateColumn HeaderText ="Action">
                            <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                      
                            <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                            <itemtemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                CommandName="EDIT" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdEdit"
                                ToolTip="Edit">
                                 <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Up" 
                                CommandName="MOVEUP" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveUp"
                                ToolTip="Move Up">
                                <span class="icon"><i class="fas fa-angle-double-up"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Move Down" 
                                CommandName="MOVEDOWN" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdMoveDown"
                                ToolTip="Move Down">
                                 <span class="icon"><i class="fas fa-angle-double-down"></i></span>
                            </asp:LinkButton>
                            <img id="Img3" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Delete" 
                                CommandName="DELETE" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdDelete"
                                ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </itemtemplate>
                            <edititemtemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                CommandName="Update" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdSave"
                                ToolTip="Save">
                                <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel"
                                CommandName="Cancel" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdCancel"
                                ToolTip="Cancel">
                                 <span class="icon"><i class="fas fa-trash-alt"></i></span>
                            </asp:LinkButton>
                        </edititemtemplate>
                            <footerstyle horizontalalign="Center" />
                            <footertemplate>
                            <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                CommandName="Add" CommandArgument='<%# Container.DataSetIndex %>' ID="cmdAdd"
                                ToolTip="Add New">
                                 <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </footertemplate>
                        </telerik:GridTemplateColumn>
                    </Columns>
                    <PagerStyle AlwaysVisible="true" Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                        PageSizeLabelText="Records per page:" CssClass=" RadGrid_Default rgPagerTextBox"  />
                </MasterTableView>
                <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                    <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                    <Scrolling AllowScroll="true" ScrollHeight="" />
                    <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                </ClientSettings>
            </telerik:RadGrid>
        </div>
        <asp:Button runat="server" ID="cmdHiddenPick" OnClick="cmdHiddenPick_Click" />
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:ConfirmMessage ID="ucConfirmMessage" runat="server" Text="" Visible="false" OnConfirmMesage="ucConfirmMessage_ConfirmMessage" style="z-index: 99" />


    </form>
</body>
</html>
