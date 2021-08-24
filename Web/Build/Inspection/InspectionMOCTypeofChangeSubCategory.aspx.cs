using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using Telerik.Web.UI;

public partial class InspectionMOCTypeofChangeSubCategory : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionMOCTypeofChangeSubCategory.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvMOCSubCategory')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCTypeofChangeSubCategoryAdd.aspx?categoryid=" + ddlCategory.SelectedValue + "')", "Add", "<i class=\"fa fa-plus-circle\"></i>", "ADD");

            MenuMOCSubCategory.AccessRights = this.ViewState;
            MenuMOCSubCategory.MenuList = toolbar.Show();

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
                cmdHiddenSubmit.Attributes.Add("style", "visibility:hidden");
                BindMOCCategory();
            }
            //BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    protected void ShowExcel()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;

        DataSet ds = new DataSet();

        string[] alColumns = { "FLDSHORTCODE", "FLDMOCSUBCATEGORYNAME", "FLDPROPOSERROLE", "FLDPERMANENTAPPROVERROLE", "FLDTEMPORARYAPPROVERROLE" ,"FLDRESPONSIBLEPERSONROLE" };
        string[] alCaptions = { "Code", "SubCategory", "Proposer Level", "Approval Level 1", "Approval Level 2", "Responsible Person" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionMOC.SubCategoryProposalApprovalSearch(General.GetNullableGuid(ddlCategory.SelectedValue), sortexpression, sortdirection,
            1,
           iRowCount,
            ref iRowCount,
            ref iTotalPageCount);


        Response.AddHeader("Content-Disposition", "attachment; filename=MOCSubCategory.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>MOC SubCategory</h3></td>");
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

    protected void MOCSubCategory_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;

            if (CommandName.ToUpper().Equals("FIND"))
            {

                ViewState["PAGENUMBER"] = 1;
                BindData();
                gvMOCSubCategory.Rebind();
            }
            else if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
            }
            else if (CommandName.ToUpper().Equals("CLEAR"))
            {
                BindData();
            }
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    private void BindData()
    {
        int iRowCount = 0;
        int iTotalPageCount = 0;
        string[] alColumns = { "FLDSHORTCODE", "FLDMOCSUBCATEGORYNAME", "FLDPROPOSERROLE", "FLDPERMANENTAPPROVERROLE", "FLDTEMPORARYAPPROVERROLE", "FLDRESPONSIBLEPERSONROLE" };
        string[] alCaptions = { "Code", "SubCategory", "Proposer Level", "Approval Level 1", "Approval Level 2", "Responsible Person" };

        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;

        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());


        DataSet ds = PhoenixInspectionMOC.SubCategoryProposalApprovalSearch(General.GetNullableGuid(ddlCategory.SelectedValue), sortexpression, sortdirection,
           (int)ViewState["PAGENUMBER"],
           gvMOCSubCategory.PageSize,
           ref iRowCount,
           ref iTotalPageCount);


        General.SetPrintOptions("gvMOCSubCategory", "MOC Subcategory", alCaptions, alColumns, ds);

        gvMOCSubCategory.DataSource = ds;
        gvMOCSubCategory.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }
    protected void gvMOCSubCategory_Sorting(object sender, GridSortCommandEventArgs se)
    {

        ViewState["SORTEXPRESSION"] = se.SortExpression;

        if (ViewState["SORTDIRECTION"] != null && ViewState["SORTDIRECTION"].ToString() == "0")
            ViewState["SORTDIRECTION"] = 1;
        else
            ViewState["SORTDIRECTION"] = 0;

        BindData();
    }
    protected void gvMOCSubCategory_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixInspectionMOC.MOCSubCategoryDelete(new Guid(((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()));
                BindData();
                gvMOCSubCategory.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("PAGE"))
                ViewState["PAGENUMBER"] = null;

        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;

        }
    }
    protected void gvMOCSubCategory_ItemDataBound(Object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            LinkButton del = (LinkButton)e.Item.FindControl("cmdDelete");
            if (del != null) del.Visible = SessionUtil.CanAccess(this.ViewState, del.CommandName);

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadLabel lblSubCategory = (RadLabel)e.Item.FindControl("lblSubCategory");
            UserControlToolTip uct = (UserControlToolTip)e.Item.FindControl("SubCategory");
            if (uct != null)
            {
                uct.Position = ToolTipPosition.TopCenter;
                uct.TargetControlId = lblSubCategory.ClientID;
            }

            if (eb != null)
            {
                eb.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionMOCTypeofChangeSubCategoryEdit.aspx?mocsubcategoryid=" + (((RadLabel)e.Item.FindControl("lblSubCategoryId")).Text.ToString()) + "');return false;");
            }
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");

            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
            }
            RadLabel lblProposerRole1 = ((RadLabel)e.Item.FindControl("lblProposerRole"));

            RadLabel lblTempApproverRole1 = ((RadLabel)e.Item.FindControl("lblTempApproverRole"));

            RadLabel lblPermanantApproverRole1 = ((RadLabel)e.Item.FindControl("lblPermanantApproverRole"));

            RadLabel lblResponsiblePerson1 = ((RadLabel)e.Item.FindControl("lblresponsiblepersonrole"));

            LinkButton proposer = (LinkButton)e.Item.FindControl("cmdProposerMoreInfo");
            LinkButton TempApprover = (LinkButton)e.Item.FindControl("cmdTempApproverMoreInfo");
            LinkButton PermanantApprover = (LinkButton)e.Item.FindControl("cmdPermanantApproverMoreInfo");
            LinkButton responsibleperson = (LinkButton)e.Item.FindControl("cmdresponsiblepersonmoreinfo");

            RadLabel proposer2 = (RadLabel)e.Item.FindControl("lblProposer");
            RadLabel TempApprover2 = (RadLabel)e.Item.FindControl("lblTempApprover");
            RadLabel PermanantApprover2 = (RadLabel)e.Item.FindControl("lblPermanantApprover");
            RadLabel ResponsiblePerson2 = (RadLabel)e.Item.FindControl("lblResponsiblePerson");

            if (proposer != null)
            {
                if (proposer2.Text == "N/A")
                    proposer.Visible = false;
                proposer.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMOCProposerApproverUserList.aspx?ApproverRoleId=" + lblProposerRole1.Text + "'); return true;");
            }

            if (TempApprover != null)
            {
                if (TempApprover2.Text == "N/A")
                    TempApprover.Visible = false;
                TempApprover.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMOCProposerApproverUserList.aspx?ApproverRoleId=" + lblTempApproverRole1.Text + "'); return true;");
            }
            if (PermanantApprover != null)
            {
                if (PermanantApprover2.Text == "N/A")
                    PermanantApprover.Visible = false;
                PermanantApprover.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMOCProposerApproverUserList.aspx?ApproverRoleId=" + lblPermanantApproverRole1.Text + "'); return true;");
            }

            if (responsibleperson != null)
            {
                if (ResponsiblePerson2.Text == "N/A")
                    responsibleperson.Visible = false;
                responsibleperson.Attributes.Add("onclick", "javascript:openNewWindow('codehelp1','','" + Session["sitepath"] + "/Inspection/InspectionMOCProposerApproverUserList.aspx?ApproverRoleId=" + lblResponsiblePerson1.Text + "'); return true;");
            }

            RadComboBox category = (RadComboBox)e.Item.FindControl("ddlCategoryEdit");
            RadLabel categoryid = (RadLabel)e.Item.FindControl("lblCategoryIdEdit");
            if (category != null && categoryid != null)
            {
                category.DataSource = PhoenixInspectionMOCCategory.MOCCategoryList(1);
                category.DataBind();
                category.Items.Insert(0, new RadComboBoxItem("--Select--", "Dummy"));
                category.SelectedValue = categoryid.Text.ToString();
            }
        }
    }
    private bool IsValidCountry(string category, string shortcode, string subcategory)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        if (General.GetNullableGuid(category) == null)
            ucError.ErrorMessage = "Category is required.";

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Code is required.";

        if (subcategory.Trim().Equals(""))
            ucError.ErrorMessage = "SubCategory is required.";

        return (!ucError.IsError);
    }

    protected void BindMOCCategory()
    {
        ddlCategory.DataSource = PhoenixInspectionMOCCategory.MOCCategoryList(PhoenixSecurityContext.CurrentSecurityContext.UserCode);
        ddlCategory.DataTextField = "FLDMOCCATEGORYNAME";
        ddlCategory.DataValueField = "FLDMOCCATEGORYID";
        ddlCategory.DataBind();
    }
    protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
    {
        BindData();
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        BindData();
    }

    protected void gvMOCSubCategory_NeedDataSource(object sender, Telerik.Web.UI.GridNeedDataSourceEventArgs e)
    {
        try
        {
            ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvMOCSubCategory.CurrentPageIndex + 1;
            BindData();
        }
        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }
}
