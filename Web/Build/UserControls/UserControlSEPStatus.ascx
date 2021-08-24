<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSEPStatus.ascx.cs" Inherits="UserControlSEPStatus" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">

    // Maintain scroll position on list box. 
    var xPos, yPos; 
    var prm = Sys.WebForms.PageRequestManager.getInstance(); 

    function setStatusScroll(divp, hdnp)
    {        
        var div = $get(divp);
        var hdn = $get(hdnp);
        hdn.value = div.scrollTop;
    }

    function EndRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= this.divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= this.hdnStatusScroll.ClientID %>');
        listBox.scrollTop = hdn.value; 
    } 

    prm.add_endRequest(EndRequestHandler); 


</script> 
<div runat="server" id="divCheckboxList" class="input" style="overflow: auto; height: 170px">
    <telerik:RadLabel ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></telerik:RadLabel>
    <asp:HiddenField ID="hdnStatusScroll" runat="server" />
     <telerik:RadListBox RenderMode="Lightweight" ID="UcSEP" runat="server"  OnSelectedIndexChanged="UcSEP_SelectedIndexChanged"
         DataTextField="FLDNAME"   DataValueField="FLDID" OnDataBound="UcSEP_DataBound" Height="40px">
    </telerik:RadListBox>    
</div>