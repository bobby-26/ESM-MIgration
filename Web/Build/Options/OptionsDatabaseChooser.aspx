<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OptionsDatabaseChooser.aspx.cs" Inherits="OptionsDatabaseChooser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
</head>
<body>
    <form id="form1" runat="server">
    <div style="height:100%">
        <table align="center" style="vertical-align:middle">
            <tr>
                <td>
                    <asp:DropDownList runat="server" ID="ddlDatabase" DataTextField="DBNAME" DataValueField="DBVALUE">                       
                    </asp:DropDownList>
                    <asp:Button runat="server" ID="btnChooseDatabase" Text="Use" OnClick="btnChooseDatabase_Click" />                   
                </td>
            </tr>
        </table>
        <input type="button" runat="server" id="isouterpage" name="isouterpage" style="visibility:hidden"/>
    </div>
    </form>
</body>
</html>
