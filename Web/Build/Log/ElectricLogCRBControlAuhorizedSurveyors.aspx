<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ElectricLogCRBControlAuhorizedSurveyors.aspx.cs" Inherits="Log_ElectricLogCRBControlAuhorizedSurveyors" %>

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
    <title>Control by Authorized Surveyors</title>
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

         .notes {
            margin-top:2%;
            margin-bottom:2%;
        }

        .notes p {
            font-size: 13px;
            font-weight:bold;
            margin-top:0px;
            margin-bottom:0px;
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
                  <asp:LinkButton runat="server" ID="exportPDf" OnClick="exportpdf_Click"><i class="fa fa-file-pdf"></i></asp:LinkButton>           
                 <h3>Control by Authorized Surveyors</h3>
                    <table>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="RadLabel1" runat="server" Text="Date of Operation"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadDatePicker RenderMode="Lightweight" runat="server" CssClass="input_mandatory" ID="txtOperationDate" MaxLength="12" AutoPostBack="true" OnSelectedDateChanged="txtOperationDate_SelectedDateChanged" >
                                    <DateInput DateFormat="dd-MMM-yyyy" runat="server"></DateInput>
                                </telerik:RadDatePicker>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblterminal" runat="server" Text="Identity of the Terminal"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtterminal" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="120px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblFrom" runat="server" Text="Identiy of the Tanks"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory"  ID="ddlTransferFrom" runat="server" DataTextField="FLDNAME" AutoPostBack="true" OnSelectedIndexChanged="ddlCleaningTank_SelectedIndexChanged" DataValueField="FLDLOCATIONID" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true">
                                </telerik:RadComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblSubstanceName" runat="server" Visible="true" Text="Name of the Substance"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtSubstanceName" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="120px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblCategory" runat="server" Visible="true" Text="Category"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtCategory" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="120px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblTanksempty" runat="server" Visible="true" Text="Have the Tank(s), pump(s) & piping system(s) been empty?" ></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" Visible="true" ID="ddlTanksempty" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCleaningTank_SelectedIndexChanged">
                                </telerik:RadComboBox>--%>

                                <telerik:RadRadioButtonList runat="server" ID="ddlTanksempty"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblprewashtin" runat="server" Visible="true" Text="Has a prewash tin accordance with the ship's Procedures and Arrangements Manual been carried out?" Width="280px" ></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" Visible="true" ID="ddlprewashtin" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCleaningTank_SelectedIndexChanged">
                                </telerik:RadComboBox>--%>

                                <telerik:RadRadioButtonList runat="server" ID="ddlprewashtin"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lbltankwashing" runat="server" Visible="true" Text="Have tank washing resulting from the prewash been discharged ashore and is the tank empty?" Width="250px"></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" Visible="true" ID="ddltankwashing" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCleaningTank_SelectedIndexChanged">
                                </telerik:RadComboBox>--%>

                                <telerik:RadRadioButtonList runat="server" ID="ddltankwashing"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblexemptiongranted" runat="server" Visible="true" Text="An exemption has been granted from mandatory prewash" ></telerik:RadLabel>
                            </td>
                            <td>
                                <%--<telerik:RadComboBox DropDownPosition="Static" CssClass="input_mandatory" Visible="true" ID="ddlexemptiongranted" runat="server" EnableLoadOnDemand="True"
                                    EmptyMessage="Type to select" Filter="Contains" MarkFirstMatch="true" AutoPostBack="true" OnSelectedIndexChanged="ddlCleaningTank_SelectedIndexChanged">
                                </telerik:RadComboBox>--%>

                                <telerik:RadRadioButtonList runat="server" ID="ddlexemptiongranted"  AutoPostBack="true" Direction="Horizontal" OnSelectedIndexChanged="ddldischargeballast_SelectedIndexChanged">
                                    <Items>
                                        <telerik:ButtonListItem Value="No" Text="No" ToolTip="No"  Selected="true" />
                                        <telerik:ButtonListItem Value="Yes" Text="Yes" ToolTip="Yes"/>
                                    </Items>
                                </telerik:RadRadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblreasonexemption" runat="server" Text="Reasons for exemption"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtreasonexemption" RenderMode="Lightweight" CssClass="input_mandatory" TextMode="MultiLine" Rows="2" runat="server" Width="200px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblsurveyor" runat="server" Text="Name and signature of authorised surveyor"></telerik:RadLabel>
                            </td>
                            <td>
<%--                                <telerik:RadTextBox ID="txtsurveyor" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="120px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>  --%>  
                                <asp:LinkButton runat="server" AlternateText="Attachment"
                                    CommandName="ATTACHMENT" ID="cmdAttachment"
                                    OnClick="cmdAttachment_Click" ToolTip="Attachment" Width="20PX" Height="20PX">
                                        <span class="icon"><i runat="server" id="attachmentIcon" class="fas fa-paperclip-na"></i></span>
                                </asp:LinkButton>                                                            
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <telerik:RadLabel ID="lblorganisation" runat="server" Text="Organisation, Company, government agency for which surveyor works" Width="250px"></telerik:RadLabel>
                            </td>
                            <td>
                                <telerik:RadTextBox ID="txtorganisation" RenderMode="Lightweight" CssClass="input_mandatory" runat="server" Width="120px" AutoPostBack="true" OnTextChanged="Refresh_TextChangedEvent">
                                </telerik:RadTextBox>                                
                            </td>
                        </tr>

                    </table>

                    <div>
                         <div>

                <%--<telerik:RadButton runat="server" ID="exportpdf" Text="Print" OnClick="exportpdf_Click"/>--%>
                
            </div>
            
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
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo1" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord1" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo2" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord2" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo3" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord3" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo4" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord4" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo5" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord5" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo6" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord6" runat="server"></telerik:radlabel>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo7" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord7" runat="server"></telerik:radlabel>
                                    <telerik:RadDropDownList runat="server" ID="ddlAttachment" Visible="false" AutoPostBack="true" OnItemSelected="ddlAttachment_ItemSelected" Width="200px"></telerik:RadDropDownList>
                                    <asp:HyperLink ID="lnkfilename" Target="_blank" Text="View" runat="server" Width="14px" Visible="false"
                                        Height="14px" ToolTip="Download File">
                                    </asp:HyperLink>
                            </td>
                        </tr>
                         <tr>
                            <td></td>
                            <td></td>
                            <td class="style_one" style="text-align:center">
                                <telerik:radlabel id="txtItemNo8" runat="server"></telerik:radlabel>
                            </td>
                            <td class="style_one"> 
                                <telerik:radlabel id="lblrecord8" runat="server"></telerik:radlabel>
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
                                        CommandName="INCHARGESIGN" ID="btnInchargeSign"  OnClick="btnInchargeSign_Click"
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