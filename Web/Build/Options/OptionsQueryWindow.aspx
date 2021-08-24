<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsQueryWindow.aspx.cs" Inherits="Options_OptionsQueryWindow" ValidateRequest="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="cmdExecute" runat="server" OnClick="cmdExecute_Click" Text="Execute" /><br />
        <asp:Literal ID="lblQuery" runat="server" Text="Query:"></asp:Literal> <p />
        <asp:TextBox runat="server" ID="txtQuery" TextMode="MultiLine" Rows="20" Columns="120"></asp:TextBox>
        <br />
        <asp:Literal ID="lblMessage" runat="server" Text="Message:"></asp:Literal><p />
        <asp:TextBox runat="server" ID="txtResult" TextMode="MultiLine" Rows="5" Columns="120"></asp:TextBox>
        <br />
        <asp:Literal ID="lblResults" runat="server" Text="Results:"></asp:Literal><p />
        <asp:GridView ID="gvResults" runat="server" AutoGenerateColumns="True" Font-Size="11px"
    Width="100%" CellPadding="3" ShowFooter="true"
    ShowHeader="true" />
                        
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility:hidden"/>
    </div>
    </form>
</body>
</html>
