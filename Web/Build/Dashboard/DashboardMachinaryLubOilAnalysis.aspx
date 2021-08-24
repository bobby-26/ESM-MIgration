<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMachinaryLubOilAnalysis.aspx.cs" 
    Inherits="DashboardMachinaryLubOilAnalysis" %>
<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
<script type="text/javascript">

    var msg = <%=this.responseMsg%>;
    if(msg != 'success')
    {
        eval(msg);
    }
    ChartData = <%=this.seriesdata%>;
    ChartData1 = <%=this.seriesdata1%>;
    dateseries = <%=this.dateList%>;


</script>
    </telerik:RadCodeBlock>