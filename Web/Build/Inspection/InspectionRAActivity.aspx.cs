using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Inspection;
using System;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Telerik.Web.UI;


public partial class InspectionRAActivity : PhoenixBasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SessionUtil.PageAccessRights(this.ViewState);
            PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAActivity.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvRAActivity')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Inspection/InspectionRAActivity.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            //toolbar.AddLinkButton("javascript:Openpopup('Copy','','../Inspection/InspectionRAActivityDMSCategoryMapping.aspx?',null);return true;", "Map DMS Category", "checklist.png", "Map");

            MenuRAActivity.AccessRights = this.ViewState;
            MenuRAActivity.MenuList = toolbar.Show();

            ViewState["COMPANYID"] = "";
            NameValueCollection nvc = PhoenixSecurityContext.CurrentSecurityContext.CompanyContext;
            if (nvc.Get("QMS") != null && nvc.Get("QMS") != "")
                ViewState["COMPANYID"] = nvc.Get("QMS");

            if (!IsPostBack)
            {
                ViewState["PAGENUMBER"] = 1;
                ViewState["SORTEXPRESSION"] = null;
                ViewState["SORTDIRECTION"] = null;
            }
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
        string[] alColumns = { "FLDCOMPANYNAME", "FLDNAME", "FLDTYPELIST" };
        string[] alCaptions = { "Company", "Name", "Element" };
        string sortexpression;
        int? sortdirection = null;

        sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
        if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
            iRowCount = 10;
        else
            iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

        ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivitySearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ucCategory.SelectedCategory),
            sortexpression, sortdirection,
            1,
            iRowCount,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ViewState["COMPANYID"].ToString()));
        Response.AddHeader("Content-Disposition", "attachment; filename=Activity.xls");
        Response.ContentType = "application/vnd.msexcel";
        Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
        Response.Write("<tr>");
        Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
        Response.Write("<td><h3>Activity</h3></td>");
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

    protected void RAActivity_TabStripCommand(object sender, EventArgs e)
    {
        try
        {
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
            {
                ViewState["PAGENUMBER"] = 1;
                Rebind();
            }
            if (CommandName.ToUpper().Equals("EXCEL"))
            {
                ShowExcel();
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
        string[] alColumns = { "FLDCOMPANYNAME", "FLDNAME", "FLDTYPELIST" };
        string[] alCaptions = { "Company", "Name", "Element" };
        string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
        int? sortdirection = null;
        DataSet ds = new DataSet();
        if (ViewState["SORTDIRECTION"] != null)
            sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

        ds = PhoenixInspectionRiskAssessmentActivity.RiskAssessmentActivitySearch(General.GetNullableString(txtName.Text),
            General.GetNullableInteger(ucCategory.SelectedCategory),
            sortexpression, sortdirection,
            Int32.Parse(ViewState["PAGENUMBER"].ToString()),
            gvRAActivity.PageSize,
            ref iRowCount,
            ref iTotalPageCount,
            General.GetNullableInteger(ViewState["COMPANYID"].ToString()));


        General.SetPrintOptions("gvRAActivity", "Activity", alCaptions, alColumns, ds);

        gvRAActivity.DataSource = ds;
        gvRAActivity.VirtualItemCount = iRowCount;

        ViewState["ROWCOUNT"] = iRowCount;
        ViewState["TOTALPAGECOUNT"] = iTotalPageCount;
    }

    protected void cmdHiddenSubmit_Click(object sender, EventArgs e)
    {
        ViewState["PAGENUMBER"] = 1;
        gvRAActivity.Rebind();
    }

    protected void Rebind()
    {
        gvRAActivity.SelectedIndexes.Clear();
        gvRAActivity.EditIndexes.Clear();
        gvRAActivity.DataSource = null;
        gvRAActivity.Rebind();
    }

    protected void gvRAActivity_ItemCommand(object sender, GridCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidRAActivity(((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                        ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                        ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                InsertRAActivity(
                    ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text,
                    ((RadTextBox)e.Item.FindControl("txtShortCodeAdd")).Text,
                    ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                );
                Rebind();
                ((RadTextBox)e.Item.FindControl("txtNameAdd")).Focus();
            }
            if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidRAActivity(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                           ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                           ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany))
                {
                    ucError.Visible = true;
                    return;
                }
                UpdateRAActivity(
                        Int32.Parse(((RadLabel)e.Item.FindControl("lblActivityIdEdit")).Text),
                         ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
                            ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
                            ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
                     );
                Rebind();
            }
            if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                DeleteRAActivity(Int32.Parse(((RadLabel)e.Item.FindControl("lblActivityId")).Text));
                Rebind();
            }
            else if(e.CommandName == "Page")
            {
                ViewState["PAGENUMBER"] = null;
            }
        }

        catch (Exception ex)
        {
            ucError.ErrorMessage = ex.Message;
            ucError.Visible = true;
        }
    }

    //protected void gvRAActivity_RowUpdating(object sender, GridViewUpdateEventArgs e)
    //{
    //	try
    //	{
    //		GridView _gridView = (GridView)sender;
    //		int nCurrentRow = e.RowIndex;

    //		if (!IsValidRAActivity(((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
    //                       ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
    //                       ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany))
    //		{
    //			ucError.Visible = true;
    //			return;
    //		}
    //		UpdateRAActivity(
    //				Int32.Parse(((RadLabel)e.Item.FindControl("lblActivityIdEdit")).Text),
    //				 ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text,
    //                    ((RadTextBox)e.Item.FindControl("txtShortCodeEdit")).Text,
    //                    ((UserControlCompany)e.Item.FindControl("ucCompanyAdd")).SelectedCompany
    //			 );
    //		_gridView.EditIndex = -1;
    //		BindData();
    //		SetPageNavigator();
    //	}
    //	catch (Exception ex)
    //	{
    //		ucError.ErrorMessage = ex.Message;
    //		ucError.Visible = true;
    //	}
    //}
    protected void gvRAActivity_ItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridEditableItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }
            UserControlCompany ucCompanyEdit = (UserControlCompany)e.Item.FindControl("ucCompanyEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucCompanyEdit != null)
            {
                if (drv["FLDCOMPANYID"] != null && drv["FLDCOMPANYID"].ToString() != "")
                    ucCompanyEdit.SelectedCompany = drv["FLDCOMPANYID"].ToString();
                else
                    ucCompanyEdit.SelectedCompany = ViewState["COMPANYID"].ToString();
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            LinkButton cmdMap = (LinkButton)e.Item.FindControl("cmdTypeMapping");
            if (cmdMap != null) cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);

            if (cmdMap != null)
            {
                cmdMap.Visible = SessionUtil.CanAccess(this.ViewState, cmdMap.CommandName);
                cmdMap.Attributes.Add("onclick", "openNewWindow('codehelp1', '', '" + Session["sitepath"] + "/Inspection/InspectionRACategoryTypeMapping.aspx?categoryid=" + drv["FLDACTIVITYID"].ToString() + "&category=" + drv["FLDNAME"].ToString() + "');return false;");
            }

            UserControlRACategory ucCategory = (UserControlRACategory)e.Item.FindControl("ucCategoryEdit");
            //DataRowView drv = (DataRowView)e.Row.DataItem;
            if (ucCategory != null) ucCategory.SelectedCategory = drv["FLDCATEGORYID"].ToString();

            RadLabel lblType = (RadLabel)e.Item.FindControl("lblType");
            if (lblType != null)
            {
                UserControlToolTip ucToolTipType = (UserControlToolTip)e.Item.FindControl("ucToolTipType");
                ucToolTipType.Position = ToolTipPosition.TopCenter;
                ucToolTipType.TargetControlId = lblType.ClientID;
                //lblType.Attributes.Add("onmouseover", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'visible');");
                //lblType.Attributes.Add("onmouseout", "showTooltip(ev,'" + ucToolTipType.ToolTip + "', 'hidden');");
            }
        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }

            UserControlCompany ucCompanyAdd = (UserControlCompany)e.Item.FindControl("ucCompanyAdd");
            if (ucCompanyAdd != null)
                ucCompanyAdd.SelectedCompany = ViewState["COMPANYID"].ToString();
        }
    }


    private void InsertRAActivity(string name, string shortcode, string companyid)
    {
        try
        {
            PhoenixInspectionRiskAssessmentActivity.InsertRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                  name, shortcode, General.GetNullableInteger(companyid));
        }
        catch (Exception e)
        {
            ucError.ErrorMessage = e.Message;
            ucError.Visible = true;
            return;
        }
    }

    private void UpdateRAActivity(int Activityid, string name, string shortcode, string companyid)
    {
        try
        {
            PhoenixInspectionRiskAssessmentActivity.UpdateRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                 Activityid, name, shortcode, General.GetNullableInteger(companyid));
            ucStatus.Text = "Information updated";
        }
        catch (Exception e)
        {
            ucError.ErrorMessage = e.Message;
            ucError.Visible = true;
            return;
        }
    }

    private bool IsValidRAActivity(string name, string shortcode, string companyid)
    {
        ucError.HeaderMessage = "Please provide the following required information";

        RadGrid _gridView = gvRAActivity;

        if (shortcode.Trim().Equals(""))
            ucError.ErrorMessage = "Short Code is required.";

        if (name.Trim().Equals(""))
            ucError.ErrorMessage = "Name is required.";
        return (!ucError.IsError);
    }

    private void DeleteRAActivity(int Activityid)
    {
        PhoenixInspectionRiskAssessmentActivity.DeleteRiskAssessmentActivity(PhoenixSecurityContext.CurrentSecurityContext.UserCode, Activityid);
    }

    public StateBag ReturnViewState()
    {
        return ViewState;
    }

    protected void gvRAActivity_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvRAActivity.CurrentPageIndex + 1;
        BindData();
    }
}
