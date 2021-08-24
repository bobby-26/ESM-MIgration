<%@ Page Language="C#" AutoEventWireup="true" CodeFile="VesselPositionEUMRVProcedureDetailViewC3.aspx.cs" Inherits="VesselPositionEUMRVProcedureDetailViewC3" %>

<%@ Import Namespace="SouthNests.Phoenix.VesselPosition" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Location</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="VesselDirectionlink" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>
    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmVPRSLocation" runat="server">
    <ajaxToolkit:ToolkitScriptManager CombineScripts="false" ID="ToolkitScriptManager1"  runat="server">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlVPRSLocation">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
           <div class="subHeader" style="position: relative">
                    <div class="subHeader" style="position: relative">
                        <eluc:Title runat="server" ID="Title1" Text="Company Procedure View" ShowMenu="true">
                        </eluc:Title>
                        <eluc:Error ID="Error1" runat="server" Text="" Visible="false"></eluc:Error>
                        <eluc:Status runat="server" ID="ucStatus" />
                    </div>
                    <div class="navSelect" style="top: 0px; right: 0px; position: absolute;">
                        <eluc:TabStrip ID="MenuProcedureDetailList" runat="server" OnTabStripCommand="MenuProcedureDetailList_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </div>
                </div>
            <div id="maindiv" style="top: 0px; position: relative; z-index: +2">
                <table width="100%">
                    <tr>
                        <td style=" width:30%">
                            <asp:Literal runat="server" ID="lblprocedure" Text="Procedure"></asp:Literal>
                        </td>
                        <td style=" width:70%">
                            <asp:DropDownList runat="server" Visible="false" ID="ddlProcedure" CssClass="dropdown_mandatory" Width="70%"></asp:DropDownList>
                            <b><asp:Label ID="txtProcedure" runat="server" ></asp:Label></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblreference" Text="Reference To Existing Procedure"></asp:Literal>
                        </td>
                        <td>
                          <b> <a id="link" href="" class="applinks"> <hyperlink id="txtDocumentNameEdit" runat="server"></hyperlink></a></b>
                           
                            <asp:TextBox ID="txtReferencetoExisting" runat="server" CssClass="input" Visible="false" Height="70px" TextMode="MultiLine"
                                    Width="70%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblVersion" Text="Version of Existing Procedure"></asp:Literal>
                        </td>
                        <td>
                           <b> <asp:Literal runat="server" ID="txtVersion" Text="0"></asp:Literal></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lbleuprocedure" Text="Description of EUMRV Procedure"></asp:Literal>
                        </td>
                        <td>
                              <asp:TextBox ID="txteuprocedure" runat="server" CssClass="input" Height="70px" TextMode="MultiLine"
                                    Width="70%" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblpersonreponsible" Text="Name of the Person Responsible"></asp:Literal>
                        </td>
                        <td>
                             <asp:TextBox runat="server" ID="txtxpersonreponsible" CssClass="input"  Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblDatatasource" Text="Data source"></asp:Literal>
                        </td>
                        <td>
                             <asp:TextBox runat="server" ID="txtDatasource" CssClass="input" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lbllocation" Text="Location where records are kept"></asp:Literal>
                        </td>
                        <td>
                             <asp:TextBox runat="server" ID="txtlocation" CssClass="input" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Literal runat="server" ID="lblSystemUsed" Text="Name of the IT System Used"></asp:Literal>
                        </td>
                        <td>
                             <asp:TextBox runat="server" ID="txtSystemUsed" CssClass="input" Width="360px"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
