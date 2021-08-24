<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsPVInvoiceAttachments.aspx.cs"
    Inherits="AccountsPVInvoiceAttachments" %>

<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Currency" Src="~/UserControls/UserControlCurrency.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Vessel" Src="~/UserControls/UserControlVessel.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Direct PO</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

    </div>
    <style type="text/css">
        .hidden
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
        function resizeDiv() {
            var obj = document.getElementById("divScroll");
            var iframe = document.getElementById("ifMoreInfo");
            var rect = iframe.getBoundingClientRect();
            var x = rect.left;
            var y = rect.top;
            var w = rect.right - rect.left;
            var h = rect.bottom - rect.top;
            obj.style.height = (document.all ? (document.documentElement && document.documentElement.clientHeight ? obj.document.documentElement.clientHeight : obj.document.body.clientHeight) : window.innerHeight) - (h + 70) + "px";
        }
    </script>

</telerik:RadCodeBlock></head>
<body>
    <form id="frmInvoiceDirctPO" runat="server">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlInvoice">
        <ContentTemplate>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
                <eluc:Status runat="server" ID="ucStatus" />
                <div class="subHeader" style="position: relative">
                    <eluc:Title runat="server" ID="Title1" Text="Invoice Attachment List"></eluc:Title>
                    <asp:Button ID="cmdHiddenSubmit" runat="server" Text="cmdHiddenSubmit" OnClick="cmdHiddenSubmit_Click"
                        CssClass="hidden" />
                </div>
              <%--  <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <span class="navSelect" style="margin-top: 0px; float: right; width: auto;">
                        <eluc:TabStrip ID="MenuDPO" runat="server" OnTabStripCommand="MenuDPO_TabStripCommand"
                            TabStrip="true"></eluc:TabStrip>
                    </span>
                </div>
                <div style="position: relative; overflow: hidden; clear: right;">
                    <iframe runat="server" id="ifMoreInfo" scrolling="no" style="min-height: 340px; width: 100%;">
                    </iframe>
                </div>--%>
                <asp:HiddenField ID="hdnScroll" runat="server" />
               <%-- <div class="navSelect" style="position: relative; width: 15px">
                    <eluc:TabStrip ID="MenuAddAmosPO" runat="server" OnTabStripCommand="MenuAddAmosPO_TabStripCommand">
                    </eluc:TabStrip>
                </div>--%>
                <div id="divScroll" style="position: relative; z-index: 1; width: 100%; height: 290px;
                    overflow: auto;" onscroll="javascript:setScroll('divScroll', 'hdnScroll');">
                    <div id="divGrid" style="position: relative; z-index: 0; width: 100%;">
                        <asp:GridView ID="gvDPO" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                            Width="100%" CellPadding="3" OnRowDataBound="gvDPO_RowDataBound" ShowHeader="true" 
                            EnableViewState="false" AllowSorting="true" OnSorting="gvDPO_Sorting" OnSelectedIndexChanging="gvDPO_SelectedIndexChanging"
                             OnRowCommand="gvDPO_RowCommand">
                            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                            <RowStyle Height="10px" />
                            <Columns>                               
                                <asp:TemplateField>
                                    <ItemStyle Wrap="False" HorizontalAlign="Left" Width="150px"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lnkVendorHeader" runat="server" CommandName="Sort" CommandArgument="FLDINVOICENUMBER"
                                            ForeColor="White">Invoice</asp:LinkButton>
                                        <img id="FLDINVOICENUMBER" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <%# ((DataRowView)Container.DataItem)["FLDINVOICENUMBER"]%>                                        
                                        <asp:Label ID="lblInvoiceRef" runat="server" Visible="false" Text='<%# DataBinder.Eval(Container,"DataItem.FLDATTACHMENTCODE") %>'></asp:Label>                                        
                                    </ItemTemplate>
                                </asp:TemplateField> 
                                 <asp:TemplateField HeaderText="Type">
                                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                                    <ItemTemplate>
                                        <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                                        <asp:Label ID="lblAttachmenttype" Visible="false" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDATTACHMENTTYPE").ToString() %>'> </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="File Name">
                                    <ItemStyle HorizontalAlign="Left" Width="30%"></ItemStyle>
                                    <HeaderTemplate>
                                        <asp:LinkButton ID="lblFileHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILENAME"
                                            ForeColor="White">File Name&nbsp;</asp:LinkButton>
                                        <img id="FLDFILENAME" runat="server" visible="false" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>
                                        <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                 <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px"
                                            Height="14px" ToolTip="Download File">
                                        </asp:HyperLink></ItemTemplate>
                                </asp:TemplateField>
                               <%-- <asp:TemplateField>
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                                    <HeaderTemplate>
                                        <asp:Label ID="lblActionHeader" runat="server">
                                            Action
                                        </asp:Label>
                                    </HeaderTemplate>
                                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                                    <ItemTemplate>                                       
                                    <asp:ImageButton runat="server" AlternateText="Attachment" ImageUrl="<%$ PhoenixTheme:images/attachment.png %>"
                                            CommandName="ATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdAttachment"
                                            ToolTip="Attachment"></asp:ImageButton>
                                   <%-- <asp:ImageButton runat="server" AlternateText="No Attachment" ImageUrl="<%$ PhoenixTheme:images/no-attachment.png %>"
                                            CommandName="NOATTACHMENT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdNoAttachment"
                                            ToolTip="No Attachment"></asp:ImageButton> 
                                    </ItemTemplate>
                                </asp:TemplateField>--%>
                            </Columns>
                        </asp:GridView>
                        <table width="100%" border="0" cellpadding="1" cellspacing="1" class="datagrid_pagestyle" >
                            <tr>
                                <td nowrap align="center">
                                    <asp:Label ID="lblPagenumber" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblPages" runat="server">
                                    </asp:Label>
                                    <asp:Label ID="lblRecords" runat="server">
                                    </asp:Label>&nbsp;&nbsp;
                                </td>
                                <td nowrap align="left" width="50px">
                                    <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                                </td>
                                <td width="20px">
                                    &nbsp;
                                </td>
                                <td nowrap align="right" width="50px">
                                    <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                                </td>
                                <td nowrap align="center">
                                    <asp:TextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server" CssClass="input">
                                    </asp:TextBox>
                                    <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="cmdGo_Click" CssClass="input"
                                        Width="40px"></asp:Button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
