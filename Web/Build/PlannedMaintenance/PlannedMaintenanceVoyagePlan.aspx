<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceVoyagePlan.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceVoyagePlan" %>

<%@ Import Namespace="System.Data" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Port" Src="~/UserControls/UserControlMultiColumnPort.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Voyage Plan</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript">
            function CloseWindow() {                
                document.getElementById('<%=DateFromFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=DateToRequiredFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=FromPortRequiredFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=ToPortRequiredFieldValidator.ClientID%>').innerHTML = "";
                document.getElementById('<%=ValidationSummary1.ClientID%>').style.display = 'none';                
            }
            function CloseModelWindow() {
                var radwindow = $find('<%=modalPopup.ClientID %>');
                radwindow.close();
                var masterTable = $find("<%= gvVoyagePlan.ClientID %>").get_masterTableView();
                masterTable.rebind();
            }
        </script>
    </telerik:RadCodeBlock>
    <style type="text/css">
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
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%">        
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuJobParameter" runat="server" OnTabStripCommand="MenuJobParameter_TabStripCommand"></eluc:TabStrip>
        <telerik:RadGrid ID="gvVoyagePlan" runat="server" Height="94%" OnItemDataBound="gvVoyagePlan_ItemDataBound"
            OnNeedDataSource="gvVoyagePlan_NeedDataSource" OnItemCommand="gvVoyagePlan_ItemCommand" OnSortCommand="gvVoyagePlan_SortCommand"
            EnableViewState="true" ShowFooter="true" AllowCustomPaging="true" AllowPaging="true" EnableHeaderContextMenu="true" MasterTableView-ShowFooter="false">
            <MasterTableView EditMode="InPlace" AutoGenerateColumns="false" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" DataKeyNames="FLDPLANID">
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="No.">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDetail" runat="server"  CommandName="SELECT" CommandArgument='<%# ((DataRowView)Container.DataItem)["FLDPLANID"]%>'
                                Text='<%#((DataRowView)Container.DataItem)["FLDVOYAGENUMBER"]%>'></asp:LinkButton>
                        </ItemTemplate>
                        <%-- <EditItemTemplate>
                            <telerik:RadTextBox ID="txtVoyageNumber" ToolTip="No." runat="server" Width="100%" MaxLength="10" CssClass="gridinput_mandatory"
                                Text='<%#((DataRowView)Container.DataItem)["FLDVOYAGENUMBER"]%>'>
                            </telerik:RadTextBox>
                        </EditItemTemplate>--%>
                         <FooterTemplate>
                            <telerik:RadTextBox ID="txtVoyageNumberAdd" ToolTip="No." runat="server" Width="100%" MaxLength="10" CssClass="gridinput_mandatory">
                            </telerik:RadTextBox>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Port From">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDFROMPORT"]%>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <eluc:Port ID="ddlFromPort" runat="server"  SelectedValue='<%#((DataRowView)Container.DataItem)["FLDFROMPORTID"].ToString()%>' Text='<%#((DataRowView)Container.DataItem)["FLDFROMPORT"].ToString()%>'/>
                        </EditItemTemplate>--%>
                        <FooterTemplate>
                           <eluc:Port ID="ddlFromPortAdd" runat="server" Width="100%" CssClass="gridinput_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Port To">
                        <ItemTemplate>
                            <%#((DataRowView)Container.DataItem)["FLDTOPORT"]%>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <eluc:Port ID="ddlToPort" runat="server" SelectedValue='<%#((DataRowView)Container.DataItem)["FLDTOPORTID"].ToString()%>' Text='<%#((DataRowView)Container.DataItem)["FLDTOPORT"].ToString()%>'/>
                        </EditItemTemplate>--%>
                        <FooterTemplate>
                            <eluc:Port ID="ddlToPortAdd" runat="server" Width="100%" CssClass="gridinput_mandatory"/>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                     <telerik:GridTemplateColumn HeaderText="Date From">
                        <ItemTemplate>
                           <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDFROMDATE"])%>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                             <eluc:Date ID="txtFromDate" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDFROMDATE"].ToString()%>' />
                        </EditItemTemplate>--%>
                        <FooterTemplate>
                            <eluc:Date ID="txtFromDateAdd" runat="server" Width="100%" CssClass="input_mandatory"/>
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Date To">
                        <ItemTemplate>
                            <%# General.GetDateTimeToString(((DataRowView)Container.DataItem)["FLDTODATE"])%>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                            <eluc:Date ID="txtToDate" runat="server" Text='<%#((DataRowView)Container.DataItem)["FLDTODATE"].ToString()%>' />
                        </EditItemTemplate>--%>
                        <FooterTemplate>
                            <eluc:Date ID="txtToDateAdd" runat="server" Width="100%" CssClass="input_mandatory" />
                        </FooterTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Status">
                        <ItemTemplate>
                           <%#((DataRowView)Container.DataItem)["FLDSTATUS"]%>
                        </ItemTemplate>                         
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Rev. No">
                        <ItemTemplate>
                           <%#((DataRowView)Container.DataItem)["FLDREVISIONNUMBER"]%>
                        </ItemTemplate>                        
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn UniqueName="Action" HeaderText="Action">
                        <HeaderStyle />
                        <ItemStyle Wrap="false" />
                        <ItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Edit" CommandName="EDITR" ID="cmdEdit" ToolTip="Edit">
                                    <span class="icon"><i class="fas fa-edit"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Delete" CommandName="DELETE" ID="cmdDelete" ToolTip="Delete" Visible="false">
                                    <span class="icon"><i class="fas fa-trash"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Revise" CommandName="REVISE" ID="cmdRevise" ToolTip="Revise">
                                    <span class="icon"><i class="fab fa-rev"></i></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Copy" CommandName="COPY" ID="cmdCopy" ToolTip="Copy">
                                    <span class="icon"><i class="fas fa-copy"></i></span>
                            </asp:LinkButton>
                        </ItemTemplate>
                        <%--<EditItemTemplate>
                            <asp:LinkButton runat="server" AlternateText="Save" CommandName="Update" ID="cmdSave" ToolTip="Save">
                                    <span class="icon"><i class="fas fa-save"></i></span>
                            </asp:LinkButton>
                            <asp:LinkButton runat="server" AlternateText="Cancel" CommandName="Cancel" ID="cmdCancel" ToolTip="Cancel">
                                    <span class="icon"><i class="fas fa-times-circle"></i></span>
                            </asp:LinkButton>
                        </EditItemTemplate>--%>
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
                <Scrolling AllowScroll="true" UseStaticHeaders="true" SaveScrollPosition="true" ScrollHeight="415px" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>
        </telerik:RadAjaxPanel>
        <telerik:RadFormDecorator ID="FormDecorator1" runat="server" DecoratedControls="all"></telerik:RadFormDecorator>
        <telerik:RadWindow ID="modalPopup" runat="server" Width="500px" Height="365px" Modal="true" OnClientClose="CloseWindow" OffsetElementID="main"
             VisibleStatusbar="false" KeepInScreenBounds="true">
            <ContentTemplate>         
                <telerik:RadAjaxPanel ID="RadAjaxPanel2" runat="server" LoadingPanelID="LoadingPanel1" OnAjaxRequest="RadAjaxPanel2_AjaxRequest">       
                    <table border="0" style="width:100%">
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTitle" runat="server" Text="No."></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtNo" runat="server" ReadOnly="true" Enabled="false"
                                    MaxLength="200" Width="180px">
                                </telerik:RadTextBox>                               
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPortFrom" runat="server" Text="Port From"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Port ID="ddlFromPort" runat="server" CssClass="gridinput_mandatory" />
                                <asp:RequiredFieldValidator
                                    ID="FromPortRequiredFieldValidator"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="ddlFromPort:RadMCPort"
                                    EnableClientScript="true" ForeColor="Red"
                                    ErrorMessage="* From Port is required!">*
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPortTo" runat="server" Text="Port To"></telerik:RadLabel>
                            </td>
                            <td>
                                <eluc:Port ID="ddlToPort" runat="server" CssClass="gridinput_mandatory" />
                                 <asp:RequiredFieldValidator
                                    ID="ToPortRequiredFieldValidator"
                                    runat="server"
                                    Display="Dynamic"
                                    ControlToValidate="ddlToPort:RadMCPort"
                                    EnableClientScript="true" ForeColor="Red"
                                    ErrorMessage="* To Port is required!">*
                                </asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDateFrom" runat="server" Text="Date From"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDateFrom" runat="server" Width="120px" CssClass="input_mandatory" />
                                <asp:RequiredFieldValidator ID="DateFromFieldValidator" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtDateFrom" ErrorMessage="* Date From is required.">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblDateTo" runat="server" Text="Date To"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtDateTo" runat="server" Width="120px" CssClass="input_mandatory" />
                                <asp:RequiredFieldValidator ID="DateToRequiredFieldValidator" runat="server" Display="Dynamic" ForeColor="Red"
                                ControlToValidate="txtDateTo" ErrorMessage="* Date To is required.">*</asp:RequiredFieldValidator>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="center">
                                <telerik:RadLabel ID="lblPlanId" runat="server" Visible="false"></telerik:RadLabel>
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
