<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PreSeaMoreInfoSemesterExamInvigilator.aspx.cs"
    Inherits="PreSeaMoreInfoSemesterExamInvigilator" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Invigilator List</title>
    <telerik:RadCodeBlock ID="radcodeblock1" runat="server">
</telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
    <div>
          <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
            </ajaxToolkit:ToolkitScriptManager>
        <asp:GridView ID="gvInvigilator" runat="server" AutoGenerateColumns="False" Font-Size="11px"
            Width="100%" CellPadding="3" Style="margin-bottom: 0px" ShowHeader="true">
            <HeaderStyle CssClass="DataGrid-HeaderStyle" />
            <AlternatingRowStyle CssClass="datagrid_alternatingstyle" />
            <SelectedRowStyle CssClass="datagrid_selectedstyle" />
            <RowStyle Height="10px" />
            <Columns>
                <asp:TemplateField>
                    <ItemStyle Wrap="False" HorizontalAlign="Left"></ItemStyle>
                    <HeaderTemplate>
                        <asp:Label ID="lblInvigilatorHeader" runat="server" Text="Invigilator Name">
                        </asp:Label>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <asp:Label ID="lblInvigilatorName" runat="server" Text='<%# DataBinder.Eval(Container,"DataItem.FLDINVNAME") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
