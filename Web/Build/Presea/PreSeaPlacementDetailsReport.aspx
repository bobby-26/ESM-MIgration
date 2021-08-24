<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaPlacementDetailsReport.aspx.cs" Inherits="Presea_PreSeaPlacementDetailsReport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="PreSeaBatch" Src="~/UserControls/UserControlPreSeaBatch.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Presea Placement Details</title>
     <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

   </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlPreSea">
        <ContentTemplate>
            <div style="top: 100px; margin-left: auto; margin-right: auto; width: 100%;">
                <div style="top: 100px; margin-left: auto; margin-right: auto; vertical-align: middle;">
                    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                    <div class="subHeader" style="position: relative; right: 0px">
                        <eluc:Title runat="server" ID="Title1" Text="Placement Details" ShowMenu="true" />
                    </div>
                </div>
                <table style="width: 100%">
                    <tr>
                        <td>
                            Batch
                        </td>
                        <td>
                            <eluc:PreSeaBatch ID="ucBatch" runat="server" CssClass="dropdown_mandatory" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                </table>
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuPreSeaScoreCradSummary" runat="server" OnTabStripCommand="MenuPreSea_TabStripCommand"></eluc:TabStrip>
                </div>
               <div id="divGrid" style="position: relative; overflow: auto; z-index: 0">
                        <asp:GridView ID="gvCrew" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" ShowHeader="true"
                            EnableViewState="false" AllowSorting="true" >
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <Columns>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Sr.No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSrNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDROW") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Candidate Name
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        Date of Birth
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblDateofBirth" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDDATEOFBIRTH") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        INDoS No
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblINDoS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINDOSNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        CDC No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblCdcNo" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSEAMANBOOKNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        SPFO PF No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSPFO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSPFO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                       Watchkeeping Certificate No./COC
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblWatchkeeping" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDWATCHKEEPING") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       Name of Ship
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblShip" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFSHIP") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       Flag of Ship
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFlag" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDFLAGOFSHIP") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       Ship Sign-on Date
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblSign" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIGNONDATE") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                       IMO No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblIMO" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDVESSELIMO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                       Official No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblOfficial" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDOFFICIALNO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       Name of Shipping Company 
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblShipping" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDNAMEOFCOMPANY") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       RPS Licence No.
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRPS" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRPSLICENCENO") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                       RPS Name 
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRPSName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRPSNAME") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField>
                                    <HeaderTemplate>
                                       Remarks
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblRemarks" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDREMARKS") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
