<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogBunkeringFuel.aspx.cs" Inherits="Log_ElectricLogBunkeringFuel" %>

<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Bunkering Fuel</title>
    <telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
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
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }

        .bold {
            font-weight: bold;
        }

        #tblTemplate {
            font-size: small;
            padding: 0 50px;
            width: 100%;
        }
        /*cellpadding="5px" cellspacing="5px" class="style_one"*/
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />

        <telerik:RadAjaxPanel runat="server">
            <eluc:Error ID="ucError" runat="server" Visible="false"></eluc:Error>
            <eluc:Message ID="ucMessage" runat="server" Text="" Visible="false"></eluc:Message>
            <eluc:TabStrip ID="gvTabStrip" runat="server" OnTabStripCommand="gvTabStrip_TabStripCommand"></eluc:TabStrip>
            <asp:Button ID="cmdHiddenSubmit" runat="server" OnClick="cmdHiddenSubmit_Click" Style="display: none;" />

            <div style="width: 100%; overflow: auto;">
                <div style="float: initial; margin: 0 50px">
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblOperationDate" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblPort" Text="Port of Bunker" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtPort" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStartDate" Text="Start Date" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtStartDate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" MaxLength="12"  OnTextChanged="txt_TextChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStartTime" Text="Time" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStartTime" CssClass="input_mandatory" runat="server"  OnSelectedDateChanged="txt_TextChanged">
                                </telerik:RadTimePicker>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblStopDate" Text="Stop Date" runat="server">
                                </telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker ID="txtStopDate" RenderMode="Lightweight" runat="server" CssClass="input_mandatory" MaxLength="12"  OnTextChanged="txt_TextChanged">
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblStopTime" Text="Time" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTimePicker ID="txtStopTime" runat="server" CssClass="input_mandatory"  OnSelectedDateChanged="txt_TextChanged"></telerik:RadTimePicker>
                            </td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBunkerQty" Text="Bunker Quantity" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBunkerQty" RenderMode="Lightweight" MinValue="0" MaxValue="99999999" CssClass="input_mandatory" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number">
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblunit" Text="MT" CssClass="bold" runat="server"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBunkerType" Text="Type of Bunker" runat="server"></telerik:RadLabel>

                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBunkerType" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblBunkerStandard" Text="Bunker ISO Standard" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtBunkerStandard" runat="server" CssClass="input_mandatory"  OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblbunkerSulphur" Text="Bunker Sulphur%" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtbunkerSulphur" runat="server" CssClass="input_mandatory" AutoPostBack="true" OnTextChanged="txt_TextChanged">
                                </telerik:RadTextBox>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                    </table>
                    <br />
                    <%-- adding dynamic button here --%>


                    <telerik:RadLabel ID="lblTableTitle" CssClass="bold" Text="Table Bunkered With MT Quantity & Final ROB" runat="server"></telerik:RadLabel>
                    <telerik:RadButton runat="server" ID="btnAdd" OnClick="btnAdd_Click" Text="Add Tanks"></telerik:RadButton>
                    <telerik:RadButton runat="server" ID="btnDelete" OnClick="btnDelete_Click" Text="Delete Tanks"></telerik:RadButton>

                    <table runat="server" id="tankList">
                        <tr>
                            <td>Tank</td>
                            <td>Before Bunker ROB</td>
                            <td>Bunker Quantity</td>
                            <td>Final ROB</td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom0" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom0" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty0" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty0" runat="server" CssClass="input_mandatory" Enabled="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed0" runat="server" Text=""></telerik:RadLabel>
                            </td>

                        </tr>
                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom1" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom1" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty1" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty1" runat="server" Enabled="false" CssClass="input_mandatory">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed1" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>
                        <%-- here need to add the other fields and make it hidden --%>
                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom2" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom2" Visible="false" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty2" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty2" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed2" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>


                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom3" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom3" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty3" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MaxLength="22" MinValue="0" MaxValue="99999999" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty3" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed3" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom4" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom4" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty4" RenderMode="Lightweight" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty4" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed4" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom5" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                               <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom5" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty5" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                     <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty5" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed5" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom6" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                               <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom6" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty6" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty6" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed6" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom7" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td> 
                               <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom7" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty7" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty7" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed7" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom8" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                               <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom8" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>

                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty8" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty8" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed8" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr>
                            <td>
                                <telerik:RadComboBox dropdownposition="Static" Style="width: 100%"  ID="ddlTransferFrom9" runat="server" DataTextField="FLDNAME" OnSelectedIndexChanged="ddlTransferFrom1_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True" Visible="false"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                               <telerik:RadNumericTextBox ID="txtBftTrnsROBFrom9" RenderMode="Lightweight" Visible="false" CssClass="input_mandatory" MinValue="0" MaxValue="99999999" runat="server" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number" >
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadNumericTextBox ID="txtbfrBunkerQty9" RenderMode="Lightweight" CssClass="input_mandatory"  runat="server" MinValue="0" MaxValue="99999999" MaxLength="22" Width="120px" Style="text-align: right;" Type="Number"  AutoPostBack="true"  OnTextChanged="txtbfrBunkerQty_TextChanged" Visible="false">
                                    <ClientEvents OnKeyPress="NumberFieldKeyPress" />
                                </telerik:RadNumericTextBox>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtTotalQty9" runat="server" Enabled="false" CssClass="input_mandatory" Visible="false">
                                </telerik:RadTextBox>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblcapacityexceed9" runat="server" Text=""></telerik:RadLabel>
                            </td>
                        </tr>


                    </table>

                    <table runat="server" id="tblTemplate" class="style_one" cellpadding="5" cellspacing="5">
                        <tr class="style_one">
                            <td class="style_one" style="width: 50px">
                                <b>
                                    <telerik:RadLabel ID="lblDate" runat="server" Text="Date"></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 40px">
                                <b>
                                    <telerik:RadLabel ID="capCode" runat="server" Text="Code"></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one" style="width: 70px">
                                <b>
                                    <telerik:RadLabel ID="capItemNo" runat="server" Text="Item No."></telerik:RadLabel>
                                </b>
                            </td>
                            <td class="style_one">
                                <b>
                                    <telerik:RadLabel ID="lblcapRecord" runat="server" Text="Record of operation / Signature of officer in charge"></telerik:RadLabel>
                                </b>
                            </td>
                        </tr>
                        <tr class="style_one">
                            <td class="style_one">
                                <telerik:RadLabel ID="txtDate" runat="server" Width="100px"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtCode" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtItemNo" runat="server"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadLabel ID="lblBunkeringId" Visible="false" runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblRecord0" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtItemNo1" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord1" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center">
                                <telerik:RadLabel ID="txtItemNo2" runat="server"></telerik:RadLabel>
                            </td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecord2" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord0">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId0" Visible="false" runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankRecord0" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord1">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId1" Visible="false" runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankRecord1" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord2" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId2" Visible="false" runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankRecord2" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord3" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId3" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord3" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord4" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId4" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord4" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord5" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId5" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord5" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord6" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId6" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord6" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord7" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId7" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord7" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord8" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId8" Visible="false" runat="server"></telerik:RadLabel>

                                <telerik:RadLabel ID="lblTankRecord8" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>

                        <tr runat="server" id="rowTankRecord9" style="display: none;">
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align: center"></td>
                            <td class="style_one">
                                <telerik:RadLabel ID="lblRecordId9" Visible="false" runat="server"></telerik:RadLabel>
                                <telerik:RadLabel ID="lblTankRecord9" runat="server"></telerik:RadLabel>
                            </td>
                        </tr>
     
                        <tr>
                            <td></td>
                            <td></td>
                            <td></td>
                            <td class="style_one">
                                <b>
                                    <telerik:RadLabel ID="lblIncharge" runat="server" Text="Officer Incharge :"></telerik:RadLabel>
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
                        <%--<tr runat="server" id="cheifEngineerRow">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblCheifEngineer" runat="server" Text="Chief Engineer:"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblCheifEngId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCheifEngName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCheifEngRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCheifEngSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblCheifEngsign" runat="server" Visible="false"></telerik:RadLabel>
                                    <asp:LinkButton runat="server" AlternateText="Incharge Sign"  
                                        CommandName="CHEIFENGINNERSIGN" ID="btnCheifEnginnerSign"  
                                        ToolTip="Chief Engineer Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>--%>
                    </table>
                </div>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
