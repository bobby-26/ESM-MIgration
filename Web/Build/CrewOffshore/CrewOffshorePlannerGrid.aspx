<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshorePlannerGrid.aspx.cs"
    Inherits="CrewOffshorePlannerGrid" %>


<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRankList.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Planner Grid</title> <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
      <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
    </telerik:RadCodeBlock></head>
<body>
    <form id="form1" runat="server">
 
   <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
  <telerik:RadAjaxPanel ID="panel1" runat="server">

            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
      
             
                <table width="100%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblVesselType" runat="server" Text="Vessel Type"></telerik:RadLabel>
                        </td>
                        <td>                            
                            <telerik:RadListBox ID="lstVesselType" runat="server"  DataTextField="FLDTYPEDESCRIPTION" DataValueField="FLDVESSELTYPEID" 
                                SelectionMode="Multiple" Width="200px"></telerik:RadListBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Rank ID="ucRank" runat="server" Width="200px"  />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblDate" runat="server" Text="Planner Grid Upto"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:Date ID="txtDate" runat="server" ></eluc:Date>
                        </td>
                    </tr>
                </table>
                <table id="Table2" width="100%" style="color: Blue">
                    <tr>
                        <td>
                            <b>
                                <telerik:RadLabel ID="lblNotes" runat="server" Text="Notes :"></telerik:RadLabel></b>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp; 1. Off - shows the count of seafarers due for relief within the given date.
                        </td>
                    </tr>
                    <tr>
                        <td>
                           &nbsp; 2. Prop - shows the count of seafarers proposed and their expected joining date is within the given date.
                        </td>
                    </tr>
                    <tr>
                        <td>
                           &nbsp; 2. On - shows the count of seafarers approved and their expected joining date is within the given date.
                        </td>
                    </tr>
                </table>
                <div id="div2" runat="server">
                  
                        <eluc:TabStrip ID="MenuExcel" runat="server" OnTabStripCommand="CrewShowExcel_TabStripCommand">
                        </eluc:TabStrip>
                
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:Label ID="ltGrid" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
        
      </telerik:RadAjaxPanel>
    </form>
</body>
</html>
