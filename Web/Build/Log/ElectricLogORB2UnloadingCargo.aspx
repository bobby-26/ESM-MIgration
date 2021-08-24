<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2UnloadingCargo.aspx.cs" Inherits="Log_ElectricLogORB2UnloadingCargo" %>



<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>UnLoading of Oil Cargo</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
        <script type="text/javascript" src="<%=Session["sitepath"]%>/js/jquery-1.12.4.min.js"></script>
        <script type="text/javascript">
            function confirm(args) {
                if (args) {
                    __doPostBack("<%=confirm.UniqueID %>", "");
                }
            }
            function NumberFieldKeyPress(obj, arg) {
                if (arg.get_keyCode() == 45) {
                    alert("This field only takes positive number.");
                    arg.set_cancel(true);
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .right {
            display: inline-block;
            float: right;
        }

        .left {
            display: inline-block;
            float: left;
            width: 50px;
        }

        .notes {
            margin-top: 2%;
            /*background-color: yellow;*/
            margin-bottom: 2%;
        }

        .notes p {
                font-size: 13px;
                /*display:inline-block;*/
                font-weight: bold;
                margin-top: 0px;
                margin-bottom: 0px;
        }

        .dropdown {
            margin-right: 20px;
        }

        .textbox {
            width: 12em;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>

        <telerik:RadAjaxPanel runat="server" ID="radAjaxPanel">

            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                    <h3>UnLoading of Oil Cargo</h3>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <DateInput ID="DateInput1" DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblUnloadTermainal" runat="server" Text="Un-Load Terminal" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtUnloadTerminal" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtOperationDate_SelectedDateChanged" >
                                </telerik:RadTextBox>

                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblQtyUnloaded" runat="server" Text="Total Quantity Un-Loaded"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtTotalQtyUnloaded" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true" OnTextChanged="txtOperationDate_SelectedDateChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                                <telerik:RadLabel ID="lblfromUnit" runat="server" Text="m3"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblGradeOfCargo" runat="server" Visible="true" Text="Grade of Cargo"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtGradeOfCargo" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtOperationDate_SelectedDateChanged" MaxLength="10">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <div>
                        <h3>Tanks Loaded with Quantity in M3</h3>
                        <p style="color:red">(type in the loaded qty for the required tanks)</p>
                        <table>
                            <tr>
                                <td runat="server" style="vertical-align:top">
                                    <asp:Repeater ID="rptrTank" runat="server" >
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Loaded Qty (m3)</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>                                            
                                            <tr class="style_one">
                                                <td class="style_one">
                                                    <telerik:RadLabel runat="server" ID="lblTankId" Visible="false" Text='<%#Eval("FLDLOCATIONID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel runat="server" ID="lblTankName" Text='<%#Eval("FLDNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadNumericTextBox runat="server" Width="85px" ID="txtLoadedQty" Text='<%#Eval("FLDVALUE") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtLoadedQty_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                                </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td style="width:10px"></td>                                
                                <td runat="server" style="vertical-align:top">
                                    <asp:Repeater ID="rptrTank20" runat="server">
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Loaded Qty (m3)</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="style_one">
                                                <td class="style_one">
                                                    <telerik:RadLabel runat="server" ID="lblTankId" Visible="false" Text='<%#Eval("FLDLOCATIONID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel runat="server" ID="lblTankName" Text='<%#Eval("FLDNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadNumericTextBox runat="server" Width="85px" ID="txtLoadedQty" Text='<%#Eval("FLDVALUE") %>' EnabledStyle-HorizontalAlign="Right" OnTextChanged="txtLoadedQty_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                                </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td style="width:10px"></td>                                
                                <td runat="server" style="vertical-align:top">
                                    <asp:Repeater ID="rptrTank30" runat="server">
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Loaded Qty (m3)</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="style_one">
                                                <td class="style_one">
                                                    <telerik:RadLabel runat="server" ID="lblTankId" Visible="false" Text='<%#Eval("FLDLOCATIONID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel runat="server" ID="lblTankName" Text='<%#Eval("FLDNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadNumericTextBox runat="server" Width="85px" ID="txtLoadedQty" Text='<%#Eval("FLDVALUE") %>' EnabledStyle-HorizontalAlign="Right" OnTextChanged="txtLoadedQty_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                                </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                                <td style="width:10px"></td>            
                                <td runat="server" style="vertical-align:top">
                                    <asp:Repeater ID="rptrTank40" runat="server">
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Loaded Qty (m3)</th>
                                                    </tr>
                                                </thead>
                                                <tbody>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr class="style_one">
                                                <td class="style_one">
                                                    <telerik:RadLabel runat="server" ID="lblTankId" Visible="false" Text='<%#Eval("FLDLOCATIONID") %>'></telerik:RadLabel>
                                                    <telerik:RadLabel runat="server" ID="lblTankName" Text='<%#Eval("FLDNAME") %>'></telerik:RadLabel>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadNumericTextBox runat="server" Width="85px" ID="txtLoadedQty" Text='<%#Eval("FLDVALUE") %>' EnabledStyle-HorizontalAlign="Right" OnTextChanged="txtLoadedQty_TextChanged" AutoPostBack="true"></telerik:RadNumericTextBox>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                        <FooterTemplate>
                                            </tbody>
                                                </table>
                                        </FooterTemplate>
                                    </asp:Repeater>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <br />

                    <div>
                        <br />
                        <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">
                            <tr class="style_one">
                                <td class="style_one" style="width: 50px">
                                    <b>
                                        <telerik:RadLabel ID="lblDate" runat="server" Visible="True" Text="Date"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 40px">
                                    <b>
                                        <telerik:RadLabel ID="capCode" runat="server" Visible="True" Text="Code"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 70px">
                                    <b>
                                        <telerik:RadLabel ID="capItemNo" runat="server" Visible="True" Text="Item No."></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblcapRecord" runat="server" Visible="True" Text="Record of operation / Signature of officer in charge"></telerik:RadLabel>
                                    </b>
                                </td>
                            </tr>
                            <tr class="style_one">
                                <td class="style_one">
                                    <telerik:RadLabel ID="txtDate" runat="server" Width="100px" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtCode" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblRecord" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo1" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo2" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr runat="server" id="inchargeRow">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblIncharge" runat="server" Text="Chief Officer :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblinchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblinchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblincsign" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Incharge Sign"
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign" OnClick="btnInchargeSign_Click"
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </div>

                    <div class="notes">
                        <p>Notes:</p>
                        <telerik:RadLabel runat="server" ID="lblnotes"></telerik:RadLabel>
                    </div>
                </div>
            </div>
            <asp:Button ID="confirm" runat="server" Text="confirm" OnClick="confirm_Click" />
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
