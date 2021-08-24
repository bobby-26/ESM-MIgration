<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CrewOffshoreVesselEmployeePersonal.aspx.cs" Inherits="CrewOffshoreVesselEmployeePersonal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register TagPrefix="eluc" TagName="TabStrip" Src="~/UserControls/UserControlTabs.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Title" Src="~/UserControls/UserControlTitle.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<%@ Register TagPrefix="eluc" TagName="Date" Src="~/UserControls/UserControlDate.ascx" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Crew</title><telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
    <div id="DivHeader" runat="server">
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenixPopup.css" />
        <link rel="stylesheet" type="text/css" href="<%=Session["sitepath"]%>/css/<%=Session["theme"]%>/phoenix.css" />
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/js_globals.aspx"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenix.js"></script>
        <script type="text/javascript" language="javascript" src="<%=Session["sitepath"]%>/js/phoenixPopup.js"></script>                
    </div>
    <script type="text/javascript">
        function showBMI() {
            var bmi = document.getElementById("divBMI");
            bmi.style.display = "block";
        }        
    </script>
</telerik:RadCodeBlock></head>
<body>
    <form id="frmCrewMainPersonel" runat="server" autocomplete="off">
    <ajaxToolkit:ToolkitScriptManager ID="ToolkitScriptManager1" runat="server" CombineScripts="false">
    </ajaxToolkit:ToolkitScriptManager>
    <div class="navigation" id="navigation" style="top: 0px; margin-left: 0px; vertical-align: top;
        width: 100%">
        <eluc:Error ID="ucError" runat="server" Text="" Visible="false"></eluc:Error>
        <div class="subHeader">
            <eluc:Title runat="server" ID="ttlCrewMainPersonel" Text="Crew Personal" ShowMenu="false">
            </eluc:Title>
        </div>        
        <table cellpadding="2" cellspacing="2" width="100%">
             <tr>
                <td colspan="2">
                  <a id="aCrewImg" runat="server">
                        <asp:Image ID="imgPhoto" runat="server" Height="100px" ImageUrl="<%$ PhoenixTheme:images/Blank.png %>"
                        Width="120px" /></a>              
                </td>
                <td>
                    <table>
                        <tr>
                            <td>
                                <asp:Literal ID="lblFirstName" runat="server" Text="First Name"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblMiddleName" runat="server" Text="Middle Name"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblLastName" runat="server" Text="Last Name"></asp:Literal>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Literal ID="lblCreatedBy" runat="server" Text="Created By"></asp:Literal>
                            </td>
                        </tr>
                    </table>
                </td>
                <td colspan="3">
                    <table>
                        <tr>
                            <td>
                                 <asp:TextBox ID="txtFirstname" runat="server" CssClass="input readonlytextbox"  ReadOnly="true" MaxLength="200"></asp:TextBox>                                     
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtMiddlename" runat="server" CssClass="readonlytextbox" ReadOnly="true" MaxLength="200"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtLastname" runat="server" MaxLength="200" Width="150px" CssClass="readonlytextbox" ReadOnly="true" ></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtCreatedBy" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6">
                <hr />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblEmployeeFileNo" runat="server" Text="Employee/File Number"></asp:Literal>                    
                </td>
                <td>
                    <asp:TextBox ID="txtEmployeeCode" runat="server" CssClass="readonlytextbox" ReadOnly="true"  Width="150px"></asp:TextBox>                    
                </td>
               <%-- <td>
                    <asp:Literal ID="lblStatus" runat="server" Text="Status"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtEmployeeStatus" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>--%>
                <td>
                    <asp:Literal ID="lblCivilStatus" runat="server" Text="Civil Status"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtMaritialStatus" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox> 
                </td>
                 <td>
                    <asp:Literal ID="lblRecrOffZone" runat="server" Text="Recr. Off.(Zone)"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtZone" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>                       
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblRankApplied" runat="server" Text="Rank Applied"></asp:Literal>
                </td>
                <td>
                   <asp:TextBox ID="txtRankApplied" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>                    
                </td>
                <td>
                   <asp:Literal ID="lblAppliedOn" runat="server" Text=" Applied On"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtAppliedOn" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblRankPosted" runat="server" Text="Rank Posted"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtRankPosted" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblLastOnboardVessel" runat="server" Text="Last/Onboard Vessel"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtLastVessel" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblDtFirstJoin" runat="server" Text="First Join Date"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtDateofJoin" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px" />
                </td>
               <td>                  
                    <asp:Literal ID="lblINDOSNumber" runat="server" Text="INDOS Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtINDOsNumber"  runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>                    
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblPlaceofBirth" runat="server" Text="Place of Birth"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPlaceofBirth" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblDateofBirth" runat="server" Text="Date of Birth"></asp:Literal>
                </td>
                <td>
                    <eluc:Date ID="txtDateofBirth" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px" />
                </td>
                <td>
                    <asp:Literal ID="lblAge" runat="server" Text="Age"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtAge" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblNationality" runat="server" Text="Nationality"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtNationality" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>  
                </td>
                <td>
                    <asp:Literal ID="lblPassportNumber" runat="server" Text="Passport Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtPassport" runat="server" MaxLength="200" CssClass="readonlytextbox"
                        ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblSeamans" runat="server" Text="Seaman's Book Number"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtSeamenBookNumber" runat="server" MaxLength="200" CssClass="readonlytextbox"
                        ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
            </tr>
             <tr>
                <td>
                    <asp:Literal ID="lblGender" runat="server" Text="Gender"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtGender" runat="server" CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblHeight" runat="server" Text="Height(Cms)"></asp:Literal>
                </td>
                <td>
                   <asp:TextBox ID="txtHeight" runat="server"  CssClass="readonlytextbox txtNumber" ReadOnly="true" Width="150px"></asp:TextBox>                       
                </td>
                <td>
                    <asp:Literal ID="lblWeight" runat="server" Text="Weight(Kg)"></asp:Literal>
                </td>
                <td>
                  <asp:TextBox ID="txtWeight" runat="server"  CssClass="readonlytextbox txtNumber" ReadOnly="true" Width="150px"></asp:TextBox>                    
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblHairColour" runat="server" Text="Hair Colour"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtHairColor" CssClass="readonlytextbox" ReadOnly="true" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblEyeColour" runat="server" text="Eye Colour"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtEyeColor" CssClass="readonlytextbox" ReadOnly="true" runat="server" MaxLength="50" Width="150px"></asp:TextBox>
                </td>
                <td>
                    <asp:Literal ID="lblDistinguishMark" runat="server" Text="Distinguish Mark"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtDistinguishMark" CssClass="readonlytextbox" ReadOnly="true" runat="server" MaxLength="200" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Literal ID="lblShoeSize" runat="server" Text="Shoe Size(Inch)"></asp:Literal>
                </td>
                <td>
                   <asp:TextBox ID="txtShoes" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>                    
                </td>
                <td>
                    <asp:Literal ID="lblShirtSize" runat="server" Text="Shirt Size"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtShirt" runat="server"  CssClass="readonlytextbox txtNumber" ReadOnly="true" Width="150px"></asp:TextBox>                    
                </td>
                <td>
                    <asp:Literal ID="lblBMI" runat="server" Text="BMI"></asp:Literal>
                </td>
                <td>
                    <asp:TextBox ID="txtBMI" runat="server" ReadOnly="true" CssClass="readonlytextbox"
                        MaxLength="4" Width="150px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                
                <td>
                    <asp:Literal ID="lblPool" runat="server" Text="Pool"></asp:Literal>
                </td>  
                <td>
                    <asp:TextBox ID="txtPool" runat="server"  CssClass="readonlytextbox" ReadOnly="true" Width="150px"></asp:TextBox>                       
                </td> 
                <td colspan="4"></td>                                               
            </tr>           
        </table>                
    </div>
    </form>
</body>
</html>

