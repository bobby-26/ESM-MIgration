<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewWorkingGearIssuance.aspx.cs"
    Inherits="CrewWorkingGearIssuance" %>
    <%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Zone" Src="~/UserControls/UserControlZone.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Size" Src="~/UserControls/UserControlWorkingGearSize.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Working Gear Issuance</title>
       <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">   
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/fonts/fontawesome/css/all.min.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewDateOfAvailability" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDOA">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="Issue from Stock" Visible="false"></eluc:Error>
                <div class="subHeader" style="position: relative">
                    <div id="divHeading" style="vertical-align: top">
                        <eluc:Title runat="server" ID="ucTitle" Text="" ShowMenu="false" />
                    </div>
                </div>
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                    <eluc:TabStrip ID="MenuWorkingGearItem" runat="server" OnTabStripCommand="MenuWorkingGearItem_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div style="color: Blue;">
                    1. First save the issue date and issue by name.
                    <br />
                    2. Then Issue items tab will be shown, there after you can add issuing items
                </div>
                <table cellpadding="1" cellspacing="1" width="100%">
                </table>
                <br style="clear: both;" />
                <table cellpadding="1" cellspacing="1" width="100%">
                    <tr>
                        <td>
                            <asp:Literal ID="lblEmployeeCode" runat="server" Text="Employee Code"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblEmployeeName" runat="server" Text="Employee Name"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmployeeName" runat="server" MaxLength="50" CssClass="readonlytextbox"
                                ReadOnly="True" Width="150px"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblPresentRank" runat="server" Text="Present Rank"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPayRank" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblLastVessel" runat="server" Text="Last Vessel"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastVessel" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblSignedOff" runat="server" Text="Signed Off"></asp:Literal>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtSignedOff" runat="server" MaxLength="20" CssClass="readonlytextbox"
                                ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal ID="lblIssueDate" runat="server" Text="Issue Date"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Date ID="txtIssueDate" runat="server" CssClass="input_mandatory" />
                        </td>
                        <td>
                            <asp:Literal ID="lblIssuedBy" runat="server" Text="Issued By"></asp:Literal>
                        </td>
                        <td>
                            <asp:TextBox ID="txtIssueBy" runat="server" CssClass="input_mandatory"></asp:TextBox>
                        </td>
                        <td>
                            <asp:Literal ID="lblIssueFrom" runat="server" Text="Issue From (Zone)"></asp:Literal>
                        </td>
                        <td>
                            <eluc:Zone ID="ucZone" AppendDataBoundItems="true" CssClass="input_mandatory" runat="server" />
                        </td>
                    </tr>
                </table>
                <h4>
                    <asp:Literal ID="lblIssuedItems" runat="server" Text="Issued Items"></asp:Literal></h4>
                <div class="navSelect" id="divTab1" runat="server" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuShowExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                    </eluc:TabStrip>
                </div>
                <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                    <asp:GridView ID="gvWorkingGear" runat="server" AutoGenerateColumns="False" Width="100%"
                        CellPadding="3" ShowFooter="false" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvWorkingGear_RowDataBound"
                        DataKeyNames="FLDITEMISSUEID" OnRowEditing="gvWorkingGear_RowEditing" OnRowUpdating="gvWorkingGear_RowUpdating"
                        OnRowDeleting="gvWorkingGear_RowDeleting">
                        <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                        <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                        <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                        <RowStyle Height="10px" />
                        <Columns>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblItemHeader" runat="server">Item</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblIssueItemid" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMISSUEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lblItem" runat="server" CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:LinkButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="lblIssueItemidEdit" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDITEMISSUEID") %>'></asp:Label>
                                    <asp:LinkButton ID="lblItemEdit" runat="server" CommandArgument='<%# Container.DataItemIndex%>'
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMNAME") %>'></asp:LinkButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuedSizeHeader" runat="server" >Size</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblSize" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDSIZENAME") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                 <eluc:Size ID="ucSizeEdit" runat="server" AppendDataBoundItems="true" SelectedSize='<%# DataBinder.Eval(Container,"DataItem.FLDSIZEID") %>' 
                                    SizeList=' <%#PhoenixWorkingGearSize.ListSize(General.GetNullableGuid(DataBinder.Eval(Container,"DataItem.FLDWORKINGGEARITEMID").ToString())) %>'
                                        CssClass="gridinput_mandatory" />
                               </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblIssuedQuatityHeader" runat="server" >Issued Quantity</asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblQuantity" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <eluc:Number ID="txtQuantityEdit" runat="server" CssClass="input_mandatory" Width="90px"
                                        Text='<%# DataBinder.Eval(Container,"DataItem.FLDRECEIVEDQUANTITY") %>' />
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                <HeaderTemplate>
                                   <asp:Label ID="lblActionHeader" runat="server"> Action</asp:Label>
                                </HeaderTemplate>
                                <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                <ItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                                        CommandName="EDIT" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdEdit"
                                        ToolTip="Edit"></asp:ImageButton>
                                    <img id="Img2" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="DELETE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdDelete"
                                        ToolTip="Delete"></asp:ImageButton>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                                        CommandName="UPDATE" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdSave"
                                        ToolTip="Save"></asp:ImageButton>
                                    <img id="Img1" runat="server" alt="" src="<%$ PhoenixTheme:images/spacer.png %>"
                                        width="3" />
                                    <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                                        CommandName="CANCEL" CommandArgument="<%# Container.DataItemIndex %>" ID="cmdCancel"
                                        ToolTip="Cancel"></asp:ImageButton>
                                </EditItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
                <eluc:Status runat="server" ID="ucStatus" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
