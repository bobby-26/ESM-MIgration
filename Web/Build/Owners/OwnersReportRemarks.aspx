<%@ Page Language="C#" AutoEventWireup="true" CodeFile="OwnersReportRemarks.aspx.cs" Inherits="Owners_OwnersReportRemarks" %>


<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Owners Report Remarks</title>
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .container {
            width: 100%;
            margin: 0;
            padding: 0;
        }

        section {
            width: 100%;
        }

            section .heading {
                text-align: center;
                padding: 5px;
                font-size: 18px;
                font-weight: bold;
                background-color:rgb(194, 220, 252);
                background-image: linear-gradient(rgb(244, 248, 250), rgb(233, 242, 251) 50%, rgb(221, 231, 245) 50%, rgb(228, 237, 248)) !important;
                color: rgb(76, 96, 122) !important;
            }

            section .textbox {
                width: 80%;
                display: inline-block;
            }

        .RadInput {
            width: 100% !important;
            height: 80px;
            box-sizing:border-box;
            
        }

        /*.signature {
            width: 15%;
            display: inline-flex;
            flex-direction: column;
            border:1px solid;
            text-align:right;
            justify-content: flex-start;
        }

       
        .sign-button {
            padding: 30px;
        }

        .sign-date {
            padding: 15px;
        }*/


    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <%--<telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server" Height="93%">--%>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" CssClass="hidden" />
            <eluc:Error runat="server" ID="ucError" Visible="false" />
            <div class="container">
                    <table style="table-layout:fixed;width:100%;padding:5px;">
                        <tr>
                            <td style="width:15%; vertical-align:top;">
                                <telerik:RadLabel runat="server" ID="lbl1" Text ="Technical Superintendent"></telerik:RadLabel>
                            </td>
                            <td style="width:60%" colspan="2">
                                <telerik:RadTextBox runat="server" ID="txtTechnicalRemarks"  TextMode="MultiLine" Resize="Both" CssClass="textbox textbox-width input_mandatory"></telerik:RadTextBox>
                            </td>
                            <td  style="width:25%">
                                <table>
                                    <tr>
                                        <td>
                                            <%--<telerik:RadButton RenderMode="Lightweight" ID="btnTechincalSign" runat="server" Text="Sign" OnCommand="btnSign_Command" CommandName="TECHNICAL" ></telerik:RadButton>--%>
                                            <asp:Button runat="server" ID="btnTechincalSign" Text="sign" OnClick="btnTechincalSign_Click" CommandName="TECHNICAL" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>Name:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblTechnicalSignName"></telerik:RadLabel></td>
                                                </tr>
                                                <tr>
                                                    <td>Date:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblTechnicalDate"></telerik:RadLabel></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        <br /></td>
                        </tr>                        
                        <tr>
                            <td style="width:15%; vertical-align:top;">
                                <telerik:RadLabel runat="server" ID="RadLabel1" Text ="Marine Superintendent"></telerik:RadLabel>
                            </td>
                            <td style="width:60%" colspan="2">
                                <telerik:RadTextBox runat="server" ID="txtMarineRemarks" Resize="Both" TextMode="MultiLine" CssClass="textbox textbox-width input_mandatory"></telerik:RadTextBox>
                            </td>
                           <td  style="width:25%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnMarine" Text="sign" OnClick="btnTechincalSign_Click" CommandName="MARINE" />

                                            <%--<telerik:RadButton RenderMode="Lightweight" ID="btnMarine" runat="server" Text="Sign" OnCommand="btnSign_Command" CommandName="MARINE" ButtonType="StandardButton"></telerik:RadButton>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                 <tr>
                                                    <td>Name:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblMarineSignName"></telerik:RadLabel></td>
                                                </tr>
                                                <tr>
                                                    <td>Date:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblMarineSignDate"></telerik:RadLabel></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        <br /></td>
                        </tr>
                        <tr>
                            <td style="width:15%; vertical-align:top;">
                                <telerik:RadLabel runat="server" ID="RadLabel2" Text ="Fleet Manager"></telerik:RadLabel>
                            </td>
                            <td style="width:60%" colspan="2">
                                <telerik:RadTextBox runat="server" ID="txtFleetRemarks" Resize="Both"  TextMode="MultiLine" CssClass="textbox textbox-width input_mandatory"></telerik:RadTextBox>
                            </td>
                            <td  style="width:25%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnFleetSign" Text="sign" OnClick="btnTechincalSign_Click" CommandName="FLEETMANAGER" />
                                                <%--<telerik:RadButton RenderMode="Lightweight" ID="btnFleetSign" runat="server" Text="Sign" OnCommand="btnSign_Command" CommandName="FLEETMANAGER" ButtonType="StandardButton"></telerik:RadButton>--%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>Name:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblFleetName"></telerik:RadLabel></td>
                                                </tr>
                                                <tr>
                                                    <td>Date:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblFleetDate"></telerik:RadLabel></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4">
                        <br /></td>
                        </tr>
                        <tr>
                            <td style="width:15%; vertical-align:top;">
                                <telerik:RadLabel runat="server" ID="lbl4" Text ="Owner"></telerik:RadLabel>
                            </td>
                            <td style="width:60%" colspan="2">
                                <telerik:RadTextBox runat="server" ID="txtowner" Resize="Both"  TextMode="MultiLine" CssClass="textbox textbox-width input_mandatory"></telerik:RadTextBox>
                            </td>
                            <td  style="width:25%">
                                <table>
                                    <tr>
                                        <td>
                                            <asp:Button runat="server" ID="btnowner" Text="sign" OnClick="btnTechincalSign_Click" CommandName="OWNER" />                                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr>
                                                    <td>Name:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblownername"></telerik:RadLabel></td>
                                                </tr>
                                                <tr>
                                                    <td>Date:</td>
                                                    <td><telerik:RadLabel runat="server" ID="lblownerdate"></telerik:RadLabel></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
            </div>
        <%--</telerik:RadAjaxPanel>--%>
    </form>
</body>
</html>
