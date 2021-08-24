<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DrydockCostSummary.aspx.cs" Inherits="DryDock_DrydockCostSummary" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Drydock Cost Summary</title>
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
        <br />
    <table>
        <tbody>
            <tr>
                <td>
                    <telerik:RadLabel runat="server" Text="Generate Drydock cost summary report with quoted price" />
                </td>

            </tr>
            <tr>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
                <td>
                 <telerik:RadRadioButtonList runat="server" ID="RadRadioButtontype" Direction="Horizontal" AutoPostBack="true">

                                <Items>
                                    <telerik:ButtonListItem runat="server" Text=" Including discount" Value="1"  Selected="true"/>

                                    <telerik:ButtonListItem runat="server" Text="Excluding discount" Value="0" />
                                </Items>
                            </telerik:RadRadioButtonList>
                    </td>
            </tr>
             <tr>
                <td>
                    &nbsp
                </td>
            </tr>
            <tr>
                <td>
                    <telerik:RadButton runat="server" ID="radreportbutton" Text="Cost Summary Report" />
                </td>
            </tr>
        </tbody>
    </table>
    </div>
    </form>
</body>
</html>
