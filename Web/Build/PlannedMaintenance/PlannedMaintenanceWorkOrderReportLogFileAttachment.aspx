<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PlannedMaintenanceWorkOrderReportLogFileAttachment.aspx.cs"
    Inherits="PlannedMaintenanceWorkOrderReportLogFileAttachment" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachment</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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

    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager RenderMode="Lightweight" ID="RadWindowManager1" runat="server" EnableShadow="true">
        </telerik:RadWindowManager>
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand"></eluc:TabStrip>
        <table id="tblFiles">
            <tr>
                <td>
                    <asp:Literal ID="lblChooseafile" runat="server" Text="Choose a file"></asp:Literal>
                </td>
                <td colspan="2">
                    <asp:FileUpload ID="txtFileUpload1" runat="server" CssClass="input" />
                </td>
            </tr>
            <tr>
                <td colspan="3" align="right">
                    <a href="#" onclick="return addFile();">Add File</a>
                </td>
            </tr>
        </table>
        <hr />
        <br />
        <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowDataBound="gvAttachment_RowDataBound"
            AllowSorting="true" OnSorting="gvAttachment_Sorting" OnRowDeleting="gvAttachment_RowDeleting">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:TemplateField HeaderText="Type">
                    <ItemStyle HorizontalAlign="Left" Width="5%"></ItemStyle>
                    <ItemTemplate>
                        <asp:Image ID="imgfiletype" runat="server" Width="14px" Height="14px" />
                        <asp:Label ID="lblDTKey" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
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
                        </asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Size(in KB)">
                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lblSizeHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILESIZE"
                            ForeColor="White">Size(in KB)&nbsp;</asp:LinkButton>
                        <img id="FLDFILESIZE" runat="server" visible="false" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <%# string.IsNullOrEmpty(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString()) ? string.Empty : Math.Round(((double.Parse(DataBinder.Eval(Container, "DataItem.FLDFILESIZE").ToString())/1024*100000)/100000.00),2).ToString()%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Synch(Yes/No)">
                    <ItemStyle HorizontalAlign="Left" Width="10%"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblsynchyesno" runat="server" Text='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))?"Yes":"No" %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                            ToolTip="Delete"></asp:ImageButton>
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
                <td width="20px">&nbsp;
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
    </form>
</body>
</html>
