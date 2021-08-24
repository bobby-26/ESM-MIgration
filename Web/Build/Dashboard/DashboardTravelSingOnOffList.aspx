<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTravelSingOnOffList.aspx.cs"
    Inherits="Dashboard_DashboardTravelSingOnOffList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Travel SignOnOff List</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="ds" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"
        runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlTravelSignOnOff">
        <ContentTemplate>
            <div>
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="">
                    </eluc:Title>
                </div>
                <div class="navSelect" style="top: 0px; right: 2px; position: absolute;">
                    <eluc:TabStrip ID="MenuDdashboradVesselParticulrs" runat="server" OnTabStripCommand="MenuDdashboradVesselParticulrs_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblTravelOnSigner" runat="server" Text="Travel On Signer"></asp:Literal> </b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div class="navSelect" style="position: relative; width: 15px">
                                <eluc:TabStrip ID="MenuTravelOnSignerList" runat="server" OnTabStripCommand="TravelOnSignerList_TabStripCommand">
                                </eluc:TabStrip>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:GridView GridLines="None" ID="gvTravelSignOn" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvTravelSignOn_ItemDataBound" EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEmployeeId" runat="server" Text="Employee Id"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbllEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblDateOfBirth" runat="server" Text="Date Of Birth"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPassportNo" runat="server" Text="Passport No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSeamanBookNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVisaNo" runat="server" Text="Visa No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVisaNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSVISANUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblRequestNo" runat="server" Text="Request No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblTravelDate" runat="server" Text="Travel Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTravelDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblArrivalDate" runat="server" Text="Arrival Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblOnSignerYN" runat="server" Text="On Signer YN"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnsignerYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERYN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblOrigin" runat="server" Text="Origin"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrigin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblDestination" runat="server" Text="Destination"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentMode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <b><asp:Literal ID="lblTravelOffSigner" runat="server" Text="Travel Off Signer"></asp:Literal> </b>
                        </td>
                    </tr>
                    </table>
                 
                 <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuTravelOffSignerList" runat="server" OnTabStripCommand="TravelOffSignerList_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <table cellpadding="2" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:GridView GridLines="None" ID="gvTravelSignOff" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                                Width="100%" CellPadding="3" OnRowDataBound="gvTravelSignOff_ItemDataBound" EnableViewState="false">
                                <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                                <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                                <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEmployeeId" runat="server" Text="Employee Id"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEmployeeID" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDEMPLOYEEID") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lbllEmployeeName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblRank" runat="server" Text="Rank"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRank" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRANKNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblDateOfBirth" runat="server" Text="Date Of Birth"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPassportNo" runat="server" Text="Passport No"></asp:Literal>
                                        </HeaderTemplate>
                                        
                                        <ItemTemplate>
                                            <asp:Label ID="lblPassportNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPASSPORTNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblSeamanBookNo" runat="server" Text="Seaman Book No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSeamanBookNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblVisaNo" runat="server" Text="Visa No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblVisaNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDUSVISANUMBER") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblRequestNo" runat="server" Text="Request No"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREQUISITIONNO") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblTravelDate" runat="server" Text="Travel Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTravelDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDTRAVELDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblArrivalDate" runat="server" Text="Arrival Date"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblArrivalDate" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDARRIVALDATE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblOnSignerYN" runat="server" Text="On Signer YN"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOnsignerYN" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDONSIGNERYN") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblOrigin" runat="server" Text="Origin"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblOrigin" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDORIGINNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblDestination" runat="server" Text="Destination"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblDestination" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDESTINATIONNAME") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemStyle Wrap="False" HorizontalAlign="Left" Width="7%"></ItemStyle>
                                        <HeaderTemplate>
                                            <asp:Literal ID="lblPaymentMode" runat="server" Text="Payment Mode"></asp:Literal>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Label ID="lblPaymentMode" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDPAYMENTMODE") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
