<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogORB2MissedOperation.aspx.cs" Inherits="Log_ElectricLogORB2MissedOperation" %>


<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="../UserControls/UserControlDate.ascx" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabsTelerik.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Status" Src="~/UserControls/UserControlStatus.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Decimal" Src="~/UserControls/UserControlDecimal.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Latitude" Src="~/UserControls/UserControlLatitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Longitude" Src="~/UserControls/UserControlLongitude.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Message" Src="~/UserControls/UserControlDisplayMessage.ascx" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">

<head id="Head1" runat="server">
    <title>Missed Operational entry</title>
    <telerik:RadCodeBlock ID="Radcodeblock1" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <script type="text/javascript" lang="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>
        <%: Scripts.Render("~/bundles/js") %>
        <%: Styles.Render("~/bundles/css") %>
    </telerik:RadCodeBlock>
    <style>
        .style_one {
            border: 1px solid black;
            border-collapse: collapse;
        }
        .iframe-style {
            margin-bottom: 2%;
        }
    </style>
    <script>
        function resizeIframe(obj) {
          obj.style.height = (obj.contentWindow.document.documentElement.scrollHeight + 20) + 'px';
        }
</script>
</head>
<body>
    <form id="form1" runat="server">
        <telerik:RadScriptManager runat="server" ID="RadScriptManager1" />
        <telerik:RadSkinManager ID="RadSkinManager1" runat="server" />
        <telerik:RadWindowManager runat="server" RenderMode="Lightweight" ID="RadWindowManager1"></telerik:RadWindowManager>
        <telerik:RadAjaxPanel ID="RadAjaxPanel1" runat="server">

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
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" OnSelectedDateChanged="txt_TextChanged" AutoPostBack="true">
                                </telerik:RadDatePicker>
                            </td>
                            <td></td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel runat="server" ID="lblLogBookName" Text="Operational Category"></telerik:RadLabel>                                
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" AutoPostBack="true" ID="ddlLogBookName" runat="server" OnSelectedIndexChanged="ddlLogBookName_SelectedIndexChanged" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                            <td>
                                <telerik:RadButton runat="server" ID="btnEditLog" OnClick="btnEditLog_Click" Text="Edit Log" Visible="false"></telerik:RadButton>
                            </td>
                            <td></td>
                            <td></td>
                        </tr>
                        <tr runat="server" visible="false">
                            <td>
                                <telerik:RadLabel runat="server" ID="lblAdditionalRemarks" Text="Operation or Additional Remark"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox runat="server" ID="txtRemarks" TextMode="MultiLine" Rows="5" Columns="5" Width="200" CssClass="input_mandatory" AutoPostBack="true" OnTextChanged="txt_TextChanged"></telerik:RadTextBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>
            <br />
            <div id="template" runat="server">
                <div style="width: 100%; overflow: auto;">
                    <div style="float: initial; margin: 0 50px">
                        <table cellpadding="5px" cellspacing="5px" class="style_one" style="font-size: small; padding: 0 50px; width: 100%">

                            <tr class="style_one">
                                <td class="style_one" style="width: 50px">
                                    <b>
                                        <telerik:RadLabel ID="lblTemplateDate" runat="server" Text="Date"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 40px">
                                    <b>
                                        <telerik:RadLabel ID="lblTemplateCode" runat="server" Text="Code"></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one" style="width: 70px">
                                    <b>
                                        <telerik:RadLabel ID="lblTemplateItemNo" runat="server" Text="Item No."></telerik:RadLabel>
                                    </b>
                                </td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="lblTemplateRecord" runat="server" Text="Record of operation / Signature of officer in charge"></telerik:RadLabel>
                                    </b>
                                </td>
                            </tr>
                            <tr class="style_one" runat="server" id="temprecord0">
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblTempDate0" runat="server" Width="100px" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode0" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo0" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord0" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>
                            <tr class="style_one" runat="server" id="temprecord1" visible="false">
                                <td class="style_one">
                                    <telerik:RadLabel ID="lblTempDate1" runat="server" Width="100px" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode1" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo1" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord1" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <%--  --%>
                            <tr class="style_one" runat="server" id="temprecord2" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode2" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo2" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord2" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord3" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode3" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo3" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord3" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord4" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode4" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo4" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord4" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord5" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode5" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo5" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord5" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>


                            <tr class="style_one" runat="server" id="temprecord6" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode6" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo6" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord6" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord7" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode7" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo7" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord7" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord8" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode8" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo8" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord8" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord9" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode9" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo9" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord9" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord10" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode10" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo10" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord10" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord11" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode11" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo11" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord11" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord12" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode12" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo12" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord12" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord13" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode13" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo13" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord13" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord14" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode14" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo14" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord14" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord15" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode15" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo15" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord15" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord16" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode16" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo16" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord16" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord17" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode17" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo17" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord17" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>

                            <tr class="style_one" runat="server" id="temprecord18" visible="false">
                                <td class="style_one"></td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempCode18" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td class="style_one" style="text-align: center">
                                    <telerik:RadLabel ID="lblTempItemNo18" runat="server" Visible="True"></telerik:RadLabel>
                                </td>
                                <td>
                                    <telerik:RadLabel ID="lblTempRecord18" runat="server"></telerik:RadLabel>
                                </td>
                            </tr>


   <%--                         <tr runat="server" id="inchargeRow">
                                <td></td>
                                <td></td>
                                <td></td>
                                <td class="style_one">
                                    <b>
                                        <telerik:RadLabel ID="RadLabel1" runat="server" Text="Officer Incharge :"></telerik:RadLabel>
                                    </b>
                                    <telerik:RadLabel ID="lblTempInchId" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTempInchName" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTempInchRank" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTempInchSignDate" runat="server" Visible="false"></telerik:RadLabel>
                                    <telerik:RadLabel ID="lblTempInchSign" runat="server"></telerik:RadLabel>
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
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign"
                                        ToolTip="Incharge Sign" Width="20PX" Height="20PX">
                                         <span class="icon"> <i class="fas fa-file-signature"></i></span>
                                    </asp:LinkButton>
                                </td>
                            </tr>--%>
                        </table>
                    </div>
                </div>
            </div>
            <br />
            <br />
            <br />
            <div style="float: initial; margin: 0 50px">
                <iframe runat="server" id="iframe" class="iframe-style" visible="false" width="100%" height="100%" scrolling="no" onload="iframe_OnLoad"></iframe>
            </div>
        </telerik:RadAjaxPanel>
    </form>
</body>
</html>
