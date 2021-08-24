<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DefectTrackerMailIncident.aspx.cs" Inherits="DefectTracker_DefectTrackerMailIncident" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Untitled Page</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
</telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <table width="100%">
            <tr>
                <td>
                    Incident
                </td>
                <td>
                    <asp:CheckBoxList ID="cblIncident" runat="server"></asp:CheckBoxList>
                </td>
                <td>
                    Module
                </td>
                <td>
                </td>
            </tr>
        </table>
    
    
    </div>
    </form>
</body>
</html>
