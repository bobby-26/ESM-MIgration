<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionCompanyVessel.aspx.cs" Inherits="InspectionCompanyVessel" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Inspection" Src="~/UserControls/UserControlInspection.ascx" %>
<%@ Register TagPrefix="eluc" TagName="VesselType" Src="~/UserControls/UserControlVesselType.ascx" %>
<%@ Register TagPrefix="eluc" TagName="AddressType" Src="~/UserControls/UserControlAddressType.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <div id="InspectionCompany" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>      
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmInspectionCompany" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1" runat="server">
    </ajaxToolkit:ToolkitScriptManager>

    <script type="text/javascript">

        // Maintain scroll position on list box. 
        var xPos, yPos;
        var prm = Sys.WebForms.PageRequestManager.getInstance();

        function setVesselTypeScroll() {
            var div = $get('<%=divVesselType.ClientID %>');
            var hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');
            hdn.value = div.scrollTop;
        }

        function BeginRequestHandler(sender, args) {
            var listBox = $get('<%= divVesselType.ClientID %>');
            var hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');

            if (listBox != null) {
                xPos = listBox.scrollLeft;
                yPos = listBox.scrollTop;
            }
        }

        function EndRequestHandler(sender, args) {
            var listBox = $get('<%= divVesselType.ClientID %>');
            var hdn = $get('<%= hdnVesselTypeScroll.ClientID %>');
            listBox.scrollTop = hdn.value;
        }

        prm.add_beginRequest(BeginRequestHandler);
        prm.add_endRequest(EndRequestHandler); 

    </script>
    
    <asp:UpdatePanel runat="server" ID="pnlBasicCause">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader" style="position: relative">
                    <div id="divHeading">
                        <eluc:Title runat="server" ID="ucTitle" Text="Vessel"></eluc:Title>
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuInspectionCompanyGeneral" runat="server" TabStrip="true" OnTabStripCommand="MenuInspectionCompanyGeneral_TabStripCommand">
                    </eluc:TabStrip>
                </div>                
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblCompany" runat="server" Text="Company "></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <asp:DropDownList ID="ddlCompany" runat="server" CssClass="input" AutoPostBack="true" Width="50%"></asp:DropDownList>
                        </td>
                        <td style="width: 10%">
                            <asp:Literal ID="lblCharterer" runat="server" Text="Charterer"></asp:Literal>
                        </td>
                        <td style="width: 20%">
                            <eluc:AddressType runat="server" ID="ucCharterer" CssClass="input" AppendDataBoundItems="true"
                                Width="80%" AutoPostBack="true" />
                        </td>
                    </tr>                    
                    <tr>
                        <td style="width: 10%">
                            <asp:Literal ID="lblVesselType" runat="server" Text="Vessel Type"></asp:Literal>
                        </td>
                        <td>   
                            <div id="divVesselType" runat="server" class="input" style="overflow: auto; width: 50%; height: 60px;"
                                onscroll="javascript:setVesselTypeScroll();">  
                                <asp:HiddenField ID="hdnVesselTypeScroll" runat="server" />
                                <asp:CheckBoxList ID="chkVeselTypeList" runat="server" Height="100%" AutoPostBack="true"
                                    RepeatColumns="1" RepeatDirection="Horizontal" OnSelectedIndexChanged="chkVeselTypeList_Changed"
                                    RepeatLayout="Flow">
                                </asp:CheckBoxList>
                            </div>
                        </td>
                        <td colspan="2"></td>
                    </tr>                    
                </table>
                <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuInspectionCompany" runat="server" OnTabStripCommand="InspectionCompany_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0">
                    <asp:GridView ID="gvInspectionCompany" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                        Width="100%" CellPadding="3"  ShowFooter="true" OnRowCommand ="gvInspection_RowCommand"                         
                        ShowHeader="true" EnableViewState="false" DataKeyNames ="FLDDTKEY" OnRowDataBound="gvInspection_RowDataBound">
                        <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <Columns>
                            <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblCompanyNameHeader" runat="server">Company Name</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblCompanyName" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDCOMPANYNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblvettingInspectionHeader" runat="server"> Vetting Inspection</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVettingInspection" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINSPECTIONNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField >
                                <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                                <HeaderTemplate>
                                    <asp:Label ID="lblVesselHeader" runat="server">Vessel</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblVessel" runat="server" Visible="true" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELNAME") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <div id="divPage" style="position: relative;">
                    <table width="100%" border="0" class="datagrid_pagestyle">
                        <tr>
                            <td nowrap="nowrap" align="center">
                                <asp:Label ID="lblPagenumber" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblPages" runat="server">
                                </asp:Label>
                                <asp:Label ID="lblRecords" runat="server">
                                </asp:Label>&nbsp;&nbsp;
                            </td>
                            <td nowrap="nowrap" align="left" width="50px">
                                <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                            </td>
                            <td width="20px">
                                &nbsp;
                            </td>
                            <td nowrap="nowrap" align="right" width="50px">
                                <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                            </td>
                            <td nowrap="nowrap" align="center">
                                <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                </asp:TextBox>
                                <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                    Width="40px"></asp:Button>
                            </td>
                        </tr>
               </table>
                </div>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
