<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlSEPBugModuleList.ascx.cs" Inherits="UserControlSEPBugModuleList" %> 
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">

    // Maintain scroll position on list box. 
    var xPos, yPos; 
    var prm = Sys.WebForms.PageRequestManager.getInstance(); 

    function setModuleScroll()
    {
        var div = $get('<%=divCheckboxList.ClientID %>');
        var hdn = $get('<%= hdnScrollModule.ClientID %>');
        hdn.value = div.scrollTop;
    }

    function BeginRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollModule.ClientID %>');
        
        if (listBox != null) 
        { 
            xPos = listBox.scrollLeft; 
            yPos = listBox.scrollTop; 
        } 
    } 

    function EndRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollModule.ClientID %>');
        listBox.scrollTop = hdn.value; 
    } 

    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler); 


</script>
<div runat="server" id="divCheckboxList" class="input" style="overflow: auto;height: 170px" onscroll="javascript:setModuleScroll();">
    <asp:HiddenField ID="hdnScrollModule" runat="server" />
    <telerik:RadLabel ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></telerik:RadLabel>
     <telerik:RadListBox ID="UcSEP" RenderMode="Lightweight" runat="server"  OnSelectedIndexChanged="UcSEP_SelectedIndexChanged"
         DataTextField="FLDMODULENAME"   DataValueField="FLDMODULEID" OnDataBound="UcSEP_DataBound" Height="40px">  <%--RepeatColumns="2"--%>
    </telerik:RadListBox>    
</div>