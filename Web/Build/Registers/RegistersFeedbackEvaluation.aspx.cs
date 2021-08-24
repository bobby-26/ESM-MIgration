using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using SouthNests.Phoenix.Framework;
using SouthNests.Phoenix.Registers;
using Telerik.Web.UI;

public partial class RegistersFeedbackEvaluation : PhoenixBasePage
{
    
	protected void Page_Load(object sender, EventArgs e)
	{
		try
		{
			SessionUtil.PageAccessRights(this.ViewState);
			PhoenixToolbar toolbar = new PhoenixToolbar();
            toolbar.AddFontAwesomeButton("../Registers/RegistersFeedbackEvaluation.aspx", "Export to Excel", "<i class=\"fas fa-file-excel\"></i>", "Excel");
            toolbar.AddFontAwesomeButton("javascript:CallPrint('gvFeedbackEvaluation')", "Print Grid", "<i class=\"fas fa-print\"></i>", "PRINT");
            toolbar.AddFontAwesomeButton("../Registers/RegistersFeedbackEvaluation.aspx", "Find", "<i class=\"fas fa-search\"></i>", "FIND");
            
			MenuFeedbackEvaluation.AccessRights = this.ViewState;
			MenuFeedbackEvaluation.MenuList = toolbar.Show();
            toolbar = new PhoenixToolbar();
            MenuTitle.AccessRights = this.ViewState;
            MenuTitle.MenuList = toolbar.Show();
            if (!IsPostBack)
			{
				ViewState["PAGENUMBER"] = 1;
				ViewState["SORTEXPRESSION"] = null;
				ViewState["SORTDIRECTION"] = null;
                gvFeedbackEvaluation.PageSize = int.Parse(PhoenixGeneralSettings.CurrentGeneralSetting.Records);
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
		string[] alColumns = { "FLDTYPENAME", "FLDSHORTCODE", "FLDDESCRIPTION" };
		string[] alCaptions = { "Type","Code", "Name" };
		string sortexpression;
		int? sortdirection = null;

		sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());
		if (ViewState["ROWCOUNT"] == null || Int32.Parse(ViewState["ROWCOUNT"].ToString()) == 0)
			iRowCount = 10;
		else
			iRowCount = Int32.Parse(ViewState["ROWCOUNT"].ToString());

		ds = PhoenixRegistersFeedbackEvaluation.FeedbackEvaluationSearch(null,txtCode.Text, txtName.Text, sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvFeedbackEvaluation.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		Response.AddHeader("Content-Disposition", "attachment; filename=FeedbackEvaluationxls");
		Response.ContentType = "application/vnd.msexcel";
		Response.Write("<TABLE BORDER='0' CELLPADDING='2' CELLSPACING='0' width='100%'>");
		Response.Write("<tr>");
		Response.Write("<td><img src='http://" + Request.Url.Authority + Session["images"] + "/esmlogo4_small.png" + "' /></td>");
		Response.Write("<td><h3> Feedback Evaluation</h3></td>");
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

	protected void FeedbackEvaluation_TabStripCommand(object sender, EventArgs e)
	{
		try
		{
            RadToolBarEventArgs dce = (RadToolBarEventArgs)e;
            string CommandName = ((RadToolBarButton)dce.Item).CommandName;
            if (CommandName.ToUpper().Equals("FIND"))
			{
				ViewState["PAGENUMBER"] = 1;
				BindData();
                gvFeedbackEvaluation.Rebind();
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
		string[] alColumns = { "FLDTYPENAME", "FLDSHORTCODE", "FLDDESCRIPTION" };
		string[] alCaptions = { "Type", "Code", "Name" };
		string sortexpression = (ViewState["SORTEXPRESSION"] == null) ? null : (ViewState["SORTEXPRESSION"].ToString());
		int? sortdirection = null;
		DataSet ds = new DataSet();
		if (ViewState["SORTDIRECTION"] != null)
			sortdirection = Int32.Parse(ViewState["SORTDIRECTION"].ToString());

		ds = PhoenixRegistersFeedbackEvaluation.FeedbackEvaluationSearch(null,txtCode.Text, txtName.Text, sortexpression, sortdirection,
			Int32.Parse(ViewState["PAGENUMBER"].ToString()),
			gvFeedbackEvaluation.PageSize,
			ref iRowCount,
			ref iTotalPageCount);

		General.SetPrintOptions("gvFeedbackEvaluation", "Feedback Evaluation", alCaptions, alColumns, ds);

		if (ds.Tables[0].Rows.Count > 0)
		{
			gvFeedbackEvaluation.DataSource = ds;
            gvFeedbackEvaluation.VirtualItemCount = iRowCount;
		}
		else
		{
            gvFeedbackEvaluation.DataSource = "";
        }
	}

	protected void cmdSearch_Click(object sender, EventArgs e)
	{		
		ViewState["PAGENUMBER"] = 1;
		BindData();
        gvFeedbackEvaluation.Rebind();
	}

	private bool IsValidFeedbackEvalaution(string type,string name)
	{
		ucError.HeaderMessage = "Please provide the following required information";
        
		if (General.GetNullableInteger(type)==null)
			ucError.ErrorMessage = "Type is required.";

		if (name.Trim().Equals(""))
			ucError.ErrorMessage = "Name is required.";

		return (!ucError.IsError);
	}

	public StateBag ReturnViewState()
	{
		return ViewState;
	}

    protected void gvFeedbackEvaluation_ItemCommand(object sender, GridCommandEventArgs e)
    {

        try
        {
            if (e.CommandName.ToUpper().Equals("SORT"))
                return;
         
            if (e.CommandName.ToUpper().Equals("ADD"))
            {
                if (!IsValidFeedbackEvalaution(((RadDropDownList)e.Item.FindControl("ddlTypeAdd")).SelectedValue,
                                        ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersFeedbackEvaluation.InsertFeedbackEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(((RadDropDownList)e.Item.FindControl("ddlTypeAdd")).SelectedValue),
                    ((RadTextBox)e.Item.FindControl("txtCodeAdd")).Text,
                   ((RadTextBox)e.Item.FindControl("txtNameAdd")).Text);

                BindData();
                gvFeedbackEvaluation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("DELETE"))
            {
                PhoenixRegistersFeedbackEvaluation.DeleteFeedbackEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                (Int32.Parse(((RadLabel)e.Item.FindControl("lblEvaluationId")).Text)));
                BindData();
                gvFeedbackEvaluation.Rebind();
            }
            else if (e.CommandName.ToUpper().Equals("UPDATE"))
            {
                if (!IsValidFeedbackEvalaution(((RadDropDownList)e.Item.FindControl("ddlTypeEdit")).SelectedValue,
                                          ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text))
                {
                    ucError.Visible = true;
                    return;
                }
                PhoenixRegistersFeedbackEvaluation.UpdateFeedbackEvaluation(PhoenixSecurityContext.CurrentSecurityContext.UserCode,
                    Convert.ToInt32(((RadLabel)e.Item.FindControl("lblEvaluationIdEdit")).Text),
                    Convert.ToInt32(((RadDropDownList)e.Item.FindControl("ddlTypeEdit")).SelectedValue),
                    ((RadTextBox)e.Item.FindControl("txtCodeEdit")).Text,
                   ((RadTextBox)e.Item.FindControl("txtNameEdit")).Text);

                BindData();
                gvFeedbackEvaluation.Rebind();
            }
            else if (e.CommandName == "Page")
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

    protected void gvFeedbackEvaluation_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        ViewState["PAGENUMBER"] = ViewState["PAGENUMBER"] != null ? ViewState["PAGENUMBER"] : gvFeedbackEvaluation.CurrentPageIndex + 1;
        BindData();
    }

    protected void gvFeedbackEvaluation_ItemDataBound(object sender, GridItemEventArgs e)
    {
       
        if (e.Item is GridDataItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdDelete");
            if (db != null)
            {
                db.Attributes.Add("onclick", "return fnConfirmDelete(event); return false;");
                db.Visible = SessionUtil.CanAccess(this.ViewState, db.CommandName);
            }

            LinkButton eb = (LinkButton)e.Item.FindControl("cmdEdit");
            if (eb != null) eb.Visible = SessionUtil.CanAccess(this.ViewState, eb.CommandName);

            LinkButton sb = (LinkButton)e.Item.FindControl("cmdSave");
            if (sb != null) sb.Visible = SessionUtil.CanAccess(this.ViewState, sb.CommandName);

            LinkButton cb = (LinkButton)e.Item.FindControl("cmdCancel");
            if (cb != null) cb.Visible = SessionUtil.CanAccess(this.ViewState, cb.CommandName);

            RadDropDownList ucTypeid = (RadDropDownList)e.Item.FindControl("ddlTypeEdit");
            DataRowView drv = (DataRowView)e.Item.DataItem;
            if (ucTypeid != null) ucTypeid.SelectedValue = drv["FLDTYPE"].ToString();

        }
        if (e.Item is GridFooterItem)
        {
            LinkButton db = (LinkButton)e.Item.FindControl("cmdAdd");
            if (db != null)
            {
                if (!SessionUtil.CanAccess(this.ViewState, db.CommandName))
                    db.Visible = false;
            }
        }
    }
}
