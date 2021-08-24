<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTechnicalAuxiliaryPerformanceLastOverhaul.aspx.cs" 
    Inherits="DashboardTechnicalAuxiliaryPerformanceLastOverhaul" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">
     
    seriesData =<%=this.overhaulData%>;
    seriesData2 =<%=this.seriesData%>;
    dateseries = <%=this.dateseries%>;

</script>
</telerik:RadCodeBlock>