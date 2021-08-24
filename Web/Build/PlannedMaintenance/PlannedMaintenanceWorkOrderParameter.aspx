<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderParameter.aspx.cs" Inherits="PlannedMaintenance_PlannedMaintenanceWorkOrderParameter" %>

<%@ Register TagPrefix="eluc" TagName="Parameter" Src="~/UserControls/UserControlJobParameterValue.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

        <script type="text/javascript">

            function refreshParent() {                
                top.closeTelerikWindow('parameter','maint,RadWindow_NavigateUrl');
            }
        </script>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="radScriptManager1" />
        <telerik:RadSkinManager ID="radSkinManager1" runat="server"></telerik:RadSkinManager>

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="MenuParameter" runat="server" OnTabStripCommand="MenuParameter_TabStripCommand"></eluc:TabStrip>
        
        <telerik:RadGrid ID="gvJobParameter" runat="server" AllowSorting="false" AllowMultiRowEdit="true"
            CellSpacing="0" GridLines="None" OnNeedDataSource="gvJobParameter_NeedDataSource" OnItemDataBound="gvJobParameter_ItemDataBound"
            EnableViewState="true" EnableHeaderContextMenu="true">
            <SortingSettings SortedBackColor="#FFF6D6" EnableSkinSortStyles="false"></SortingSettings>
            <MasterTableView EditMode="InPlace" InsertItemPageIndexAction="ShowItemOnCurrentPage" HeaderStyle-Font-Bold="true" ShowHeadersWhenNoRecords="true" AllowNaturalSort="false"
                AutoGenerateColumns="false">
                <HeaderStyle Width="102px" />
                <Columns>
                    <telerik:GridTemplateColumn HeaderText="Parameter" HeaderStyle-Width="35%">
                        <ItemTemplate>
                            <telerik:RadLabel ID="lblParameterOptionId" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETEROPTIONID")%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblParameterId" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETERID")%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblValueId" runat="server" Visible="false" Text='<%#Bind("FLDVALUEID")%>'></telerik:RadLabel>
                            <telerik:RadLabel ID="lblParameterName" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDPARAMETERNAME") + ":" + (!DataBinder.Eval(Container, "DataItem.FLDMINVALUE").ToString().Equals("") ? "( " + DataBinder.Eval(Container, "DataItem.FLDMINVALUE") + " - " + DataBinder.Eval(Container, "DataItem.FLDMAXVALUE") + " )" : "")%>'></telerik:RadLabel>
                        </ItemTemplate>
                    </telerik:GridTemplateColumn>
                    <telerik:GridTemplateColumn HeaderText="Value" HeaderStyle-Width="25%">
                        <ItemTemplate>
                            <telerik:RadTextBox ID="txtJobParameterValue" runat="server" Text='<%#Bind("FLDVALUE")%>'></telerik:RadTextBox>
                            <eluc:Number ID="txtJobParameternumber" runat="server" CssClass="input" Width="100px" IsInteger="true" />
                            <eluc:Decimal ID="txtJobParameterdecimal" runat="server" CssClass="input" Width="100px" />
                            <telerik:RadDropDownList ID="ddlparameteroption" runat="server" DataTextField="FLDOPTIONNAME" DataValueField="FLDJOBPARAMETEROPTIONSID"></telerik:RadDropDownList>
                            <telerik:RadLabel ID="lblParameterType" runat="server" Visible="false" Text='<%#Bind("FLDPARAMETERTYPE")%>'></telerik:RadLabel>
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
            </MasterTableView>
            <ClientSettings EnableRowHoverStyle="true" AllowColumnsReorder="true" ReorderColumnsOnClient="true" AllowColumnHide="true" ColumnsReorderMethod="Reorder">
                <Selecting AllowRowSelect="true" EnableDragToSelectRows="false" UseClientSelectColumnOnly="true" />
                <Scrolling AllowScroll="true" UseStaticHeaders="true" />
                <Resizing EnableRealTimeResize="true" AllowResizeToFit="true" AllowColumnResize="true" />
            </ClientSettings>
        </telerik:RadGrid>

    </form>
</body>
</html>
