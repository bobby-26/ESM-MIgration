<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVoyagePlanDetail.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceVoyagePlanDetail" 
    MaintainScrollPositionOnPostback="true"%>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Import Namespace="SouthNests.Phoenix.Inspection" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Element" Src="~/UserControls/UserControlRACategory.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voyage Plan</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {                               
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';                
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvActivity.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
        .bg-success {
            background-color: #1c84c6;
            color: #ffffff;
            text-align: center;
        }
        .overflow {
            overflow-y: scroll;
        }
        .alignleft{
            text-align:left;
        }
    </style>      
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />        
        <telerik:RadWindowManager ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <div class="bg-success">
            <br />
            <h3><asp:Literal ID="lblHeading" runat="server"></asp:Literal></h3>
            <br />
        </div>
        <eluc:Status ID="ucStatus" runat="server" />
        <eluc:TabStrip ID="MenuMain" runat="server" OnTabStripCommand="MainMenu_TabStripCommand"></eluc:TabStrip>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="75%" CssClass="overflow">        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuPartA" runat="server" OnTabStripCommand="MenuPartA_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvActivity" runat="server" OnItemDataBound="gvActivity_ItemDataBound"
            OnNeedDataSource="gvActivity_NeedDataSource" OnItemCommand="gvActivity_ItemCommand" OnSortCommand="gvActivity_SortCommand"
            ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" MasterTableView-ShowFooter="false" EnableViewState="true">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDVOYAGEACTIVITYID">                
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Element" UniqueName="FLDELEMENTNAME">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDELEMENTNAME"]%>
                        </ItemTemplate>                        
                         <FooterTemplate>
                            <eluc:Element ID="ddlElement" runat="server" AppendDataBoundItems="true" CssClass="input_mandatory"
                                CategoryList = "<%#PhoenixInspectionRiskAssessmentCategory.ListRiskAssessmentCategory()%>" AutoPostBack="true" OnTextChangedEvent="ddlElement_TextChangedEvent"  />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Activity" UniqueName="FLDACTIVITYNAME">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDACTIVITYNAME"]%>
                        </ItemTemplate>                         
                         <FooterTemplate>
                            <telerik:RadComboBox ID="ddlActivity" runat="server" DataTextField="FLDNAME" DataValueField="FLDACTIVITYID" CssClass="input_mandatory"
                                EmptyMessage="Type to select Activity" Filter="Contains" MarkFirstMatch="true" CheckBoxes="true" EnableCheckAllItemsCheckBox="true" >
                            </telerik:RadComboBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>      
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>                           
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>                       
                        <FooterStyle HorizontalAlign="Center" />
                        <FooterTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Add" ID="cmdAdd" ToolTip="Add New">
                                    <span class="icon"><i class="fa fa-plus-circle"></i></span>
                            </asp:LinkButton>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
             <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
            <br />
        <eluc:TabStrip ID="MenuPartB" runat="server" OnTabStripCommand="MenuPartB_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvVoyagePlan" runat="server" OnItemDataBound="gvVoyagePlan_ItemDataBound"
            OnNeedDataSource="gvVoyagePlan_NeedDataSource" OnItemCommand="gvVoyagePlan_ItemCommand" OnSortCommand="gvVoyagePlan_SortCommand"
            EnableViewState="false" ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Work Order No.">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCATEGORY"]%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Planned Date">
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Duration">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPLANINGDURATIONINDAYS"].ToString().Equals("") ? "" : ((DataRowView)Container.DataItem)["FLDPLANINGDURATIONINDAYS"].ToString() + " Days"%>
                            <%#((DataRowView)Container.DataItem)["FLDPLANNINGESTIMETDURATION"].ToString().Equals("") ? "" : ((DataRowView)Container.DataItem)["FLDPLANNINGESTIMETDURATION"].ToString() + " Hours"%>                           
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Assigned To">
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemTemplate>
                           <%# ((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>                   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
             <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        <br />
        <eluc:TabStrip ID="MenuPartC" runat="server" OnTabStripCommand="MenuPartC_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvMPartC" runat="server" OnItemDataBound="gvVoyagePlan_ItemDataBound"
            OnNeedDataSource="gvMPartC_NeedDataSource" OnItemCommand="gvVoyagePlan_ItemCommand" OnSortCommand="gvMPartC_SortCommand"
            EnableViewState="false" ShowFooter="false" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDWODETAILID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Work Order No.">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDWORKORDERNUMBER"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Category">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDCATEGORY"]%>
                        </ItemTemplate>                       
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Planned Date">
                        <ItemTemplate>
                            <%#General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDPLANNINGDUEDATE"])%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Duration">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDPLANINGDURATIONINDAYS"].ToString().Equals("") ? "" : ((DataRowView)Container.DataItem)["FLDPLANINGDURATIONINDAYS"].ToString() + " Days"%>
                            <%#((DataRowView)Container.DataItem)["FLDPLANNINGESTIMETDURATION"].ToString().Equals("") ? "" : ((DataRowView)Container.DataItem)["FLDPLANNINGESTIMETDURATION"].ToString() + " Hours"%>                           
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Assigned To">
                        <ItemTemplate>
                            <%# ((DataRowView)Container.DataItem)["FLDDISCIPLINENAME"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemTemplate>
                           <%# ((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>                   
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDIT" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                </Columns>
                <NoRecordsTemplate>
                    <table width="100%" border="0">
                        <tr>
                            <td align="center">
                                <telerik:RadLabel ID="noRecordFound" runat="server" Text="No Records Found" Font-Size="Larger" Font-Bold="true"></telerik:RadLabel>
                            </td>
                        </tr>
                    </table>
                </NoRecordsTemplate>
                <PagerStyle Mode="NextPrevNumericAndAdvanced" PageButtonCount="10" PagerTextFormat="{4}<strong>{5}</strong> Records matching your search criteria"
                                PageSizeLabelText="Records per page:" AlwaysVisible="true" CssClass="RadGrid_Default rgPagerTextBox" />
            </MasterTableView>
             <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true"  ScrollHeight="115px"/>
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all" DecorationZoneID="modalPopup"></telerik:RadFormDecorator>
         <telerik:RadWindow ID="modalPopup" runat="server" Width="600px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>         
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1" OnAjaxRequest="RadAjaxPanel2_AjaxRequest">
                    <table border="0" style="width: 100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblElement" runat="server" Text="Element"></telerik:RadLabel>
                            </td>
                            <td>
                               <telerik:RadCheckBoxList runat="server" ID="cblElement" Columns="2" Layout="Flow" AutoPostBack="true" 
                                   OnSelectedIndexChanged="cblElement_SelectedIndexChanged" CausesValidation="false">
                               </telerik:RadCheckBoxList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblActivity" runat="server" Text="Activity"></telerik:RadLabel>
                            </td>
                            <td>
                               <telerik:RadCheckBoxList runat="server" ID="cblActivity" Columns="2" Layout="Flow"
                                   DataBindings-DataTextField="FLDNAME" DataBindings-DataValueField="FLDACTIVITYID" AppendDataBoundItems="true" >
                               </telerik:RadCheckBoxList>
                                <asp:RequiredFieldValidator ID="ActivityValidator" runat="server"
                                    ControlToValidate="cblActivity"
                                    Display="None"
                                    ErrorMessage="select atleast one activity" ></asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">                                
                                <telerik:RadButton ID="btnCreate" Text="Create" runat="server" OnClick="btnCreate_Click"></telerik:RadButton>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                 <asp:ValidationSummary ID="ValidationSummary1" runat="server" Width="200px" CssClass="rfdValidationSummaryControl alignleft"
                                        BorderWidth="1px" HeaderText="List of errors"></asp:ValidationSummary>
                            </td>
                        </tr>
                    </table>
                </telerik:RadAjaxPanel>             
            </ContentTemplate>
        </telerik:RadWindow>
        <telerik:RadCodeBlock runat="server" ID="rdbScripts">
            <script type="text/javascript">
                $modalWindow.modalWindowID = "<%=modalPopup.ClientID %>";
            </script>
        </telerik:RadCodeBlock>
    </form>
</body>
</html>
