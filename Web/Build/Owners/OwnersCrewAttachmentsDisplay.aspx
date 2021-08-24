<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersCrewAttachmentsDisplay.aspx.cs" Inherits="OwnersCrewAttachmentsDisplay" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="asp" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Splitter" Src="~/UserControls/UserControlSplitter.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Attachments</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="Div3" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>

    </div>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmAttachementsDisplay" runat="server">
    <asp:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server">
    </asp:ToolkitScriptManager>
    <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
                width: 100%">
    <div class="subHeader">
        <eluc:Title runat="server" ID="Attachment" Text="Attachments" ShowMenu="false"></eluc:Title>
    </div>
    <div class="navSelect" style="top: 0px; right: 0px; position: absolute">
        <eluc:TabStrip ID="AttachmentList" runat="server" OnTabStripCommand="AttachmentList_TabStripCommand">
        </eluc:TabStrip>
    </div>
    <asp:GridView ID="gvAttachment" runat="server" AutoGenerateColumns="False" Font-Size="11px"
        Width="100%" CellPadding="3" ShowHeader="true" EnableViewState="false" 
        OnRowDataBound="gvAttachment_RowDataBound"
        OnRowEditing="gvAttachment_RowEditing" 
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
                        ForeColor="White">File Name&nbsp;</asp:LinkButton>
                    <img id="FLDFILENAME" runat="server" visible="false" />
                </HeaderTemplate>
                <ItemTemplate>
                    <asp:Label ID="lblFileName" runat="server" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILENAME").ToString() %>'> </asp:Label>                    
                    <asp:Label ID="lblFilePath" runat="server" Visible="false" Text='<%#DataBinder.Eval(Container,"DataItem.FLDFILEPATH").ToString() %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField>
                <ItemTemplate><asp:HyperLink ID="lnkfilename" Target="_blank" Text="View"
                        runat="server" Width="14px" Height="14px" ToolTip="Download File">
                    </asp:HyperLink></ItemTemplate>
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
