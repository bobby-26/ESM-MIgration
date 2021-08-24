<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AccountsSOAGenerationPdfAttachment.aspx.cs"
    Inherits="AccountsSOAGenerationPdfAttachment" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attachment</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div3" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
    </div>
    <script type="text/javascript">
        var Template = new Array();
        Template[0] = '<a href="#" id="lnkRemoveFile{counter}" onclick="return removeFile(this);">Remove</a>';
        Template[1] = '<input id="txtFileUpload{counter}" name="txtFileUpload{counter}" type="file" class="input" />';
        Template[2] = 'Choose a file';
        var counter = 1;

        function addFile(description) {
            counter++;
            var tbl = document.getElementById("tblFiles");
            var rowCount = tbl.rows.length;
            var row = tbl.insertRow(rowCount - 1);
            var cell;

            for (var i = 0; i < Template.length; i++) {
                cell = row.insertCell(0);
                cell.innerHTML = Template[i].replace(/\{counter\}/g, counter).replace(/\{value\}/g, (description == null) ? '' : description);
            }
        }
        function removeFile(ctrl) {
            var tbl = document.getElementById("tblFiles");
            if (tbl.rows.length > 2)
                tbl.deleteRow(ctrl.parentNode.parentNode.rowIndex);
        }
        
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </asp:ToolkitScriptManager>
    <asp:UpdatePanel runat="server" ID="pnlDebitReference">
        <ContentTemplate>
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
                <div class="subHeader">
                    <eluc:Title runat="server" ID="Attachment" Text="Report History" ShowMenu="false">
                    </eluc:Title>
                </div>
                
                <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
                    <eluc:TabStrip ID="MenuFormMain" runat="server" OnTabStripCommand="MenuFormMain_TabStripCommand" TabStrip="true"></eluc:TabStrip>
                </div>
                <asp:GridView ID="gvReportHistory" runat="server" AutoGenerateColumns="False" Font-Size="11px"
                    Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" AllowSorting="true"
                    OnSorting="gvAttachment_Sorting" GridLines="None">
                    <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
                    <HeaderStyle CssClass="DataGrid-HeaderStyle" />
                    <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
                    <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
                    <RowStyle Height="10px" />
                    <Columns>
                        <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
                        <asp:TemplateField HeaderText="Type" Visible="false">
                            <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                                <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date / Time">
                            <ItemStyle HorizontalAlign="Left" Width="15%"></ItemStyle>
                            <HeaderTemplate>
                                <asp:Label ID="lblFileHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILENAME">Date / Time&nbsp;</asp:Label>
                                <img id="FLDFILENAME" runat="server" visible="false" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:Label ID="lblHistoryDate" runat="server" Text='<%#  DataBinder.Eval(Container,"DataItem.FLDGENERATEDDATE") %>'></asp:Label>
                                <%--                        <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></asp:Label>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Type">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblType" runat="server" Text='<%#Bind("FLDTYPE") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="User Name">
                            <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                            <ItemTemplate>
                                <asp:Label ID="lblUserName" runat="server" Text='<%#Bind("FLDUSERNAME") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
                <eluc:Splitter ID="ucSplitter" runat="server" TargetControlID="ifMoreInfo" />
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    </form>
</body>
</html>
