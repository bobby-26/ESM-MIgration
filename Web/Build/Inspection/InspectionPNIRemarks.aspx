<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InspectionPNIRemarks.aspx.cs"
    Inherits="InspectionPNIRemarks" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlStatus" Src="~/UserControls/UserControlStatus.ascx" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Discussion forum</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>

        <script type="text/javascript">
            function resizediv() {
                var tbl = document.getElementById("tblComments");
                if (tbl != null) {
                    for (var i = 0; i < tbl.rows.length; i++) {
                        tbl.rows[i].cells[2].getElementsByTagName("div")[0].style.width = tbl.rows[i].cells[2].offsetWidth + "px";
                    }
                }
            } //script added for fixing Div width for the comments table
        </script>
    </telerik:RadCodeBlock>
</head>
<body onload="resizediv();">
    <form id="frmCrewGeneralRemarks" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:UserControlStatus ID="ucStatus" runat="server" />
            <eluc:TabStrip ID="CrewMenu" runat="server" OnTabStripCommand="CrewMenu_TabStripCommand" TabStrip="true"></eluc:TabStrip>
        
            <eluc:TabStrip ID="MenuDiscussion" runat="server" OnTabStripCommand="MenuDiscussion_TabStripCommand"></eluc:TabStrip>
              
            <div>
                <telerik:RadLabel ID="Title1" runat="server"></telerik:RadLabel>
                <table width="90%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblGroup" runat="server" Visible="false"></telerik:RadLabel>
                            <telerik:RadLabel ID="lblFirstName" runat="server" Text="First Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtFirstName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblMiddleName" runat="server" Text="Middle Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtMiddleName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblLastName" runat="server" Text="Last Name"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtLastName" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
            </div>

            <div>
                <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                    width="99%">
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblFollowUpDate" runat="server" Text="FollowUp Date"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <eluc:Date ID="txtFollowupDate" runat="server" CssClass="input_mandatory" />
                        </td>
                    </tr>

                </table>
                <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                    width="99%">
                    <tr>
                        <td align="left" colspan="2">
                            <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                        </td>
                        <td align="left" style="vertical-align: top;" colspan="2">
                            <telerik:RadTextBox ID="txtNotesDescription" runat="server"
                                CssClass="gridinput_mandatory" Height="49px" TextMode="MultiLine" Width="692px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                    width="99%" id="tblComments" name="tblComments">
                   <%-- <asp:Repeater ID="repDiscussion" runat="server">--%>
                    <telerik:RadListView ID="repDiscussion" runat="server" AllowCustomPaging="true" AllowPaging="true" PageSize="100">
                       
                        <ItemTemplate>
                            <tr>
                                <td width="70px" style="border-bottom: 1px solid">
                                    <telerik:RadLabel ID="lblPostedBy" runat="server" Text="Posted By"></telerik:RadLabel>
                                </td>
                                <td width="70px" style="border-bottom: 1px solid">
                                    <b>
                                        <%#DataBinder.Eval(Container, "DataItem.NAME")%>
                                    </b>
                                </td>
                                <td align="left" style="border-bottom: 1px solid; border-left: 1px solid;">
                                    <telerik:RadLabel ID="lblCOmments" runat="server" Text="Comments"></telerik:RadLabel>
                                    -<br />
                                    <div style="height: 34px; float: left; width: 200px; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                        <%# Eval("DESCRIPTION")%>
                                    </div>
                                </td>
                                <td width="50px" align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid">
                                    <telerik:RadLabel ID="lblFollowupDate" runat="server" Text="Followup Date"></telerik:RadLabel>
                                </td>
                                <td width="50px" align="left" valign="top" style="border-bottom: 1px solid">
                                    <b>
                                        <%#DataBinder.Eval(Container, "DataItem.FOLLOWUPDATE")%>
                                       
                                    </b>
                                </td>
                                <td width="30px" align="left" valign="top" style="border-left: 1px solid; border-bottom: 1px solid">
                                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                </td>
                                <td width="50px" align="left" valign="top" style="border-bottom: 1px solid">
                                    <b>
                                         <%#DataBinder.Eval(Container, "DataItem.POSTEDDATE")%>
                                      
                                    </b>
                                </td>
                            </tr>
                        </ItemTemplate>
              
                   </telerik:RadListView>
                </table>
            </div>
            
        </div>

    </form>
</body>
</html>
