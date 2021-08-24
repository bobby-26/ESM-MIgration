<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreApprovalRejection.aspx.cs"
    Inherits="CrewOffshoreApprovalRejection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="User" Src="../UserControls/UserControlMultiColumnUser.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Reject Proposal</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
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
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
            <eluc:Status ID="ucStatus" runat="server" />

            <eluc:TabStrip ID="MenuDiscussion" runat="server" Title="Reject Proposal" OnTabStripCommand="MenuDiscussion_TabStripCommand"></eluc:TabStrip>

            <div>
                <table runat="server" id="tblPersonalMaster" width="100%">
                    <tr>
                        <td>
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
                        <td runat="server" id="tdempno">
                            <telerik:RadLabel ID="lblEmployeeNo" runat="server" Text="Employee Number"></telerik:RadLabel>
                        </td>
                        <td runat="server" id="tdempnum">
                            <telerik:RadTextBox runat="server" ID="txtEmployeeNumber" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblRank" runat="server" Text="Rank"></telerik:RadLabel>
                        </td>
                        <td colspan="3">
                            <telerik:RadTextBox runat="server" ID="txtRank" CssClass="readonlytextbox" ReadOnly="true"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <hr />
                        </td>
                    </tr>
                </table>
            </div>
            <div>
                <table border="0" cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                    width="99%">
                    <tr>
                        <td align="left" colspan="2">
                            <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                        </td>
                        <td align="left" style="vertical-align: top;" colspan="2">
                            <eluc:Hard ID="ucPDStatus" runat="server" HardTypeCode="99" AppendDataBoundItems="true" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <telerik:RadLabel ID="lblPostYourCommentsHere" runat="server" Text="Post Your Comments Here"></telerik:RadLabel>
                        </td>
                        <td align="left" style="vertical-align: top;" colspan="2">
                            <telerik:RadTextBox ID="txtNotesDescription" runat="server" CssClass="gridinput_mandatory"
                                Height="49px" TextMode="MultiLine" Width="692px"></telerik:RadTextBox>
                        </td>
                    </tr>
                </table>
                <table>
                    <tr>
                        <td>
                            <telerik:RadLabel ID="lblSearchBy" runat="server" Text="Search By"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblUser" runat="server" Text="User"></telerik:RadLabel>
                        </td>
                        <td>
                            <eluc:User ID="ucUser" runat="server" AppendDataBoundItems="true" 
                                ActiveYN="172" AutoPostBack="true" />
                            <asp:LinkButton ID="ImgSearch" runat="server" ImageAlign="AbsBottom" 
                                ToolTip="Search" OnClick="ImgSearch_Click" >
                                <span class="icon"><i class="fas fa-search"></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                </table>
                <table cellpadding="1" cellspacing="0" style="padding: 1px; margin: 1px; border-style: solid; border-width: 1px;"
                    width="99%" id="tblComments" name="tblComments">
                    <asp:Repeater ID="repDiscussion" runat="server">
                        <HeaderTemplate>
                        </HeaderTemplate>
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
                                    <telerik:RadLabel ID="lblComments" runat="server" Text="Comments"></telerik:RadLabel>
                                    -<br />
                                    <div style="height: 34px; float: left; width: 200px; border-width: 1px; overflow-y: auto; white-space: normal; word-wrap: break-word; font-weight: bold">
                                        <%# Eval("DESCRIPTION")%>
                                    </div>
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
                        <FooterTemplate>
                        </FooterTemplate>
                    </asp:Repeater>
                </table>
            </div>
            <div id="divPage" style="position: relative;">
                <table width="100%" border="0" class="datagrid_pagestyle">
                    <tr>
                        <td nowrap align="center">
                            <telerik:RadLabel ID="lblPagenumber" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblPages" runat="server">
                            </telerik:RadLabel>
                            <telerik:RadLabel ID="lblRecords" runat="server">
                            </telerik:RadLabel>&nbsp;&nbsp;
                        </td>
                        <td nowrap align="left" width="50px">
                            <asp:LinkButton ID="cmdPrevious" runat="server" OnCommand="PagerButtonClick" CommandName="prev">Prev << </asp:LinkButton>
                        </td>
                        <td width="20px">&nbsp;
                        </td>
                        <td nowrap align="right" width="50px">
                            <asp:LinkButton ID="cmdNext" OnCommand="PagerButtonClick" runat="server" CommandName="next">Next >></asp:LinkButton>
                        </td>
                        <td nowrap align="center">
                            <telerik:RadTextBox ID="txtnopage" MaxLength="3" Width="20px" runat="server">
                            </telerik:RadTextBox>
                            <asp:Button ID="btnGo" runat="server" Text="Go" OnClick="btnGo_Click"
                                Width="40px"></asp:Button>
                        </td>
                    </tr>
                </table>
            </div>
        </div>
      
    </form>
</body>
</html>
