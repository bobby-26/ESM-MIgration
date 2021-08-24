<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PurchaseCommittedBudgetCodeWise.aspx.cs" Inherits="PurchaseCommittedBudgetCodeWise" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Import Namespace="SouthNests.Phoenix.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Confirm" Src="~/UserControls/UserControlConfirmMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="ToolTip" Src="~/UserControls/UserControlToolTip.ascx" %>
<%@ Register TagPrefix="eluc" TagName="BudgetCode" Src="~/UserControls/UserControlBudgetCode.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Committed Cost Report Budget Code wise</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmBudgetCodeCommittedCost" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlOrderForm">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="frmTitle" Text="Committed Cost - Budget Code wise"></eluc:Title>
                    <%--<asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click" />--%>
                </div>
                <%--<div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuOrderFormMain" runat="server" OnTabStripCommand="MenuOrderFormMain_TabStripCommand"
                        TabStrip="true" ></eluc:TabStrip>
                </div>--%>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                               <asp:Literal ID="lblCommittedDateFrom" runat="server" Text=" Committed Date (From)"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="ucFromDate" runat="server" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="ucFromDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                            <td>
                                <asp:Literal ID="lblCommittedDateTo" runat="server" Text="Committed Date (To)"></asp:Literal>
                            </td>
                            <td>
                                <asp:TextBox ID="ucToDate" runat="server" CssClass="input"></asp:TextBox>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" Format="dd/MMM/yyyy"
                                    Enabled="True" TargetControlID="ucToDate" PopupPosition="TopLeft">
                                </ajaxToolkit:CalendarExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <asp:Literal ID="lblFleet" runat="server" Text="Fleet"></asp:Literal>
                            </td>
                            <td>
                                <div runat="server" id="divFleet" class="input" style="overflow: auto;width: 60%;height: 80px" onscroll="javascript:setFleetScroll();">
                                    <asp:HiddenField ID="hdnScrollFleet" runat="server" />
                                    <asp:CheckBoxList ID="chkFleetList" runat="server" AutoPostBack="true" Height="100%"
                                        OnSelectedIndexChanged="chkFleetList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                            <td>
                               <asp:Literal ID="lblVessel" runat="server" Text="Vessel"></asp:Literal>
                            </td>
                            <td>
                                <div runat="server" id="dvVessel" class="input" style="overflow: auto;width: 40%;height: 80px" onscroll="javascript:setVesselScroll();">
                                    <asp:HiddenField ID="hdnScrollVessel" runat="server" />
                                    <asp:CheckBoxList ID="chkVesselList" runat="server" Height="100%" AutoPostBack="true"
                                        OnSelectedIndexChanged="chkVesselList_Changed" RepeatColumns="1" RepeatDirection="Horizontal"
                                        RepeatLayout="Flow">
                                    </asp:CheckBoxList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                               <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                            </td>
                            <td>
                                <eluc:BudgetCode runat="server" ID="ucBudgetCode" AppendDataBoundItems="true" CssClass="input" AutoPostBack="true" />
                            </td>
                        </tr>
                    </table>
                </div>
                <br />
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuOrderForm" runat="server" OnTabStripCommand="MenuOrderForm_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 1; width: 100%;">
                    <asp:GridView ID="gvCommittedCost" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3" OnRowCommand="gvCommittedCost_RowCommand" OnRowDataBound="gvCommittedCost_ItemDataBound"
                        ShowHeader="true" EnableViewState="false">
                        
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" BorderColor="#FF0066" />
                        <RowStyle Height="10px" />
                        
                        <Columns>
                            <asp:TemplateField HeaderText="number">
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblVesselName" runat="server" Text="Vessel Name"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblOrderId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORDERID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselId" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELID") %>'></asp:Label>
                                    <asp:Label ID="lblVesselName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblBudgetCode" runat="server" Text="Budget Code"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetCode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSUBACCOUNT") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblDescription" runat="server" Text="Description"></asp:Literal>
                                   
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetDescription" runat="server" Text='<%# DataBinder.Eval(Container, "DataItem.FLDDESCRIPTION") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblBudgetGroup" runat="server" Text="Budget Group "></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblBudgetGroup" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDHARDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblPONumber" runat="server" Text="PO Number"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPONumber" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREFERENCENUMBER") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Literal ID="lblAmountUSD" runat="server" Text="Amount (USD)"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblAmountInUSD" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDAMOUNTINUSD") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                  <asp:Literal ID="lblCommittedDate" runat="server" Text="Committed Date"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCommittedDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFAPPROVAL","{0:dd/MMM/yyyy}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            
                            <asp:TemplateField>
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                  <asp:Literal ID="lblVendor" runat="server" Text="Vendor"></asp:Literal>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVendor" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>

    <script type="text/javascript">

        // Maintain scroll position on list box. 
        //var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function setVesselScroll() {
            var div = $get('<%=dvVessel.ClientID %>');
            var hdn = $get('<%= hdnScrollVessel.ClientID %>');
            hdn.value = div.scrollTop;
        }

        function setFleetScroll() {
            var div1 = $get('<%=divFleet.ClientID %>');
            var hdn1 = $get('<%= hdnScrollFleet.ClientID %>');
            hdn1.value = div1.scrollTop;
        }

//        function BeginRequestHandler(sender, args) {
//            var listBox = $get('<%= dvVessel.ClientID %>');
//            var hdn = $get('<%= hdnScrollVessel.ClientID %>');

//            if (listBox != null) {
//                xPos = listBox.scrollLeft;
//                yPos = listBox.scrollTop;
//            }
//        }

        function EndRequestHandler(sender, args) {
        
            var listBox = $get('<%= dvVessel.ClientID %>');
            var hdn = $get('<%= hdnScrollVessel.ClientID %>');
            listBox.scrollTop = hdn.value;

            var listBox1 = $get('<%= divFleet.ClientID %>');
            var hdn1 = $get('<%= hdnScrollFleet.ClientID %>');
            listBox1.scrollTop = hdn1.value;
        }

//        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler); 
    </script>
</body>
</html>
