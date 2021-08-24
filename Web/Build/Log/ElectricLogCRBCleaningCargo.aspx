<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBCleaningCargo.aspx.cs" Inherits="Log_ElectricLogCRBCleaningCargo" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Number" Src="~/UserControls/UserControlMaskNumber.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>



<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Cleaning of Cargo Tanks Except Mandatory Prewash (Other Prewash Operations, Final Wash, Ventilation, etc.)</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .notes {
            margin-top: 2%;
            margin-bottom: 2%;
        }

            .notes p {
                font-size: 13px;
                font-weight: bold;
                margin-top: 0px;
                margin-bottom: 0px;
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
                    <h3>Cleaning of Cargo Tanks Except Mandatory Prewash (Other Prewash Operations, Final Wash, Ventilation, etc.)</h3>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation" ></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStartTime" runat="server" Text="Tank Washing Started(Time)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStartTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel2" runat="server" Text="HH:MM"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblEndTime" runat="server" Text="Tank Washing Completed(Time)"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtEndTime" runat="server" CssClass="input_mandatory" AutoPostBack="true" Width="100px" OnSelectedDateChanged="txtStartTime_SelectedDateChanged">
                                    <TimeView Interval="00:30:00"></TimeView>
                                </telerik:RadTimePicker>
                                <telerik:RadLabel ID="Radlabel4" runat="server" Text="HH:MM"></telerik:RadLabel>

                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblwashingmd" runat="server" Text="Washing Method" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtwashingmethod" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCleaningAgents" runat="server" Width="250px" Text="Cleaning Agent(s) (Identify Agent(s) and Quantities" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCleaningAgents" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblventilation" runat="server" Text="Ventilation Procedure Used (State Number of Fans Used, Duration of Ventilation)" Style="vertical-align: text-bottom" Width="250px"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtventilationproc" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbltransferredto" Font-Bold="true" runat="server" Text="Tank Washing Transferred to:" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td></td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblIntoSea" runat="server" Text="16.1 Into the sea" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtIntoSea" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblReception" runat="server" Text="16.2 To Reception Facility (Identify Port)*" Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtReception" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSlops" runat="server" Text="16.3 To Slops Collecting Tank (Identify Tank)." Style="vertical-align: text-bottom"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSlops" runat="server" CssClass="input_mandatory" RenderMode="Lightweight" AutoPostBack="true" OnTextChanged="txtAfrTrnsROBFrom_TextChangedEvent">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                    <br />


                    <div>
                        <h3>Identity of Tanks</h3>
                        <table>
                            <tr>
                                <td>
                                    <asp:Repeater ID="rptrTank" runat="server" >
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Name of Substance</th>
                                                        <th class="style_one" style="text-align:center">Category</th>
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
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtSubstance" Text='<%#Eval("FLDSUBNAME") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtCategory" Text='<%#Eval("FLDCATEGORY") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
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
                                <td>
                                    <asp:Repeater ID="rptrTank20" runat="server">
                                        <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Name of Substance</th>
                                                        <th class="style_one" style="text-align:center">Category</th>
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
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtSubstance" Text='<%#Eval("FLDSUBNAME") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtCategory" Text='<%#Eval("FLDCATEGORY") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
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
                                <td>
                                    <asp:Repeater ID="rptrTank30" runat="server">
                                       <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Name of Substance</th>
                                                        <th class="style_one" style="text-align:center">Category</th>
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
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtSubstance" Text='<%#Eval("FLDSUBNAME") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtCategory" Text='<%#Eval("FLDCATEGORY") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
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
                                <td>
                                    <asp:Repeater ID="rptrTank40" runat="server">
                                       <HeaderTemplate>
                                            <table class="style_one" cellpadding="3px" cellspacing="3px" style="padding: 0 50px; width: 100%" >
                                                <thead>
                                                    <tr class="style_one">
                                                        <th class="style_one" style="text-align:center">Identification of Tanks</th>
                                                        <th class="style_one" style="text-align:center">Name of Substance</th>
                                                        <th class="style_one" style="text-align:center">Category</th>
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
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtSubstance" Text='<%#Eval("FLDSUBNAME") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
                                                </td>
                                                <td class="style_one">
                                                    <telerik:RadTextBox runat="server" Width="85px" ID="txtCategory" Text='<%#Eval("FLDCATEGORY") %>' EnabledStyle-HorizontalAlign="Right"  OnTextChanged="txtSubstance_TextChanged" AutoPostBack="true"></telerik:RadTextBox>
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
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo3" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo4" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo5" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr>
                                <td></td>
                                <td></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="txtItemNo6" runat="server"></telerik:RadLabel>
                                </td>
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblrecord6" runat="server"></telerik:RadLabel>
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
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
