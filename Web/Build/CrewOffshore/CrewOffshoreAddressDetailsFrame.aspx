<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAddressDetailsFrame.aspx.cs" Inherits="CrewOffshore_CrewOffshoreAddressDetailsFrame" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <link href="../Content/bootstrap.min.v3.css" rel="stylesheet" />
        <link href="../Content/fontawesome-all.min.css" rel="stylesheet" />
        <link href="../Content/dashboard.css" rel="stylesheet" />
    </telerik:RadCodeBlock>
</head>
<body style="background-color:white">
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <%--<eluc:TabStrip ID="CrewMainPersonal" runat="server" OnTabStripCommand="CrewMainPersonal_TabStripCommand" Title="Personal"></eluc:TabStrip>--%>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="100%" Font-Size="Smaller">
            <div class="gray-bg">
                <div class="row">
                    <div class="col-lg-12">
                        <table class="table table-bordered">
                            <tr class="gradeX">
                                <td>
                                    <table class="table table-bordered">
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue" colspan="2">   Permanent Address                                              
                                            </td>
                                           
                                            <td style="background-color: aliceblue" colspan="2">Local Address
                                            </td>
                                         
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">Address1
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAddressLine1" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Address1
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtLocalAddressLine1" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                     <%--   <tr class="gradeX">
                                            <td style="background-color: aliceblue">Address2
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAddressLine2" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Address2
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtLocalAddressLine2" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                           <tr class="gradeX">
                                            <td style="background-color: aliceblue">Address3
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAddressLine3" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Address3
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtLocalAddressLine3" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                           <tr class="gradeX">
                                            <td style="background-color: aliceblue">Address4
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtAddressLine4" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Address4
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtLocalAddressLine4" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>--%>
                                           <tr class="gradeX">
                                            <td style="background-color: aliceblue">Country
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtcountry" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Country
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalcountry" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">State
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtstate" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">State
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalstate" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                        <tr class="gradeX">
                                            <td style="background-color: aliceblue">City
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtcity" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">City
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalcity" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                         <tr class="gradeX">
                                            <td style="background-color: aliceblue">Postal 
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtpostalcode" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Postal 
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalpostalcode" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                         <tr class="gradeX">
                                            <td style="background-color: aliceblue">Phone 
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtphno" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Phone 
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalphno" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                          <tr class="gradeX">
                                            <td style="background-color: aliceblue">Mobile 
                                            </td>
                                            <td>
                                                <telerik:RadLabel ID="txtmobile" runat="server"></telerik:RadLabel>
                                            </td>
                                            <td style="background-color: aliceblue">Mobile 
                                            </td>
                                            <td>
                                                 <telerik:RadLabel ID="txtlocalmobile" runat="server"></telerik:RadLabel>
                                            </td>
                                        </tr>
                                           <tr class="gradeX">
                                            <td style="background-color: aliceblue">Email
                                            </td>
                                            <td colspan="3">
                                                <telerik:RadLabel ID="txtemail" runat="server"></telerik:RadLabel>
                                            </td>                                           
                                        </tr>
                                       <%-- <tr><td colspan="4">
                                            Note: For landline number, exclude the '0' before the 'Area Code'. For Mobile number, exclude the '0' before the number.
                                            </td></tr>
                                         <tr><td colspan="4">
                                            use "," to add more E-Mail Eg: (xx@xx.com, yy@yy.com)
                                            </td></tr>--%>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
