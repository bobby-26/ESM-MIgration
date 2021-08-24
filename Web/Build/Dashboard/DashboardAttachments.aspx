<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardAttachments.aspx.cs" Inherits="Dashboard_DashboardAttachments" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Attachment</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div3" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"] %>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
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
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <div class="subHeader">
            <eluc:Title runat="server" ID="Attachment" Text=""></eluc:Title>
        </div>
        <div class="navSelect" style="top: 0px; right: 2px; position: absolute;">
                    <eluc:TabStrip ID="MenuDdashboradVesselParticulrs" runat="server" OnTabStripCommand="MenuDdashboradVesselParticulrs_TabStripCommand"
                        TabStrip="true"></eluc:TabStrip>
        </div>
        <br />
        <div class="navSelect" style="position: relative; width: 15px">
            <eluc:TabStrip ID="SubMenuAttachment" runat="server">
            </eluc:TabStrip>
        </div>
        <asp:GridView GridLines="None" ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" OnRowUpdating="gvAttachment_RowUpdating"
            OnRowCancelingEdit="gvAttachment_RowCancelingEdit" OnRowDataBound="gvAttachment_RowDataBound"
            OnRowEditing="gvAttachment_RowEditing" OnRowDeleting="gvAttachment_RowDeleting"
            AllowSorting="true" OnSorting="gvAttachment_Sorting">
            <FooterStyle CssClass="datagrid_footerstyle"></FooterStyle>
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" Font-Bold="true" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:ButtonField Text="DoubleClick" CommandName="Edit" Visible="false" />
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
                            >File Name&nbsp;</asp:LinkButton>
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
                <asp:TemplateField HeaderText="Size(in KB)">
                    <ItemStyle HorizontalAlign="Right" Width="10%"></ItemStyle>
                    <HeaderTemplate>
                        <asp:LinkButton ID="lblSizeHeader" runat="server" CommandName="Sort" CommandArgument="FLDFILESIZE"
                            >Size(in KB)&nbsp;</asp:LinkButton>
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
                    <EditItemTemplate>
                        <asp:Label ID="lblDTKeyEdit" runat="server" Visible="false" Text=' <%#DataBinder.Eval(Container,"DataItem.FLDDTKEY").ToString() %>'></asp:Label>
                        <asp:CheckBox ID="chkSynch" runat="server" Checked='<%# (DataBinder.Eval(Container,"DataItem.FLDSYNCYN").ToString().Equals("1"))? true:false %>' />
                    </EditItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created By">
                    <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lblCreatedBy" runat="server" Text='<%#Bind("FLDCREATEDBYNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Created Date">
                    <ItemStyle HorizontalAlign="Center" Width="10%"></ItemStyle>
                    <ItemTemplate>
                        <asp:Label ID="lbldate" runat="server" Text='<%#Bind("FLDCREATEDDATE","{0:dd/MMM/yyyy}") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField>
                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle"></HeaderStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblActionHeader" runat="server">Action</asp:Label>
                    </HeaderTemplate>
                    <ItemStyle Wrap="False" HorizontalAlign="Center" Width="100px"></ItemStyle>
                    <ItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Edit" ImageUrl="<%$ PhoenixTheme:images/te_edit.png %>"
                            CommandName="EDIT" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdEdit"
                            ToolTip="Edit"></asp:ImageButton>
                        <img id="Img1" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Delete" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="DELETE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdDelete"
                            ToolTip="Delete"></asp:ImageButton>
                    </ItemTemplate>
                    <EditItemTemplate>
                        <asp:ImageButton runat="server" AlternateText="Save" ImageUrl="<%$ PhoenixTheme:images/save.png %>"
                            CommandName="UPDATE" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdSave"
                            ToolTip="Save"></asp:ImageButton>
                        <img id="Img2" runat="server" src="<%$ PhoenixTheme:images/spacer.png %>" width="3" />
                        <asp:ImageButton runat="server" AlternateText="Cancel" ImageUrl="<%$ PhoenixTheme:images/te_del.png %>"
                            CommandName="Cancel" CommandArgument='<%# Container.DataItemIndex %>' ID="cmdCancel"
                            ToolTip="Cancel"></asp:ImageButton>
                    </EditItemTemplate>
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
    </form>
</body>
</html>
