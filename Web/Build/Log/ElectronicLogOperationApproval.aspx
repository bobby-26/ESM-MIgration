<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectronicLogOperationApproval.aspx.cs" Inherits="ElectronicLog_ElectronicLogOperationApproval" %>

<%@ Import Namespace="SouthNests.Phoenix.Registers" %>
<%@ Import Namespace="SouthNests.Phoenix.Framework" %>

<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<%@ Register TagPrefix="eluc" TagName="Hard" Src="~/UserControls/UserControlHard.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Record of Operation</title>
    <telerik:radcodeblock id="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>

    </telerik:radcodeblock>
    <script src="jquery.min.js"></script>

    <style>
        /*table, th, td {
            border: 1px solid black;
            border-collapse: collapse;
        }*/
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }
    </style>



</head>
<body>
    <form id="frmgvCounterUpdate" runat="server" submitdisabledcontrols="true">
        <telerik:radscriptmanager runat="server" id="RadScriptManager1" />
        <telerik:radskinmanager id="RadSkinManager1" runat="server" />

        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <%--<eluc:Status runat="server" ID="ucStatus" />--%>
        <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" />
        <eluc:TabStrip ID="MenuOperationApproval" runat="server" OnTabStripCommand="MenuOperationApproval_TabStripCommand"></eluc:TabStrip>
        <div>
            <table>
                <tr>
                    <td>
                        <b>
                            <telerik:radlabel id="capDate" runat="server" visible="True" text="Date of Operation"></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="lblOperationDate" runat="server" visible="True" text='<%# DataBinder.Eval(Container,"DataItem.FLDDATE") %>'></telerik:radlabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <b>
                            <telerik:radlabel id="lblfromlocation" runat="server" text="Sludge Transfered From:"></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtFromLocation" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMLOCATION") %>'></telerik:radlabel>
                    </td>
                    <td>
                        <b>
                            <telerik:radlabel id="lbltolocation" runat="server" text="Sludge Transfered To:"></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtToLocation" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDTOLOCATION") %>'></telerik:radlabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <b>
                            <telerik:radlabel id="lblbfrfromROB" runat="server" text="Before Transfer ROB : "></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtbfrFromRob" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMROB") %>'></telerik:radlabel>
                    </td>
                    <td>
                        <b>
                            <telerik:radlabel id="lblbfrtoROB" runat="server" text="Before Transfer ROB : "></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtbfrtoRob" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:radlabel>
                    </td>
                </tr>

                <tr>
                    <td>
                        <b>
                            <telerik:radlabel id="lblaftfromROB" runat="server" text="After Transfer ROB : "></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtaftfrmROB" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDFROMROB") %>'></telerik:radlabel>
                    </td>
                    <td>
                        <b>
                            <telerik:radlabel id="lblafttorob" runat="server" text="After Transfer ROB : "></telerik:radlabel>
                        </b>
                    </td>
                    <td>
                        <telerik:radlabel id="txtafttorob" runat="server" text='<%# DataBinder.Eval(Container,"DataItem.FLDTOROB") %>'></telerik:radlabel>
                    </td>
                </tr>
            </table>
            <br />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                    <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
                        <tr class="style_one">
                            <td class="style_one" style="width: 50px">
                                <b>
                                    <telerik:radlabel id="lblDate" runat="server" visible="True" text="Date"></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 40px">
                                <b>
                                    <telerik:radlabel id="capCode" runat="server" visible="True" text="Code"></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 70px">
                                <b>
                                    <telerik:radlabel id="capItemNo" runat="server" visible="True" text="Item No."></telerik:radlabel>
                                </b>
                            </td>
                            <td class="style_one">
                                <b>
                                    <telerik:radlabel id="lblcapRecord" runat="server" visible="True" text="Record of operation / Signature of officer in charge"></telerik:radlabel>
                                </b>
                            </td>
                        </tr>
                        <tr class="style_one">
                            <td class="style_one">
                                <telerik:radlabel id="txtDate" runat="server" visible="True"></telerik:radlabel>
                            </td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtCode" runat="server" visible="True"></telerik:radlabel>
                            </td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo" runat="server" visible="True"></telerik:radlabel>
                            </td>
                            <td>
                                <telerik:radlabel id="lblRecord" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                        <tr id="rowTwo" runat="server">
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="style_one">
                                <telerik:radlabel id="lbltorecord" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="style_one">
                                <b>
                                    <telerik:radlabel id="lblIncharge" runat="server" text="Officer Incharge :"></telerik:radlabel>
                                </b>
                                <telerik:radlabel id="lblincsign" runat="server" visible="false"></telerik:radlabel>
                                <telerik:radtextbox id="txtUserName" runat="server"></telerik:radtextbox>
                                <telerik:radtextbox id="txtPassword" runat="server" textmode="Password" emptymessage="Password"></telerik:radtextbox>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="true"
                                    CommandName="INCHARGESIGN" ID="btnIncharge" OnClick="btnIncharge_Click"
                                    ToolTip="Sign" Width="20PX" Height="20PX">
                                <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                </asp:LinkButton>
                                <telerik:radlabel id="lblsignonedate" style="float:right;padding-right:200px;" runat="server" visible="True"></telerik:radlabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="style_one">
                                <b>
                                    <telerik:radlabel id="lblCheif" runat="server" text="Chief Engineer    :"></telerik:radlabel>
                                </b>
                                <telerik:radlabel id="lblChiefSign" runat="server" visible="false"></telerik:radlabel>
                                &nbsp&nbsp <%--emptymessage="User Name"--%>
                                <telerik:radtextbox id="txtCheifUserName" runat="server" ></telerik:radtextbox>
                                <telerik:radtextbox id="txtCheifPassword" runat="server" textmode="Password" emptymessage="Password"></telerik:radtextbox>
                                <asp:LinkButton runat="server" AlternateText="Delete" Visible="true"
                                    CommandName="OFFICERSIGN" ID="btnOfficer" OnClick="btnOfficer_Click"
                                    ToolTip="Sign" Width="20PX" Height="20PX">
                                <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                </asp:LinkButton>

                                <telerik:radlabel id="lblsigntwodate" style="float:right;padding-right:200px;" runat="server" visible="True"></telerik:radlabel>

                            </td>
                        </tr>
                    </table>
                </div>
            </div>
        </div>
    </form>
</body>
</html>


