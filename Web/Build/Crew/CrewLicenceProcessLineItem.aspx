<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewLicenceProcessLineItem.aspx.cs" Inherits="CrewLicenceProcessLineItem" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="../UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head" runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixGridResize.js"></script>

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="frmCrewAboutUsBy" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlAboutUsBy" Text="Licence Request" ShowMenu="false">
            </eluc:Title>
            <br />
        </div>
        <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand">
            </eluc:TabStrip>
        </div>      
        <asp:GridView ID="gvMissingLicence" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:TemplateField HeaderText="Select">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblReqId" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDREQUESTID") %>'></asp:Label>
                        <asp:CheckBox ID="chk_select" runat="server" />
                    </ItemTemplate>                   
                </asp:TemplateField>  
                 <asp:TemplateField HeaderText="Vessel">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container, "DataItem.FLDVESSELNAME")%>                        
                    </ItemTemplate>                   
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Name">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container, "DataItem.FLDNAME")%>                        
                    </ItemTemplate>                   
                </asp:TemplateField>  
                <asp:TemplateField HeaderText="Rank">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container, "DataItem.FLDRANKNAME")%>                        
                    </ItemTemplate>                   
                </asp:TemplateField>      
                <asp:TemplateField HeaderText="Licence Name">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <%#DataBinder.Eval(Container, "DataItem.FLDLICENCE")%>                        
                    </ItemTemplate>                   
                </asp:TemplateField>               
                <asp:TemplateField HeaderText="Status">
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <ItemTemplate>
                        <%# DataBinder.Eval(Container, "DataItem.FLDSTATUS").ToString() == "1" ? "Missing" : "Expired"%>
                    </ItemTemplate>                  
                </asp:TemplateField>              
            </Columns>
        </asp:GridView>
        <eluc:Status ID="ucStatus" runat="server" />
    </div>
    </form>
</body>
</html>
