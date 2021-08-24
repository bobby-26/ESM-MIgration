<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformanceSwitchboard.aspx.cs" Inherits="DashboardTechnicalAuxiliaryPerformanceSwitchboard" %>

<%@ Register TagPrefix="eluc" TagName="Error" Src="~/UserControls/UserControlErrorMessage.ascx" %>
<div class="ae02Chart currentAE" id="ae02Chart02">
    <div id="ae02Chart02Graph" style="height: 500px; width: 100%; margin: 0 auto; margin-top: 20px;"></div>
</div>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">

     seriesData= <%=this.SeriesData%>;
    dateseries = <%=this.dateseries%>;



</script>
    </telerik:RadCodeBlock>
