<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataTransferXML.aspx.cs" Inherits="DataTransfer_DataTransferXML" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <div>
            <asp:GridView runat="server" ID="gvXml" OnRowCommand="gvXml_RowCommand">
                <RowStyle Wrap="false" />
                <Columns>
                    <asp:TemplateField>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <HeaderTemplate>
                            <asp:Label ID="lblActionHeader" runat="server"> Action </asp:Label>
                        </HeaderTemplate>
                        <ItemStyle HorizontalAlign="Center" Width="100px" Wrap="False" />
                        <ItemTemplate>
                            <asp:ImageButton ID="cmdArchive" runat="server" AlternateText="Import"
                                CommandArgument="<%# Container.DataItemIndex %>" CommandName="IMPORT"
                                ImageUrl="<%$ PhoenixTheme:images/archive.png %>" ToolTip="Import" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
