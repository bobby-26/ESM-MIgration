<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DashboardTecnicalAEPerformanceHeader.aspx.cs" 
    Inherits="Dashboard_DashboardTecnicalAEPerformanceHeader" %>

<telerik:RadCodeBlock ID="RadCodeBlock2" runat="server">
   
       <div class="titleA">
           <span>Vessel Name: </span><span><%Response.Write(vesselName); %></span><br>
           <span>Type of T/C: </span><span><%Response.Write(TypeofTC); %></span>
       </div>  
       <div class="titleB">
           <span>A/E Model: </span><span><%Response.Write(AeModel);%></span><br>
           <span>A/E No: </span><span><%Response.Write(AENo);%></span>
       </div>
       <div class="titleC">
           <span>Make of A/E:</span><span><%Response.Write(AeMaker); %></span>
       </div>
 </telerik:RadCodeBlock>
