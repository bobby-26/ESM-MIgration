<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreAvailabilitySearch.aspx.cs"
    Inherits="CrewOffshoreAvailabilitySearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Rank" Src="~/UserControls/UserControlRank.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="UserControlNationality" Src="~/UserControls/UserControlNationality.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew Query Activity</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager ID="radscript1" runat="server"></telerik:RadScriptManager>
        <telerik:RadSkinManager ID="radskin1" runat="server"></telerik:RadSkinManager>
        <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top; width: 100%">
            <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>


            <eluc:TabStrip ID="CrewQuery" runat="server" OnTabStripCommand="CrewQuery_TabStripCommand"></eluc:TabStrip>

            <table width="50%" cellpadding="1" cellspacing="1">
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblName" runat="server" Text="Name Contains"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtName" runat="server" Width="150px"  MaxLength="200"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblFileNo" runat="server" Text="File No Contains"></telerik:RadLabel>
                    </td>
                    <td>
                        <telerik:RadTextBox ID="txtFileNo" runat="server" Width="150px"  MaxLength="50"></telerik:RadTextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblCurrentRank" runat="server" Text="Current Rank"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:Rank ID="ddlRank" Width="150px" runat="server"  AppendDataBoundItems="true" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblStatus" runat="server" Text="Status"></telerik:RadLabel>
                    </td>
                    <td>


                        <%-- <eluc:Hard id ="ddlstatus" runat ="server"  AppendDataBoundItems="true" ></eluc:Hard>--%>
                        <telerik:RadComboBox ID="ddlstatus" runat="server" AppendDataBoundItems="true" EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true"
                             Width="150px" EnableViewState="true">
                        </telerik:RadComboBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblNationality" runat="server" Text="Nationality"></telerik:RadLabel>
                    </td>
                    <td>
                        <eluc:UserControlNationality ID="ddlNationality" runat="server" AppendDataBoundItems="true"
                             Width="150px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblDOA" runat="server" Text="DOA"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lblDOAFrom" runat="server" Text="From"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDOAFrom" runat="server"  DatePicker="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblDOATo" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtDOATo" runat="server"  DatePicker="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblcontactdate" runat="server" Text="Last Contact Date"></telerik:RadLabel>
                    </td>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <telerik:RadLabel ID="lbllastcontactfromdate" runat="server" Text="From"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtcontactFrom" runat="server"  DatePicker="true" />
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lbllastcontacttodate" runat="server" Text="To"></telerik:RadLabel>
                                </td>
                                <td>
                                    <eluc:Date ID="txtcontactto" runat="server"  DatePicker="true" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <telerik:RadLabel ID="lblincludentbr" runat="server" Text="Include NTBR"></telerik:RadLabel>
                    </td>
                    <td>
                        <asp:CheckBox ID="chkincludentbr" runat="server" Checked="false" />
                    </td>
                </tr>
            </table>
        </div>

    </form>
</body>
</html>
