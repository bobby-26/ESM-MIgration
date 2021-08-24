<%@ Control Language="C#" AutoEventWireup="true" CodeFile="UserControlPromotionEvaluationItem.ascx.cs" Inherits="UserControlPromotionEvaluationItem" %>


<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<telerik:RadComboBox DropDownPosition="Static" ID="ddlPromotionQuestion" runat="server" DataTextField="FLDPROMOTIONQUESTION" DataValueField="FLDPROMOTIONQUESTIONID" EnableLoadOnDemand="True"
    OnDataBound="ddlPromotionQuestion_DataBound"  OnTextChanged="ddlPromotionQuestion_TextChanged" EmptyMessage="Type to select Promotion Evaluation Item" Filter="Contains" MarkFirstMatch="true">
</telerik:RadComboBox>
