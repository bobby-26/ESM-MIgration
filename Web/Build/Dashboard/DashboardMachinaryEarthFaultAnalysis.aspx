<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardMachinaryEarthFaultAnalysis.aspx.cs" 
    Inherits="DashboardMachinaryEarthFaultAnalysis" %>

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