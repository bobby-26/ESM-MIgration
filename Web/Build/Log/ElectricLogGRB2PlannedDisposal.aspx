<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogGRB2PlannedDisposal.aspx.cs" Inherits="Log_ElectricLogGRB2PlannedDisposal" %>



<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Planned Discharge</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript">

            function NumberFieldKeyPress(obj, arg) {
                if (arg.get_keyCode() == 45) {
                    alert("This field only takes positive number.");
                    arg.set_cancel(true);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .container  {
            padding: 3%;
        }
        .ui-control {
            width: 150px !important;
        }
        .ui-textarea {
            width: 250px !important;
        }

        .ui-attachment {
            margin-left: 30px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server" ID="radAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />
      
            <div class="container">
                <table runat="server" id="tblEntry">
                    <tr>
                        <td>Date</td>
                        <td> <telerik:RadDateTimePicker RenderMode="Lightweight" ID="dtPicker"  Width="100%" runat="server" CssClass="input_mandatory">
                            <TimeView TimeFormat="HH:mm:ss" runat="server">
                            </TimeView>
                            <DateInput DisplayDateFormat="dd-MM-yyyy HH:mm:ss" runat="server">
                            </DateInput>
                            </telerik:RadDateTimePicker>
                        </td>
                    </tr>
                    <tr>
                        <td>Port / Vessel / Position</td>
                        <td>
                            <telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtVesselName" CssClass="ui-control input_mandatory"></telerik:RadTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>Category</td>
                        <td><telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="ddlCategory" CssClass="ui-control input_mandatory"></telerik:RadComboBox></td>
                    </tr>
                    <tr>
                        <td>Method</td>
                        <td><telerik:RadComboBox runat="server" RenderMode="Lightweight" ID="ddlMethod" CssClass="ui-control" AutoPostBack="true" OnTextChanged="ddlMethod_TextChanged"></telerik:RadComboBox></td>
                    </tr>
                    <tr>
                        <td>Discharge Qty</td>
                        <td><telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtValue" CssClass="ui-control input_mandatory"></telerik:RadNumericTextBox></td>
                        <td>
                            <telerik:RadLabel ID="RadLabel4" runat="server" Text="m3"></telerik:RadLabel>
                            <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="ATTACHMENT" ID="lnkAttachment" CssClass="ui-attachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class='icon'><i runat="server" id="attachmentIcon" class='fas fa-paperclip-na'></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <%--<tr>
                        <td>Discharge into sea</td>
                        <td>
                            <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtAmountDischargeSea" CssClass="ui-control"></telerik:RadNumericTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                    </tr>
                    <tr>
                        <td>Discharge to reception</td>
                        <td>
                            <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtAmountDischargeReception" CssClass="ui-control" AutoPostBack="true" OnTextChanged="txtAmountDischargeReception_TextChanged"></telerik:RadNumericTextBox>
                            <telerik:RadLabel ID="RadLabel1" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel3" runat="server" Text="m3"></telerik:RadLabel>
                            <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="ATTACHMENT" ID="lnkAttachment" CssClass="ui-attachment"
                                    ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class='icon'><i runat="server" id="attachmentIcon" class='fas fa-paperclip-na'></i></span>
                            </asp:LinkButton>
                        </td>
                    </tr>
                    <tr>
                        <td>Incineration</td>
                        <td>
                            <telerik:RadNumericTextBox runat="server" RenderMode="Lightweight" ID="txtAmountIncineration" CssClass="ui-control" AutoPostBack="true" OnTextChanged="txtAmountIncineration_TextChanged"></telerik:RadNumericTextBox>
                        </td>
                        <td>
                            <telerik:RadLabel ID="RadLabel2" runat="server" Text="m3"></telerik:RadLabel>
                        </td>
                    </tr>--%>
                    <tr>
                        <td>Remarks</td>
                        <td><telerik:RadTextBox runat="server" RenderMode="Lightweight" ID="txtRemarks" TextMode="MultiLine" Rows="10" Columns="10" CssClass="ui-control ui-textarea input_mandatory"></telerik:RadTextBox></td>
                    </tr>
                    <tr>
                        <td>Officer Incharge</td>
                        <td><asp:LinkButton runat="server" AlternateText="Incharge Sign"
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                            </asp:LinkButton>
                            <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>

                        </td>
                    </tr>
                </table>
                <div class="notes">
                        <p><b>Notes:</b></p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                </div>


            </div>

            
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>