using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.DocumentManagement;
using SouthNests.Phoenix.Framework;
using System.Data;
using System.Collections.Specialized;
using System.Web.UI.HtmlControls;
using Telerik.Web.UI;

public partial class DocumentManagementDocumentListByVessel : PhoenixBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            cmdHiddenSubmit.Attributes.Add("style", "display:none;");
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentListByVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvDocumentByVessel')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            if(PhoenixSecurityContext.CurrentSecurityContext.UserCode == 2564) //only for developers login
                toolbar.AddImageButton("javascript:Openpopup('Redistribute','','../DocumentManagement/DocumentManagementRedistribution.aspx',null);return true;", "Redistribution", "checklist.png", "Redistribution");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentList.aspx", "Filter", "search.png", "FIND");
            //toolbar.AddImageButton("../DocumentManagement/DocumentManagementAdminDocumentList.aspx", "Clear Filter", "clear-filter.png", "CLEAR");           
            toolbar.AddButton("Documents / Forms count by Category", "", ToolBarDirection.Right);
            MenuDocumentList.AccessRights = this.ViewState;
            MenuDocumentList.MenuList = toolbar.Show();            

            toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../DocumentManagement/DocumentManagementDocumentListByVessel.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRiskAssessment')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddButton("JHA / Risk Assessments count by Category", "", ToolBarDirection.Right);
            MenuRiskAssessment.AccessRights = this.ViewState;
            MenuRiskAssessment.MenuList = toolbar.Show();

            toolbar = new PhoenixToolbar();
            toolbar.AddButton("Vessel", "VESSEL", ToolBarDirection.Right);
            toolbar.AddButton("General Distribution", "GENERAL", ToolBarDirection.Right);
            toolbar.AddButton("Bulk Distribution", "BULK", ToolBarDirection.Right);

            MenuDocument.AccessRights = this.ViewState;
            MenuDocument.MenuList = toolbar.Show();
            MenuDocument.SelectedMenuIndex = 0;

            if (!IsPostBack)
            {

                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                ViewState["CATEGORYID"] = "";
                ViewState["ACTIVITYID"] = "";

            }

            if (General.GetNullableInteger(ucVessel.SelectedVessel) != null)
                lblVessel.Text = "Total Documents distributed to " + ucVessel.SelectedVesselName + "  : ";
            

            //RadLabel lblDocumentsTotal = (RadLabel)gvDocumentByVessel.FooterItem.FindControl("lblTotal");
            //RadLabel lblRATotal = (Label)gvRiskAssessment.FooterRow.FindControl("lblTotal");

            //if (lblDocumentsTotal != null && General.GetNullableInteger(lblDocumentsTotal.Text) != null)
            //    lblGrandTotal.Text = lblDocumentsTotal.Text;

            //if (lblRATotal != null && General.GetNullableInteger(lblRATotal.Text) != null)
            //    lblGrandTotal.Text = (General.GetNullableInteger(lblGrandTotal.Text) + General.GetNullableInteger(lblRATotal.Text)).ToString();

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void ShowExcel()
    {  
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTCOUNT" };
        string[] alCaptions = { "Category", "Count" };   

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        ds = PhoenixDocumentManagementDocument.DistributedDocumentByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                             );

        Response.AddHeader("Content-Disposition", "attachment; filename=DocumentorFormsByCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Document/Forms by Category</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuDocumentList_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                BindData();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                Filter.CurrentDMSDocumentFilter = null;
                ViewState["CATEGORYID"] = "";
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }    

    protected void MenuDocument_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("BULK"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentBulkDistribution.aspx?");
            }
            if (CommandName.ToUpper().Equals("GENERAL"))
            {
                Response.Redirect("../DocumentManagement/DocumentManagementDocumentDistribution.aspx?");
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvDocumentByVessel_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindData();
    }

    private void BindData()
    {

        string[] alColumns = { "FLDCATEGORYNAME", "FLDDOCUMENTCOUNT" };
        string[] alCaptions = { "Category", "Count" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        DataSet ds = new DataSet();

        int companyid = PhoenixSecurityContext.CurrentSecurityContext.UserCode;

        ds = PhoenixDocumentManagementDocument.DistributedDocumentByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                             );

        General.SetPrintOptions("gvDocumentByVessel", "Documents", alCaptions, alColumns, ds);
        gvDocumentByVessel.DataSource = ds;
        if (ds.Tables[0].Rows.Count > 0)
        {
            if (General.GetNullableGuid(ViewState["CATEGORYID"].ToString()) == null)
            {
                ViewState["CATEGORYID"] = ds.Tables[0].Rows[0]["FLDCATEGORYID"].ToString();                
            }                    
        }

    }

    protected void gvDocumentByVessel_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;
            int nCurrentRow = e.Item.ItemIndex;
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    
    protected void gvDocumentByVessel_ItemDataBound(Object sender, Telerik.Web.UI.GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            RadLabel lblCategoryId = (RadLabel)e.Item.FindControl("lblCategoryId");
            
            LinkButton lbtn = (LinkButton)e.Item.FindControl("lnkDocumentCount");
            lbtn.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedDocumentList.aspx?categoryid=" + lblCategoryId.Text
                                                                                                                        + "&vesselid=" + ucVessel.SelectedVessel + "&companyid=" + PhoenixSecurityContext.CurrentSecurityContext.CompanyID + "'); return false;");

        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblTotal = (RadLabel)e.Item.FindControl("lblTotal");

            DataSet ds = PhoenixDocumentManagementDocument.DistributedDocumentByCategory(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                   );
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];                
                if (lblTotal != null)
                    lblTotal.Text = dr["FLDGRANDTOTAL"].ToString();
            }

            RadLabel lblDocumentsTotal = (RadLabel)e.Item.FindControl("lblTotal");
            

            if (lblDocumentsTotal != null && General.GetNullableInteger(lblDocumentsTotal.Text) != null)
                lblGrandTotal.Text = lblDocumentsTotal.Text;

        }
    }    

    private bool IsValidDocument(string documentname, string categoryid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableString(documentname) == null)
            ucError.ErrorMessage = "Document name is required.";

        if (General.GetNullableGuid(categoryid) == null)
            ucError.ErrorMessage = "Document category is required.";

        return (!ucError.IsError);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }   

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        BindData();

    }

    protected void ucVessel_changed(object sender, EventArgs e)
    {
        BindData();
        string script = "resizeDiv(divDocument);resizeDiv(divRiskAssessment);\r\n;";
        ScriptManager.RegisterStartupScript(this, typeof(Page), "BookmarkScript", script, true);
    }


    protected void gvRiskAssessment_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        BindRiskAssessment();
    }

    private void BindRiskAssessment()
    {
        try
        {
            string[] alColumns = { "FLDNAME", "FLDJHACOUNT", "FLDPROCESSCOUNT", "FLDGENERICCOUNT", "FLDMACHINERYCOUNT", "FLDNAVIGATIONCOUNT", "FLDCARGOCOUNT", "FLDTOTAL" };
            string[] alCaptions = { "Activity", "JHA", "Process", "Generic", "Machinery", "Navigation", "Cargo", "Count" };

            int? companyid = PhoenixSecurityContext.CurrentSecurityContext.CompanyID;
            DataSet ds;
            if (rblRA.SelectedValue.Equals("1"))
            {
                ds = PheonixDocumentManagementDistributionExtn.DistributedRAByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                );
            }
            else
            {
                ds = PhoenixDocumentManagementDocument.DistributedRAByCategory(
                                                                   PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                 , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                 , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                );
            }

            General.SetPrintOptions("gvRiskAssessment", "JHA/RA Count by Category", alCaptions, alColumns, ds);
            gvRiskAssessment.DataSource = ds;
            if (ds.Tables[0].Rows.Count > 0)
            {

                if (General.GetNullableGuid(ViewState["ACTIVITYID"].ToString()) == null)
                {
                    ViewState["ACTIVITYID"] = ds.Tables[0].Rows[0]["FLDACTIVITYID"].ToString();
                    //gvRiskAssessment.SelectedIndex = 0;
                }
                //SetRowSelection();
            }        
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcelRiskAssessment()
    {        
        DataSet ds = new DataSet();

        string[] alColumns = { "FLDNAME", "FLDJHACOUNT", "FLDPROCESSCOUNT", "FLDGENERICCOUNT", "FLDMACHINERYCOUNT", "FLDNAVIGATIONCOUNT", "FLDCARGOCOUNT", "FLDTOTAL" };
        string[] alCaptions = { "Activity", "JHA", "Process", "Generic", "Machinery", "Navigation","Cargo", "Count" };

        ds = PhoenixDocumentManagementDocument.DistributedRAByCategory(
                                                               PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                             , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                             , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                );

        Response.AddHeader("Content-Disposition", "attachment; filename=JHAorRACountByCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>JHA/RA Count by Category</h3></td>");
        Response.Write("<td colspan='" + (alColumns.Length - 2).ToString() + "'>&nbsp;</td>");
        Response.Write("</tr>");
        Response.Write("</TABLE>");
        Response.Write("<br />");
        Response.Write("<TABLE BORDER='1' CELLPADDING='2' CELLSPACING='2' width='100%'>");
        Response.Write("<tr>");
        for (int i = 0; i < alCaptions.Length; i++)
        {
            Response.Write("<td width='20%'>");
            Response.Write("<b>" + alCaptions[i] + "</b>");
            Response.Write("</td>");
        }
        Response.Write("</tr>");
            
        foreach (DataRow dr in ds.Tables[0].Rows)
        {
            Response.Write("<tr>");
            for (int i = 0; i < alColumns.Length; i++)
            {
                Response.Write("<td>");
                Response.Write(dr[alColumns[i]]);
                Response.Write("</td>");

            }
            Response.Write("</tr>");
        }

        //alColumns = new string[7];
        //alColumns[0] = "FLDNAME";
        //alColumns[1] = "FLDJHATOTAL";
        //alColumns[2] = "FLDPROCESSTOTAL";
        //alColumns[3] = "FLDGENERICTOTAL";
        //alColumns[4] = "FLDMACHINERYTOTAL";
        //alColumns[5] = "FLDNAVIGATIONTOTAL";
        //alColumns[6] = "FLDGRANDTOTAL";

        //foreach (DataRow dr in ds.Tables[1].Rows)
        //{
        //    Response.Write("<tr>");
        //    for (int i = 0; i < alColumns.Length; i++)
        //    {
        //        Response.Write("<td>");
        //        Response.Write(dr[alColumns[i]]);
        //        Response.Write("</td>");

        //    }
        //    Response.Write("</tr>");
        //}
        Response.Write("</TABLE>");
        Response.End();
    }

    protected void MenuRiskAssessment_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcelRiskAssessment();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvRiskAssessment_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;

            GridEditableItem eeditedItem = e.Item as GridEditableItem;
            RadGrid _gridView = (RadGrid)sender;

            int nCurrentRow = e.Item.ItemIndex;

            if (e.CommandName.ToUpper().Equals("SELECT"))
            {
                
            }            

        }
        catch (Exception ex)
        {
            ucError.HeaderMessage = "Please make the required correction";
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }

    protected void gvRiskAssessment_ItemDataBound(object sender, Telerik.Web.UI.GridItemEventArgs e)
    {

        if (e.Item is GridEditableItem)
        {
            RadLabel lblAcitivityID = (RadLabel)e.Item.FindControl("lblAcitivityID");

            LinkButton lnkJHACount = (LinkButton)e.Item.FindControl("lnkJHACount");
            lnkJHACount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text
                                                                                                                        + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=1'); return false;");
            LinkButton lnkProcessCount = (LinkButton)e.Item.FindControl("lnkProcessCount");
            lnkProcessCount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text
                                                                                                                        + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=2'); return false;");
            LinkButton lnkGenericCount = (LinkButton)e.Item.FindControl("lnkGenericCount");
            lnkGenericCount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text
                                                                                                                        + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=3'); return false;");
            LinkButton lnkMachineryCount = (LinkButton)e.Item.FindControl("lnkMachineryCount");
            lnkMachineryCount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text
                                                                                                                        + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=4'); return false;");
            LinkButton lnkNavigationCount = (LinkButton)e.Item.FindControl("lnkNavigationCount");
            lnkNavigationCount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=5'); return false;");

            LinkButton lnkCargoCount = (LinkButton)e.Item.FindControl("lnkCargoCount");
            lnkCargoCount.Attributes.Add("onclick", "openNewWindow('DocumentList', '', '" + Session["sitepath"] + "/DocumentManagement/DocumentManagementDistributedJHARAList.aspx?categoryid=" + lblAcitivityID.Text + "&vesselid=" + ucVessel.SelectedVessel + "&NEW=" + rblRA.SelectedValue + "&type=6'); return false;");

        }
        if (e.Item is GridFooterItem)
        {
            RadLabel lblJHATotal = (RadLabel)e.Item.FindControl("lblJHATotal");
            RadLabel lblProcessTotal = (RadLabel)e.Item.FindControl("lblProcessTotal");
            RadLabel lblGenericTotal = (RadLabel)e.Item.FindControl("lblGenericTotal");
            RadLabel lblMachineryTotal = (RadLabel)e.Item.FindControl("lblMachineryTotal");
            RadLabel lblNavigationTotal = (RadLabel)e.Item.FindControl("lblNavigationTotal");
            RadLabel lblCargoTotal = (RadLabel)e.Item.FindControl("lblCargoTotal");
            RadLabel lblTotal = (RadLabel)e.Item.FindControl("lblTotal");
            
            DataSet ds = PhoenixDocumentManagementDocument.DistributedRAByCategory(
                                                                      PhoenixSecurityContext.CurrentSecurityContext.UserCode
                                                                    , PhoenixSecurityContext.CurrentSecurityContext.CompanyID
                                                                    , General.GetNullableInteger(ucVessel.SelectedVessel)
                                                                   );
            if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0)
            {
                DataRow dr = ds.Tables[1].Rows[0];
                if (lblJHATotal != null)
                    lblJHATotal.Text = dr["FLDJHATOTAL"].ToString();
                if (lblProcessTotal != null)
                    lblProcessTotal.Text = dr["FLDPROCESSTOTAL"].ToString();
                if (lblGenericTotal != null)
                    lblGenericTotal.Text = dr["FLDGENERICTOTAL"].ToString();
                if (lblMachineryTotal != null)
                    lblMachineryTotal.Text = dr["FLDMACHINERYTOTAL"].ToString();
                if (lblNavigationTotal != null)
                    lblNavigationTotal.Text = dr["FLDNAVIGATIONTOTAL"].ToString();
                if (lblCargoTotal != null)
                    lblCargoTotal.Text = dr["FLDCARGOTOTAL"].ToString();
                if (lblTotal != null)
                    lblTotal.Text = dr["FLDGRANDTOTAL"].ToString();                
            }

            RadLabel lblRATotal = (RadLabel)e.Item.FindControl("lblTotal");

            if (lblRATotal != null && General.GetNullableInteger(lblRATotal.Text) != null)
                lblGrandTotal.Text = (General.GetNullableInteger(lblGrandTotal.Text) + General.GetNullableInteger(lblRATotal.Text)).ToString();
        }
    }

    protected void rblRA_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvRiskAssessment.Rebind();
    }
}
