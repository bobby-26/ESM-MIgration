<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControITStatusList.ascx.cs" Inherits="UserControITStatusList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<script type="text/javascript">

    // Maintain scroll position on list box. 
    var xPos, yPos; 
    var prm = Sys.WebForms.PageRequestManager.getInstance(); 

    function setITStatusScroll()
    {
        var div = $get('<%=divCheckboxList.ClientID %>');
        var hdn = $get('<%= hdnScrollITStatus.ClientID %>');
        hdn.value = div.scrollTop;
    }

    function BeginRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollITStatus.ClientID %>');
        
        if (listBox != null) 
        { 
            xPos = listBox.scrollLeft; 
            yPos = listBox.scrollTop; 
        } 
    } 

    function EndRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollITStatus.ClientID %>');
        listBox.scrollTop = hdn.value; 
    } 

    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler); 


</script>

<div runat="server" id="divCheckboxList" class="input" style="overflow: auto;height: 140px" onscroll="javascript:setITStatusScroll();">
    <asp:HiddenField ID="hdnScrollITStatus" runat="server" />
    <telerik:RadLabel ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></telerik:RadLabel>
     <telerik:RadCheckBoxList RenderMode="Lightweight" ID="UcITStatusList" runat="server"  OnSelectedIndexChanged="UcITStatusList_SelectedIndexChanged"
         DataTextField="FLDSTATUSNAME"   DataValueField="FLDSTATUSID" OnDataBound="UcITStatusList_DataBound" Height="40px">
    </telerik:RadCheckBoxList>    
</div>
