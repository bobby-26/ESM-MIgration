<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DryDockAdhocJobWorkRequest.aspx.cs" Inherits="DryDockAdhocJobWorkRequest" %>

<%@ Import Namespace="System.Data" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitleTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Split" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Discipline" Src="~/UserControls/UserControlDiscipline.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Work Order Done History</title>
    <telerik:RadCodeBlock runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock>

</head>
<body>
    <form id="frmWorkOrder" runat="server" autocomplete="off">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
            <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>

            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuWorkOrder" runat="server" OnTabStripCommand="MenuWorkOrder_TabStripCommand" TabStrip="true"></eluc:TabStrip>
            </div>
            <div style="font-weight: 600; font-size: 12px;" runat="server">
                <eluc:TabStrip ID="MenuDivWorkOrder" runat="server" OnTabStripCommand="MenuDivWorkOrder_TabStripCommand"></eluc:TabStrip>
            </div>
            <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                <%-- <asp:GridView ID="gvWorkOrder" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" ShowHeader="true" OnRowDataBound="gvWorkOrder_ItemDataBound"
                        OnRowUpdating="gvWorkOrder_RowUpdating" OnRowCancelingEdit="gvWorkOrder_RowCancelingEdit"
                        OnRowEditing="gvWorkOrder_RowEditing" EnableViewState="false" DataKeyNames="FLDWORKORDERID">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>                     --%>
                <telerik:RadGrid RenderMode="Lightweight" ID="gvWorkOrder" runat="server" AllowCustomPaging="true" AllowSorting="true" AllowPaging="true"
                    CellSpacing="0" GridLines="None"
                   
                    OnNeedDataSource="gvWorkOrder_NeedDataSource"
                    OnItemDataBound ="gvWorkOrder_ItemDataBound1"
                    OnItemCommand="gvWorkOrder_ItemCommand"
                    OnUpdateCommand="gvWorkOrder_UpdateCommand">
                    <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>

                    <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                        AutoGenerateColumns="false" DataKeyNames="FLDWORKORDERID" TableLayout="Fixed" CommandItemDisplay="Top" Height="10px">
                        <HeaderStyle Width="102px" />
                        <CommandItemSettings ShowRefreshButton="false" ShowPrintButton="false" ShowExportToExcelButton="false" ShowAddNewRecordButton="false" ShowExportToPdfButton="false"></CommandItemSettings>
                        <Columns>
                            <telerik:GridTemplateColumn HeaderText="Work Order Number">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%# DataBinder.Eval(Container,"DataItem.FLDWORKORDERNUMBER") %>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Work Order Title">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDWORKORDERNAME"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Component">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNUMBER"]%> - <%#((DataRowView)Container.DataItem)["FLDCOMPONENTNAME"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Priority">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDPLANINGPRIORITY"]%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Due Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Status">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDSTATUSNAME"] %>
                                </itemtemplate>
                                <edititemtemplate>
                                    <telerik:RadDropDownList ID="ddlStatus" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnSelectedIndexChanged="ddlStatus_SelectedIndexChanged">
                                        
                                       <Items>
                                           <telerik:DropDownListItem Value="1" Text="Active" />
                                           <telerik:DropDownListItem  Value="0" Text="Cancelled" />
                                           <telerik:DropDownListItem Value="2" Text="Postponed" />
                                       </Items>
                                      
                                    </telerik:RadDropDownList>
                                    <telerik:RadLabel ID="lblStatus" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDSTATUS"] %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblVesselId" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDVESSELID"] %>'></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblJobRegister" runat="server" Visible="false" Text='<%#((DataRowView)Container.DataItem)["FLDJOBREGISTER"] %>'></telerik:RadLabel>
                                </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Reason">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDREASON"] %>
                                </itemtemplate>
                                <edititemtemplate>
                                    <telerik:RadTextBox ID="txtReason" CssClass="gridinput_mandatory" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDREASON"] %>'></telerik:RadTextBox>
                                </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="PostPoned Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                   <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDPOSTPONEDDATE"])%>
                                </itemtemplate>
                                <edititemtemplate>
                                    <eluc:Date id="txtPostponedDate" runat="server" CssClass="input" DatePicker="true" Text='<%#((DataRowView)Container.DataItem)["FLDPOSTPONEDDATE"]%>' />
                                </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Completed Satisfactorily">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDCOMPSATISFACTORILYNAME"]%>
                                </itemtemplate>
                                <edititemtemplate>
                                    <telerik:RadDropDownList  ID="ddlCompletedStatisfactorilyYN" runat="server" CssClass="input">
                                        <Items>
                                            <telerik:DropDownListItem Value="" Text="--Select--" />
                                            <telerik:DropDownListItem  Value="1" Text="Yes" />
                                            <telerik:DropDownListItem  Value="0" Text="No" />
                                        </Items>
                                                                     
                                    </telerik:RadDropDownList>
                                </edititemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Done Date">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#string.Format("{0:dd/MMM/yyyy}", ((DataRowView)Container.DataItem)["FLDWORKDONEDATE"])%>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn HeaderText="Done By">
                                <itemstyle wrap="False" horizontalalign="Left"></itemstyle>
                                <itemtemplate>
                                    <%#((DataRowView)Container.DataItem)["FLDWORKDONEBY"] %>
                                </itemtemplate>
                            </telerik:GridTemplateColumn>
                            <telerik:GridTemplateColumn>
                                <headerstyle horizontalalign="Center" verticalalign="Middle"></headerstyle>
                                <headertemplate>                                  
                                        <telerik:RadLabel ID="lblActionHeader" runat="server">
                                    Action
                                        </telerik:RadLabel>                                    
                                </headertemplate>
                                 <itemstyle wrap="False" horizontalalign="Center" width="100px"></itemstyle>
                                <itemtemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Edit" 
                                        CommandName="EDIT" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdEdit"
                                        ToolTip="Edit">
                                         <span class="icon"><i class="fas fa-edit"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />                                    
                                </itemtemplate>
                                <edititemtemplate>
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Save" 
                                        CommandName="Update" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdSave"
                                        ToolTip="Save">
                                        <span class="icon"><i class="fas fa-save"></i></span>
                                    </asp:LinkButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                                    <asp:LinkButton Width="20px" Height="20px" runat="server" AlternateText="Cancel" 
                                        CommandName="Cancel" CommandArgument="<%# Container.DataSetIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel">
                                        <span class="icon"><i class="fas fa-trash"></i></span>
                                    </asp:LinkButton>
                                </edititemtemplate>
                                <footerstyle horizontalalign="Center" />
                            </telerik:GridTemplateColumn>
                        </Columns>
                        <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                            PageSizeLabelText="Records per page:" />
                    </MasterTableView>
                    <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                        <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                        <Scrolling AllowScroll="true" ScrollHeight="" />
                        <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
                    </ClientSettings>
                </telerik:RadGrid>

            </div>
        </div>

        <eluc:Split runat="server" ID="ucSplit" TargetControlID="ifMoreInfo" />
    </form>
</body>
</html>
