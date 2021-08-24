<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlITCategoryList.ascx.cs" Inherits="UserControlITCategoryList" %>
<script type="text/javascript">

    // Maintain scroll position on list box. 
    var xPos, yPos; 
    var prm = Sys.WebForms.PageRequestManager.getInstance(); 

    function setITCategoryScroll()
    {
        var div = $get('<%=divCheckboxList.ClientID %>');
        var hdn = $get('<%= hdnScrollITCategory.ClientID %>');
        hdn.value = div.scrollTop;
    }

    function BeginRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollITCategory.ClientID %>');
        
        if (listBox != null) 
        { 
            xPos = listBox.scrollLeft; 
            yPos = listBox.scrollTop; 
        } 
    } 

    function EndRequestHandler(sender, args) 
    { 
        var listBox = $get('<%= divCheckboxList.ClientID %>'); 
        var hdn = $get('<%= hdnScrollITCategory.ClientID %>');
        listBox.scrollTop = hdn.value; 
    } 

    prm.add_beginRequest(BeginRequestHandler);
    prm.add_endRequest(EndRequestHandler); 


</script>
<div runat="server" id="divCheckboxList" class="input" style="overflow: auto;height: 140px" onscroll="javascript:setITCategoryScroll();">
    <asp:HiddenField ID="hdnScrollITCategory" runat="server" />
    <asp:Label ID="lblCheckboxList" runat="server" Text="Label" Visible="false"></asp:Label>
     <asp:CheckBoxList ID="UcITCategory" runat="server"  OnSelectedIndexChanged="UcITCategory_SelectedIndexChanged"
         DataTextField="FLDNAME"   DataValueField="FLDCATEGORYID" OnDataBound="UcITCategory_DataBound" Height="40px">
    </asp:CheckBoxList>    
</div>